using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Exceptions
{
    public class TypeDescriptionApplyingException : Exception
    {
        public ObjectTypeDescription TypeDescription { get; }

        public TypeDescriptionApplyingException(ObjectTypeDescription typeDescription) : base($"Ошибка применения описания типа {typeDescription.TypeName}")
        {
            TypeDescription = typeDescription;
        }
    }
}
