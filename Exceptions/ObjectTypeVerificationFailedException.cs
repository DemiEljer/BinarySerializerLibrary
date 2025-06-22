using BinarySerializerLibrary.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Exceptions
{
    public class ObjectTypeVerificationFailedException : Exception
    {
        public Type Type { get; }

        public ObjectTypeVerificationFailedException(Type type) : base($"В типе {type.ToString()} нарушены правила назначения атрибутов сериализации или сериализуемые свойства не публичные" +
            $"ИЛИ тип не соответствует требованиям (класс, публичный, помеченный атрибутом BinarySerializableObject, имеет конструктор по умолчанию)")

        {
            Type = type;
        }
    }
}
