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
        public BinaryTypeCharAttribute(BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single) : base(fieldType)
        {
            FieldSize = 16;
        }

        public override BinaryTypeBaseAttribute CloneAndChangeType(BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single)
        {
            return new BinaryTypeCharAttribute(fieldType);
        }
    }
}
