using BinarySerializerLibrary.Attributes;
using BinarySerializerLibrary.Base;
using BinarySerializerLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Serializers
{
    public class ComplexTypeSerializerMapper
    {
        private static Dictionary<(Type, BinaryArgumentTypeEnum type), ComplexBaseTypeSerializer> _Serializers { get; } = new();

        static ComplexTypeSerializerMapper()
        {
            _Serializers.Add((typeof(BinaryTypeBoolAttribute), BinaryArgumentTypeEnum.Array), new ArrayTypeSerializer());
            _Serializers.Add((typeof(BinaryTypeCharAttribute), BinaryArgumentTypeEnum.Array), new ArrayTypeSerializer());
            _Serializers.Add((typeof(BinaryTypeDoubleAttribute), BinaryArgumentTypeEnum.Array), new ArrayTypeSerializer());
            _Serializers.Add((typeof(BinaryTypeFloatAttribute), BinaryArgumentTypeEnum.Array), new ArrayTypeSerializer());
            _Serializers.Add((typeof(BinaryTypeIntAttribute), BinaryArgumentTypeEnum.Array), new ArrayTypeSerializer());
            _Serializers.Add((typeof(BinaryTypeUIntAttribute), BinaryArgumentTypeEnum.Array), new ArrayTypeSerializer());
            _Serializers.Add((typeof(BinaryTypeObjectAttribute), BinaryArgumentTypeEnum.Array), new ArrayTypeSerializer());
            _Serializers.Add((typeof(BinaryTypeStringAttribute), BinaryArgumentTypeEnum.Array), new ArrayTypeSerializer());

            _Serializers.Add((typeof(BinaryTypeBoolAttribute), BinaryArgumentTypeEnum.List), new ListTypeSerializer());
            _Serializers.Add((typeof(BinaryTypeCharAttribute), BinaryArgumentTypeEnum.List), new ListTypeSerializer());
            _Serializers.Add((typeof(BinaryTypeDoubleAttribute), BinaryArgumentTypeEnum.List), new ListTypeSerializer());
            _Serializers.Add((typeof(BinaryTypeFloatAttribute), BinaryArgumentTypeEnum.List), new ListTypeSerializer());
            _Serializers.Add((typeof(BinaryTypeIntAttribute), BinaryArgumentTypeEnum.List), new ListTypeSerializer());
            _Serializers.Add((typeof(BinaryTypeUIntAttribute), BinaryArgumentTypeEnum.List), new ListTypeSerializer());
            _Serializers.Add((typeof(BinaryTypeObjectAttribute), BinaryArgumentTypeEnum.List), new ListTypeSerializer());
            _Serializers.Add((typeof(BinaryTypeStringAttribute), BinaryArgumentTypeEnum.List), new ListTypeSerializer());

            _Serializers.Add((typeof(BinaryTypeObjectAttribute), BinaryArgumentTypeEnum.Single), new ObjectTypeSerializer());
            _Serializers.Add((typeof(BinaryTypeStringAttribute), BinaryArgumentTypeEnum.Single), new StringTypeSerializer());
        }

        public static ComplexBaseTypeSerializer? GetSerializer(BinaryTypeBaseAttribute fieldAttribute)
        {
            var serializerKey = (fieldAttribute.GetType(), fieldAttribute.Type);

            if (_Serializers.ContainsKey(serializerKey))
            {
                return _Serializers[serializerKey];
            }
            else
            {
                return null;
            }
        }

        public static TValue? DeserializeObject<TValue>(BinaryTypeBaseAttribute attribute, Type objType, BinaryArrayReader reader)
        {
            return (TValue?)DeserializeObject(attribute, objType, reader);
        }

        public static object? DeserializeObject(BinaryTypeBaseAttribute attribute, Type objType, BinaryArrayReader reader)
        {
            var serializer = GetSerializer(attribute);

            if (serializer != null)
            {
                return serializer.Deserialize(attribute, objType, reader);
            }
            else
            {
                return null;
            }
        }

        public static void SerializeObject(BinaryTypeBaseAttribute attribute, object obj, BinaryArrayBuilder builder)
        {
            var serializer = GetSerializer(attribute);

            if (serializer != null)
            {
                serializer.Serialize(attribute, obj, builder);
            }
        }
    }
}
