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
        /// ��������� ������� �������� ��� ��������� ��������
        /// </summary>
        private static ConcurrentDictionary<Type, ObjectSerializationRecipe> _ObjectsRecipes { get; } = new();
        /// <summary>
        /// �������� ������ ��������� �������
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static ObjectSerializationRecipe GetRecipe(Type objectType)
        {
            if (!_ObjectsRecipes.ContainsKey(objectType))
            {
                _ObjectsRecipes.TryAdd(objectType, ObjectSerializationRecipeFabric.CreateRecipe(objectType));
            }
            return _ObjectsRecipes[objectType];
        }
    }
}
