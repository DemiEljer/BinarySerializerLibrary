using BinarySerializerLibrary.Attributes;
using BinarySerializerLibrary.BinaryDataHandlers;
using BinarySerializerLibrary.Exceptions;
using BinarySerializerLibrary.Serializers.AtomicTypes;
using BinarySerializerLibrary.Serializers.ComplexTypes;
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
        public ComplexBaseTypeSerializer Serializer  {get; }

        public ComplexObjectPropertySerializationRecipe(Type objectType, PropertyInfo propertyProperty, BinaryTypeBaseAttribute propertyAttribute) : base(objectType, propertyProperty, propertyProperty.PropertyType, propertyAttribute)
        {
            var serializer = ComplexTypeSerializerMapper.GetSerializer(PropertyAttribute);

            if (serializer is not null)
            {
                Serializer = serializer;
            }
            else
            {
                throw new UnresolvedSerializerOfPropertyException(propertyProperty);
            }
        }

        public override object? _Deserialization(ABinaryDataReader reader) => ComplexBaseTypeSerializer.DeserializeComplexValue(Serializer, PropertyAttribute, PropertyType, reader);

        public override void _Serialization(object? propertyValue, ABinaryDataWriter builder) => ComplexBaseTypeSerializer.SerializeComplexValue(Serializer, PropertyAttribute, propertyValue, builder);
    }
}
