using BinarySerializerLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Attributes
{
    public class BinaryTypeBoolAttribute : BinaryTypeBaseAttribute
    {
        public BinaryTypeBoolAttribute(BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : this(NullableTypeEnum.NotNullable, BinaryAlignmentTypeEnum.NoAlignment, fieldType)
        {
        }

        public BinaryTypeBoolAttribute(NullableTypeEnum nullable, BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : this(nullable, BinaryAlignmentTypeEnum.NoAlignment, fieldType)
        {
        }

        public BinaryTypeBoolAttribute(BinaryAlignmentTypeEnum alignment, BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : this(NullableTypeEnum.NotNullable, alignment, fieldType)
        {
        }

        public BinaryTypeBoolAttribute(NullableTypeEnum nullable, BinaryAlignmentTypeEnum alignment, BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : base(1, fieldType, alignment, nullable)
        {
        }

        public override BinaryTypeBaseAttribute CloneAndChangeType(BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single)
        {
            return new BinaryTypeBoolAttribute(Nullable, Alignment, fieldType);
        }
    }
}
