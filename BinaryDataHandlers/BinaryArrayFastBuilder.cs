using BinarySerializerLibrary.Base;
using BinarySerializerLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BinarySerializerLibrary.BinaryDataHandlers.BinaryArrayBuilder;

namespace BinarySerializerLibrary.BinaryDataHandlers
{
    public class BinaryArrayFastBuilder : ABinaryArrayBuilder
    {
        #region SubStructures

        public class BinaryRecordsCollection
        {
            /// <summary>
            /// Список групп
            /// </summary>
            public LinkedList<BinaryRecord[]> RecordsGroups { get; } = new LinkedList<BinaryRecord[]>();
            /// <summary>
            /// Последняя группа элементов
            /// </summary>
            private BinaryRecord[]? _LastRecordsGroup { get; set; }
            /// <summary>
            /// Созданное количество записей
            /// </summary>
            public int CreatedRecordsCount { get; private set; } = 0;
            /// <summary>
            /// Текущее количество добавленных записей
            /// </summary>
            public int CurrentRecordsCount { get; private set; } = 0;
            /// <summary>
            /// Индекс элемента в последней группе
            /// </summary>
            private int _LastGroupRecordIndex { get; set; } = 0;
            /// <summary>
            /// Минимальный размер группы
            /// </summary>
            private int _MinGroupSize { get; set; } = 10;
            /// <summary>
            /// Минимальный размер группы
            /// </summary>
            public int MinGroupSize 
            { 
                get => _MinGroupSize; 
                set => _MinGroupSize = Math.Max(1, value); 
            }
            /// <summary>
            /// Множитель увеличения размера группы с увеличением количества записей
            /// </summary>
            private double _GroupIncreaseFactor { get; set; } = 2.0;
            /// <summary>
            /// Множитель увеличения размера группы с увеличением количества записей
            /// </summary>
            public double GroupIncreaseFactor 
            { 
                get => _GroupIncreaseFactor; 
                set => _GroupIncreaseFactor = Math.Max(0, value); 
            }
            /// <summary>
            /// Коллекция записей
            /// </summary>
            /// <returns></returns>
            public IEnumerable<BinaryRecord> GetRecords()
            {
                int recordIndex = 0;
                foreach (var recordArray in RecordsGroups)
                {
                    foreach (var record in recordArray)
                    {
                        if (recordIndex == CurrentRecordsCount)
                        {
                            yield break;
                        }
                        recordIndex++;

                        yield return record;
                    }
                }
            }
            /// <summary>
            /// Добавить запись в начало коллекции
            /// </summary>
            /// <param name="binaryValue"></param>
            /// <param name="binarySize"></param>
            public void AppendRecordHead(UInt64 binaryValue, sbyte binarySize)
            {
                RecordsGroups.AddFirst(new BinaryRecord[] { new BinaryRecord(binaryValue, binarySize) });

                CreatedRecordsCount++;
                CurrentRecordsCount++;
            }
            /// <summary>
            /// Добавить запись в конец коллекции
            /// </summary>
            /// <param name="binaryValue"></param>
            /// <param name="binarySize"></param>
            public void AppendRecord(UInt64 binaryValue, sbyte binarySize)
            {
                _CreateRecordsGroup();

                if (_LastRecordsGroup is not null)
                {
                    _LastRecordsGroup[_LastGroupRecordIndex].BinaryValue = binaryValue;
                    _LastRecordsGroup[_LastGroupRecordIndex].BinarySize = binarySize;
                }

                CurrentRecordsCount++;
                _LastGroupRecordIndex++;
            }
            /// <summary>
            /// Создать группу записей
            /// </summary>
            private void _CreateRecordsGroup()
            {
                if (CreatedRecordsCount == CurrentRecordsCount)
                {
                    _LastGroupRecordIndex = 0;

                    int creatingRecordsCount = Math.Max(MinGroupSize, (int)(Math.Log(CreatedRecordsCount) * GroupIncreaseFactor));

                    _LastRecordsGroup = new BinaryRecord[creatingRecordsCount];

                    RecordsGroups.AddLast(_LastRecordsGroup);
                    CreatedRecordsCount += creatingRecordsCount;
                }
            }
            /// <summary>
            /// Очистить коллекцию
            /// </summary>
            public void Clear()
            {
                RecordsGroups.Clear();
                CreatedRecordsCount = 0;
                CurrentRecordsCount = 0;
                _LastGroupRecordIndex = 0;
                _LastRecordsGroup = null;
            }
        }

