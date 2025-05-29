using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Serializers
{
    public abstract class BaseTypeSerializer
    {
        /// <summary>
        /// Преобразование в двоичный вид
        /// </summary>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public abstract UInt64 GetBinaryValue(Type targetType, object value, int size);
        /// <summary>
        /// Преобразование из двоичного вида
        /// </summary>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public abstract object GetFromBinaryValue(Type valueType, UInt64 value, int size);
    }
}
