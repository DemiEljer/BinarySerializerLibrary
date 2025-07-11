using BinarySerializerLibrary.Base;
using BinarySerializerLibrary.BinaryDataHandlers.Helpers;
using BinarySerializerLibrary.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.ObjectSerializationRecipes
{
    public static class ObjectTypeMapper
    {
        /// <summary>
        /// Коллекция типов объектов
        /// </summary>
        private static ConcurrentBag<ObjectTypeCodePair> _ObjectTypeCodePairs { get; } = new();
        /// <summary>
        /// Доступ к коллекции пар типов объектов
        /// </summary>
        public static IEnumerable<ObjectTypeCodePair> ObjectTypes => _ObjectTypeCodePairs;
        /// <summary>
        /// Количество пар типов объектов
        /// </summary>
        public static int ObjectTypesCount => _ObjectTypeCodePairs.Count;
        /// <summary>
        /// Зарегистрировать тип
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="typeCode"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void RegisterType(Type objectType, int typeCode)
        {
            if (objectType is null)
            {
                throw new ArgumentNullException(nameof(objectType));
            }
            // Приготовление рецепта для обработки типа
            ObjectSerializationRecipesMapper.GetOrCreateRecipe(objectType);
            // Валидация максимального значения кода типа
            if (typeCode > ObjectTypeParameterHelpers.MaxTypeCode
                || typeCode < 0)
            {
                throw new UnavailablePairOfTypeAndCodeException(objectType, typeCode);
            }

            var foundObjectPairByType = _GetObjectTypePairByType(objectType);
            var foundObjectPairByCode = _GetObjectTypePairByCode(typeCode);
            // В случае, если код типа занят и осуществляется попытка назначения другому типу, выбрасывается ошибка
            if (foundObjectPairByCode != null
                && foundObjectPairByCode != foundObjectPairByType)
            {
                throw new UnavailablePairOfTypeAndCodeException(objectType, typeCode);
            }

            // Проверка, что создается новая пара
            if (foundObjectPairByType is null)
            {
                _ObjectTypeCodePairs.Add(new ObjectTypeCodePair(objectType, typeCode));
            }
            else
            {
                foundObjectPairByType.SetTypeCode(typeCode);
            }
        }
        /// <summary>
        /// Получить тип, соответствующий коду
        /// </summary>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        public static Type? GetType(int typeCode) => _GetObjectTypePairByCode(typeCode)?.ObjectType;
        /// <summary>
        /// Получить код, соответствующий типу
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static int GetCode(Type? objectType)
        {
            var objectPair = _GetObjectTypePairByType(objectType);

            if (objectPair is not null)
            {
                return objectPair.ObjectTypeCode;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// Получить пару тип-код по коду типа
        /// </summary>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        private static ObjectTypeCodePair? _GetObjectTypePairByCode(int typeCode)
        {
            if (typeCode == 0)
            {
                return null;
            }

            return _ObjectTypeCodePairs.FirstOrDefault(pair => pair.ObjectTypeCode == typeCode);
        }
        /// <summary>
        /// Получить пару тип-код по типу
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        private static ObjectTypeCodePair? _GetObjectTypePairByType(Type? objectType) => _ObjectTypeCodePairs.FirstOrDefault(pair => pair.ObjectType == objectType);
    }
}
