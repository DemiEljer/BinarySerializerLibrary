using BinarySerializerLibrary.Attributes;
using BinarySerializerLibrary.BinaryDataHandlers;
using BinarySerializerLibrary.Enums;
using BinarySerializerLibrary.ObjectSerializationRecipes;
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
            // Проверка, что есть данные для чтения 
            if (binaryReader.IsEndOfArray)
            {
                return null;
            }
            // Проверяем размер читаемого объекта
            {
                var serializedObjectSize = BinaryDataLengthParameterHelpers.UnpackOriginObjectBinaryCollectionSizeWithShifting(binaryReader);
                // Если размер прочитать невозможно или сериализованный объект пустой, то возвращаем null
                if (serializedObjectSize is null
                    || serializedObjectSize == 0
                    || binaryReader.ByteLength < (binaryReader.ByteIndex + serializedObjectSize))
                {
                    return null;
                }
            }

            var resultObject = ComplexBaseTypeSerializer.DeserializeComplexValue(_Attribute, objectType, binaryReader);
            // Применение выравнивания для обеспечения согласованности при последовательном десериализации нескольких объектов
            binaryReader.MakeAlignment(BinaryAlignmentTypeEnum.ByteAlignment);

            return resultObject;
        }
    }
}
