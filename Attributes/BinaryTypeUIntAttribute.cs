using BinarySerializerLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Attributes
{
    public class BinaryTypeUIntAttribute : BinaryTypeBaseAttribute
    {
        public BinaryTypeUIntAttribute(int size, BinaryArgumentTypeEnum type = BinaryArgumentTypeEnum.Single) : this(size, BinaryNullableTypeEnum.NotNullable, BinaryAlignmentTypeEnum.NoAlignment, type)
        {
        }

        public BinaryTypeUIntAttribute(int size, BinaryNullableTypeEnum nullable, BinaryArgumentTypeEnum type = BinaryArgumentTypeEnum.Single) : this(size, nullable, BinaryAlignmentTypeEnum.NoAlignment, type)
        {
        }

        public BinaryTypeUIntAttribute(int size, BinaryAlignmentTypeEnum alignment, BinaryArgumentTypeEnum type = BinaryArgumentTypeEnum.Single) : this(size, BinaryNullableTypeEnum.NotNullable, alignment, type)
        {
        }

        public BinaryTypeUIntAttribute(int size, BinaryNullableTypeEnum nullable, BinaryAlignmentTypeEnum alignment, BinaryArgumentTypeEnum type = BinaryArgumentTypeEnum.Single) : base(Math.Max(size, 0), type, alignment, nullable)
        {
        }

        public override BinaryTypeBaseAttribute CloneAndChange(BinaryArgumentTypeEnum? type = null, BinaryAlignmentTypeEnum? alignment = null)
        {
            return new BinaryTypeUIntAttribute
            (
                Size
                , Nullable
                , alignment is null ? Alignment : (BinaryAlignmentTypeEnum)alignment
                , type is null ? Type : (BinaryArgumentTypeEnum)type
            );
        }
    }
}
