using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Base
{
    public class ByteVectorHandler
    {
        /// <summary>
        /// Массив масок
        /// </summary>
        private static UInt64[] _Masks { get; }
        /// <summary>
        /// Массив знаков
        /// </summary>
        private static UInt64[] _SignBits { get; }
        /// <summary>
        /// Массив максимальных значений
        /// </summary>
        private static UInt64[] _MaxVectorValues { get; }
        /// <summary>
        /// Конструктор класса
        /// </summary>
        static ByteVectorHandler()
        {
            // Инициализация масок
            UInt64 value = 0;

            _Masks = new UInt64[65];

            for (int i = 0; i < 65; i++)
            {
                _Masks[i] = value;

                value = value << 1 | 1;
            }

            // Инициализация битов знака
            value = 1;

            _SignBits = new UInt64[65];

            _SignBits[0] = 0x00;

            for (int i = 1; i < 65; i++)
            {
                _SignBits[i] = value;

                value = value << 1;
            }

            // Инициализация максимальных значений

            _MaxVectorValues = new UInt64[65];

            _MaxVectorValues[0] = 0;

            for (int i = 1; i < 65; i++)
            {
                _MaxVectorValues[i] = (ulong)Math.Pow(2, i) - 1;
            }
        }
        /// <summary>
        /// Получить значение маски
        /// </summary>
        /// <param name="bitSize"></param>
        /// <returns></returns>
        public static UInt64 GetMask(int bitSize)
        {
            if (bitSize > _Masks.Length || bitSize < 0)
            {
                return _Masks[0];
            }
            else
            {
                return _Masks[bitSize];
            }
        }
        /// <summary>
        /// Получить максимальное значений
        /// </summary>
        /// <param name="bitSize"></param>
        /// <returns></returns>
        public static UInt64 GetMaxValue(int bitSize)
        {
            if (bitSize > _MaxVectorValues.Length || bitSize < 0)
            {
                return _MaxVectorValues[0];
            }
            else
            {
                return _MaxVectorValues[bitSize];
            }
        }
        /// <summary>
        /// Получить знаковыый бит
        /// </summary>
        /// <param name="bitSize"></param>
        /// <returns></returns>
        public static UInt64 GetSignBit(int bitSize)
        {
            if (bitSize > _MaxVectorValues.Length || bitSize < 0)
            {
                return _SignBits[0];
            }
            else
            {
                return _SignBits[bitSize];
            }
        }
        /// <summary>
        /// Установить значения вектора данных
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        public static byte[] SetVectorData(byte[] data, byte value)
        {
            if (data == null) return Array.Empty<byte>();

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = value;
            }

            return data;
        }
        /// <summary>
        /// Проверка, что значение помещается в отведенное количество бит
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bitSize"></param>
        /// <returns></returns>
        public static bool DoesValueSuitsBitSizeUInt(UInt64 value, int bitSize) => value <= GetMaxValue(bitSize);
        /// <summary>
        /// Проверка, что значение помещается в отведенное количество бит
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bitSize"></param>
        /// <returns></returns>
        public static bool DoesValueSuitsBitSizeInt(Int64 value, int bitSize)
        {
            // Обработка граничного случая
            if (bitSize == 0)
            {
                return value == 0;
            }
            else
            {
                var halfMaxValue = (long)(GetMaxValue(bitSize) >> 1);

                return value <= halfMaxValue && value >= -(halfMaxValue + 1);
            }
        }

        public static Int64 GetIntFromUInt(UInt64 value, int bitSize)
        {
            value &= GetMask(bitSize);

            if (bitSize == 64 || (value & GetSignBit(bitSize)) == 0)
            {
                return (long)value;
            }
            else
            {
                return (long)value - (long)GetMask(bitSize) - 1;
            }
        }

        public static UInt64 GetUIntFromInt(Int64 value, int bitSize)
        {
            value &= (long)GetMask(bitSize);

            if (bitSize == 64 || value >= 0)
            {
                return (ulong)value;
            }
            else
            {
                return GetMask(bitSize) - (ulong)(-value) + 1;
            }
        }
        /// <summary>
        /// Получить количество байт из бит
        /// </summary>
        /// <param name="bitsCount"></param>
        /// <returns></returns>
        public static long GetBytesCount(long bitsCount) => (bitsCount / 8) + (bitsCount % 8 == 0 ? 0 : 1);
        /// <summary>
        /// Получить количество недостаюших бит до байта
        /// </summary>
        /// <param name="bitsCount"></param>
        /// <returns></returns>
        public static int GetDeltaBits(long bitsCount) => (int)(bitsCount % 8 == 0 ? 0 : 8 - (bitsCount % 8));
        /// <summary>
        /// Получить значение параметра из вектора данных
        /// </summary>
        /// <param name="data"></param>
        /// <param name="bitSize"></param>
        /// <param name="bitPosition"></param>
        /// <returns></returns>
        public static UInt64 GetVectorParamValue(byte[] data, int bitSize, long bitPosition)
        {
            if (data == null)
            {
                return 0;
            }

            // Смещение выходного значения
            int shift = (int)(bitPosition % 8);

            UInt64 result = 0;

            if (bitSize > (8 - shift))
            {
                // Текущий индекс байта
                int currentBytePosition = (int)(bitPosition / 8);
                // Текущий индекс бита
                int currentBitPosition = 8 - shift;
                // Инициализируем первые биты
                result |= (UInt64)data[currentBytePosition] >> shift;
                currentBytePosition += 1;

                while (currentBitPosition < bitSize)
                {
                    result |= (UInt64)data[currentBytePosition] << currentBitPosition;
                    currentBitPosition += 8;
                    currentBytePosition += 1;
                }
            }
            else
            {
                result |= (UInt64)data[bitPosition / 8] >> shift;
            }

            return result & GetMask(bitSize);
        }
        /// <summary>
        /// Установить значение параметра в вектор данных
        /// </summary>
        /// <param name="data"></param>
        /// <param name="bitSize"></param>
        /// <param name="bitPosition"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void SetVectorParamValue(byte[]? data, int bitSize, long bitPosition, UInt64 value)
        {
            if (data is null 
                || data.Length == 0
                || bitSize == 0
                || (data.Length * 8) < (bitPosition + bitSize))
            {
                return;
            }

            // Смещение выходного значения
            int shift = (int)(bitPosition % 8);

            if (bitSize > (8 - shift))
            {
                // Текущий индекс байта
                int currentBytePosition = (int)(bitPosition / 8);
                // Текущий индекс бита
                int currentBitPosition = 8 - shift;
                // Инициализируем первые биты
                data[currentBytePosition] |= (byte)(value << shift);
                currentBytePosition += 1;

                while (currentBitPosition < bitSize)
                {
                    data[currentBytePosition] |= (byte)(value >> currentBitPosition);
                    currentBitPosition += 8;
                    currentBytePosition += 1;
                }
            }
            else
            {
                data[bitPosition / 8] |= (byte)(value << shift);
            }
        }
        /// <summary>
        /// Установить значение параметра в вектор данных
        /// </summary>
        /// <param name="data"></param>
        /// <param name="bitSize"></param>
        /// <param name="bitPosition"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IEnumerable<byte> SetVectorParamValue(IEnumerable<byte>? data, int bitSize, long bitPosition, UInt64 value)
        {
            if (data is null || !data.Any())
            {
                yield break;
            }

            // Смещение выходного значения
            int shift = (int)(bitPosition % 8);

            if (bitSize > (8 - shift))
            {
                // Текущий индекс бита
                int currentBitPosition = 8 - shift;

                int elementIndex = 0;
                foreach (var dataElement in data)
                {
                    byte newDataElementValue = dataElement;

                    if (elementIndex == 0)
                    {
                        newDataElementValue |= (byte)(value << shift);
                    }
                    else
                    {
                        newDataElementValue |= (byte)(value >> currentBitPosition);
                        currentBitPosition += 8;
                    }

                    elementIndex++;
                    yield return newDataElementValue;
                }
            }
            else
            {
                yield return (byte)(data.First() | (byte)(value << shift));
            }
        }
    }
}
