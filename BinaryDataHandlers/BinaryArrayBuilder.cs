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
    /// <summary>
    /// Класс для построения бинарного массива
    /// </summary>
    public class BinaryArrayBuilder : ABinaryDataWriter
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
        /// Коллекция байт
        /// </summary>
        private LinkedListWrapper<byte> _ByteList { get; } = new();
        /// <summary>
        /// Реальный размер массива в битах
        /// </summary>
        public override int BytesCount => _ByteList.Count;
        /// <summary>
        /// Реальный размер массива в битах
        /// </summary>
        public long ActualBitsCount => (long)_ByteList.Count * 8;
        /// <summary>
        /// Получить коллекцию байт
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<byte> GetBytes() => _ByteList.Elements;
        /// <summary>
        /// Текущий индекс в коллекции
        /// </summary>
        public long CurrentBitIndex { get; protected set; } = 0;
        /// <summary>
        /// Добавить некоторое число бит к коллекции
        /// </summary>
        public void AppendBits(long bitsCount)
        {
            _VerifyCollectionBitSizeModification(bitsCount);

            long targetBitLength = CurrentBitIndex + bitsCount;
            // В случае, если целевое количество бит превышает текущее выделенное, то расширяем коллекцию байт
            if (targetBitLength > ActualBitsCount)
            {
                long appendingBytesCount = targetBitLength - ActualBitsCount;
                appendingBytesCount = appendingBytesCount / 8 + (appendingBytesCount % 8 > 0 ? 1 : 0);

                _ByteList.CreateAndAppendToEnd((int)(appendingBytesCount));
            }
        }
        /// <summary>
        /// Добавить байт в начало коллекции
        /// </summary>
        public override void AppendByteToHead(byte byteValue)
        {
            _VerifyCollectionBitSizeModification(8);

            CurrentBitIndex += 8;
            _ByteList.AppendToHead(byteValue);
        }
        /// <summary>
        /// Записать битовое поле
        /// </summary>
        public override void AppendValue(int bitsCount, ulong value, BinaryAlignmentTypeEnum alignment)
        {
            // Применение выравнивания
            MakeAlignment(alignment);

            // Вызов логики дополнения вектора байт
            {
                AppendBits(bitsCount);
            }

            // Количества задействованных байт
            int takingBytesCount = _GetTakenBytesCount(bitsCount);

            // Обработка установки значения
            {
                _ByteList.Take(takingBytesCount, (oldValues) =>
                {
                    return ByteVectorHandler.SetVectorParamValue(oldValues, bitsCount, CurrentBitIndex % 8, value);
                });
            }

            // Смещение коллекции байт и индекса бита
            {
                // Смещение индекса текущего бита
                CurrentBitIndex += bitsCount;
                // Определяем смещение в коллекции байт
                if (takingBytesCount == 1
                    && CurrentBitIndex % 8 == 0)
                {
                    // Если полностью был задействован байт
                    _ByteList.Shift(takingBytesCount);
                }
                else if (takingBytesCount > 1)
                {
                    if (CurrentBitIndex % 8 == 0)
                    {
                        // Если полностью был задействован байт
                        _ByteList.Shift(takingBytesCount);
                    }
                    else
                    {
                        // Если что-то осталось
                        _ByteList.Shift(takingBytesCount - 1);
                    }
                }
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
            // Модификация коллекции
            {
                _ByteList.AppendElements(builder.GetBytes());

                _ByteList.ShiftToEnd();
                CurrentBitIndex = ActualBitsCount;
            }
        }
        /// <summary>
        /// Расчет количества задействованных байт
        /// </summary>
        /// <returns></returns>
        private int _GetTakenBytesCount(int bitsCount)
        {
            if (bitsCount == 0)
            {
                return 0;
            }

            int byteShift = 8 - (int)(CurrentBitIndex % 8);
            //int targetBitLength = CurrentBitIndex + bitsCount;

            // Определение, задействован ли полностью текущий байт
            int firstPart = byteShift > 0 ? 1 : 0;
            // Определение количества полных байт
            int middlePart = (bitsCount - byteShift) / 8;
            // Определение, задействован последний неполный байт
            int lastPart = (bitsCount - byteShift) % 8 > 0 ? 1 : 0;

            return firstPart + middlePart + lastPart;
        }
        /// <summary>
        /// Сделать байтовое выравнивание
        /// </summary>
        public override void MakeAlignment(BinaryAlignmentTypeEnum alignment)
        {
            long alignmentOffset = 0;
            // Выбор типа выравнивания
            switch (alignment)
            {
                case BinaryAlignmentTypeEnum.ByteAlignment:
                    alignmentOffset = CurrentBitIndex % 8 == 0 ? 0 : 8 - CurrentBitIndex % 8;
                    break;
                default: break;
            }

            if (alignmentOffset > 0)
            {
                _VerifyCollectionByteSizeModification(alignmentOffset);
                // Добавление недостающего вектора данных
                AppendBits(alignmentOffset);

                // Количество байт сдвига (гарантированно на 1 байт)
                int alignmentByteShifting = (int)(alignmentOffset / 8) + 1;
                // Сдвиг в векторе данных
                _ByteList.Shift(alignmentByteShifting);

                // Сдвиг индекса битов
                CurrentBitIndex += alignmentOffset;
            }
        }
        /// <summary>
        /// Очистить объект построения бинарной коллекции
        /// </summary>
        public override void Clear()
        {
            _ByteList.Clear();
            CurrentBitIndex = 0;
        }
        /// <summary>
        /// Верификация размера коллекции в байтах
        /// </summary>
        /// <param name="size"></param>
        private void _VerifyCollectionBitSizeModification(long addingBitsCount)
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
        private void _VerifyCollectionByteSizeModification(long addingByteCount)
        {
            if ((BytesCount + addingByteCount) > MaxBytesCount)
            {
                throw new ByteArrayIsOutOfMaximumLength();
            }
        }
    }
}
