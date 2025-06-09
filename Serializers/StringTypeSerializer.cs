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
    internal class StringTypeSerializer : ComplexBaseTypeSerializer
    {
        public override object? Deserialize(BinaryTypeBaseAttribute attribute, Type objType, BinaryArrayReader reader)
        {
            // Десераилизация длины строки
            var stringLength = ComplexBaseTypeSerializer.DeserializeCollectionSize(attribute, reader);

            if (stringLength > 0)
            {
                IEnumerable<char> _GetStringSymboles()
                {
                    foreach (var symboleIndex in Enumerable.Range(0, stringLength))
                    {
                        yield return BaseTypeSerializerMapper.DeserializeValue<char>(reader.ReadValue(attribute.FieldSize, attribute.Alignment), attribute.FieldSize);
                    }
                }

                return new string(_GetStringSymboles().ToArray());
            }
            else
            {
                return "";
            }
        }

        public override void Serialize(BinaryTypeBaseAttribute attribute, object? obj, BinaryArrayBuilder builder)
        {
            var stringObject = (string)obj;
            // Сериализация длины строки
            int stringLength = ComplexBaseTypeSerializer.SerializeCollectionSize(attribute, stringObject.Length, builder);

            if (stringLength > 0)
            {
                foreach (var stringSymbole in stringObject)
                {
                    builder.AppendBitValue(attribute.FieldSize, BaseTypeSerializerMapper.SerializeValue(stringSymbole, attribute.FieldSize), attribute.Alignment);
                }
            }
        }
    }
}
