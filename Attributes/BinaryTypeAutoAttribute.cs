using BinarySerializerLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Attributes
{
    public class BinaryTypeAutoAttribute : BinaryTypeBaseAttribute
    {
        public BinaryTypeAutoAttribute(BinaryAlignmentTypeEnum alignment = BinaryAlignmentTypeEnum.NoAlignment) : base(0, BinaryArgumentTypeEnum.Single, alignment, BinaryNullableTypeEnum.NotNullable)
        {
        }

        public override BinaryTypeBaseAttribute CloneAndChange(BinaryArgumentTypeEnum? type = null, BinaryAlignmentTypeEnum? alignment = null)
        {
            return new BinaryTypeAutoAttribute();
        }
    }
}
