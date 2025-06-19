using BinarySerializerLibrary.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.ObjectSerializationRecipes
{
    public static class ObjectPropertyTypeVerificationHandler
    {
        public static bool VerifyProperty(Type? propertyType, BinaryTypeBaseAttribute attribute)
        {
            if (propertyType is null)
            {
                return false;
            }

            if (propertyType.Name == "List`1")
            {
                if (attribute.Type == Enums.BinaryArgumentTypeEnum.List)
                {
                    return VerifyProperty(propertyType.GetGenericArguments().FirstOrDefault(), attribute.CloneAndChangeType(Enums.BinaryArgumentTypeEnum.Single));
                }
                else
                {
                    return false;
                }
            }
            else if (propertyType.IsArray)
            {
                if (attribute.Type == Enums.BinaryArgumentTypeEnum.Array)
                {
                    return VerifyProperty(propertyType.GetElementType(), attribute.CloneAndChangeType(Enums.BinaryArgumentTypeEnum.Single));
                }
                else
                {
                    return false;
                }
            }
            else if (propertyType.Name == "Nullable`1")
            {
                if (attribute.Type == Enums.BinaryArgumentTypeEnum.Single
                    && attribute.Nullable == Enums.BinaryNullableTypeEnum.Nullable)
                {
                    return VerifyProperty(propertyType.GetGenericArguments().FirstOrDefault(), attribute);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (attribute.Type == Enums.BinaryArgumentTypeEnum.Single)
                {
                    return _TypeVerification(propertyType, attribute);
                }
                else
                {
                    return false;
                }
            }
        }

        private static bool _TypeVerification(Type propertyType, BinaryTypeBaseAttribute attribute)
        {
            if (propertyType == typeof(string)
                && attribute is BinaryTypeStringAttribute)
            {
                return true;
            }
            else if (propertyType.IsClass
                     && attribute is BinaryTypeObjectAttribute)
            {
                return true;
            }
            else if (propertyType == typeof(bool)
                     && attribute is BinaryTypeBoolAttribute)
            {
                return true;
            }
            else if (propertyType == typeof(char)
                     && attribute is BinaryTypeCharAttribute)
            {
                return true;
            }
            else if (propertyType == typeof(float)
                     && attribute is BinaryTypeFloatAttribute)
            {
                return true;
            }
            else if (propertyType == typeof(double)
                     && attribute is BinaryTypeDoubleAttribute)
            {
                return true;
            }
            else if (propertyType == typeof(byte)
                     && attribute is BinaryTypeUIntAttribute
                     && attribute.FieldSize <= 8)
            {
                return true;
            }
            else if (propertyType == typeof(UInt16)
                     && attribute is BinaryTypeUIntAttribute
                     && attribute.FieldSize <= 16)
            {
                return true;
            }
            else if (propertyType == typeof(UInt32)
                     && attribute is BinaryTypeUIntAttribute
                     && attribute.FieldSize <= 32)
            {
                return true;
            }
            else if (propertyType == typeof(UInt64)
                     && attribute is BinaryTypeUIntAttribute
                     && attribute.FieldSize <= 64)
            {
                return true;
            }
            else if (propertyType == typeof(sbyte)
                     && attribute is BinaryTypeIntAttribute
                     && attribute.FieldSize <= 8)
            {
                return true;
            }
            else if (propertyType == typeof(Int16)
                     && attribute is BinaryTypeIntAttribute
                     && attribute.FieldSize <= 16)
            {
                return true;
            }
            else if (propertyType == typeof(Int32)
                     && attribute is BinaryTypeIntAttribute
                     && attribute.FieldSize <= 32)
            {
                return true;
            }
            else if (propertyType == typeof(Int64)
                     && attribute is BinaryTypeIntAttribute
                     && attribute.FieldSize <= 64)
            {
                return true;
            }

            return false;
        }
    }
}
