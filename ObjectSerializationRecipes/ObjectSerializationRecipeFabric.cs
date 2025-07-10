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
            if (!VerifyType(objectType))
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
                        var propertyRecipe = CreatePropertyRecipe(objectType, property, propertyAttribute as BinaryTypeBaseAttribute);

                        if (propertyRecipe is not null)
                        {
                            recipes.Add(propertyRecipe);
                        }
                        else
                        {
                            return null;
                        }

                        break;
                    }
                }
            }

            return new ObjectSerializationRecipe(objectType, recipes.ToArray());
        }

        public bool VerifyRecipe(Type objectType, ObjectSerializationRecipe recipe)
        {
            // В случае, если тип не является классом или нет конструкторов по умолчанию, то прекращаем генерацию рецепта
            if (!VerifyType(objectType))
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
                // Запрет на использование данного атрибута
                if (property.PropertyAttribute is BinaryTypeAutoAttribute)
                {
                    return false;
                }
                // Верификация типа
                if (!VerifyProperty(property.Property, property.PropertyAttribute))
                {
                    return false;
                }
            }

            return true;
        }

        public BaseObjectPropertySerializationRecipe? CreatePropertyRecipe(Type objectType, PropertyInfo property, BinaryTypeBaseAttribute? attribute)
        {
            // Проверка, что свойство принадлежит типу
            if (!objectType.GetProperties().Contains(property))
            {
                return null;
            }
            // Извлечение атрибута типа
            if (attribute is BinaryTypeAutoAttribute)
            {
                attribute = ExtractPropertyAttribute(property, attribute);
            }
            // В случае, если невозможно определить атрибут свойства
            if (attribute is null)
            {
                return null;
            }
            // Верификация свойства
            if (!VerifyProperty(property, attribute))
            {
                return null;
            }
            // Проверка и создание итогового рецепта свойства
            if (ComplexBaseTypeSerializer.IsComplexType(attribute))
            {
                return new ComplexObjectPropertySerializationRecipe(objectType, property, attribute);
            }
            else
            {
                return new AtomicObjectPropertySerializationRecipe(objectType, property, attribute);
            }
        }

        public bool VerifyType(Type objectType) => _TypeVerificationAndExtractionHandler.VerifyObjectType(objectType);

        public bool VerifyProperty(PropertyInfo property, BinaryTypeBaseAttribute attribute) => _TypeVerificationAndExtractionHandler.VerifyProperty(property, attribute);

        public BinaryTypeBaseAttribute? ExtractPropertyAttribute(PropertyInfo property, BinaryTypeBaseAttribute attribute) => _TypeVerificationAndExtractionHandler.GetPropertyAttribute(property, attribute);
    }
}
