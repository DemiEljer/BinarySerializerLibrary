using BinarySerializerLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Attributes
{
    public class BinaryTypeUIntAttribute : BinaryTypeBaseAttribute
    {
        public BinaryTypeUIntAttribute(int size, BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : this(size, BinaryNullableTypeEnum.NotNullable, BinaryAlignmentTypeEnum.NoAlignment, fieldType)
        {
        }

        public BinaryTypeUIntAttribute(int size, BinaryNullableTypeEnum nullable, BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : this(size, nullable, BinaryAlignmentTypeEnum.NoAlignment, fieldType)
        {
        }

        public BinaryTypeUIntAttribute(int size, BinaryAlignmentTypeEnum alignment, BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : this(size, BinaryNullableTypeEnum.NotNullable, alignment, fieldType)
        {
        }

        public BinaryTypeUIntAttribute(int size, BinaryNullableTypeEnum nullable, BinaryAlignmentTypeEnum alignment, BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : base(Math.Clamp(size, 0, 64), fieldType, alignment, nullable)
        {
        }

        public override BinaryTypeBaseAttribute CloneAndChangeType(BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single)
        {
            return new BinaryTypeUIntAttribute(FieldSize, Nullable, Alignment, fieldType);
        }
    }
}
