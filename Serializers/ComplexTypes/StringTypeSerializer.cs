using BinarySerializerLibrary.Attributes;
using BinarySerializerLibrary.BinaryDataHandlers;
using BinarySerializerLibrary.ObjectSerializationRecipes;
using BinarySerializerLibrary.Serializers.AtomicTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Serializers.ComplexTypes
{
    internal class StringTypeSerializer : ComplexBaseTypeSerializer
    {
        public override object? Deserialize(BinaryTypeBaseAttribute attribute, Type objType, ABinaryDataReader reader)
        {
            // Десераилизация длины строки
            var stringLength = DeserializeCollectionSize(attribute, reader);

            if (stringLength > 0)
            {
                IEnumerable<char> _GetStringSymbols()
                {
                    foreach (var symbolIndex in Enumerable.Range(0, stringLength))
                    {
                        yield return BaseTypeSerializerMapper.DeserializeValue<char>(reader.ReadValue(attribute.Size, attribute.Alignment), attribute.Size);
                    }
                }

                return new string(_GetStringSymbols().ToArray());
            }
            else
            {
                return "";
            }
        }

        public override void Serialize(BinaryTypeBaseAttribute attribute, object obj, ABinaryDataWriter builder)
        {
            var stringObject = (string)obj;
            // Сериализация длины строки
            int stringLength = SerializeCollectionSize(attribute, stringObject.Length, builder);

            if (stringLength > 0)
            {
                foreach (var stringSymbol in stringObject)
                {
                    builder.AppendValue(attribute.Size, BaseTypeSerializerMapper.SerializeValue(stringSymbol, attribute.Size), attribute.Alignment);
                }
            }
        }
    }
}
