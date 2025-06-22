using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Exceptions
{
    public class TypeTooSmallForValueException : Exception
    {
        public TypeTooSmallForValueException(int size, UInt64 value) : base($"Значение {value}[UInt64] не помещается в количество бит {size}") { }

        public TypeTooSmallForValueException(int size, Int64 value) : base($"Значение {value}[Int64] не помещается в количество бит {size}") { }
    }
}
