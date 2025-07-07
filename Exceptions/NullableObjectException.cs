using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Exceptions
{
    public class NullableObjectException : Exception
    {
        public NullableObjectException() : base("Сериализуемый объект имеет значение null, а не должен") { }
    }
}
