using BinarySerializerLibrary.Attributes;
using BinarySerializerLibrary.BinaryDataHandlers;
using BinarySerializerLibrary.ObjectSerializationRecipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Serializers.ComplexTypes
{
    public class ObjectTypeSerializer : ComplexBaseTypeSerializer
    {
        public override object? Deserialize(BinaryTypeBaseAttribute attribute, Type objType, ABinaryDataReader reader)
        {
            var objectRecipe = ObjectSerializationRecipesMapper.GetOrCreateRecipe(objType);

            var resultObject = Activator.CreateInstance(objType);

            if (resultObject != null)
            {
                objectRecipe.Deserialization(resultObject, reader);
            }

            return resultObject;
        }
        public override void Serialize(BinaryTypeBaseAttribute attribute, object obj, ABinaryDataWriter builder)
        {
            var objectRecipe = ObjectSerializationRecipesMapper.GetOrCreateRecipe(obj.GetType());

            objectRecipe.Serialization(obj, builder);
        }
    }
}
