using BinarySerializerLibrary.Attributes;
using BinarySerializerLibrary.BinaryDataHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Serializers.ComplexTypes
{
    public abstract class CollectionBaseTypeSerializer : ComplexBaseTypeSerializer
    {
        public override object? Deserialize(BinaryTypeBaseAttribute attribute, Type objType, ABinaryDataReader reader)
        {
            var collectionType = _GetCollectionElementType(objType);
            var collectionElementType = GetCollectionFieldType(collectionType);
            // Получение атрибута едичного объекта коллекции
            attribute = attribute.CloneAndChange(Enums.BinaryArgumentTypeEnum.Single);

            if (collectionElementType is not null && collectionType is not null)
            {
                // Десераилизация размера коллекции
                var collectionSize = DeserializeCollectionSize(attribute, reader);
                // Создание экземпляра объекта
                var collectionObject = _CreateObjectInstance(objType, collectionType, collectionSize);

                // Десериализация элементов коллекции 
                if (collectionObject != null)
                {
                    // Десерализация составных объектов-полей
                    if (IsComplexType(attribute))
                    {
                        foreach (var index in Enumerable.Range(0, collectionSize))
                        {
                            var collectionElementValue = DeserializeComplexValue(attribute, collectionElementType, reader);

                            _SetCollectionElement(collectionObject, objType, collectionElementValue, index);
                        }
                    }
                    // Десерализация атомарных объектов-полей
                    else
                    {
                        foreach (var index in Enumerable.Range(0, collectionSize))
                        {
                            var collectionElementValue = DeserializeAtomicValue(attribute, collectionElementType, reader);

                            _SetCollectionElement(collectionObject, objType, collectionElementValue, index);
                        }
                    }
                }

                return collectionObject;
            }
            else
            {
                return null;
            }
        }

        public override void Serialize(BinaryTypeBaseAttribute attribute, object obj, ABinaryDataWriter builder)
        {
            var collectionElementType = GetCollectionFieldType(_GetCollectionElementType(obj.GetType()));
            // Получение аттрибута едичного объекта коллекции
            attribute = attribute.CloneAndChange(Enums.BinaryArgumentTypeEnum.Single);

            if (collectionElementType != null)
            {
                var collectionSize = _GetCollectionSize(obj);

                // Сериализация размера коллекции
                collectionSize = SerializeCollectionSize(attribute, collectionSize, builder);

                // Сериализация элементов коллекции
                if (collectionSize > 0)
                {
                    // Сериализация в случае, если объект составной
                    if (IsComplexType(attribute))
                    {
                        foreach (var collectionElementValue in _GetCollectionElements(obj))
                        {
                            SerializeComplexValue(attribute, collectionElementValue, builder);
                        }
                    }
                    // Сериализация в случае, если объект представлен атомарным полем
                    else
                    {
                        foreach (var collectionElementValue in _GetCollectionElements(obj))
                        {
                            SerializeAtomicValue(attribute, collectionElementType, collectionElementValue, builder);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Получить тип элемента коллекции
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected abstract Type? _GetCollectionElementType(Type type);
        /// <summary>
        /// Получить размер коллекции
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected abstract int _GetCollectionSize(object obj);
        /// <summary>
        /// Получить перечисление элементов коллекции
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected abstract IEnumerable<object?> _GetCollectionElements(object obj);
        /// <summary>
        /// Создать экзепляр объекта коллекции
        /// </summary>
        /// <param name="collectionType"></param>
        /// <param name="elementType"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        protected abstract object? _CreateObjectInstance(Type collectionType, Type elementType, int collectionSize);
        /// <summary>
        /// Установить значение элемента коллекции
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <param name="index"></param>
        protected abstract void _SetCollectionElement(object obj, Type objectType, object? elementValue, int elementIndex);
    }
}
