using BinarySerializerLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Attributes
{
    public class BinaryTypeFloatAttribute : BinaryTypeBaseAttribute
    {
        public BinaryTypeFloatAttribute(BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : this(NullableTypeEnum.NotNullable, AlignmentTypeEnum.NoAlignment, fieldType)
        {
        }

        public BinaryTypeFloatAttribute(NullableTypeEnum nullable, BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : this(nullable, AlignmentTypeEnum.NoAlignment, fieldType)
        {
        }

        public BinaryTypeFloatAttribute(AlignmentTypeEnum alignment, BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : this(NullableTypeEnum.NotNullable, alignment, fieldType)
        {
        }

        public BinaryTypeFloatAttribute(NullableTypeEnum nullable, AlignmentTypeEnum alignment, BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : base(32, fieldType, alignment, nullable)
        {
        }

        public override BinaryTypeBaseAttribute CloneAndChangeType(BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single)
        {
            return new BinaryTypeFloatAttribute(Nullable, Alignment, fieldType);
        }
    }
}
