using BinarySerializerLibrary.Base;
using BinarySerializerLibrary.Enums;
using BinarySerializerLibrary.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.BinaryDataHandlers
{
    public class CompositeBinaryDataReader : ABinaryDataReader
    {
        public class CompositeLogic
        {
            /// <summary>
            /// Коллекция объектов чтения бинарных коллекций
            /// </summary>
            private List<ABinaryDataReader> _Readers { get; } = new();
            /// <summary>
            /// Коллекция объектов чтения бинарных коллекций
            /// </summary>
            public IEnumerable<ABinaryDataReader> Readers => _Readers;
            /// <summary>
            /// Абсолютный индекс первого бита текущего обрабатываемого объекта чтения бинарной коллекции
            /// </summary>
            public long CurrentReaderFirstBitIndex { get; private set; }
            /// <summary>
            /// Абсолютный индекс последнего бита текущего обрабатываемого объекта чтения бинарной коллекции
            /// </summary>
            public long CurrentReaderLastBitIndex
            {
                get
                {
                    if (CurrentReader is not null)
                    {
                        return CurrentReaderFirstBitIndex + CurrentReader.BitsCount;
                    }
                    else
                    {
                        return CurrentReaderFirstBitIndex;
                    }
                }
            }
            /// <summary>
            /// Индекс текущего подчиненного объекта чтения
            /// </summary>
            private int _ReaderIndex { get; set; } = 0;
            /// <summary>
            /// Итоговый индекс бита в обобщённой коллекции
            /// </summary>
            public long ResultBitIndex
            {
                get
                {
                    if (CurrentReader is not null)
                    {
                        return CurrentReaderFirstBitIndex + CurrentReader.BitIndex;
                    }
                    else
                    {
                        return CurrentReaderFirstBitIndex;
                    }
                }
            }
            /// <summary>
            /// Общее количество байт во всех подчиненных коллекциях
            /// </summary>
            public long TotalBytesCount => _Readers.Sum(reader => reader.BytesCount);
            /// <summary>
            /// Общее количество бит во всех подчиненных коллекциях
            /// </summary>
            public long TotalBitsCount => TotalBytesCount * 8;
            /// <summary>
            /// Текущий объект чтения
            /// </summary>
            public ABinaryDataReader? CurrentReader
            {
                get
                {
                    if (_ReaderIndex < _Readers.Count)
                    {
                        return _Readers[_ReaderIndex];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            /// <summary>
            /// Установить битовый индекс в обобщенной бинарной коллекции 
            /// </summary>
            /// <param name="bitIndex"></param>
            public void SetBitIndex(long bitIndex)
            {
                bitIndex = Math.Clamp(bitIndex, 0, TotalBitsCount);

                if (TotalBitsCount > 0)
                {
                    Reset();

                    foreach (var index in Enumerable.Range(0, _Readers.Count))
                    {
                        if (bitIndex >= CurrentReaderFirstBitIndex && bitIndex < CurrentReaderLastBitIndex)
                        {
                            break;
                        }
                        else
                        {
                            _MoveNextReader();
                        }
                    }

                    if (CurrentReader is not null)
                    {
                        CurrentReader.SetBitIndex(bitIndex - CurrentReaderFirstBitIndex);
                    }
                }
            }
            /// <summary>
            /// Добавить подчиненный объект чтения
            /// </summary>
            /// <param name="reader"></param>
            public void AppendReader(ABinaryDataReader reader)
            {
                if (!_Readers.Contains(reader))
                {
                    reader.ResetBitIndex();
                    _Readers.Add(reader);
                }
            }
            /// <summary>
            /// Прочитать значение
            /// </summary>
            /// <param name="bitSize"></param>
            /// <returns></returns>
            public UInt64 ReadValue(int bitSize)
            {
                int leftBitsToRead = bitSize;
                UInt64 resultValue = 0;

                while (leftBitsToRead > 0)
                {
                    if (CurrentReader is not null)
                    {
                        long currentReaderLeftBits = CurrentReader.BitsCount - CurrentReader.BitIndex;

                        if (currentReaderLeftBits >= leftBitsToRead)
                        {
                            resultValue |= CurrentReader.ReadValue(leftBitsToRead, BinaryAlignmentTypeEnum.NoAlignment) << (bitSize - leftBitsToRead);

                            leftBitsToRead = 0;
                        }
                        else
                        {
                            resultValue |= CurrentReader.ReadValue((int)currentReaderLeftBits, BinaryAlignmentTypeEnum.NoAlignment) << (bitSize - leftBitsToRead);

                            leftBitsToRead -= (int)currentReaderLeftBits;
                        }

                        if (CurrentReader.IsEndOfCollection)
                        {
                            _MoveNextReader();
                        }
                    }
                    else
                    {
                        throw new ByteArrayReaderIsOverException();
                    }
                }

                return resultValue;
            }
            /// <summary>
            /// Сделать выравнивание
            /// </summary>
            /// <param name="alignment"></param>
            public void MakeAlignment(BinaryAlignmentTypeEnum alignment)
            {
                int shiftingBits = 0;

                switch (alignment)
                {
                    case BinaryAlignmentTypeEnum.ByteAlignment:
                        shiftingBits = ByteVectorHandler.GetDeltaBits(ResultBitIndex);
                        break;
                }

                while (shiftingBits > 0)
                {
                    if (CurrentReader is not null)
                    {
                        var currentReaderLeftBits = CurrentReader.BitsCount - CurrentReader.BitIndex;

                        if (currentReaderLeftBits >= shiftingBits)
                        {
                            CurrentReader.SetBitIndex(CurrentReader.BitIndex + shiftingBits);

                            shiftingBits = 0;

                            if (CurrentReader.IsEndOfCollection)
                            {
                                _MoveNextReader();
                            }
                        }
                        else
                        {
                            shiftingBits -= (int)currentReaderLeftBits;

                            _MoveNextReader();
                        }
                    }
                    else
                    {
                        throw new ByteArrayReaderIsOverException();
                    }
                }
            }
            /// <summary>
            /// Перейти к следующему подчиненному объекту чтения бинарной коллекции
            /// </summary>
            private void _MoveNextReader()
            {
                if (CurrentReader is not null)
                {
                    CurrentReader.ResetBitIndex();

                    CurrentReaderFirstBitIndex += CurrentReader.BitsCount;
                    _ReaderIndex++;
                }
                else
                {
                    throw new ByteArrayReaderIsOverException();
                }
            }
            /// <summary>
            /// Удалить подчиненные объекты чтения бинарных коллекций, которые уже были обработаны
            /// </summary>
            public void ClearPassedReaders()
            {
                if (_ReaderIndex < _Readers.Count)
                {
                    foreach (var index in Enumerable.Range(0, _ReaderIndex))
                    {
                        _Readers.RemoveAt(0);
                    }
                    CurrentReaderFirstBitIndex = 0;
                    _ReaderIndex = 0;
                }
                else
                {
                    Clear();
                }
            }
            /// <summary>
            /// Удалить все подчиненные бинарные коллекции
            /// </summary>
            public void Clear()
            {
                Reset();
                _Readers.Clear();
            }

            public void Reset()
            {
                CurrentReaderFirstBitIndex = 0;
                _ReaderIndex = 0;

                foreach (var reader in _Readers)
                {
                    reader.ResetBitIndex();
                }
            }
        }
        /// <summary>
        /// Итератор по коллекции объектов чтения бинарных коллекций
        /// </summary>
        private CompositeLogic _Logic { get; } = new();
        /// <summary>
        /// Коллекция подчиненных объектов чтения бинарных коллекций
        /// </summary>
        public IEnumerable<ABinaryDataReader> Readers => _Logic.Readers;
        /// <summary>
        /// Текущий индекс бита
        /// </summary>
        public override long BitIndex
        {
            get => _Logic.ResultBitIndex;
            protected set => _Logic.SetBitIndex(value);
        }
        /// <summary>
        /// Количество байт
        /// </summary>
        public override long BytesCount => _Logic.TotalBytesCount;
        /// <summary>
        /// Прочитать значение
        /// </summary>
        /// <param name="bitSize"></param>
        /// <param name="alignment"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override ulong ReadValue(int bitSize, BinaryAlignmentTypeEnum alignment)
        {
            // Применение выравнивания
            MakeAlignment(alignment);

            long nextBitIndex = BitIndex + bitSize;
            // Проверка на достижение конца массива
            if (IsEndOfCollection || nextBitIndex > BitsCount)
            {
                throw new ByteArrayReaderIsOverException();
            }

            return _Logic.ReadValue(bitSize);
        }
        /// <summary>
        /// Сделать выравнивание
        /// </summary>
        /// <param name="alignment"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void MakeAlignment(BinaryAlignmentTypeEnum alignment)
        {
            _Logic.MakeAlignment(alignment);
        }
        /// <summary>
        /// Добавить объект чтения
        /// </summary>
        public void AppendReader(ABinaryDataReader reader)
        {
            if (reader is not null)
            {
                _Logic.AppendReader(reader);
            }
        }
        /// <summary>
        /// Сбросить индекс битов
        /// </summary>
        public override void ResetBitIndex()
        {
            _Logic.Reset();
        }
        /// <summary>
        /// Очистить объект чтения бинарной коллекции
        /// </summary>
        public override void Clear()
        {
            _Logic.Clear();
        }
        /// <summary>
        /// Удалить уже пройденные подчиненные обработчики чтения
        /// </summary>
        public void RemovePassedReaders()
        {
            _Logic.ClearPassedReaders();
        }
    }
}
