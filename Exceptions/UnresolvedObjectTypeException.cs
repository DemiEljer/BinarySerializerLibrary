using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Exceptions
{
    public class UnresolvedObjectTypeException : Exception
    {
        public int ObjectTypeCode { get; }

        public UnresolvedObjectTypeException(int objectTypeCode) : base($"Невозможно определить тип обрабатываемого объекта с кодом {objectTypeCode}")
        {
            ObjectTypeCode = objectTypeCode;
        }
    }
}
