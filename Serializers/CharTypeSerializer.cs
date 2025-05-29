using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Serializers
{
    public class CharTypeSerializer : BaseTypeSerializer
    {
        /// <summary>
        /// Преобразование в двоичный вид
        /// </summary>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public override UInt64 GetBinaryValue(Type targetType, object value, int size)
        {
            return (char)value;
        }
        /// <summary>
        /// Преобразование из двоичного вида
        /// </summary>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public override object GetFromBinaryValue(Type valueType, UInt64 value, int size)
        {
            return (char)value;
        }
    }
}
