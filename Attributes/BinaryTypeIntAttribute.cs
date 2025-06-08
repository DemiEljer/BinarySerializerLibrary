using BinarySerializerLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Attributes
{
    public class BinaryTypeIntAttribute : BinaryTypeBaseAttribute
    {
        public BinaryTypeIntAttribute(int size, BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : this(size, AlignmentTypeEnum.NoAlignment, fieldType)
        {
        }

        public BinaryTypeIntAttribute(int size, AlignmentTypeEnum alignment, BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : base(fieldType, alignment)
        {
            FieldSize = Math.Clamp(size, 0, 64);
        }

        public override BinaryTypeBaseAttribute CloneAndChangeType(BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single)
        {
            return new BinaryTypeIntAttribute(FieldSize, Alignment, fieldType);
        }
    }
}
