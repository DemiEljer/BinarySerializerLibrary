using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Exceptions
{
    public class UnresolvedSerializerOfPropertyException : Exception
    {
        public PropertyInfo Property { get; }

        public UnresolvedSerializerOfPropertyException(PropertyInfo property) : base($"Невозможно определить сериализватор для свойства {property.Name} с типом {property.PropertyType}")
        {
            Property = property;
        }
    }
}
