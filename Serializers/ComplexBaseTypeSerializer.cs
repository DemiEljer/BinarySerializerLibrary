using BinarySerializerLibrary.Attributes;
using BinarySerializerLibrary.Base;
using BinarySerializerLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Serializers
{
    public abstract class ComplexBaseTypeSerializer
    {
        /// <summary>
        /// Проверка, что тип является комплексным
        /// </summary>
        /// <param name="fieldAttribute"></param>
        /// <returns></returns>
        public static bool IsComplexType(BinaryTypeBaseAttribute fieldAttribute)
        {
            return fieldAttribute is BinaryTypeObjectAttribute
                   || fieldAttribute is BinaryTypeStringAttribute
                   || fieldAttribute.FieldType != BinaryArgumentTypeEnum.Single;
        }
        /// <summary>
        /// Сериализовать размер коллекции
        /// </summary>
        public static int SerializeCollectionSize(BinaryTypeBaseAttribute attribute, int collectionSize, BinaryArrayBuilder builder)
        {
            if (collectionSize <= 0x3F)
            {
                builder.AppendBitValue(2, BaseTypeSerializerMapper.SerializeValue<byte>(0, 2), attribute.Alignment);
                builder.AppendBitValue(6, BaseTypeSerializerMapper.SerializeValue<UInt32>((UInt32)collectionSize, 6), AlignmentTypeEnum.NoAlignment);

                return collectionSize;
            }
            else if (collectionSize <= 0x3FFF)
            {
                builder.AppendBitValue(2, BaseTypeSerializerMapper.SerializeValue<byte>(1, 2), attribute.Alignment);
                builder.AppendBitValue(14, BaseTypeSerializerMapper.SerializeValue<UInt32>((UInt32)collectionSize, 14), AlignmentTypeEnum.NoAlignment);

                return collectionSize;
            }
            else if (collectionSize <= 0x3FFFFF)
            {
                builder.AppendBitValue(2, BaseTypeSerializerMapper.SerializeValue<byte>(2, 2), attribute.Alignment);
                builder.AppendBitValue(22, BaseTypeSerializerMapper.SerializeValue<UInt32>((UInt32)collectionSize, 22), AlignmentTypeEnum.NoAlignment);

                return collectionSize;
            }
            else if (collectionSize <= 0x3FFFFFFF)
            {
                builder.AppendBitValue(2, BaseTypeSerializerMapper.SerializeValue<byte>(3, 2), attribute.Alignment);
                builder.AppendBitValue(30, BaseTypeSerializerMapper.SerializeValue<UInt32>((UInt32)collectionSize, 30), AlignmentTypeEnum.NoAlignment);

                return collectionSize;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// Десериализовать размер коллекции
        /// </summary>
        /// <returns></returns>
        public static int DeserializeCollectionSize(BinaryTypeBaseAttribute attribute, BinaryArrayReader reader)
        {
            var sizeByteCount = BaseTypeSerializerMapper.DeserializeValue<byte>(reader.ReadValue(2, attribute.Alignment), 2);

            switch (sizeByteCount)
            {
                case 0x00: return (Int32)BaseTypeSerializerMapper.DeserializeValue<UInt32>(reader.ReadValue(6, AlignmentTypeEnum.NoAlignment), 6);
                case 0x01: return (Int32)BaseTypeSerializerMapper.DeserializeValue<UInt32>(reader.ReadValue(14, AlignmentTypeEnum.NoAlignment), 14);
                case 0x02: return (Int32)BaseTypeSerializerMapper.DeserializeValue<UInt32>(reader.ReadValue(22, AlignmentTypeEnum.NoAlignment), 22);
                case 0x03: return (Int32)BaseTypeSerializerMapper.DeserializeValue<UInt32>(reader.ReadValue(30, AlignmentTypeEnum.NoAlignment), 30);
                default: return 0;
            }
        }
        /// <summary>
        /// Сериализовать значение
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        /// <param name="builder"></param>
        public static void SerializeComplexValue(BinaryTypeBaseAttribute attribute, object? value, BinaryArrayBuilder builder)
        {
            ComplexBaseTypeSerializer.CheckNullObjectSerialization(attribute, value, builder, () =>
            {
                ComplexTypeSerializerMapper.SerializeObject(attribute, value, builder);
            });
        }
        /// <summary>
        /// Десериализовать значение
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        /// <param name="builder"></param>
        public static object? DeserializeComplexValue(BinaryTypeBaseAttribute attribute, Type valueType, BinaryArrayReader reader)
        {
            return ComplexBaseTypeSerializer.CheckNullObjectDeserialization(attribute, reader, () =>
            {
                return ComplexTypeSerializerMapper.DeserializeObject(attribute, valueType, reader);
            });
        }
        /// <summary>
        /// Сериализовать значение
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        /// <param name="builder"></param>
        public static void SerializeAtomicValue(BinaryTypeBaseAttribute attribute, Type valueType, object? value, BinaryArrayBuilder builder)
        {
            ComplexBaseTypeSerializer.CheckNullObjectSerialization(attribute, value, builder, () =>
            {
                builder.AppendBitValue(attribute.FieldSize, BaseTypeSerializerMapper.SerializeValue(valueType, value, attribute.FieldSize), attribute.Alignment);
            });
        }
        /// <summary>
        /// Десериализовать значение
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        /// <param name="builder"></param>
        public static object? DeserializeAtomicValue(BinaryTypeBaseAttribute attribute, Type valueType, BinaryArrayReader reader)
        {
            return ComplexBaseTypeSerializer.CheckNullObjectDeserialization(attribute, reader, () =>
            {
                return BaseTypeSerializerMapper.DeserializeValue(valueType, reader.ReadValue(attribute.FieldSize, attribute.Alignment), attribute.FieldSize);
            });
        }
        /// <summary>
        /// Проверить, что объект необходимо сериализовать
        /// </summary>
        /// <returns></returns>
        public static void CheckNullObjectSerialization(BinaryTypeBaseAttribute attribute, object? obj, BinaryArrayBuilder builder, Action objectSerializationHandler)
        {
            if (attribute.Nullable == Enums.NullableTypeEnum.Nullable
                || IsComplexType(attribute))
            {
                if (obj is null)
                {
                    builder.AppendBitValue(1, BaseTypeSerializerMapper.SerializeValue<bool>(false, 1), attribute.Alignment);
                }
                else
                {
                    builder.AppendBitValue(1, BaseTypeSerializerMapper.SerializeValue<bool>(true, 1), attribute.Alignment);

                    objectSerializationHandler.Invoke();
                }
            }
            else
            {
                objectSerializationHandler.Invoke();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static object? CheckNullObjectDeserialization(BinaryTypeBaseAttribute attribute, BinaryArrayReader reader, Func<object?> objectDeserializationHandler)
        {
            if (attribute.Nullable == Enums.NullableTypeEnum.Nullable
                || IsComplexType(attribute))
            {
                var objectExistence = BaseTypeSerializerMapper.DeserializeValue<bool>(reader.ReadValue(1, attribute.Alignment), 1);

                if (objectExistence)
                {
                    return objectDeserializationHandler.Invoke();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return objectDeserializationHandler.Invoke();
            }
        }
        /// <summary>
        /// Получить базовый тип коллекции
        /// </summary>
        /// <param name="collectionType"></param>
        /// <returns></returns>
        public static Type? GetCollectionFieldType(Type? collectionType)
        {
            if (collectionType is null)
            {
                return null;
            }
            else if (collectionType.IsGenericType)
            {
                return collectionType.GenericTypeArguments.First();
            }
            else
            {
                return collectionType;
            }
        }
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
