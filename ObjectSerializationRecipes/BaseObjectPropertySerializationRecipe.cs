using BinarySerializerLibrary.Attributes;
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
        public PropertyInfo FieldProperty { get; }
        /// <summary>
        /// Тип поля
        /// </summary>
        public Type FieldType { get; }
        /// <summary>
        /// Аттрибут сериализации поля
        /// </summary>
        public BinaryTypeBaseAttribute FieldAttribute { get; }

        public BaseObjectPropertySerializationRecipe(PropertyInfo fieldProperty, Type fieldType, BinaryTypeBaseAttribute fieldAttribute)
        {
            FieldProperty = fieldProperty;
            FieldType = fieldType;
            FieldAttribute = fieldAttribute;
        }
        /// <summary>
        /// Сереализация свойства
        /// </summary>
        public void Serialization(object serializingObject, ABinaryDataWriter builder)
        {
            var propertyValue = FieldProperty.GetValue(serializingObject);

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

            FieldProperty.SetValue(deserializingObject, propertyValue);
        }
        /// <summary>
        /// Десерииализация свойства
        /// </summary>
        public abstract object? _Deserialization(ABinaryDataReader reader);
    }
}
