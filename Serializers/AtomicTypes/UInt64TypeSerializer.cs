using BinarySerializerLibrary.Base;
using BinarySerializerLibrary.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Serializers.AtomicTypes
{
    internal class UInt64TypeSerializer : BaseTypeSerializer
    {
        /// <summary>
        /// Преобразование в двоичный вид
        /// </summary>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public override ulong GetBinaryValue(Type targetType, object value, int size)
        {
            ulong realValue = (ulong)value;

            if (!ByteVectorHandler.DoesValueSuitsBitSizeUInt(realValue, size))
            {
                throw new TypeTooSmallForValueException(size, realValue);
            }

            return realValue & ByteVectorHandler.GetMask(size);
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
            var _realValue = value;

            return Convert.ToUInt64(_realValue);
        }
    }
}
