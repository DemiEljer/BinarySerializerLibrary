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
        public static ObjectSerializationRecipe CreateReciepe(Type objectType)
        {
            List<BaseObjectFieldSerializationRecipe> recipes = new();

            foreach (var property in objectType.GetProperties())
            {
                var propertyAttributes = property.GetCustomAttributes(true);

                foreach (var propertyAttribut in propertyAttributes)
                {
                    if (propertyAttribut is BinaryTypeBaseAttribute)
                    {
                        var binaryFieldAttribute = (BinaryTypeBaseAttribute)propertyAttribut;

                        if (ComplexBaseTypeSerializer.IsComplexType(binaryFieldAttribute))
                        {
                            recipes.Add(new ComplexObjectFieldSerializationRecipe(property, binaryFieldAttribute));
                        }
                        else
                        {
                            recipes.Add(new SimpleObjectFieldSerializationRecipe(property, binaryFieldAttribute));
                        }
                    }
                }
            }

            return new ObjectSerializationRecipe(recipes.ToArray());
        }
    }
}
