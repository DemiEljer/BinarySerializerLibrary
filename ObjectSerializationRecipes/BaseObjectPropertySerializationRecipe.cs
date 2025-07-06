using BinarySerializerLibrary.Attributes;
using BinarySerializerLibrary.Base;
using BinarySerializerLibrary.BinaryDataHandlers;
using BinarySerializerLibrary.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.ObjectSerializationRecipes
{
    public abstract class BaseObjectPropertySerializationRecipe
    {
        /// <summary>
        /// Обрабатываемое свойство
        /// </summary>
        public PropertyInfo Property { get; }
        /// <summary>
        /// Тип поля
        /// </summary>
        public Type PropertyType { get; }
        /// <summary>
        /// Атрибут сериализации поля
        /// </summary>
        public BinaryTypeBaseAttribute PropertyAttribute { get; }
        /// <summary>
        /// Делегат чтения значения свойства
        /// </summary>
        private Func<object, object?> _PropertyGetter { get; }
        /// <summary>
        /// Делегат установки значения свойства
        /// </summary>
        private Action<object, object?> _PropertySetter { get; }

        public BaseObjectPropertySerializationRecipe(Type objectType, PropertyInfo property, Type propertyType, BinaryTypeBaseAttribute propertyAttribute)
        {
            Property = property;
            PropertyType = propertyType;
            PropertyAttribute = propertyAttribute;

            _PropertyGetter = MethodAccessDelegateCompiler.CreatePropertyGetterDelegate(objectType, Property.GetMethod);
            _PropertySetter = MethodAccessDelegateCompiler.CreatePropertySetterDelegate(objectType, Property.SetMethod);
        }
        /// <summary>
        /// Сереализация свойства
        /// </summary>
        public void Serialization(object serializingObject, ABinaryDataWriter builder)
        {
            var propertyValue = _PropertyGetter.Invoke(serializingObject);//Property.GetValue(serializingObject);

            _Serialization(propertyValue, builder);
        }
        /// <summary>
        /// Сереализация свойства
        /// </summary>
        public abstract void _Serialization(object? propertyValue, ABinaryDataWriter builder);
        /// <summary>
        /// Десерииализация свойства
        /// </summary>
        /// <returns></returns>
        public void Deserialization(object deserializingObject, ABinaryDataReader reader)
        {
            var propertyValue = _Deserialization(reader);

            _PropertySetter.Invoke(deserializingObject, propertyValue);//Property.SetValue(deserializingObject, propertyValue);
        }
        /// <summary>
        /// Десерииализация свойства
        /// </summary>
        public abstract object? _Deserialization(ABinaryDataReader reader);
    }
}
