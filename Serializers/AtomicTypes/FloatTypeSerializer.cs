using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Serializers.AtomicTypes
{
    public class FloatTypeSerializer : BaseTypeSerializer
    {
        /// <summary>
        /// Преобразование в двоичный вид
        /// </summary>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public override ulong GetBinaryValue(Type targetType, object value, int size)
        {
            var bytes = BitConverter.GetBytes((float)value);

            return bytes[0]
                | (ulong)bytes[1] << 8
                | (ulong)bytes[2] << 16
                | (ulong)bytes[3] << 24;
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
            };


            return BitConverter.ToSingle(bytes);
        }
    }
}
