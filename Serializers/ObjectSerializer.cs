using BinarySerializerLibrary.Attributes;
using BinarySerializerLibrary.BinaryDataHandlers;
using BinarySerializerLibrary.BinaryDataHandlers.Helpers;
using BinarySerializerLibrary.Enums;
using BinarySerializerLibrary.Exceptions;
using BinarySerializerLibrary.ObjectSerializationRecipes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Serializers
{
    public static class ObjectSerializer
    {
        private static BinaryTypeObjectAttribute _Attribute { get; } = new BinaryTypeObjectAttribute(BinaryAlignmentTypeEnum.ByteAlignment);

        public static void SerializeObject<TObject>(ABinaryDataWriter binaryBuilder, TObject? obj)
            where TObject : class
        {
            SerializeObject(binaryBuilder, obj as object);
        }

        public static void SerializeObject(ABinaryDataWriter binaryBuilder, object? obj)
        {
            if (obj is not null)
            {
                // Верификация типа объекта
                ObjectSerializationRecipesMapper.CheckRecipeVerification(obj.GetType());
                // Упаковка типа объекта
                ObjectTypeParameterHelpers.PackObjectType(binaryBuilder, obj.GetType());

                ComplexBaseTypeSerializer.SerializeComplexValue(_Attribute, obj, binaryBuilder);
            }

            BinaryDataLengthParameterHelpers.PackBinaryCollectionSize(binaryBuilder);
        }

        public static TObject? DeserializeObject<TObject>(ABinaryDataReader binaryReader)
            where TObject : class
        {
            return DeserializeObject(binaryReader, typeof(TObject)) as TObject;
        }

        public static object? DeserializeObject(ABinaryDataReader binaryReader, Type objectType)
        {
            // Верификация типа объекта
            ObjectSerializationRecipesMapper.CheckRecipeVerification(objectType);

            if (_CheckBinaryDataReaderReasons(binaryReader))
            {
                // Распаковка типа объекта (в данном сценарии он не учитывается)
                ObjectTypeParameterHelpers.UnpackObjectType(binaryReader);

                return _DeserializeObject(binaryReader, objectType);
            }
            else
            {
                return null;
            }
        }

        public static object? AutoDeserializeObject(ABinaryDataReader binaryReader)
        {
            if (_CheckBinaryDataReaderReasons(binaryReader))
            {
                // Распаковка типа объекта
                var objectTypePair = ObjectTypeParameterHelpers.UnpackObjectType(binaryReader);

                if (objectTypePair.objectType is null)
                {
                    throw new UnresolvedObjectTypeException(objectTypePair.objectTypeCode);
                }

                return _DeserializeObject(binaryReader, objectTypePair.objectType);
            }
            else
            {
                return null;
            }
        }

        private static bool _CheckBinaryDataReaderReasons(ABinaryDataReader binaryReader)
        {
            // Проверка, что есть данные для чтения 
            if (binaryReader.IsEndOfCollection)
            {
                return false;
            }
            // Проверяем размер читаемого объекта
            {
                var serializedObjectSize = BinaryDataLengthParameterHelpers.UnpackOriginObjectBinaryCollectionSizeWithShifting(binaryReader);
                // Если размер прочитать невозможно или сериализованный объект пустой, то возвращаем null
                if (serializedObjectSize is null
                    || serializedObjectSize == 0
                    || binaryReader.BytesCount < (binaryReader.ByteIndex + serializedObjectSize))
                {
                    return false;
                }
            }

            return true;
        }

        private static object? _DeserializeObject(ABinaryDataReader binaryReader, Type objectType)
        {
            var resultObject = ComplexBaseTypeSerializer.DeserializeComplexValue(_Attribute, objectType, binaryReader);
            // Применение выравнивания для обеспечения согласованности при последовательном десериализации нескольких объектов
            binaryReader.MakeAlignment(BinaryAlignmentTypeEnum.ByteAlignment);

            return resultObject;
        }
    }
}
