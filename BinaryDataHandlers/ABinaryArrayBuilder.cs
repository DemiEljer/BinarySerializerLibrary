using BinarySerializerLibrary.Base;
using BinarySerializerLibrary.BinaryDataHandlers.Helpers;
using BinarySerializerLibrary.Enums;
using BinarySerializerLibrary.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.BinaryDataHandlers
{
    public abstract class ABinaryArrayBuilder : ABinaryDataWriter
    {
        /// <summary>
        /// Максимальный размер коллекции в байтах
        /// </summary>
        public static int MaxBytesCount { get; } = Array.MaxLength - BinaryDataLengthParameterHelpers.MaxDataArrayLengthFieldSize;
        /// <summary>
        /// Максимальный размер коллекции в битах
        /// </summary>
        public static long MaxBitsCount { get; } = (long)MaxBytesCount * 8;
        /// <summary>
        /// Текущий индекс в коллекции
        /// </summary>
        public long CurrentBitIndex { get; protected set; } = 0;
        /// <summary>
        /// Верификация размера коллекции в байтах
        /// </summary>
        /// <param name="size"></param>
        protected void _VerifyCollectionBitSizeModification(long addingBitsCount)
        {
            if ((CurrentBitIndex + addingBitsCount) > MaxBitsCount)
            {
                throw new ByteArrayIsOutOfMaximumLength();
            }
        }
        /// <summary>
        /// Верификация размера коллекции в байтах
        /// </summary>
        /// <param name="size"></param>
        protected void _VerifyCollectionByteSizeModification(long addingByteCount)
        {
            if ((BytesCount + addingByteCount) > MaxBytesCount)
            {
                throw new ByteArrayIsOutOfMaximumLength();
            }
        }
        /// <summary>
        /// Добавить содержимое другого объекта построения байтового массива и сдвинуться в конец
        /// </summary>
        public override void AppenBuilderAndShiftToEnd(ABinaryDataWriter builder)
        {
            if (builder is null
                || builder.BytesCount == 0)
            {
                return;
            }

            _VerifyCollectionByteSizeModification(builder.BytesCount);

            _AppenBuilderAndShiftToEnd(builder);
             
            CurrentBitIndex += builder.BitsCount;
        }
        /// <summary>
        /// Добавить содержимое другого объекта построения байтового массива и сдвинуться в конец
        /// </summary>
        protected abstract void _AppenBuilderAndShiftToEnd(ABinaryDataWriter builder);
        /// <summary>
        /// Добавить байт в начало коллекции
        /// </summary>
        public override void AppendByteToHead(byte byteValue)
        {
            _VerifyCollectionBitSizeModification(8);

            _AppendByteToHead(byteValue);
            // Смещение индекса текущего бита
            CurrentBitIndex += 8;
        }
        /// <summary>
        /// Добавить байт в начало коллекции
        /// </summary>
        protected abstract void _AppendByteToHead(byte byteValue);
        /// <summary>
        /// Записать битовое поле
        /// </summary>
        public override void AppendValue(int bitsCount, ulong value, BinaryAlignmentTypeEnum alignment)
        {
            // Применение выравнивания
            MakeAlignment(alignment);
            // Верификация коллекции на предмет достижения максимального размера
            _VerifyCollectionBitSizeModification(bitsCount);
            // Добавление бинарного значения в коллекцию
            _AppendValue(bitsCount, value);
            // Смещение индекса текущего бита
            CurrentBitIndex += bitsCount;
        }
        /// <summary>
        /// Записать битовое поле
        /// </summary>
        protected abstract void _AppendValue(int bitsCount, ulong value);
        /// <summary>
        /// Сделать байтовое выравнивание
        /// </summary>
        public override void MakeAlignment(BinaryAlignmentTypeEnum alignment)
        {
            long alignmentBitsCount = 0;
            // Выбор типа выравнивания
            switch (alignment)
            {
                case BinaryAlignmentTypeEnum.ByteAlignment:
                    alignmentBitsCount = ByteVectorHandler.GetDeltaBits(CurrentBitIndex);
                    break;
                default: break;
            }

            if (alignmentBitsCount > 0)
            {
                // Верификация коллекции на предмет достижения максимального размера
                _VerifyCollectionBitSizeModification(alignmentBitsCount);
                // Применение выравнивания
                _MakeAlignment((int)alignmentBitsCount);
                // Смещение индекса текущего бита
                CurrentBitIndex += alignmentBitsCount;
            }
        }
        /// <summary>
        /// Сделать байтовое выравнивание
        /// </summary>
        protected abstract void _MakeAlignment(int alignmentBitsCount);
        /// <summary>
        /// Очистить объект построения бинарной коллекции
        /// </summary>
        /// </summary>
        public override void Clear()
        {
            CurrentBitIndex = 0;

            _Clear();
        }
        /// <summary>
        /// Очистить объект построения бинарной коллекции
        /// </summary>
        /// </summary>
        protected abstract void _Clear();
    }
}
