using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Exceptions
{
    public class ByteArrayIsOutOfMaximumLength : Exception
    {
        public ByteArrayIsOutOfMaximumLength() : base("Превышен максимальный размер бинарного массива") { }
    }
}
