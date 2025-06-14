using BinarySerializerLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Attributes
{
    public class BinaryTypeStringAttribute : BinaryTypeCharAttribute
    {
        public BinaryTypeStringAttribute(BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : this(BinaryAlignmentTypeEnum.NoAlignment, fieldType)
        {
        }

        public BinaryTypeStringAttribute(BinaryAlignmentTypeEnum alignment, BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : base(NullableTypeEnum.Nullable, alignment, fieldType)
        {
        }

        public override BinaryTypeBaseAttribute CloneAndChangeType(BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single)
        {
            return new BinaryTypeStringAttribute(Alignment, fieldType);
        }
    }
}
