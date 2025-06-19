using BinarySerializerLibrary.Attributes;
using BinarySerializerLibrary.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.ObjectSerializationRecipes
{
    public static class ObjectSerializationRecipesMapper
    {
        /// <summary>
        /// Словарь готовых рецептов объектов
        /// </summary>
        private static ConcurrentDictionary<Type, ObjectSerializationRecipe> _ValidObjectsRecipes { get; } = new();
        /// <summary>
        /// Список ошибочных типов, в которых нарушена логика выставления атрибутов сериализации
        /// </summary>
        private static ConcurrentBag<Type> _InvalidObjectsRecipes { get; } = new();
        /// <summary>
        /// Получить рецепт обработки объекта
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static ObjectSerializationRecipe GetRecipe(Type objectType)
        {
            if (_InvalidObjectsRecipes.Contains(objectType))
            {
                _ThrowInvalidTypeException();
            }

            if (!_ValidObjectsRecipes.ContainsKey(objectType))
            {
                var recipe = ObjectSerializationRecipeFabric.CreateRecipe(objectType);
                // В случае, если тип содержит ошибки назначения атрибутов свойств, то помещаем его в соответсвующий список
                if (recipe is null)
                {
                    _InvalidObjectsRecipes.Add(objectType);

                    _ThrowInvalidTypeException();
                }
                else
                {
                    _ValidObjectsRecipes.TryAdd(objectType, recipe);
                }
            }
            return _ValidObjectsRecipes[objectType];
        }

        private static void _ThrowInvalidTypeException()
        {
            throw new InvalidOperationException("Объект не подлежит сериализации по причине нарушения правил выставления атрибутов");
        }
    }
}
