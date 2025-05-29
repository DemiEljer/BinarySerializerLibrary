using BinarySerializerLibrary.Attributes;
using BinarySerializerLibrary.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.ObjectSerializationRecipes
{
    public abstract class BaseObjectFieldSerializationRecipe
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

        public BaseObjectFieldSerializationRecipe(PropertyInfo fieldProperty, BinaryTypeBaseAttribute fieldAttribute)
        {
            FieldProperty = fieldProperty;
            FieldType = FieldProperty.PropertyType;
            FieldAttribute = fieldAttribute;
        }

        /// <summary>
        /// Сереализация поля
        /// </summary>
        public abstract void Serialization(object serializingObject, BinaryArrayBuilder builder);
        /// <summary>
        /// Десерииализация поля
        /// </summary>
        /// <returns></returns>
        public abstract void Deserialization(object deserializingObject, BinaryArrayReader reader);
    }
}
