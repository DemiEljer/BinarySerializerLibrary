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
        /// ������ ����
        /// </summary>
        public int FieldSize { get; } = 0;
        /// <summary>
        /// ��� ����
        /// </summary>
        public BinaryArgumentTypeEnum Type { get; }
        /// <summary>
        /// ������������ ����
        /// </summary>
        public BinaryAlignmentTypeEnum Alignment { get; }
        /// <summary>
        /// ��������� ����������� �������������� �������� � null
        /// </summary>
        public BinaryNullableTypeEnum Nullable { get; }

        public BinaryTypeBaseAttribute(int fieldSize, BinaryArgumentTypeEnum type, BinaryAlignmentTypeEnum alignment, BinaryNullableTypeEnum nullable)
        {
            FieldSize = fieldSize;
            Type = type;
            Alignment = alignment;
            Nullable = nullable;
        }

        public abstract BinaryTypeBaseAttribute CloneAndChangeType(BinaryArgumentTypeEnum type = BinaryArgumentTypeEnum.Single);
    }
}
