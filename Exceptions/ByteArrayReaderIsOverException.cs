using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Exceptions
{
    public class ByteArrayReaderIsOverException : Exception
    {
        public ByteArrayReaderIsOverException() : base("Достигнут конец бинарного массива") { }
    }
}
