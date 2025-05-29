using BinarySerializerLibrary.Attributes;
using BinarySerializerLibrary.Base;
using BinarySerializerLibrary.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.ObjectSerializationRecipes
{
    public class ComplexObjectFieldSerializationRecipe : BaseObjectFieldSerializationRecipe
    {
        public ComplexObjectFieldSerializationRecipe(PropertyInfo fieldProperty, BinaryTypeBaseAttribute fieldAttribute) : base(fieldProperty, fieldAttribute)
        {
        }

        public override void Deserialization(object deserializingObject, BinaryArrayReader reader)
        {
            FieldProperty.SetValue(deserializingObject, ComplexTypeSerializerMapper.DeserializeObject(FieldAttribute, FieldType, reader));
        }

        public override void Serialization(object serializingObject, BinaryArrayBuilder builder)
        {
            ComplexTypeSerializerMapper.SerializeObject(FieldAttribute, FieldProperty.GetValue(serializingObject), builder);
        }
    }
}
