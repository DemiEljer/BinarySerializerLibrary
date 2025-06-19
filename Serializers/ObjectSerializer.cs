using BinarySerializerLibrary.Attributes;
using BinarySerializerLibrary.Base;
using BinarySerializerLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Serializers
{
    public static class ObjectSerializer
    {
        private static BinaryTypeObjectAttribute _Attribute { get; } = new BinaryTypeObjectAttribute(BinaryAlignmentTypeEnum.ByteAlignment);

        public static void SerializeObject<TObject>(BinaryArrayBuilder binaryBuilder, TObject? obj)
            where TObject : class
        {
            ComplexBaseTypeSerializer.SerializeComplexValue(_Attribute, obj, binaryBuilder);
        }

        public static TObject? DeserializeObject<TObject>(BinaryArrayReader binaryReader)
            where TObject : class
        {
            return ComplexBaseTypeSerializer.DeserializeComplexValue(_Attribute, typeof(TObject), binaryReader) as TObject;
        }
    }
}
