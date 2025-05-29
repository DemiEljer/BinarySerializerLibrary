using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Base
{
    /// <summary>
    /// Класс для построения бинарного массива
    /// </summary>
    public class BinaryArrayBuilder
    {
        /// <summary>
        /// Коллекция байт
        /// </summary>
        private LinkedListWrapper<byte> _ByteList { get; } = new();
        /// <summary>
        /// Реальный размер массива в битах
        /// </summary>
        public int RealBitLength => _ByteList.Count * 8;
        /// <summary>
        /// Преобразовать в массив
        /// </summary>
        /// <returns></returns>
        public byte[] GetByteArray() => _ByteList.ToArray();
        /// <summary>
        /// Текущий индекс в коллекции
        /// </summary>
        public int CurrentBitIndex { get; private set; } = 0;
        /// <summary>
        /// Добавить некоторое число бит к коллекции
        /// </summary>
        /// <param name="bitCount"></param>
        public void AppendBits(int bitsCount)
        {
            int targetBitLength = CurrentBitIndex + bitsCount;
            // В случае, если целевое количество бит превышает текущее выделенное, то расширяем коллекцию байт
            if (targetBitLength > RealBitLength)
            {
                int appendingBytesCount = targetBitLength - RealBitLength;
                appendingBytesCount = (appendingBytesCount / 8) + ((appendingBytesCount % 8) > 0 ? 1 : 0);

                _ByteList.CreateAndAppendToEnd(appendingBytesCount);
            }
        }
        /// <summary>
        /// Записать битовое поле
        /// </summary>
        public void AppendBitValue(int bitsCount, UInt64 value)
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

            // Смещение коллекции байт и индекса бита
            {
                // Смещение индекса текущего бита
                CurrentBitIndex += bitsCount;
                // Определяем смещение в коллекции байт
                if (takingBytesCount == 1
                    && (CurrentBitIndex % 8) == 0)
                {
                    // Если полностью был задействован байт
                    _ByteList.Shift(takingBytesCount);
                }
                else if (takingBytesCount > 1)
                {
                    if ((CurrentBitIndex % 8) == 0)
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

            int byteShift = 8 - (CurrentBitIndex % 8);
            //int targetBitLength = CurrentBitIndex + bitsCount;

            // Определение, задействован ли полностью текущий байт
            int firstPart = byteShift > 0 ? 1 : 0;
            // Определение количества полных байт
            int middlePart = (bitsCount - byteShift) / 8;
            // Определение, задействован последний неполный байт
            int lastPart = ((bitsCount - byteShift) % 8) > 0 ? 1 : 0;

            return firstPart + middlePart + lastPart;
        }

        public void Clear()
        {
            _ByteList.Clear();
            CurrentBitIndex = 0;
        }
    }
}
