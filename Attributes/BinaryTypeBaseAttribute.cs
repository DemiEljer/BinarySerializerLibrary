using BinarySerializerLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Attributes
{
    public abstract class BinaryTypeBaseAttribute : Attribute
    {
        /// <summary>
        /// Размер поля
        /// </summary>
        public int FieldSize { get; } = 0;
        /// <summary>
        /// Тип поля
        /// </summary>
        public BinaryArgumentTypeEnum FieldType { get; }
        /// <summary>
        /// Выравнивание поля
        /// </summary>
        public AlignmentTypeEnum Alignment { get; }
        /// <summary>
        /// Обработка возможности преобразования значения в null
        /// </summary>
        public NullableTypeEnum Nullable { get; }

        public BinaryTypeBaseAttribute(int fieldSize, BinaryArgumentTypeEnum fieldType, AlignmentTypeEnum alignment, NullableTypeEnum nullable)
        {
            FieldSize = fieldSize;
            FieldType = fieldType;
            Alignment = alignment;
            Nullable = nullable;
        }

        public abstract BinaryTypeBaseAttribute CloneAndChangeType(BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single);
    }
}
