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
    public class AtomicObjectPropertySerializationRecipe : BaseObjectPropertySerializationRecipe
    {
        public BaseTypeSerializer Serializer { get; }

        public AtomicObjectPropertySerializationRecipe(Type objectType, PropertyInfo propertyProperty, BinaryTypeBaseAttribute propertyAttribute) : base(objectType, propertyProperty, BaseTypeSerializer.GetPropertyType(propertyProperty), propertyAttribute)
        {
            var serializer = BaseTypeSerializerMapper.GetSerializer(PropertyType);

            if (serializer is not null)
            {
                Serializer = serializer;
            }
            else
            {
                throw new UnresolvedSerializerOfPropertyException(propertyProperty);
            }
        }

        public override object? _Deserialization(ABinaryDataReader reader) => ComplexBaseTypeSerializer.DeserializeAtomicValue(Serializer, PropertyAttribute, PropertyType, reader);

        public override void _Serialization(object? propertyValue, ABinaryDataWriter builder) => ComplexBaseTypeSerializer.SerializeAtomicValue(Serializer, PropertyAttribute, PropertyType, propertyValue, builder);
    }
}
