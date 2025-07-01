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
        public abstract long BitIndex { get; protected set; }
        /// <summary>
        /// Индекс в байтах
        /// </summary>
        public long ByteIndex => BitIndex / 8;
        /// <summary>
        /// Размер вектора в битах
        /// </summary>
        public long BitsCount => BytesCount * 8;
        /// <summary>
        /// Количество байт
        /// </summary>
        public abstract long BytesCount { get; }
        /// <summary>
        /// Флаг, что достигнут конец файла
        /// </summary>
        public bool IsEndOfCollection => BitIndex >= BitsCount;
        /// <summary>
        /// Получить значение
        /// </summary>
        public abstract ulong ReadValue(int bitSize, BinaryAlignmentTypeEnum alignment);
        /// <summary>
        /// Сделать байтовое выравнивание
        /// </summary>
        public abstract void MakeAlignment(BinaryAlignmentTypeEnum alignment);
        /// <summary>
        /// Очистить объект чтения бинарной коллекции
        /// </summary>
        public abstract void Clear();
        /// <summary>
        /// Сбросить индекс битов
        /// </summary>
        public abstract void ResetBitIndex();

        /// <summary>
        /// Сдвинуться на определенное количество бит
        /// </summary>
        public void ShitBitIndex(long bitsCount)
        {
            long nextBitIndex = BitIndex + bitsCount;
            // Проверка на достижение конца массива
            if (nextBitIndex > BitsCount)
            {
                throw new ByteArrayReaderIsOverException();
            }

            BitIndex = Math.Clamp(nextBitIndex, 0, BitsCount);
        }
        /// <summary>
        /// Сдвинуться на определенное количество бит
        /// </summary>
        public void SetBitIndex(long bitIndex)
        {
            // Проверка на достижение конца массива
            if (bitIndex > BitsCount)
            {
                throw new ByteArrayReaderIsOverException();
            }

            BitIndex = Math.Clamp(bitIndex, 0, BitsCount);
        }
    }
}
