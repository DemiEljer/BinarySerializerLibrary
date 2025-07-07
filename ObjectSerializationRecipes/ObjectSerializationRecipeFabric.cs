using BinarySerializerLibrary.Attributes;
using BinarySerializerLibrary.ObjectSerializationRecipes.TypesWrapperDelegates;
using BinarySerializerLibrary.Serializers.ComplexTypes;
using System;
using System.Collections.Generic;
using System.Linq;
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
                        var binaryFieldAttribute = propertyAttribute as BinaryTypeBaseAttribute;
                        // Извлечение атрибута типа
                        if (binaryFieldAttribute is BinaryTypeAutoAttribute)
                        {
                            binaryFieldAttribute = _TypeVerificationAndExtractionHandler.GetPropertyAttribute(property, binaryFieldAttribute);
                        }
                        // В случае, если невозможно определить атрибут свойства
                        if (binaryFieldAttribute is null)
                        {
                            return null;
                        }
                        // Верификация типа
                        if (!_TypeVerificationAndExtractionHandler.VerifyProperty(property, binaryFieldAttribute))
                        {
                            return null;
                        }

                        if (ComplexBaseTypeSerializer.IsComplexType(binaryFieldAttribute))
                        {
                            recipes.Add(new ComplexObjectPropertySerializationRecipe(objectType, property, binaryFieldAttribute));
                        }
                        else
                        {
                            recipes.Add(new AtomicObjectPropertySerializationRecipe(objectType, property, binaryFieldAttribute));
                        }

                        break;
                    }
                }
            }

            return new ObjectSerializationRecipe(recipes.ToArray());
        }
    }
}
