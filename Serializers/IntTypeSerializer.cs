using BinarySerializerLibrary.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BinarySerializerLibrary.Serializers
{
    public class IntTypeSerializer : BaseTypeSerializer
    {
        /// <summary>
        /// Преобразование в двоичный вид
        /// </summary>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public override UInt64 GetBinaryValue(Type targetType, object value, int size)
        {
            if (targetType == typeof(sbyte))
            {
                return ByteVectorHandler.GetUIntFromInt((sbyte)value, size); ;
            }
            else if (targetType == typeof(Int16))
            {
                return ByteVectorHandler.GetUIntFromInt((Int16)value, size); ;
            }
            else if (targetType == typeof(Int32))
            {
                return ByteVectorHandler.GetUIntFromInt((Int32)value, size); ;
            }
            else if (targetType == typeof(Int64))
            {
                return ByteVectorHandler.GetUIntFromInt((Int64)value, size); ;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// Преобразование из двоичного вида
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public override object GetFromBinaryValue(Type targetType, UInt64 value, int size)
        {
            var _realValue = ByteVectorHandler.GetIntFromUInt(value, size);

            if (targetType == typeof(sbyte))
            {
                return Convert.ToSByte(_realValue);
            }
            else if (targetType == typeof(Int16))
            {
                return Convert.ToInt16(_realValue);
            }
            else if (targetType == typeof(Int32))
            {
                return Convert.ToInt32(_realValue);
            }
            else if (targetType == typeof(Int64))
            {
                return Convert.ToInt64(_realValue);
            }
            else
            {
                return 0;
            }
        }
    }
}
