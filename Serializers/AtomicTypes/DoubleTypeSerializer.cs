using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Serializers.AtomicTypes
{
    public class DoubleTypeSerializer : BaseTypeSerializer
    {
        /// <summary>
        /// Преобразование в двоичный вид
        /// </summary>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public override ulong GetBinaryValue(Type targetType, object value, int size)
        {
            var bytes = BitConverter.GetBytes((double)value);

            return bytes[0]
                | (ulong)bytes[1] << 8
                | (ulong)bytes[2] << 16
                | (ulong)bytes[3] << 24
                | (ulong)bytes[4] << 32
                | (ulong)bytes[5] << 40
                | (ulong)bytes[6] << 48
                | (ulong)bytes[7] << 56;
        }
        /// <summary>
        /// Преобразование из двоичного вида
        /// </summary>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public override object GetFromBinaryValue(Type valueType, ulong value, int size)
        {
            byte[] bytes =
            {
                (byte)(value >> 0 & 0xFF)
                , (byte)(value >> 8 & 0xFF)
                , (byte)(value >> 16 & 0xFF)
                , (byte)(value >> 24 & 0xFF)
                , (byte)(value >> 32 & 0xFF)
                , (byte)(value >> 40 & 0xFF)
                , (byte)(value >> 48 & 0xFF)
                , (byte)(value >> 56 & 0xFF)
            };

            return BitConverter.ToDouble(bytes);
        }
    }
}
