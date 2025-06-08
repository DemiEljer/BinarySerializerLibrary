using BinarySerializerLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Attributes
{
    public class BinaryTypeObjectAttribute : BinaryTypeBaseAttribute
    {
        public BinaryTypeObjectAttribute(BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : this(AlignmentTypeEnum.NoAlignment, fieldType)
        {
        }

        public BinaryTypeObjectAttribute(AlignmentTypeEnum alignment, BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : base(fieldType, alignment)
        {
        }

        public override BinaryTypeBaseAttribute CloneAndChangeType(BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single)
        {
            return new BinaryTypeObjectAttribute(Alignment, fieldType);
        }
    }
}
