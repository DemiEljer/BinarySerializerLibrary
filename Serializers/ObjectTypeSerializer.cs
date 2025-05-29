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
            // Десераилизация размера массива
            var objectExistance = BaseTypeSerializerMapper.DeserializeValue<bool>(reader.ReadValue(1), 1);

            if (objectExistance)
            {
                var objectRecipe = ObjectSerializationRecipesMapper.GetRecipe(objType);

                var resultObject = Activator.CreateInstance(objType);

                if (resultObject != null)
                {
                    objectRecipe.Deserialization(resultObject, reader);
                }

                return resultObject;
            }
            else
            {
                return null;
            }
        }

        public override void Serialize(BinaryTypeBaseAttribute attribute, object? obj, BinaryArrayBuilder builder)
        {
            if (obj is null)
            {
                // Сериализация флага присутсвия объекта (существует ли объект)
                builder.AppendBitValue(1, BaseTypeSerializerMapper.SerializeValue<bool>(false, 1));
            }
            else
            {
                // Сериализация флага присутсвия объекта (существует ли объект)
                builder.AppendBitValue(1, BaseTypeSerializerMapper.SerializeValue<bool>(true, 1));

                var objectRecipe = ObjectSerializationRecipesMapper.GetRecipe(obj.GetType());

                objectRecipe.Serialization(obj, builder);
            }
        }
    }
}
