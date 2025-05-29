using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Serializers
{
    public static class BaseTypeSerializerMapper
    {
        private static Dictionary<Type, BaseTypeSerializer> _Serializers { get; } = new();

        static BaseTypeSerializerMapper()
        {
            _Serializers.Add(typeof(float), new FloatTypeSerializer());
            _Serializers.Add(typeof(double), new DoubleTypeSerializer());
            _Serializers.Add(typeof(byte), new UIntTypeSerializer());
            _Serializers.Add(typeof(UInt16), new UIntTypeSerializer());
            _Serializers.Add(typeof(UInt32), new UIntTypeSerializer());
            _Serializers.Add(typeof(UInt64), new UIntTypeSerializer());
            _Serializers.Add(typeof(sbyte), new IntTypeSerializer());
            _Serializers.Add(typeof(Int16), new IntTypeSerializer());
            _Serializers.Add(typeof(Int32), new IntTypeSerializer());
            _Serializers.Add(typeof(Int64), new IntTypeSerializer());
            _Serializers.Add(typeof(char), new CharTypeSerializer());
            _Serializers.Add(typeof(bool), new BoolTypeSerializer());
        }

        public static BaseTypeSerializer? GetSerializer(Type valueType)
        {
            if (_Serializers.ContainsKey(valueType))
            {
                return _Serializers[valueType];
            }
            else
            {
                return null;
            }
        }

        public static TValue DeserializeValue<TValue>(UInt64 value, int size)
        {
            return (TValue)DeserializeValue(typeof(TValue), value, size);
        }

        public static object DeserializeValue(Type valueType, UInt64 value, int size)
        {
            var serializer = GetSerializer(valueType);

            if (serializer != null)
            {
                return serializer.GetFromBinaryValue(valueType, value, size);
            }
            else
            {
                return 0;
            }
        }

        public static UInt64 SerializeValue<TValue>(TValue value, int size)
        {
            return SerializeValue(typeof(TValue), value, size);
        }

        public static UInt64 SerializeValue(Type valueType, object? value, int size)
        {
            var serializer = GetSerializer(valueType);

            if (serializer != null)
            {
                return serializer.GetBinaryValue(valueType, value, size);
            }
            else
            {
                return 0;
            }
        }
    }
}
