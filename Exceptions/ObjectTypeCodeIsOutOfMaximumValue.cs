using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Exceptions
{
    public class ObjectTypeCodeIsOutOfMaximumValue : Exception
    {
        public ObjectTypeCodeIsOutOfMaximumValue() : base("Превышено максимальное значение кода типа объекта") { }
    }
}
