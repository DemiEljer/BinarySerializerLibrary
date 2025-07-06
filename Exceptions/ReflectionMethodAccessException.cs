using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Exceptions
{
    public class ReflectionMethodAccessException : Exception
    {
        public Type ObjectType { get; }

        public MethodInfo? Method { get; }

        public ReflectionMethodAccessException(string message, Type objectType, MethodInfo? method) : base($"{message} :: {objectType.Name} -> {method.Name}") 
        {
            ObjectType = objectType;
            Method = method;
        }
    }
}
