using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Exceptions
{
    public class UnavailablePairOfTypeAndCodeException : Exception
    {
        public Type? Type { get; }

        public int TypeCode { get; }

        public UnavailablePairOfTypeAndCodeException(Type? type, int code) : base($"Недопустимое сочетание типа и кода типа объекта: {type} и {code}")
        {
            Type = type;
            TypeCode = code;
        }
    }
}
