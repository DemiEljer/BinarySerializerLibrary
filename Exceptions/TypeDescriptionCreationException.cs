using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Exceptions
{
    public class TypeDescriptionCreationException : Exception
    {
        public Type? ObjectType { get; }

        public TypeDescriptionCreationException(Type? objectType) : base($"Ошибка создания описания типа {objectType}") 
        {
            ObjectType = objectType;
        }
    }
}
