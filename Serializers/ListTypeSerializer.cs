using BinarySerializerLibrary.Attributes;
using BinarySerializerLibrary.Base;
using BinarySerializerLibrary.ObjectSerializationRecipes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Serializers
{
    public class ListTypeSerializer : ComplexBaseTypeSerializer
    {
        public override object? Deserialize(BinaryTypeBaseAttribute attribute, Type objType, BinaryArrayReader reader)
        {
            var listElementType = ComplexBaseTypeSerializer.GetCollectionFieldType(objType.GetGenericArguments().First());
            // Получение аттрибута едичного объекта списка
            attribute = attribute.CloneAndChangeType(Enums.BinaryArgumentTypeEnum.Single);

            if (listElementType != null)
            {
                // Десераилизация размера списка
                var listSize = ComplexBaseTypeSerializer.DeserializeCollectionSize(attribute, reader);
                // Создание экземпляра объекта
                var listObject = Activator.CreateInstance(objType);

                // Десериализация элементов списка 
                {
                    // Десерализация составных объектов-полей
                    if (ComplexBaseTypeSerializer.IsComplexType(attribute))
                    {
                        foreach (var index in Enumerable.Range(0, listSize))
                        {
                            var elementValue = ComplexBaseTypeSerializer.DeserializeComplexValue(attribute, listElementType, reader);

                            objType.GetMethod("Add")?.Invoke(listObject, new object?[] { elementValue });
                        }
                    }
                    // Десерализация атомарных объектов-полей
                    else
                    {
                        foreach (var index in Enumerable.Range(0, listSize))
                        {
                            var elementValue = ComplexBaseTypeSerializer.DeserializeAtomicValue(attribute, listElementType, reader);

                            objType.GetMethod("Add")?.Invoke(listObject, new object?[] { elementValue });
                        }
                    }
                }

                return listObject;
            }
            else
            {
                return null;
            }
        }

        public override void Serialize(BinaryTypeBaseAttribute attribute, object? obj, BinaryArrayBuilder builder)
        {
            var listElementType = ComplexBaseTypeSerializer.GetCollectionFieldType(obj?.GetType().GetGenericArguments().First());
            // Получение аттрибута едичного объекта списка
            attribute = attribute.CloneAndChangeType(Enums.BinaryArgumentTypeEnum.Single);

            if (listElementType != null)
            {
                int listSize = ((IList)obj).Count;

                // Сериализация размер списка
                ComplexBaseTypeSerializer.SerializeCollectionSize(attribute, listSize, builder);

                // Сериализация элементов списка
                if (listSize > 0)
                {
                    // Сериализация в случае, если объект составной
                    if (ComplexBaseTypeSerializer.IsComplexType(attribute))
                    {
                        foreach (var listValue in ((IList)obj))
                        {
                            ComplexBaseTypeSerializer.CheckNullObjectSerialization(attribute, listValue, builder, () =>
                            {
                                ComplexTypeSerializerMapper.SerializeObject(attribute, listValue, builder);
                            });
                        }
                    }
                    // Сериализация в случае, если объект представлен атомарным полем
                    else
                    {
                        foreach (var listValue in ((IList)obj))
                        {
                            ComplexBaseTypeSerializer.SerializeAtomicValue(attribute, listElementType, listValue, builder);
                        }
                    }
                }
            }
        }
    }
}
