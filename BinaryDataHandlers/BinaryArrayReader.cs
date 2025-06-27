using BinarySerializerLibrary.Base;
using BinarySerializerLibrary.Enums;
using BinarySerializerLibrary.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.BinaryDataHandlers
{
    public class BinaryArrayReader : ABinaryDataReader
    {
        /// <summary>
        /// Массив байт
        /// </summary>
        public byte[]? ByteArray { get; private set; } = null;
        /// <summary>
        /// Количество байт
        /// </summary>
        public override long ByteLength => ByteArray == null ? 0 : ByteArray.Length;

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
        /// Получить значение
        /// </summary>
        public override ulong ReadValue(int bitSize, BinaryAlignmentTypeEnum alignment)
        {
            if (ByteArray is null)
            {
                throw new ByteArrayReaderIsOverException();
            }

            // Применение выравнивания
            MakeAlignment(alignment);

            long nextBitIndex = BitIndex + bitSize;
            // Проверка на достижение конца массива
            if (IsEndOfArray || nextBitIndex > BitLength)
            {
                throw new ByteArrayReaderIsOverException();
            }

            // Вычленение значения из вектора байт
            var resultValue = ByteVectorHandler.GetVectorParamValue(ByteArray, bitSize, BitIndex);
            // Инкрементирование индекса в векторе байтов
            BitIndex = Math.Clamp(nextBitIndex, 0, BitLength);

            return resultValue;
        }
        /// <summary>
        /// Сделать байтовое выравнивание
        /// </summary>
        public override void MakeAlignment(BinaryAlignmentTypeEnum alignment)
        {
            // Применение выравнивания
            switch (alignment)
            {
                case BinaryAlignmentTypeEnum.ByteAlignment:
                    BitIndex = BitIndex % 8 == 0 ? BitIndex : (BitIndex / 8 + 1) * 8;
                    break;
                default: break;
            }
            // Ограничение индекса бита фактическим размером массива
            BitIndex = Math.Clamp(BitIndex, 0, BitLength);
        }
    }
}
