using BinarySerializerLibrary.Base;
using BinarySerializerLibrary.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BinarySerializerLibrary.Serializers
{
    public class UIntTypeSerializer : BaseTypeSerializer
    {
        /// <summary>
        /// Преобразование в двоичный вид
        /// </summary>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public override UInt64 GetBinaryValue(Type targetType, object value, int size)
        {
            if (targetType == typeof(byte))
            {
                byte realValue = (byte)value;

                if (!ByteVectorHandler.DoesValueSuitsBitSizeUInt(realValue, size))
                {
                    throw new TypeTooSmallForValueException(size, realValue);
                }

                return (UInt64)realValue & ByteVectorHandler.GetMask(size);
            }
            else if (targetType == typeof(UInt16))
            {
                UInt16 realValue = (UInt16)value;

                if (!ByteVectorHandler.DoesValueSuitsBitSizeUInt(realValue, size))
                {
                    throw new TypeTooSmallForValueException(size, realValue);
                }

                return (UInt64)realValue & ByteVectorHandler.GetMask(size);
            }
            else if (targetType == typeof(UInt32))
            {
                UInt32 realValue = (UInt32)value;

                if (!ByteVectorHandler.DoesValueSuitsBitSizeUInt(realValue, size))
                {
                    throw new TypeTooSmallForValueException(size, realValue);
                }

                return (UInt64)realValue & ByteVectorHandler.GetMask(size);
            }
            else if (targetType == typeof(UInt64))
            {
                UInt64 realValue = (UInt64)value;

                if (!ByteVectorHandler.DoesValueSuitsBitSizeUInt(realValue, size))
                {
                    throw new TypeTooSmallForValueException(size, realValue);
                }

                return (UInt64)realValue & ByteVectorHandler.GetMask(size);
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
            var _realValue = value;

            if (targetType == typeof(byte))
            {
                return Convert.ToByte(_realValue);
            }
            else if (targetType == typeof(UInt16))
            {
                return Convert.ToUInt16(_realValue);
            }
            else if (targetType == typeof(UInt32))
            {
                return Convert.ToUInt32(_realValue);
            }
            else if (targetType == typeof(UInt64))
            {
                return Convert.ToUInt64(_realValue);
            }
            else
            {
                return 0;
            }
        }
    }
}
