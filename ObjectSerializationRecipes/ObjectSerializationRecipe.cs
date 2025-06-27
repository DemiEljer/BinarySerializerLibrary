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
        public BaseObjectPropertySerializationRecipe[] FieldsRecipes { get; }

        public ObjectSerializationRecipe(BaseObjectPropertySerializationRecipe[]? fieldsRecipes)
        {
            if (fieldsRecipes is null)
            {
                FieldsRecipes = Array.Empty<BaseObjectPropertySerializationRecipe>();
            }
            else
            {
                FieldsRecipes = fieldsRecipes;
            }
        }

        public void Deserialization(object deserializingObject, ABinaryDataReader reader)
        {
            foreach (var fieldRecipe in FieldsRecipes)
            {
                fieldRecipe.Deserialization(deserializingObject, reader);
            }
        }

        public void Serialization(object serializingObject, ABinaryDataWriter builder)
        {
            foreach (var fieldRecipe in FieldsRecipes)
            {
                fieldRecipe.Serialization(serializingObject, builder);
            }
        }
    }
}
