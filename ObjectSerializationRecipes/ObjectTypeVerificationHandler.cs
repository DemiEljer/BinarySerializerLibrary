using BinarySerializerLibrary.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.ObjectSerializationRecipes
{
    public static class ObjectTypeVerificationHandler
    {
        public static bool VerifyObjectType(Type? objectType)
        {
            if (objectType is null || !objectType.IsClass || !objectType.IsPublic)
            {
                return false;
            }
            // Поиск атрибута метки, что данный класс подлежит сериализации
            if (objectType.GetCustomAttributes(true).FirstOrDefault(attribute => attribute is BinarySerializableObjectAttribute) == null)
            {
                return false;
            }

            // Поиск конструктора по умолчанию
            if (objectType.GetConstructors().FirstOrDefault(constructor => constructor.GetParameters().Length == 0 && constructor.IsPublic) == null)
            {
                return false;
            }

            return true;
        }

        public static bool VerifyProperty(PropertyInfo? propertyInfo, BinaryTypeBaseAttribute attribute)
        {
            if (propertyInfo is null
                || propertyInfo.SetMethod?.IsPublic != true
                || propertyInfo.GetMethod?.IsPublic != true)
            {
                return false;
            }

            return VerifyPropertyType(propertyInfo.PropertyType, attribute);
        }

        public static bool VerifyPropertyType(Type? propertyType, BinaryTypeBaseAttribute attribute)
        {
            if (propertyType is null)
            {
                return false;
            }

            if (propertyType.Name == "List`1")
            {
                if (attribute.Type == Enums.BinaryArgumentTypeEnum.List)
                {
                    return VerifyPropertyType(propertyType.GetGenericArguments().FirstOrDefault(), attribute.CloneAndChange(Enums.BinaryArgumentTypeEnum.Single));
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
                    return VerifyPropertyType(propertyType.GetElementType(), attribute.CloneAndChange(Enums.BinaryArgumentTypeEnum.Single));
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
                    return VerifyPropertyType(propertyType.GetGenericArguments().FirstOrDefault(), attribute);
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
                    return _VerifyPropertyAtomicType(propertyType, attribute);
                }
                else
                {
                    return false;
                }
            }
        }

        private static bool _VerifyPropertyAtomicType(Type? propertyType, BinaryTypeBaseAttribute attribute)
        {
            if (propertyType is null)
            {
                return false;
            }

            if (propertyType == typeof(string))
            {
                if (attribute is BinaryTypeStringAttribute)
                {
                    return true;
                }
            }
            else if (propertyType.IsClass)
            {
                if (attribute is BinaryTypeObjectAttribute)
                {
                    return true;
                }
            }
            else if (propertyType == typeof(bool))
            {
                if (attribute is BinaryTypeBoolAttribute)
                {
                    return true;
                }
            }
            else if (propertyType == typeof(char))
            {
                if (attribute is BinaryTypeCharAttribute)
                {
                    return true;
                }
            }
            else if (propertyType == typeof(float))
            {
                if (attribute is BinaryTypeFloatAttribute)
                {
                    return true;
                }
            }
            else if (propertyType == typeof(double))
            {
                if (attribute is BinaryTypeDoubleAttribute)
                {
                    return true;
                }
            }
            else if (propertyType == typeof(byte))
            {
                if (attribute is BinaryTypeUIntAttribute
                     && attribute.Size <= 8)
                {
                    return true;
                }
            }
            else if (propertyType == typeof(UInt16))
            {
                if (attribute is BinaryTypeUIntAttribute
                     && attribute.Size <= 16)
                {
                    return true;
                }
            }
            else if (propertyType == typeof(UInt32))
            {
                if (attribute is BinaryTypeUIntAttribute
                     && attribute.Size <= 32)
                {
                    return true;
                }
            }
            else if (propertyType == typeof(UInt64))
            {
                if (attribute is BinaryTypeUIntAttribute
                     && attribute.Size <= 64)
                {
                    return true;
                }
            }
            else if (propertyType == typeof(sbyte))
            {
                if (attribute is BinaryTypeIntAttribute
                     && attribute.Size <= 8)
                {
                    return true;
                }
            }
            else if (propertyType == typeof(Int16))
            {
                if (attribute is BinaryTypeIntAttribute
                     && attribute.Size <= 16)
                {
                    return true;
                }
            }
            else if (propertyType == typeof(Int32))
            {
                if (attribute is BinaryTypeIntAttribute
                     && attribute.Size <= 32)
                {
                    return true;
                }
            }
            else if (propertyType == typeof(Int64))
            {
                if (attribute is BinaryTypeIntAttribute
                     && attribute.Size <= 64)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
