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

namespace BinarySerializerLibrary.Serializers.ComplexTypes
{
    public class ArrayTypeSerializer : CollectionBaseTypeSerializer
    {
        /// <summary>
        /// Получить тип элемента коллекции
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected override Type? _GetCollectionElementType(Type type) => type?.GetElementType();
        /// <summary>
        /// Получить размер коллекции
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected override int _GetCollectionSize(object obj) => ((Array)obj).Length;
        /// <summary>
        /// Получить перечисление элементов коллекции
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected override IEnumerable<object?> _GetCollectionElements(object obj)
        {
            foreach (var arrayValue in (Array)obj)
            {
                yield return arrayValue;
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
            => Array.CreateInstance(elementType, collectionSize);
        /// <summary>
        /// Установить значение элемента коллекции
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <param name="index"></param>
        protected override void _SetCollectionElement(object obj, Type objectType, object? elementValue, int elementIndex)
        {
            ((Array)obj).SetValue(elementValue, elementIndex);
        }
    }
}
