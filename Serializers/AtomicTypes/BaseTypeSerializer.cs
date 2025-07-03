using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Serializers.AtomicTypes
{
    public abstract class BaseTypeSerializer
    {
        /// <summary>
        /// Получить тип свойства
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static Type GetPropertyType(PropertyInfo property)
        {
            var propertyType = property.PropertyType;

            if (propertyType.IsGenericType)
            {
                return propertyType.GetGenericArguments().First();
            }
            else
            {
                return propertyType;
            }
        }
        /// <summary>
        /// Преобразование в двоичный вид
        /// </summary>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public abstract ulong GetBinaryValue(Type targetType, object value, int size);
        /// <summary>
        /// Преобразование из двоичного вида
        /// </summary>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public abstract object GetFromBinaryValue(Type valueType, ulong value, int size);
    }
}
