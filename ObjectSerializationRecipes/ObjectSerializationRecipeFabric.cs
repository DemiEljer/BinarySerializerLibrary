using BinarySerializerLibrary.Attributes;
using BinarySerializerLibrary.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.ObjectSerializationRecipes
{
    public static class ObjectSerializationRecipeFabric
    {
        public static ObjectSerializationRecipe? CreateRecipe(Type objectType)
        {
            // В случае, если тип не является классом или нет конструкторов по умолчанию, то прекращаем генерацию рецепта
            if (!ObjectTypeVerificationHandler.VerifyObjectType(objectType))
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
                        var binaryFieldAttribute = (BinaryTypeBaseAttribute)propertyAttribute;

                        if (ObjectTypeVerificationHandler.VerifyProperty(property, binaryFieldAttribute))
                        {
                            if (ComplexBaseTypeSerializer.IsComplexType(binaryFieldAttribute))
                            {
                                recipes.Add(new ComplexObjectPropertySerializationRecipe(property, binaryFieldAttribute));
                            }
                            else
                            {
                                recipes.Add(new AtomicObjectPropertySerializationRecipe(property, binaryFieldAttribute));
                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }

            return new ObjectSerializationRecipe(recipes.ToArray());
        }
    }
}
