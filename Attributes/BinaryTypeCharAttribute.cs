using BinarySerializerLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Attributes
{
    public class BinaryTypeCharAttribute : BinaryTypeBaseAttribute
    {
        public BinaryTypeCharAttribute(BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : this(NullableTypeEnum.NotNullable, BinaryAlignmentTypeEnum.NoAlignment, fieldType)
        {
        }

        public BinaryTypeCharAttribute(NullableTypeEnum nullable, BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : this(nullable, BinaryAlignmentTypeEnum.NoAlignment, fieldType)
        {
        }

        public BinaryTypeCharAttribute(BinaryAlignmentTypeEnum alignment, BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : this(NullableTypeEnum.NotNullable, alignment, fieldType)
        {
        }

        public BinaryTypeCharAttribute(NullableTypeEnum nullable, BinaryAlignmentTypeEnum alignment, BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : base(16, fieldType, alignment, nullable)
        {
        }

        public override BinaryTypeBaseAttribute CloneAndChangeType(BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single)
        {
            return new BinaryTypeCharAttribute(Nullable, Alignment, fieldType);
        }
    }
}
