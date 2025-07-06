using BinarySerializerLibrary.Attributes;
using BinarySerializerLibrary.BinaryDataHandlers;
using BinarySerializerLibrary.Enums;
using BinarySerializerLibrary.Exceptions;
using BinarySerializerLibrary.Serializers.AtomicTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Serializers.ComplexTypes
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
                   || fieldAttribute.Type != BinaryArgumentTypeEnum.Single;
        }
        /// <summary>
        /// Сериализовать масштабируемое знаковое целочисленное значение
        /// </summary>
        /// <returns></returns>
        public static int SerializeScalableIntValue(BinaryAlignmentTypeEnum alignment, int value, ABinaryDataWriter builder)
        {
            if (value <= 0x3F)
            {
                builder.AppendValue(2, BaseTypeSerializerMapper.SerializeValue<byte>(0, 2), alignment);
                builder.AppendValue(6, BaseTypeSerializerMapper.SerializeValue((uint)value, 6), BinaryAlignmentTypeEnum.NoAlignment);

                return value;
            }
            else if (value <= 0x3FFF)
            {
                builder.AppendValue(2, BaseTypeSerializerMapper.SerializeValue<byte>(1, 2), alignment);
                builder.AppendValue(14, BaseTypeSerializerMapper.SerializeValue((uint)value, 14), BinaryAlignmentTypeEnum.NoAlignment);

                return value;
            }
            else if (value <= 0x3FFFFF)
            {
                builder.AppendValue(2, BaseTypeSerializerMapper.SerializeValue<byte>(2, 2), alignment);
                builder.AppendValue(22, BaseTypeSerializerMapper.SerializeValue((uint)value, 22), BinaryAlignmentTypeEnum.NoAlignment);

                return value;
            }
            else if (value <= 0x3FFFFFFF)
            {
                builder.AppendValue(2, BaseTypeSerializerMapper.SerializeValue<byte>(3, 2), alignment);
                builder.AppendValue(30, BaseTypeSerializerMapper.SerializeValue((uint)value, 30), BinaryAlignmentTypeEnum.NoAlignment);

                return value;
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// Сериализовать размер коллекции
        /// </summary>
        public static int SerializeCollectionSize(BinaryTypeBaseAttribute attribute, int collectionSize, ABinaryDataWriter builder)
        {
            int resultCollectionSize = SerializeScalableIntValue(attribute.Alignment, collectionSize, builder);

            if (resultCollectionSize >= 0)
            {
                return resultCollectionSize;
            }
            else
            {
                throw new CollectionSizeIsTooLargeException(collectionSize, 0x3FFFFFFF);
            }
        }
        /// <summary>
        /// Десериализовать масштабируемое знаковое целочисленное значение
        /// </summary>
        /// <returns></returns>
        public static int DeserializeScalableIntValue(BinaryAlignmentTypeEnum alignment, ABinaryDataReader reader)
        {
            var sizeByteCount = BaseTypeSerializerMapper.DeserializeValue<byte>(reader.ReadValue(2, alignment), 2);

            switch (sizeByteCount)
            {
                case 0x00: return (int)BaseTypeSerializerMapper.DeserializeValue<uint>(reader.ReadValue(6, BinaryAlignmentTypeEnum.NoAlignment), 6);
                case 0x01: return (int)BaseTypeSerializerMapper.DeserializeValue<uint>(reader.ReadValue(14, BinaryAlignmentTypeEnum.NoAlignment), 14);
                case 0x02: return (int)BaseTypeSerializerMapper.DeserializeValue<uint>(reader.ReadValue(22, BinaryAlignmentTypeEnum.NoAlignment), 22);
                case 0x03: return (int)BaseTypeSerializerMapper.DeserializeValue<uint>(reader.ReadValue(30, BinaryAlignmentTypeEnum.NoAlignment), 30);
                default: return -1;
            }
        }
        /// <summary>
        /// Десериализовать размер коллекции
        /// </summary>
        /// <returns></returns>
        public static int DeserializeCollectionSize(BinaryTypeBaseAttribute attribute, ABinaryDataReader reader)
        {
            return DeserializeScalableIntValue(attribute.Alignment, reader);
        }
        /// <summary>
        /// Сериализовать значение
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        /// <param name="builder"></param>
        public static void SerializeComplexValue(ComplexBaseTypeSerializer serializer, BinaryTypeBaseAttribute attribute, object? value, ABinaryDataWriter builder)
        {
            CheckNullObjectSerialization(attribute, value, builder, (_value, _) =>
            {
                serializer.Serialize(attribute, _value, builder);
            });
        }
        /// <summary>
        /// Десериализовать значение
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        /// <param name="builder"></param>
        public static object? DeserializeComplexValue(ComplexBaseTypeSerializer serializer, BinaryTypeBaseAttribute attribute, Type valueType, ABinaryDataReader reader)
        {
            return CheckNullObjectDeserialization(attribute, reader, (_) =>
            {
                return serializer.Deserialize(attribute, valueType, reader);
            });
        }
        /// <summary>
        /// Сериализовать значение
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        /// <param name="builder"></param>
        public static void SerializeComplexValue(BinaryTypeBaseAttribute attribute, object? value, ABinaryDataWriter builder)
        {
            CheckNullObjectSerialization(attribute, value, builder, (_value, _) =>
            {
                ComplexTypeSerializerMapper.SerializeObject(attribute, _value, builder);
            });
        }
        /// <summary>
        /// Десериализовать значение
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        /// <param name="builder"></param>
        public static object? DeserializeComplexValue(BinaryTypeBaseAttribute attribute, Type valueType, ABinaryDataReader reader)
        {
            return CheckNullObjectDeserialization(attribute, reader, (_) =>
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
        public static void SerializeAtomicValue(BaseTypeSerializer serializer, BinaryTypeBaseAttribute attribute, Type valueType, object? value, ABinaryDataWriter builder)
        {
            CheckNullObjectSerialization(attribute, value, builder, (_value, _attribute) =>
            {
                builder.AppendValue(_attribute.Size, serializer.GetBinaryValue(valueType, _value, _attribute.Size), _attribute.Alignment);
            });
        }
        /// <summary>
        /// Десериализовать значение
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        /// <param name="builder"></param>
        public static object? DeserializeAtomicValue(BaseTypeSerializer serializer, BinaryTypeBaseAttribute attribute, Type valueType, ABinaryDataReader reader)
        {
            return CheckNullObjectDeserialization(attribute, reader, (_attribute) =>
            {
                return serializer.GetFromBinaryValue(valueType, reader.ReadValue(_attribute.Size, _attribute.Alignment), _attribute.Size);
            });
        }
        /// <summary>
        /// Сериализовать значение
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        /// <param name="builder"></param>
        public static void SerializeAtomicValue(BinaryTypeBaseAttribute attribute, Type valueType, object? value, ABinaryDataWriter builder)
        {
            CheckNullObjectSerialization(attribute, value, builder, (_value, _attribute) =>
            {
                builder.AppendValue(_attribute.Size, BaseTypeSerializerMapper.SerializeValue(valueType, _value, _attribute.Size), _attribute.Alignment);
            });
        }
        /// <summary>
        /// Десериализовать значение
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        /// <param name="builder"></param>
        public static object? DeserializeAtomicValue(BinaryTypeBaseAttribute attribute, Type valueType, ABinaryDataReader reader)
        {
            return CheckNullObjectDeserialization(attribute, reader, (_attribute) =>
            {
                return BaseTypeSerializerMapper.DeserializeValue(valueType, reader.ReadValue(_attribute.Size, _attribute.Alignment), _attribute.Size);
            });
        }
        /// <summary>
        /// Проверить, что объект необходимо сериализовать
        /// </summary>
        /// <returns></returns>
        public static void CheckNullObjectSerialization(BinaryTypeBaseAttribute attribute, object? obj, ABinaryDataWriter builder, Action<object, BinaryTypeBaseAttribute> objectSerializationHandler)
        {
            if (attribute.Nullable == BinaryNullableTypeEnum.Nullable
                || IsComplexType(attribute))
            {
                if (obj is null)
                {
                    builder.AppendValue(1, BaseTypeSerializerMapper.SerializeValue(false, 1), attribute.Alignment);
                }
                else
                {
                    builder.AppendValue(1, BaseTypeSerializerMapper.SerializeValue(true, 1), attribute.Alignment);

                    objectSerializationHandler.Invoke(obj, attribute.CloneAndChange(null, BinaryAlignmentTypeEnum.NoAlignment));
                }
            }
            else
            {
                if (obj is null)
                {
                    throw new ObjectValueIsNullException();
                }
                objectSerializationHandler.Invoke(obj, attribute);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static object? CheckNullObjectDeserialization(BinaryTypeBaseAttribute attribute, ABinaryDataReader reader, Func<BinaryTypeBaseAttribute, object?> objectDeserializationHandler)
        {
            if (attribute.Nullable == BinaryNullableTypeEnum.Nullable
                || IsComplexType(attribute))
            {
                var objectExistence = BaseTypeSerializerMapper.DeserializeValue<bool>(reader.ReadValue(1, attribute.Alignment), 1);

                if (objectExistence)
                {
                    return objectDeserializationHandler.Invoke(attribute.CloneAndChange(null, BinaryAlignmentTypeEnum.NoAlignment));
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return objectDeserializationHandler.Invoke(attribute);
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
        public abstract void Serialize(BinaryTypeBaseAttribute attribute, object obj, ABinaryDataWriter builder);
        /// <summary>
        /// Десериализовать объект
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="objType"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public abstract object? Deserialize(BinaryTypeBaseAttribute attribute, Type objType, ABinaryDataReader reader);
    }
}
