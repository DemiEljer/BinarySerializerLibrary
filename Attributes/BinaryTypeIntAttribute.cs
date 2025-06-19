using BinarySerializerLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Attributes
{
    public class BinaryTypeIntAttribute : BinaryTypeBaseAttribute
    {
        public BinaryTypeIntAttribute(int size, BinaryArgumentTypeEnum type = BinaryArgumentTypeEnum.Single) : this(size, BinaryNullableTypeEnum.NotNullable, BinaryAlignmentTypeEnum.NoAlignment, type)
        {
        }

        public BinaryTypeIntAttribute(int size, BinaryNullableTypeEnum nullable, BinaryArgumentTypeEnum type = BinaryArgumentTypeEnum.Single) : this(size, nullable, BinaryAlignmentTypeEnum.NoAlignment, type)
        {
        }

        public BinaryTypeIntAttribute(int size, BinaryAlignmentTypeEnum alignment, BinaryArgumentTypeEnum type = BinaryArgumentTypeEnum.Single) : this(size, BinaryNullableTypeEnum.NotNullable, alignment, type)
        {
        }

        public BinaryTypeIntAttribute(int size, BinaryNullableTypeEnum nullable, BinaryAlignmentTypeEnum alignment, BinaryArgumentTypeEnum type = BinaryArgumentTypeEnum.Single) : base(Math.Clamp(size, 0, 64), type, alignment, nullable)
        {
        }

        public override BinaryTypeBaseAttribute CloneAndChangeType(BinaryArgumentTypeEnum type = BinaryArgumentTypeEnum.Single)
        {
            return new BinaryTypeIntAttribute(FieldSize, Nullable, Alignment, type);
        }
    }
}
