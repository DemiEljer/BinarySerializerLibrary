using BinarySerializerLibrary.Base;
using BinarySerializerLibrary.BinaryDataHandlers.Helpers;
using BinarySerializerLibrary.Exceptions;
using BinarySerializerLibrary.ObjectSerializationRecipes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary
{
    public static class BinarySerializerObjectTypeMapper
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
            ObjectSerializationRecipesMapper.GetRecipe(objectType);

            if (_CheckCodeTypePairIsAvailable(objectType, typeCode))
            {
                _ObjectTypeCodePairs.Add(new ObjectTypeCodePair(objectType, typeCode));
            }
        }
        /// <summary>
        /// Проверить, что код и тип соответствуют правилам
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        /// <exception cref="UnavailablePairOfTypeAndCodeException"></exception>
        private static bool _CheckCodeTypePairIsAvailable(Type objectType, int typeCode)
        {
            var objectPair = _ObjectTypeCodePairs.FirstOrDefault(pair => pair.ObjectType == objectType || pair.ObjectTypeCode == typeCode);

            // В случае, если обрабатывается точно такой же объект, как и до этого
            if (objectPair?.ObjectType == objectType
                && objectPair?.ObjectTypeCode == typeCode)
            {
                return false;
            }
            else if (objectPair is not null
                     || objectType is null
                     || typeCode == 0
                     || typeCode > ObjectTypeParameterHelpers.MaxTypeCode)
            {
                throw new UnavailablePairOfTypeAndCodeException(objectType, typeCode);
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// Получить тип, соответствующий коду
        /// </summary>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        public static Type? GetType(int typeCode)
        {
            var objectPair = _ObjectTypeCodePairs.FirstOrDefault(pair => pair.ObjectTypeCode == typeCode);

            return objectPair?.ObjectType;
        }
        /// <summary>
        /// Получить код, соотвутсвующий типу
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static int GetCode(Type? objectType)
        {
            var objectPair = _ObjectTypeCodePairs.FirstOrDefault(pair => pair.ObjectType == objectType);

            if (objectPair is not null)
            {
                return objectPair.ObjectTypeCode;
            }
            else
            {
                return 0;
            }
        }
    }
}
