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
        public BinaryTypeCharAttribute(BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : this(AlignmentTypeEnum.NoAlignment, fieldType)
        {
        }

        public BinaryTypeCharAttribute(AlignmentTypeEnum alignment, BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : base(fieldType, alignment)
        {
            FieldSize = 16;
        }

        public override BinaryTypeBaseAttribute CloneAndChangeType(BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single)
        {
            return new BinaryTypeCharAttribute(Alignment, fieldType);
        }
    }
}
