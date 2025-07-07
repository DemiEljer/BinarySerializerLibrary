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
    /// <summary>
    /// Класс для построения бинарного массива
    /// </summary>
    public class BinaryArraySlowBuilder : ABinaryArrayBuilder
    {
        /// <summary>
        /// Коллекция байт
        /// </summary>
        private LinkedListWrapper<byte> _ByteList { get; } = new();
        /// <summary>
        /// Реальный размер массива в байтах
        /// </summary>
        public override long BytesCount => _ByteList.Count;
        /// <summary>
        /// Получить массив байт
        /// </summary>
        /// <returns></returns>
        public override byte[] GetByteArray() => _ByteList.ToArray();
        /// <summary>
        /// Добавить содержимое другого объекта построения байтового массива и сдвинуться в конец
        /// </summary>
        protected override void _AppenBuilderAndShiftToEnd(ABinaryDataWriter builder)
        {
            _ByteList.AppendElements(builder.GetByteArray());
            _ByteList.ShiftToEnd();
        }
        /// <summary>
        /// Добавить некоторое число бит к коллекции
        /// </summary>
        public void AppendBits(long bitsCount)
        {
            _VerifyCollectionBitSizeModification(bitsCount);

            long targetBitLength = CurrentBitIndex + bitsCount;
            // В случае, если целевое количество бит превышает текущее выделенное, то расширяем коллекцию байт
            if (targetBitLength > BitsCount)
            {
                long appendingBytesCount = targetBitLength - BitsCount;
                appendingBytesCount = ByteVectorHandler.GetBytesCount(appendingBytesCount);

                _ByteList.CreateAndAppendToEnd((int)(appendingBytesCount));
            }
        }
        /// <summary>
        /// Добавить байт в начало коллекции
        /// </summary>
        protected override void _AppendByteToHead(byte byteValue)
        {
            _ByteList.AppendToHead(byteValue);
        }
        /// <summary>
        /// Записать битовое поле
        /// </summary>
        protected override void _AppendValue(int bitsCount, ulong value)
        {
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

            long targetBitIndex = CurrentBitIndex + bitsCount;
            // Смещение коллекции байт и индекса бита
            {
                // Определяем смещение в коллекции байт
                if (takingBytesCount == 1
                    && targetBitIndex % 8 == 0)
                {
                    // Если полностью был задействован байт
                    _ByteList.Shift(takingBytesCount);
                }
                else if (takingBytesCount > 1)
                {
                    if (targetBitIndex % 8 == 0)
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
        /// Расчет количества задействованных байт
        /// </summary>
        /// <returns></returns>
        private int _GetTakenBytesCount(int bitsCount)
        {
            if (bitsCount == 0)
            {
                return 0;
            }

            int byteShift = ByteVectorHandler.GetDeltaBits(CurrentBitIndex);

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
        protected override void _MakeAlignment(int alignmentBitsCount)
        {
            // Добавление недостающего вектора данных
            AppendBits(alignmentBitsCount);

            // Количество байт сдвига (гарантированно на 1 байт)
            int alignmentByteShifting = (int)(alignmentBitsCount / 8) + 1;
            // Сдвиг в векторе данных
            _ByteList.Shift(alignmentByteShifting);
        }
        /// <summary>
        /// Очистить объект построения бинарной коллекции
        /// </summary>
        /// </summary>
        protected override void _Clear()
        {
            _ByteList.Clear();
        }
    }
}
