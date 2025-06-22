using BinarySerializerLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Attributes
{
    public class BinaryTypeStringAttribute : BinaryTypeBaseAttribute
    {
        public BinaryTypeStringAttribute(BinaryArgumentTypeEnum type = BinaryArgumentTypeEnum.Single) : this(BinaryAlignmentTypeEnum.NoAlignment, type)
        {
        }

        public BinaryTypeStringAttribute(BinaryAlignmentTypeEnum alignment, BinaryArgumentTypeEnum type = BinaryArgumentTypeEnum.Single) : base(16, type, alignment, BinaryNullableTypeEnum.Nullable)
        {
        }

        public override BinaryTypeBaseAttribute CloneAndChange(BinaryArgumentTypeEnum? type = null, BinaryAlignmentTypeEnum? alignment = null)
        {
            return new BinaryTypeStringAttribute
            (
                alignment is null ? Alignment : (BinaryAlignmentTypeEnum)alignment
                , type is null ? Type : (BinaryArgumentTypeEnum)type
            );
        }
    }
}
