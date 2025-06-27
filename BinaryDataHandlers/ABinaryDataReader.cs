using BinarySerializerLibrary.Enums;
using BinarySerializerLibrary.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.BinaryDataHandlers
{
    public abstract class ABinaryDataReader
    {
        /// <summary>
        /// Текущий индекс бита
        /// </summary>
        public long BitIndex { get; protected set; }
        /// <summary>
        /// Индекс в байтах
        /// </summary>
        public long ByteIndex => BitIndex / 8;
        /// <summary>
        /// Размер вектора в битах
        /// </summary>
        public long BitLength => ByteLength * 8;
        /// <summary>
        /// Количество байт
        /// </summary>
        public abstract long ByteLength { get; }
        /// <summary>
        /// Флаг, что достигнут конец файла
        /// </summary>
        public bool IsEndOfArray => BitIndex >= BitLength;
        /// <summary>
        /// Получить значение
        /// </summary>
        public abstract ulong ReadValue(int bitSize, BinaryAlignmentTypeEnum alignment);
        /// <summary>
        /// Сделать байтовое выравнивание
        /// </summary>
        public abstract void MakeAlignment(BinaryAlignmentTypeEnum alignment);
        /// <summary>
        /// Сдвинуться на определенное количество бит
        /// </summary>
        public void ShitBitIndex(long bitsCount)
        {
            long nextBitIndex = BitIndex + bitsCount;
            // Проверка на достижение конца массива
            if (nextBitIndex > BitLength)
            {
                throw new ByteArrayReaderIsOverException();
            }

            BitIndex = Math.Clamp(nextBitIndex, 0, BitLength);
        }
        /// <summary>
        /// Сдвинуться на определенное количество бит
        /// </summary>
        public void SetBitIndex(long bitIndex)
        {
            // Проверка на достижение конца массива
            if (bitIndex > BitLength)
            {
                throw new ByteArrayReaderIsOverException();
            }

            BitIndex = Math.Clamp(bitIndex, 0, BitLength);
        }
    }
}
