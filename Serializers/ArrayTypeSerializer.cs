using BinarySerializerLibrary.Attributes;
using BinarySerializerLibrary.Base;
using BinarySerializerLibrary.ObjectSerializationRecipes;
using System;
using System.Collections.Generic;
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
            var arrayElementType = objType.GetElementType();
            // Получение аттрибута едичного объекта массива
            attribute = attribute.CloneAndChangeType(Enums.BinaryArgumentTypeEnum.Single);

            if (arrayElementType != null)
            {
                // Десераилизация размера массива
                var arraySize = BaseTypeSerializerMapper.DeserializeValue<Int32>(reader.ReadValue(32), 32);

                var arrayObject = Array.CreateInstance(arrayElementType, arraySize);
                // Десериализация элементов массива 
                {
                    // Десерализация составных объектов-полей
                    if (ObjectSerializationRecipesMapper.IsComplexType(attribute))
                    {
                        foreach (var index in Enumerable.Range(0, arraySize))
                        {
                            var elementValue = ComplexTypeSerializerMapper.DeserializeObject(attribute, arrayElementType, reader);

                            ((Array)arrayObject).SetValue(elementValue, index);
                        }
                    }
                    // Десерализация атомарных объектов-полей
                    else
                    {
                        foreach (var index in Enumerable.Range(0, arraySize))
                        {
                            var elementValue = BaseTypeSerializerMapper.DeserializeValue(arrayElementType, reader.ReadValue(attribute.FieldSize), attribute.FieldSize);

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
            // В случае, если бы передан нулевой объект, то помечаем массив как пустой
            if (obj is null)
            {
                // Сериализация размер массива
                builder.AppendBitValue(32, BaseTypeSerializerMapper.SerializeValue<Int32>(0, 32));

                return;
            }

            var arrayElementType = obj.GetType().GetElementType();
            // Получение аттрибута едичного объекта массива
            attribute = attribute.CloneAndChangeType(Enums.BinaryArgumentTypeEnum.Single);

            if (arrayElementType != null)
            {
                int arraySize = ((Array)obj).Length;

                // Сериализация размер массива
                builder.AppendBitValue(32, BaseTypeSerializerMapper.SerializeValue<Int32>(arraySize, 32));

                // Сериализация элементов вектора
                if (arraySize > 0)
                {
                    // Сериализация в случае, если объект составной
                    if (ObjectSerializationRecipesMapper.IsComplexType(attribute))
                    {
                        foreach (var arrayValue in ((Array)obj))
                        {
                            ComplexTypeSerializerMapper.SerializeObject(attribute, arrayValue, builder);
                        }
                    }
                    // Сериализация в случае, если объект представлен атомарным полем
                    else
                    {
                        foreach (var arrayValue in ((Array)obj))
                        {
                            builder.AppendBitValue(attribute.FieldSize, BaseTypeSerializerMapper.SerializeValue(arrayElementType, arrayValue, attribute.FieldSize));
                        }
                    }
                }
            }
        }
    }
}
