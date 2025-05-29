using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Base
{
    public class BinaryArrayReader
    {
        /// <summary>
        /// Массив байт
        /// </summary>
        public byte[]? ByteArray { get; private set; } = null;
        /// <summary>
        /// Текущий индекс бита
        /// </summary>
        public int BitIndex { get; private set; }
        /// <summary>
        /// Размер вектора в битах
        /// </summary>
        public int BitLength => ByteCount * 8;
        /// <summary>
        /// Количество байт
        /// </summary>
        public int ByteCount => ByteArray == null ? 0 : ByteArray.Length;

        public BinaryArrayReader() { }

        public BinaryArrayReader(byte[]? byteArray)
        {
            SetByteArray(byteArray);
        }
        /// <summary>
        /// Проинициализировать читаемый вектор
        /// </summary>
        /// <param name="byteArray"></param>
        public void SetByteArray(byte[]? byteArray)
        {
            ByteArray = byteArray;
            BitIndex = 0;
        }
        /// <summary>
        /// Сбросить индекс битов
        /// </summary>
        public void ResetBitIndex()
        {
            BitIndex = 0;
        }
        /// <summary>
        /// Получить значение поля
        /// </summary>
        /// <param name="bitSize"></param>
        /// <returns></returns>
        public UInt64 ReadValue(int bitSize)
        {
            if (ByteArray is null)
            {
                return 0;
            }
            // Вычленение значения из вектора байт
            var resultValue = ByteVectorHandler.GetVectorParamValue(ByteArray, bitSize, BitIndex);
            // Инкрементирование индекса в векторе байтов
            BitIndex = Math.Clamp(BitIndex + bitSize, 0, BitLength);

            return resultValue;
        }
    }
}
