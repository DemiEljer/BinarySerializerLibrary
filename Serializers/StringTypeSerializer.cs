using BinarySerializerLibrary.Attributes;
using BinarySerializerLibrary.Base;
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
            var stringLength = BaseTypeSerializerMapper.DeserializeValue<Int32>(reader.ReadValue(32), 32);

            if (stringLength > 0)
            {
                IEnumerable<char> _GetStringSymboles()
                {
                    foreach (var symboleIndex in Enumerable.Range(0, stringLength))
                    {
                        yield return BaseTypeSerializerMapper.DeserializeValue<char>(reader.ReadValue(attribute.FieldSize), attribute.FieldSize);
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
            // В случае, если бы передан нулевой объект, то помечаем строку как пустую
            if (obj is null)
            {
                // Сериализация длины строки
                builder.AppendBitValue(32, BaseTypeSerializerMapper.SerializeValue<Int32>(0, 32));

                return;
            }

            var stringObject = (string)obj;
            // Сериализация длины строки
            builder.AppendBitValue(32, BaseTypeSerializerMapper.SerializeValue<Int32>(stringObject.Length, 32));

            foreach (var stringSymbole in stringObject)
            {
                builder.AppendBitValue(attribute.FieldSize, BaseTypeSerializerMapper.SerializeValue(stringSymbole, attribute.FieldSize));
            }
        }
    }
}
