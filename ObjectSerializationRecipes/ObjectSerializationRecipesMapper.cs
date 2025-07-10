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
        /// Коллекция готовых рецептов объектов
        /// </summary>
        public static IEnumerable<ObjectSerializationRecipe> ObjectsRecipes => _ObjectsRecipes
            .Where(recipePair => recipePair.Value is not null)
            .Select(recipePair => recipePair.Value)
            .Cast<ObjectSerializationRecipe>();

        private static ObjectSerializationRecipeFabric _ObjectRecipesFabric { get; } = new();

        static ObjectSerializationRecipesMapper()
        {
            _ObjectRecipesFabric.ObjectTypeHasBeenDetectedEvent += (type) => GetOrCreateRecipe(type);
        }
        /// <summary>
        /// Создать рецепт обработки объекта
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static ObjectSerializationRecipe GetOrCreateRecipe(Type objectType)
        {
            if (!_ObjectsRecipes.ContainsKey(objectType))
            {
                _ObjectsRecipes.TryAdd(objectType, _ObjectRecipesFabric.CreateRecipe(objectType));
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
        /// Добавить рецепт обработки объекта
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="recipe"></param>
        /// <returns></returns>
        /// <exception cref="ObjectTypeVerificationFailedException"></exception>
        public static void AddRecipe(Type objectType, ObjectSerializationRecipe recipe)
        {
            if (recipe is null || !_ObjectRecipesFabric.VerifyRecipe(objectType, recipe))
            {
                throw new ObjectTypeVerificationFailedException(objectType);
            }
            // При использовании данного метода рецепт будет добавлен в любом случае
            if (!_ObjectsRecipes.ContainsKey(objectType))
            {
                _ObjectsRecipes.TryAdd(objectType, recipe);
            }
            else
            {
                _ObjectsRecipes[objectType] = recipe;
            }
        }
        /// <summary>
        /// Добавить рецепт обработки объекта
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static ObjectSerializationRecipeBuilder AddRecipe(Type objectType)
        {
            return new ObjectSerializationRecipeBuilder(objectType);
        }
        /// <summary>
        /// Получить рецепт обработки объекта
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static ObjectSerializationRecipe? GetRecipe(Type objectType)
        {
            if (_ObjectsRecipes.ContainsKey(objectType))
            {
                return _ObjectsRecipes[objectType];
            }
            return null;
        }
        /// <summary>
        /// Проверить верификацию рецепта
        /// </summary>
        /// <param name="objectType"></param>
        public static void CheckRecipeVerification(Type objectType) => GetOrCreateRecipe(objectType);
        /// <summary>
        /// Получить доступ к фабрике рецептов
        /// </summary>
        /// <returns></returns>
        public static ObjectSerializationRecipeFabric GetFabric() => _ObjectRecipesFabric;
    }
}
