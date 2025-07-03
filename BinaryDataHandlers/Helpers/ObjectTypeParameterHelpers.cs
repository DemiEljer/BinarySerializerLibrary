using BinarySerializerLibrary.Enums;
using BinarySerializerLibrary.Exceptions;
using BinarySerializerLibrary.Serializers.ComplexTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.BinaryDataHandlers.Helpers
{
    public static class ObjectTypeParameterHelpers
    {
        public const int MaxTypeCode = 0x3FFFFFFF;

        public static void PackObjectType(ABinaryDataWriter? binaryWriter, Type? objectType)
        {
            if (binaryWriter is not null)
            {
                int objectTypeCode = BinarySerializerObjectTypeMapper.GetCode(objectType);

                if (ComplexBaseTypeSerializer.SerializeScalableIntValue(BinaryAlignmentTypeEnum.ByteAlignment, objectTypeCode, binaryWriter) < 0)
                {
                    throw new ObjectTypeCodeIsOutOfMaximumValue();
                }
            }
        }

        public static (Type? objectType, int objectTypeCode) UnpackObjectType(ABinaryDataReader? binaryReader)
        {
            if (binaryReader is not null)
            {
                int objectTypeCode = ComplexBaseTypeSerializer.DeserializeScalableIntValue(BinaryAlignmentTypeEnum.ByteAlignment, binaryReader);

                return (BinarySerializerObjectTypeMapper.GetType(objectTypeCode), objectTypeCode);
            }

            return (null, 0);
        }
    }
}
