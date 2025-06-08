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
    public class SimpleObjectFieldSerializationRecipe : BaseObjectFieldSerializationRecipe
    {
        public SimpleObjectFieldSerializationRecipe(PropertyInfo fieldProperty, BinaryTypeBaseAttribute fieldAttribute) : base(fieldProperty, fieldAttribute)
        {

        }

        public override void Deserialization(object deserializingObject, BinaryArrayReader reader)
        {
            FieldProperty.SetValue(deserializingObject, BaseTypeSerializerMapper.DeserializeValue(FieldType, reader.ReadValue(FieldAttribute.FieldSize, FieldAttribute.Alignment), FieldAttribute.FieldSize));
        }

        public override void Serialization(object serializingObject, BinaryArrayBuilder builder)
        {
            builder.AppendBitValue(FieldAttribute.FieldSize, BaseTypeSerializerMapper.SerializeValue(FieldType, FieldProperty.GetValue(serializingObject), FieldAttribute.FieldSize), FieldAttribute.Alignment);
        }
    }
}
