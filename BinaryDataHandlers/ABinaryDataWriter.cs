using BinarySerializerLibrary.Enums;
using BinarySerializerLibrary.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.BinaryDataHandlers
{
    public abstract class ABinaryDataWriter
    {
        /// <summary>
        /// Реальный размер массива в байтах
        /// </summary>
        public abstract long BytesCount { get; }
        /// <summary>
        /// Реальный размер массива в битах
        /// </summary>
        public long BitsCount => BytesCount * 8;
        /// <summary>
        /// Получить массив байт
        /// </summary>
        /// <returns></returns>
        public abstract byte[] GetByteArray();
        /// <summary>
        /// Добавить байт в начало коллекции
        /// </summary>
        public abstract void AppendByteToHead(byte byteValue);
        /// <summary>
        /// Записать битовое поле
        /// </summary>
        public abstract void AppendValue(int bitsCount, ulong value, BinaryAlignmentTypeEnum alignment);
        /// <summary>
        /// Сделать байтовое выравнивание
        /// </summary>
        public abstract void MakeAlignment(BinaryAlignmentTypeEnum alignment);
        /// <summary>
        /// Очистить объект построения бинарной коллекции
        /// </summary>
        public abstract void Clear();
        /// <summary>
        /// Добавить содержимое другого объекта построения байтового массива и сдвинуться в конец
        /// </summary>
        public abstract void AppenBuilderAndShiftToEnd(ABinaryDataWriter builder);

    }
}
