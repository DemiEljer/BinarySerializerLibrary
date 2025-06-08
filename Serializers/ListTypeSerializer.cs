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
            var listElementType = objType.GetGenericArguments().First();
            // Получение аттрибута едичного объекта списка
            attribute = attribute.CloneAndChangeType(Enums.BinaryArgumentTypeEnum.Single);

            if (listElementType != null)
            {
                // Десераилизация размера списка
                var listSize = BaseTypeSerializerMapper.DeserializeValue<Int32>(reader.ReadValue(32, attribute.Alignment), 32);
                // Создание экземпляра объекта
                var listObject = Activator.CreateInstance(objType);

                // Десериализация элементов списка 
                {
                    // Десерализация составных объектов-полей
                    if (ObjectSerializationRecipesMapper.IsComplexType(attribute))
                    {
                        foreach (var index in Enumerable.Range(0, listSize))
                        {
                            var elementValue = ComplexTypeSerializerMapper.DeserializeObject(attribute, listElementType, reader);

                            objType.GetMethod("Add")?.Invoke(listObject, new object?[] { elementValue });
                        }
                    }
                    // Десерализация атомарных объектов-полей
                    else
                    {
                        foreach (var index in Enumerable.Range(0, listSize))
                        {
                            var elementValue = BaseTypeSerializerMapper.DeserializeValue(listElementType, reader.ReadValue(attribute.FieldSize, attribute.Alignment), attribute.FieldSize);

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
            // В случае, если бы передан нулевой объект, то помечаем массив как пустой
            if (obj is null)
            {
                // Сериализация размер списка
                builder.AppendBitValue(32, BaseTypeSerializerMapper.SerializeValue<Int32>(0, 32), attribute.Alignment);

                return;
            }

            var listElementType = obj.GetType().GetGenericArguments().First();
            // Получение аттрибута едичного объекта списка
            attribute = attribute.CloneAndChangeType(Enums.BinaryArgumentTypeEnum.Single);

            if (listElementType != null)
            {
                int listSize = ((IList)obj).Count;

                // Сериализация размер списка
                builder.AppendBitValue(32, BaseTypeSerializerMapper.SerializeValue<Int32>(listSize, 32), attribute.Alignment);

                // Сериализация элементов списка
                if (listSize > 0)
                {
                    // Сериализация в случае, если объект составной
                    if (ObjectSerializationRecipesMapper.IsComplexType(attribute))
                    {
                        foreach (var arrayValue in ((IList)obj))
                        {
                            ComplexTypeSerializerMapper.SerializeObject(attribute, arrayValue, builder);
                        }
                    }
                    // Сериализация в случае, если объект представлен атомарным полем
                    else
                    {
                        foreach (var arrayValue in ((IList)obj))
                        {
                            builder.AppendBitValue(attribute.FieldSize, BaseTypeSerializerMapper.SerializeValue(listElementType, arrayValue, attribute.FieldSize), attribute.Alignment);
                        }
                    }
                }
            }
        }
    }
}
