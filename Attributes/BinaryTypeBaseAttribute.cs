using BinarySerializerLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Attributes
{
    public abstract class BinaryTypeBaseAttribute : Attribute
    {
        public int FieldSize { get; protected set; } = 0;

        public BinaryArgumentTypeEnum FieldType { get; protected set; }

        public BinaryTypeBaseAttribute(BinaryArgumentTypeEnum fieldType)
        {
            FieldType = fieldType;
        }

        public abstract BinaryTypeBaseAttribute CloneAndChangeType(BinaryArgumentTypeEnum fieldType = BinaryArgumentTypeEnum.Single);
    }
}
