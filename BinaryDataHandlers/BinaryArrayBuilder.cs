using BinarySerializerLibrary.Base;
using BinarySerializerLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.BinaryDataHandlers
{
    public class BinaryArrayBuilder : ABinaryArrayBuilder
    {
        #region SubClasses

        public abstract class BaseBinaryRecord
        {
            /// <summary>
            /// Размер бинарного поля
            /// </summary>
            public sbyte BinarySize { get; }

            public BaseBinaryRecord(sbyte binarySize)
            {
                BinarySize = binarySize;
            }
            /// <summary>
            /// Получить бинарное значение поля
            /// </summary>
            /// <returns></returns>
            public abstract ulong GetValue();

            public static BaseBinaryRecord CreateRecord(UInt64 value, sbyte binarySize)
            {
                if (binarySize <= 8)
                {
                    return new UInt8ValueRecord((byte)value, binarySize);
                }
                else if(binarySize <= 16)
                {
                    return new UInt16ValueRecord((UInt16)value, binarySize);
                }
                else if (binarySize <= 32)
                {
                    return new UInt32ValueRecord((UInt32)value, binarySize);
                }
                else if (binarySize <= 64)
                {
                    return new UInt64ValueRecord((UInt64)value, binarySize);
                }

                throw new Exception();
            }
        }

        public class EmptyValueRecord : BaseBinaryRecord
        {
            public EmptyValueRecord(sbyte binarySize) : base(binarySize)
            {
            }

            public override ulong GetValue() => 0;
        }

        public class UInt8ValueRecord : BaseBinaryRecord
        {
            /// <summary>
            /// Бинарное значение полу
            /// </summary>
            public byte BinaryValue { get; }

            public UInt8ValueRecord(byte binaryValue, sbyte binarySize) : base(binarySize)
            {
                BinaryValue = binaryValue;
            }

            public override ulong GetValue() => BinaryValue;
        }

        public class UInt16ValueRecord : BaseBinaryRecord
        {
            /// <summary>
            /// Бинарное значение полу
            /// </summary>
            public UInt16 BinaryValue { get; }

            public UInt16ValueRecord(UInt16 binaryValue, sbyte binarySize) : base(binarySize)
            {
                BinaryValue = binaryValue;
            }

            public override ulong GetValue() => BinaryValue;
        }

        public class UInt32ValueRecord : BaseBinaryRecord
        {
            /// <summary>
            /// Бинарное значение полу
            /// </summary>
            public UInt32 BinaryValue { get; }

            public UInt32ValueRecord(UInt32 binaryValue, sbyte binarySize) : base(binarySize)
            {
                BinaryValue = binaryValue;
            }

            public override ulong GetValue() => BinaryValue;
        }

        public class UInt64ValueRecord : BaseBinaryRecord
        {
            /// <summary>
            /// Бинарное значение полу
            /// </summary>
            public UInt64 BinaryValue { get; }

            public UInt64ValueRecord(UInt64 binaryValue, sbyte binarySize) : base(binarySize)
            {
                BinaryValue = binaryValue;
            }

            public override ulong GetValue() => BinaryValue;
        }

        #endregion SubClasses
        
        /// <summary>
        /// Коллекция сериализуемых записей
        /// </summary>
        private LinkedList<BaseBinaryRecord> _Records { get; } = new();
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
            foreach (var record in _Records)
            {
                ByteVectorHandler.SetVectorParamValue(data, record.BinarySize, bitPosition, record.GetValue());
                bitPosition += record.BinarySize;
            }

            return data;
        }
        /// <summary>
        /// Добавить содержимое другого объекта построения байтового массива и сдвинуться в конец
        /// </summary>
        protected override void _AppenBuilderAndShiftToEnd(ABinaryDataWriter builder)
        {
            if (builder is BinaryArrayBuilder)
            {
                foreach (var record in ((BinaryArrayBuilder)builder)._Records)
                {
                    _Records.AddLast(record);
                }
            }
            else
            {
                foreach (var byteValue in builder.GetByteArray())
                {
                    _Records.AddLast(new UInt8ValueRecord(byteValue, 8));
                }
            }
        }
        /// <summary>
        /// Добавить байт в начало коллекции
        /// </summary>
        protected override void _AppendByteToHead(byte byteValue)
        {
            _Records.AddFirst(new UInt8ValueRecord(byteValue, 8));
        }
        /// <summary>
        /// Записать битовое поле
        /// </summary>
        protected override void _AppendValue(int bitsCount, ulong value)
        {
            _Records.AddLast(BaseBinaryRecord.CreateRecord(value, (sbyte)bitsCount));
        }
        /// <summary>
        /// Сделать байтовое выравнивание
        /// </summary>
        protected override void _MakeAlignment(int alignmentBitsCount)
        {
            _Records.AddLast(new EmptyValueRecord((sbyte)alignmentBitsCount));
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
