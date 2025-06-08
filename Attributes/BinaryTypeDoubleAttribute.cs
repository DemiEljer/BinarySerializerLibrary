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
        public BinaryTypeDoubleAttribute(BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : this(AlignmentTypeEnum.NoAlignment, fieldType)
        {
        }

        public BinaryTypeDoubleAttribute(AlignmentTypeEnum alignment, BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : base(fieldType, alignment)
        {
            FieldSize = 64;
        }

        public override BinaryTypeBaseAttribute CloneAndChangeType(BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single)
        {
            return new BinaryTypeDoubleAttribute(Alignment, fieldType);
        }
    }
}
