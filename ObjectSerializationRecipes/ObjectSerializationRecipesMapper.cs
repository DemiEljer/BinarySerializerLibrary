using BinarySerializerLibrary.Attributes;
using BinarySerializerLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.ObjectSerializationRecipes
{
    public static class ObjectSerializationRecipesMapper
    {
        private static Dictionary<Type, ObjectSerializationRecipe> _ObjectsRecipes { get; } = new();

        public static ObjectSerializationRecipe GetRecipe(Type objectType)
        {
            if (!_ObjectsRecipes.ContainsKey(objectType))
            {
                _ObjectsRecipes.Add(objectType, ObjectSerializationRecipeFabric.CreateReciepe(objectType));
            }
            return _ObjectsRecipes[objectType];
        }
    }
}
