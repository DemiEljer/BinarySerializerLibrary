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
        /// Размер свойства
        /// </summary>
        public int Size { get; } = 0;
        /// <summary>
        /// Тип свойства
        /// </summary>
        public BinaryArgumentTypeEnum Type { get; }
        /// <summary>
        /// Выравнивание свойства
        /// </summary>
        public BinaryAlignmentTypeEnum Alignment { get; }
        /// <summary>
        /// Может ли свойство принимать значение null
        /// </summary>
        public BinaryNullableTypeEnum Nullable { get; }

        public BinaryTypeBaseAttribute(int size, BinaryArgumentTypeEnum type, BinaryAlignmentTypeEnum alignment, BinaryNullableTypeEnum nullable)
        {
            Size = size;
            Type = type;
            Alignment = alignment;
            Nullable = nullable;
        }

        public abstract BinaryTypeBaseAttribute CloneAndChange(BinaryArgumentTypeEnum? type = null, BinaryAlignmentTypeEnum? alignment = null);

        public override string ToString() => $"{GetType().Name}({Size}, {Type}, {Alignment}, {Nullable})";
    }
}
