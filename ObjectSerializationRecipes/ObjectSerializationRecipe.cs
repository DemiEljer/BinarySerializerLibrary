using BinarySerializerLibrary.BinaryDataHandlers;
using BinarySerializerLibrary.Serializers;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.ObjectSerializationRecipes
{
    public class ObjectSerializationRecipe
    {
        public Type ObjectType { get; }

        public BaseObjectPropertySerializationRecipe[] PropertiesRecipes { get; private set; }

        public ObjectSerializationRecipe(Type objectType, BaseObjectPropertySerializationRecipe[]? fieldsRecipes)
        {
            ObjectType = objectType;

            if (fieldsRecipes is null)
            {
                PropertiesRecipes = Array.Empty<BaseObjectPropertySerializationRecipe>();
            }
            else
            {
                PropertiesRecipes = fieldsRecipes;
            }
        }

        public ObjectTypeDescription GetDescription() => new ObjectTypeDescription(this);

        public bool TryToResetPropertiesSequence(string[]? propertiesNames)
        {
            if (propertiesNames is null
                || PropertiesRecipes.Length != propertiesNames.Length)
            {
                return false;
            }

            var newPropertiesRecipesSequence = new BaseObjectPropertySerializationRecipe[propertiesNames.Length];

            foreach (var propertyNameIndex in Enumerable.Range(0, newPropertiesRecipesSequence.Length))
            {
                var propertyRecipe = PropertiesRecipes.FirstOrDefault(_propertyRecipe => _propertyRecipe.Property.Name == propertiesNames[propertyNameIndex]);
                // В случае, если свойство не было обнаружено в списке, то выводим ошибку
                if (propertyRecipe is not null)
                {
                    newPropertiesRecipesSequence[propertyNameIndex] = propertyRecipe;
                }
                else
                {
                    return false;
                }
            }
            // В случае, если был проинициализирован не весь список рецептов свойств 
            if (newPropertiesRecipesSequence.FirstOrDefault(recipe => recipe is null) != null)
            {
                return false;
            }

            PropertiesRecipes = newPropertiesRecipesSequence;

            return true;
        }

        public void Deserialization(object deserializingObject, ABinaryDataReader reader)
        {
            foreach (var fieldRecipe in PropertiesRecipes)
            {
                fieldRecipe.Deserialization(deserializingObject, reader);
            }
        }

        public void Serialization(object serializingObject, ABinaryDataWriter builder)
        {
            foreach (var fieldRecipe in PropertiesRecipes)
            {
                fieldRecipe.Serialization(serializingObject, builder);
            }
        }
    }
}
