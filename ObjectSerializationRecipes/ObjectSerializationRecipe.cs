using BinarySerializerLibrary.Base;
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
        public BaseObjectFieldSerializationRecipe[] FieldsRecipes { get; }

        public ObjectSerializationRecipe(BaseObjectFieldSerializationRecipe[]? fieldsRecipes)
        {
            if (fieldsRecipes is null)
            {
                FieldsRecipes = Array.Empty<BaseObjectFieldSerializationRecipe>();
            }
            else
            {
                FieldsRecipes = fieldsRecipes;
            }
        }

        public void Deserialization(object deserializingObject, BinaryArrayReader reader)
        {
            foreach (var fieldRecipe in FieldsRecipes)
            {
                fieldRecipe.Deserialization(deserializingObject, reader);
            }
        }

        public void Serialization(object serializingObject, BinaryArrayBuilder builder)
        {
            foreach (var fieldRecipe in FieldsRecipes)
            {
                fieldRecipe.Serialization(serializingObject, builder);
            }
        }
    }
}
