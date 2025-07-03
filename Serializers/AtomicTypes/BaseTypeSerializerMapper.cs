using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Serializers.AtomicTypes
{
    public static class BaseTypeSerializerMapper
    {
        private static Dictionary<Type, BaseTypeSerializer> _Serializers { get; } = new();

        static BaseTypeSerializerMapper()
        {
            _Serializers.Add(typeof(float), new FloatTypeSerializer());
            _Serializers.Add(typeof(double), new DoubleTypeSerializer());
            _Serializers.Add(typeof(byte), new UInt8TypeSerializer());
            _Serializers.Add(typeof(ushort), new UInt16TypeSerializer());
            _Serializers.Add(typeof(uint), new UInt32TypeSerializer());
            _Serializers.Add(typeof(ulong), new UInt64TypeSerializer());
            _Serializers.Add(typeof(sbyte), new Int8TypeSerializer());
            _Serializers.Add(typeof(short), new Int16TypeSerializer());
            _Serializers.Add(typeof(int), new Int32TypeSerializer());
            _Serializers.Add(typeof(long), new Int64TypeSerializer());
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

        public static TValue DeserializeValue<TValue>(ulong value, int size)
        {
            return (TValue)DeserializeValue(typeof(TValue), value, size);
        }

        public static object DeserializeValue(Type valueType, ulong value, int size)
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

        public static ulong SerializeValue<TValue>(TValue value, int size)
            where TValue : struct
        {
            return SerializeValue(typeof(TValue), value, size);
        }

        public static ulong SerializeValue(Type valueType, object value, int size)
        {
            var serializer = GetSerializer(valueType);

            if (serializer is not null)
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
