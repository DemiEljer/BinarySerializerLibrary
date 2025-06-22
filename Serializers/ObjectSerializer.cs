using BinarySerializerLibrary.Attributes;
using BinarySerializerLibrary.Base;
using BinarySerializerLibrary.Enums;
using BinarySerializerLibrary.ObjectSerializationRecipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Serializers
{
    public static class ObjectSerializer
    {
        private static BinaryTypeObjectAttribute _Attribute { get; } = new BinaryTypeObjectAttribute(BinaryAlignmentTypeEnum.ByteAlignment);

        public static void SerializeObject<TObject>(BinaryArrayBuilder binaryBuilder, TObject? obj)
            where TObject : class
        {
            // ��������� ����������� ���� �������
            ObjectSerializationRecipesMapper.CheckRecipeVerification(typeof(TObject));
            // ������������ �������������� ������ � ������ ������������ ����������� �������
            if (obj is null)
            {
                return;
            }

            ComplexBaseTypeSerializer.SerializeComplexValue(_Attribute, obj, binaryBuilder);
        }

        public static TObject? DeserializeObject<TObject>(BinaryArrayReader binaryReader)
            where TObject : class
        {
            return DeserializeObject(binaryReader, typeof(TObject)) as TObject;
        }

        public static object? DeserializeObject(BinaryArrayReader binaryReader, Type objectType)
        {
            // ��������� ����������� ���� �������
            ObjectSerializationRecipesMapper.CheckRecipeVerification(objectType);
            // � ������, ���� ������ ������ ������ ��� null, ���������� null 
            if (binaryReader.IsEndOfArray)
            {
                return null;
            }

            var resultObject = ComplexBaseTypeSerializer.DeserializeComplexValue(_Attribute, objectType, binaryReader);
            // ������������ ��������� � �������, ��� ��������� ���������� ���� BinaryArrayReader.IsEndOfArray
            binaryReader.MakeAlignment(BinaryAlignmentTypeEnum.ByteAlignment);

            return resultObject;
        }
    }
}
