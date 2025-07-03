using BinarySerializerLibrary.Attributes;
using BinarySerializerLibrary.Base;
using BinarySerializerLibrary.ObjectSerializationRecipes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Serializers.ComplexTypes
{
    public class ListTypeSerializer : CollectionBaseTypeSerializer
    {
        /// <summary>
        /// Получить тип элемента коллекции
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected override Type? _GetCollectionElementType(Type type) => type?.GetGenericArguments().First();
        /// <summary>
        /// Получить размер коллекции
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected override int _GetCollectionSize(object obj) => ((IList)obj).Count;
        /// <summary>
        /// Получить перечисление элементов коллекции
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected override IEnumerable<object?> _GetCollectionElements(object obj)
        {
            foreach (var listValue in (IList)obj)
            {
                yield return listValue;
            }
        }
        /// <summary>
        /// Создать экзепляр объекта коллекции
        /// </summary>
        /// <param name="collectionType"></param>
        /// <param name="elementType"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        protected override object? _CreateObjectInstance(Type collectionType, Type elementType, int collectionSize)
            => Activator.CreateInstance(collectionType);
        /// <summary>
        /// Установить значение элемента коллекции
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <param name="index"></param>
        protected override void _SetCollectionElement(object obj, Type objectType, object? elementValue, int elementIndex)
        {
            objectType.GetMethod("Add")?.Invoke(obj, new object?[] { elementValue });
        }
    }
}
