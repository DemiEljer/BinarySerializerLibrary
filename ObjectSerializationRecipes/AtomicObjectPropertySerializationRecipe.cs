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
    public class AtomicObjectPropertySerializationRecipe : BaseObjectPropertySerializationRecipe
    {
        public AtomicObjectPropertySerializationRecipe(PropertyInfo fieldProperty, BinaryTypeBaseAttribute fieldAttribute) : base(fieldProperty, BaseTypeSerializer.GetPropertyType(fieldProperty), fieldAttribute)
        {

        }

        public override object? _Deserialization(BinaryArrayReader reader) => ComplexBaseTypeSerializer.DeserializeAtomicValue(FieldAttribute, FieldType, reader);

        public override void _Serialization(object? propertyValue, BinaryArrayBuilder builder) => ComplexBaseTypeSerializer.SerializeAtomicValue(FieldAttribute, FieldType, propertyValue, builder);
    }
}
