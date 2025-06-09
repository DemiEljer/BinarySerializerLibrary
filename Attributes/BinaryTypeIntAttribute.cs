using BinarySerializerLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Attributes
{
    public class BinaryTypeIntAttribute : BinaryTypeBaseAttribute
    {
        public BinaryTypeIntAttribute(int size, BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : this(size, NullableTypeEnum.NotNullable, AlignmentTypeEnum.NoAlignment, fieldType)
        {
        }

        public BinaryTypeIntAttribute(int size, NullableTypeEnum nullable, BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : this(size, nullable, AlignmentTypeEnum.NoAlignment, fieldType)
        {
        }

        public BinaryTypeIntAttribute(int size, AlignmentTypeEnum alignment, BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : this(size, NullableTypeEnum.NotNullable, alignment, fieldType)
        {
        }

        public BinaryTypeIntAttribute(int size, NullableTypeEnum nullable, AlignmentTypeEnum alignment, BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : base(Math.Clamp(size, 0, 64), fieldType, alignment, nullable)
        {
        }

        public override BinaryTypeBaseAttribute CloneAndChangeType(BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single)
        {
            return new BinaryTypeIntAttribute(FieldSize, Nullable, Alignment, fieldType);
        }
    }
}