        public struct BinaryRecord
        {
            /// <summary>
            /// Бинарное значение полу
            /// </summary>
            public UInt64 BinaryValue { get; set; }
            /// <summary>
            /// Размер битового поля
            /// </summary>
            public sbyte BinarySize { get; set; }

            public BinaryRecord(UInt64 binaryValue, sbyte binarySize)
            {
                BinaryValue = binaryValue;
                BinarySize = binarySize;
            }
        }

        #endregion

        /// <summary>
        /// Минимальный размер группы
        /// </summary>
        public int MinGroupSize 
        { 
            get => _Records.MinGroupSize; 
            set => _Records.MinGroupSize = value; 
        }
        /// <summary>
        /// Множитель увеличения размера группы с увеличением количества записей
        /// </summary>
        public double GroupIncreaseFactor 
        { 
            get => _Records.GroupIncreaseFactor; 
            set => _Records.GroupIncreaseFactor = value; 
        }
        /// <summary>
        /// Коллекция сериализуемых записей
        /// </summary>
        private BinaryRecordsCollection _Records { get; } = new();
        /// <summary>
        /// Количество байт
        /// </summary>
        public override long BytesCount => ByteVectorHandler.GetBytesCount(CurrentBitIndex);
        /// <summary>
        /// Получить массив байт
        /// </summary>
        /// <returns></returns>
        public override byte[] GetByteArray()
        {
            byte[] data = new byte[ByteVectorHandler.GetBytesCount(CurrentBitIndex)];

            int bitPosition = 0;
            foreach (var record in _Records.GetRecords())
            {
                ByteVectorHandler.SetVectorParamValue(data, record.BinarySize, bitPosition, record.BinaryValue);
                bitPosition += record.BinarySize;
            }

            return data;
        }
        /// <summary>
        /// Добавить содержимое другого объекта построения байтового массива и сдвинуться в конец
        /// </summary>
        protected override void _AppenBuilderAndShiftToEnd(ABinaryDataWriter builder)
        {
            if (builder is BinaryArrayFastBuilder)
            {
                foreach (var record in ((BinaryArrayFastBuilder)builder)._Records.GetRecords())
                {
                    _Records.AppendRecord(record.BinaryValue, record.BinarySize);
                }
            }
            else
            {
                foreach (var byteValue in builder.GetByteArray())
                {
                    _Records.AppendRecord(byteValue, 8);
                }
            }
        }
        /// <summary>
        /// Добавить байт в начало коллекции
        /// </summary>
        protected override void _AppendByteToHead(byte byteValue)
        {
            _Records.AppendRecordHead(byteValue, 8);
        }
        /// <summary>
        /// Записать битовое поле
        /// </summary>
        protected override void _AppendValue(int bitsCount, ulong value)
        {
            _Records.AppendRecord(value, (sbyte)bitsCount);
        }
        /// <summary>
        /// Сделать байтовое выравнивание
        /// </summary>
        protected override void _MakeAlignment(int alignmentBitsCount)
        {
            _Records.AppendRecord(0, (sbyte)alignmentBitsCount);
        }
        /// <summary>
        /// Очистить объект построения бинарной коллекции
        /// </summary>
        /// </summary>
        protected override void _Clear()
        {
            _Records.Clear();
        }
    }
}
