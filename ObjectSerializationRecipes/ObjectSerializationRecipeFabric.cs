using BinarySerializerLibrary.Attributes;
using BinarySerializerLibrary.ObjectSerializationRecipes.TypesWrapperDelegates;
using BinarySerializerLibrary.Serializers.ComplexTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.ObjectSerializationRecipes
{
    public class ObjectSerializationRecipeFabric
    {
        public event Action<Type>? ObjectTypeHasBeenDetectedEvent;

        private ObjectTypeVerificationAndExtractionHandler _TypeVerificationAndExtractionHandler { get; } = new();

        public ObjectSerializationRecipeFabric()
        {
            _TypeVerificationAndExtractionHandler.ObjectTypeHasBeenDetectedEvent += (type) => ObjectTypeHasBeenDetectedEvent?.Invoke(type);

            _TypeVerificationAndExtractionHandler.ListTypeHasBeenDetectedEvent += (type) => ListTypeWrapperDelegateCollection.CookType(type);
        }

        public ObjectSerializationRecipe? CreateRecipe(Type objectType)
        {
            // В случае, если тип не является классом или нет конструкторов по умолчанию, то прекращаем генерацию рецепта
            if (!_TypeVerificationAndExtractionHandler.VerifyObjectType(objectType))
            {
                return null;
            }

            List<BaseObjectPropertySerializationRecipe> recipes = new();
            // Поиск свойств для сериализации
            foreach (var property in objectType.GetProperties())
            {
                var propertyAttributes = property.GetCustomAttributes(true);
                // Поиск атрибута сериализации свойства
                foreach (var propertyAttribute in propertyAttributes)
                {
                    if (propertyAttribute is BinaryTypeBaseAttribute)
                    {
                        var binaryPropertyAttribute = propertyAttribute as BinaryTypeBaseAttribute;
                        // Извлечение атрибута типа
                        if (binaryPropertyAttribute is BinaryTypeAutoAttribute)
                        {
                            binaryPropertyAttribute = _TypeVerificationAndExtractionHandler.GetPropertyAttribute(property, binaryPropertyAttribute);
                        }
                        // В случае, если невозможно определить атрибут свойства
                        if (binaryPropertyAttribute is null)
                        {
                            return null;
                        }
                        // Верификация типа
                        if (!_TypeVerificationAndExtractionHandler.VerifyProperty(property, binaryPropertyAttribute))
                        {
                            return null;
                        }

                        recipes.Add(CreatePropertyRecipe(objectType, property, binaryPropertyAttribute));

                        break;
                    }
                }
            }

            return new ObjectSerializationRecipe(objectType, recipes.ToArray());
        }

        public static BaseObjectPropertySerializationRecipe CreatePropertyRecipe(Type objectType, PropertyInfo property, BinaryTypeBaseAttribute attribute)
        {
            if (ComplexBaseTypeSerializer.IsComplexType(attribute))
            {
                return new ComplexObjectPropertySerializationRecipe(objectType, property, attribute);
            }
            else
            {
                return new AtomicObjectPropertySerializationRecipe(objectType, property, attribute);
            }
        }

        public bool VerifyRecipe(Type objectType, ObjectSerializationRecipe recipe)
        {
            // В случае, если тип не является классом или нет конструкторов по умолчанию, то прекращаем генерацию рецепта
            if (!_TypeVerificationAndExtractionHandler.VerifyObjectType(objectType))
            {
                return false;
            }

            foreach (var property in recipe.PropertiesRecipes)
            {
                // Проверка, что свойство принадлежит типу
                if (!objectType.GetProperties().Contains(property.Property))
                {
                    return false;
                }
                var binaryPropertyAttribute = property.PropertyAttribute;
                // Автоматическое определение атрибута свойства
                if (binaryPropertyAttribute is BinaryTypeAutoAttribute)
                {
                    binaryPropertyAttribute = _TypeVerificationAndExtractionHandler.GetPropertyAttribute(property.Property, binaryPropertyAttribute);
                }
                // Верификация типа
                if (!_TypeVerificationAndExtractionHandler.VerifyProperty(property.Property, binaryPropertyAttribute))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
