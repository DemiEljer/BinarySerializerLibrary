using BinarySerializerLibrary.Base;
using BinarySerializerLibrary.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Serializers.AtomicTypes
{
    internal class Int8TypeSerializer : BaseTypeSerializer
    {
        /// <summary>
        /// Преобразование в двоичный вид
        /// </summary>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public override ulong GetBinaryValue(Type targetType, object value, int size)
        {
            sbyte realValue = (sbyte)value;

            if (!ByteVectorHandler.DoesValueSuitsBitSizeInt(realValue, size))
            {
                throw new TypeTooSmallForValueException(size, realValue);
            }

            return ByteVectorHandler.GetUIntFromInt(realValue, size);
        }
        /// <summary>
        /// Преобразование из двоичного вида
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public override object GetFromBinaryValue(Type targetType, ulong value, int size)
        {
            var _realValue = ByteVectorHandler.GetIntFromUInt(value, size);

            return Convert.ToSByte(_realValue);
        }
    }
}
