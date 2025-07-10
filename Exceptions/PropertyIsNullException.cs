using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Exceptions
{
    public class PropertyIsNullException : Exception
    {
        public PropertyIsNullException() : base("") { }
    }
}
