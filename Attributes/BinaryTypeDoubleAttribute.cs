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
        public BinaryTypeDoubleAttribute(BinaryArgumentTypeEnum type = BinaryArgumentTypeEnum.Single) : this(BinaryNullableTypeEnum.NotNullable, BinaryAlignmentTypeEnum.NoAlignment, type)
        {
        }

        public BinaryTypeDoubleAttribute(BinaryNullableTypeEnum nullable, BinaryArgumentTypeEnum type = BinaryArgumentTypeEnum.Single) : this(nullable, BinaryAlignmentTypeEnum.NoAlignment, type)
        {
        }

        public BinaryTypeDoubleAttribute(BinaryAlignmentTypeEnum alignment, BinaryArgumentTypeEnum type = BinaryArgumentTypeEnum.Single) : this(BinaryNullableTypeEnum.NotNullable, alignment, type)
        {
        }

        public BinaryTypeDoubleAttribute(BinaryNullableTypeEnum nullable, BinaryAlignmentTypeEnum alignment, BinaryArgumentTypeEnum type = BinaryArgumentTypeEnum.Single) : base(64, type, alignment, nullable)
        {
        }

        public override BinaryTypeBaseAttribute CloneAndChange(BinaryArgumentTypeEnum? type = null, BinaryAlignmentTypeEnum? alignment = null)
        {
            return new BinaryTypeDoubleAttribute
            (
                Nullable
                , alignment is null ? Alignment : (BinaryAlignmentTypeEnum)alignment
                , type is null ? Type : (BinaryArgumentTypeEnum)type
            );
        }
    }
}
