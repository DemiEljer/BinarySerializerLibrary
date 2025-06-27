using BinarySerializerLibrary.Attributes;
using BinarySerializerLibrary.BinaryDataHandlers;
using BinarySerializerLibrary.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.ObjectSerializationRecipes
{
    public class ComplexObjectPropertySerializationRecipe : BaseObjectPropertySerializationRecipe
    {
        public ComplexObjectPropertySerializationRecipe(PropertyInfo fieldProperty, BinaryTypeBaseAttribute fieldAttribute) : base(fieldProperty, fieldProperty.PropertyType, fieldAttribute)
        {
        }

        public override object? _Deserialization(ABinaryDataReader reader) => ComplexBaseTypeSerializer.DeserializeComplexValue(FieldAttribute, FieldType, reader);

        public override void _Serialization(object? propertyValue, ABinaryDataWriter builder) => ComplexBaseTypeSerializer.SerializeComplexValue(FieldAttribute, propertyValue, builder);
    }
}
