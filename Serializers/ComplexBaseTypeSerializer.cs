using BinarySerializerLibrary.Attributes;
using BinarySerializerLibrary.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Serializers
{
    public abstract class ComplexBaseTypeSerializer
    {
        /// <summary>
        /// Сериализовать объект
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="obj"></param>
        /// <param name="builder"></param>
        public abstract void Serialize(BinaryTypeBaseAttribute attribute, object? obj, BinaryArrayBuilder builder);
        /// <summary>
        /// Десериализовать объект
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="objType"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public abstract object? Deserialize(BinaryTypeBaseAttribute attribute, Type objType, BinaryArrayReader reader);
    }
}
