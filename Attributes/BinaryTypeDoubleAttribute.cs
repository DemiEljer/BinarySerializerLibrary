using BinarySerializerLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Attributes
{
    public class BinaryTypeDoubleAttribute : BinaryTypeBaseAttribute
    {
        public BinaryTypeDoubleAttribute(BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : this(NullableTypeEnum.NotNullable, BinaryAlignmentTypeEnum.NoAlignment, fieldType)
        {
        }

        public BinaryTypeDoubleAttribute(NullableTypeEnum nullable, BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : this(nullable, BinaryAlignmentTypeEnum.NoAlignment, fieldType)
        {
        }

        public BinaryTypeDoubleAttribute(BinaryAlignmentTypeEnum alignment, BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : this(NullableTypeEnum.NotNullable, alignment, fieldType)
        {
        }

        public BinaryTypeDoubleAttribute(NullableTypeEnum nullable, BinaryAlignmentTypeEnum alignment, BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : base(64, fieldType, alignment, nullable)
        {
        }

        public override BinaryTypeBaseAttribute CloneAndChangeType(BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single)
        {
            return new BinaryTypeDoubleAttribute(Nullable, Alignment, fieldType);
        }
    }
}
