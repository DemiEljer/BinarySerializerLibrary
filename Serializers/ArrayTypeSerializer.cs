using BinarySerializerLibrary.Attributes;
using BinarySerializerLibrary.Base;
using BinarySerializerLibrary.ObjectSerializationRecipes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Serializers
{
    public class ArrayTypeSerializer : ComplexBaseTypeSerializer
    {
        public override object? Deserialize(BinaryTypeBaseAttribute attribute, Type objType, BinaryArrayReader reader)
        {
            var arrayElementType = ComplexBaseTypeSerializer.GetCollectionFieldType(objType.GetElementType());
            // Получение аттрибута едичного объекта массива
            attribute = attribute.CloneAndChangeType(Enums.BinaryArgumentTypeEnum.Single);

            if (arrayElementType != null)
            {
                // Десераилизация размера массива
                var arraySize = ComplexBaseTypeSerializer.DeserializeCollectionSize(attribute, reader);

                var arrayObject = Array.CreateInstance(objType.GetElementType(), arraySize);
                // Десериализация элементов массива 
                {
                    // Десерализация составных объектов-полей
                    if (ComplexBaseTypeSerializer.IsComplexType(attribute))
                    {
                        foreach (var index in Enumerable.Range(0, arraySize))
                        {
                            var elementValue = ComplexBaseTypeSerializer.DeserializeComplexValue(attribute, arrayElementType, reader);

                            ((Array)arrayObject).SetValue(elementValue, index);
                        }
                    }
                    // Десерализация атомарных объектов-полей
                    else
                    {
                        foreach (var index in Enumerable.Range(0, arraySize))
                        {
                            var elementValue = ComplexBaseTypeSerializer.DeserializeAtomicValue(attribute, arrayElementType, reader);

                            ((Array)arrayObject).SetValue(elementValue, index);
                        }
                    }
                }

                return arrayObject;
            }
            else
            {
                return null;
            }
        }

        public override void Serialize(BinaryTypeBaseAttribute attribute, object? obj, BinaryArrayBuilder builder)
        {
            var arrayElementType = ComplexBaseTypeSerializer.GetCollectionFieldType(obj?.GetType().GetElementType());
            // Получение аттрибута едичного объекта массива
            attribute = attribute.CloneAndChangeType(Enums.BinaryArgumentTypeEnum.Single);

            if (arrayElementType != null)
            {
                int arraySize = ((Array)obj).Length;

                // Сериализация размер массива
                arraySize = ComplexBaseTypeSerializer.SerializeCollectionSize(attribute, arraySize, builder);

                // Сериализация элементов вектора
                if (arraySize > 0)
                {
                    // Сериализация в случае, если объект составной
                    if (ComplexBaseTypeSerializer.IsComplexType(attribute))
                    {
                        foreach (var arrayValue in ((Array)obj))
                        {
                            ComplexBaseTypeSerializer.SerializeComplexValue(attribute, arrayValue, builder);
                        }
                    }
                    // Сериализация в случае, если объект представлен атомарным полем
                    else
                    {
                        foreach (var arrayValue in ((Array)obj))
                        {
                            ComplexBaseTypeSerializer.SerializeAtomicValue(attribute, arrayElementType, arrayValue, builder);
                        }
                    }
                }
            }
        }
    }
}
