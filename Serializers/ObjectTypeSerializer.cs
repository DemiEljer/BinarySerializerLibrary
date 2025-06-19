using BinarySerializerLibrary.Attributes;
using BinarySerializerLibrary.Base;
using BinarySerializerLibrary.ObjectSerializationRecipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Serializers
{
    public class ObjectTypeSerializer : ComplexBaseTypeSerializer
    {
        public override object? Deserialize(BinaryTypeBaseAttribute attribute, Type objType, BinaryArrayReader reader)
        {
            var objectRecipe = ObjectSerializationRecipesMapper.GetRecipe(objType);

            var resultObject = Activator.CreateInstance(objType);

            if (resultObject != null)
            {
                objectRecipe.Deserialization(resultObject, reader);
            }

            return resultObject;
        }
        public override void Serialize(BinaryTypeBaseAttribute attribute, object obj, BinaryArrayBuilder builder)
        {
            var objectRecipe = ObjectSerializationRecipesMapper.GetRecipe(obj.GetType());

            objectRecipe.Serialization(obj, builder);
        }
    }
}
