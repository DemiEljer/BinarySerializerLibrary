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
        public SimpleObjectFieldSerializationRecipe(PropertyInfo fieldProperty, BinaryTypeBaseAttribute fieldAttribute) : base(fieldProperty, BaseTypeSerializer.GetPropertyType(fieldProperty), fieldAttribute)
        {

        }

        public override void Deserialization(object deserializingObject, BinaryArrayReader reader)
        {
            var fieldValue = ComplexBaseTypeSerializer.DeserializeAtomicValue(FieldAttribute, FieldType, reader);

            FieldProperty.SetValue(deserializingObject, fieldValue);
        }

        public override void Serialization(object serializingObject, BinaryArrayBuilder builder)
        {
            var fieldValue = FieldProperty.GetValue(serializingObject);

            ComplexBaseTypeSerializer.SerializeAtomicValue(FieldAttribute, FieldType, fieldValue, builder);
        }
    }
}
