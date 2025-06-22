using BinarySerializerLibrary.Attributes;
using BinarySerializerLibrary.Enums;
using BinarySerializerLibrary.Exceptions;
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
        private static ConcurrentDictionary<Type, ObjectSerializationRecipe?> _ObjectsRecipes { get; } = new();
        /// <summary>
        /// Получить рецепт обработки объекта
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static ObjectSerializationRecipe GetRecipe(Type objectType)
        {
            if (!_ObjectsRecipes.ContainsKey(objectType))
            {
                _ObjectsRecipes.TryAdd(objectType, ObjectSerializationRecipeFabric.CreateRecipe(objectType));
            }

            var recipe = _ObjectsRecipes[objectType];

            if (recipe is null)
            {
                throw new ObjectTypeVerificationFailedException(objectType);
            }
            else
            {
                return recipe;
            }
        }
        /// <summary>
        /// Проверить верификацию рецепта
        /// </summary>
        /// <param name="objectType"></param>
        public static void CheckRecipeVerification(Type objectType) => GetRecipe(objectType);
    }
}
