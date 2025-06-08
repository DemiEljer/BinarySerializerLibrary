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
        public BinaryTypeBoolAttribute(BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : this(AlignmentTypeEnum.NoAlignment, fieldType)
        {
        }

        public BinaryTypeBoolAttribute(AlignmentTypeEnum alignment, BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : base(fieldType, alignment)
        {
            FieldSize = 1;
        }

        public override BinaryTypeBaseAttribute CloneAndChangeType(BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single)
        {
            return new BinaryTypeBoolAttribute(Alignment, fieldType);
        }
    }
}
