using BinarySerializerLibrary.Attributes;
using BinarySerializerLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.ObjectSerializationRecipes
{
    public class ObjectTypeVerificationAndExtractionHandler
    {
        public event Action<Type>? ObjectTypeHasBeenDetectedEvent;

        #region SubClasses

        public class AttributeArguments
        {
            /// <summary>
            /// Тип свойства
            /// </summary>
            public BinaryArgumentTypeEnum Type { get; set; }
            /// <summary>
            /// Выравнивание свойства
            /// </summary>
            public BinaryAlignmentTypeEnum Alignment { get; set; }
            /// <summary>
            /// Может ли свойство принимать значение null
            /// </summary>
            public BinaryNullableTypeEnum Nullable { get; set; }
        }

        #endregion SubClasses

        #region VerificationLogic

        public bool VerifyObjectType(Type? objectType)
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

        public bool VerifyProperty(PropertyInfo? propertyInfo, BinaryTypeBaseAttribute? attribute)
        {
            if (propertyInfo is null
                || propertyInfo.SetMethod?.IsPublic != true
                || propertyInfo.GetMethod?.IsPublic != true
                || attribute is null)
            {
                return false;
            }

            return VerifyPropertyType(propertyInfo.PropertyType, attribute);
        }

        public bool VerifyPropertyType(Type? propertyType, BinaryTypeBaseAttribute attribute)
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

        private bool _VerifyPropertyAtomicType(Type? propertyType, BinaryTypeBaseAttribute attribute)
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
                    ObjectTypeHasBeenDetectedEvent?.Invoke(propertyType);

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

        #endregion VerificationLogic

        #region AttributeExtractionLogic

        public BinaryTypeBaseAttribute? GetPropertyAttribute(PropertyInfo? property, BinaryTypeBaseAttribute attribute)
        {
            AttributeArguments attributeArguments = new()
            {
                Alignment = attribute.Alignment
            };

            return GetPropertyAttribute(property?.PropertyType, attribute);
        }

        public BinaryTypeBaseAttribute? GetPropertyAttribute(Type? propertyType, BinaryTypeBaseAttribute attribute)
        {
            if (propertyType is null)
            {
                return null;
            }

            AttributeArguments attributeArguments = new()
            {
                Alignment = attribute.Alignment
            };

            return _GetPropertyAttribute(propertyType, attributeArguments);
        }

        private BinaryTypeBaseAttribute? _GetPropertyAttribute(Type? propertyType, AttributeArguments attributeArguments)
        {
            if (propertyType is null)
            {
                return null;
            }

            if (propertyType.Name == "List`1")
            {
                if (attributeArguments.Type == BinaryArgumentTypeEnum.Single
                    && attributeArguments.Nullable == BinaryNullableTypeEnum.NotNullable)
                {
                    attributeArguments.Type = BinaryArgumentTypeEnum.List;

                    return _GetPropertyAttribute(propertyType.GetGenericArguments().FirstOrDefault(), attributeArguments);
                }
                else
                {
                    return null;
                }
            }
            else if (propertyType.IsArray)
            {
                if (attributeArguments.Type == BinaryArgumentTypeEnum.Single
                    && attributeArguments.Nullable == BinaryNullableTypeEnum.NotNullable)
                {
                    attributeArguments.Type = BinaryArgumentTypeEnum.Array;

                    return _GetPropertyAttribute(propertyType.GetElementType(), attributeArguments);
                }
                else
                {
                    return null;
                }
            }
            else if (propertyType.Name == "Nullable`1")
            {
                if (attributeArguments.Nullable == BinaryNullableTypeEnum.NotNullable)
                {
                    attributeArguments.Nullable = BinaryNullableTypeEnum.Nullable;

                    return _GetPropertyAttribute(propertyType.GetGenericArguments().FirstOrDefault(), attributeArguments);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return _GetPropertyAtomicAttribute(propertyType, attributeArguments);
            }
        }

        private BinaryTypeBaseAttribute? _GetPropertyAtomicAttribute(Type? propertyType, AttributeArguments attributeArguments)
        {
            if (propertyType is null)
            {
                return null;
            }

            if (propertyType == typeof(string))
            {
                return new BinaryTypeStringAttribute(attributeArguments.Alignment, attributeArguments.Type);
            }
            else if (propertyType.IsClass)
            {
                return new BinaryTypeObjectAttribute(attributeArguments.Alignment, attributeArguments.Type);
            }
            else if (propertyType == typeof(bool))
            {
                return new BinaryTypeBoolAttribute(attributeArguments.Nullable, attributeArguments.Alignment, attributeArguments.Type);
            }
            else if (propertyType == typeof(char))
            {
                return new BinaryTypeCharAttribute(attributeArguments.Nullable, attributeArguments.Alignment, attributeArguments.Type);
            }
            else if (propertyType == typeof(float))
            {
                return new BinaryTypeFloatAttribute(attributeArguments.Nullable, attributeArguments.Alignment, attributeArguments.Type);
            }
            else if (propertyType == typeof(double))
            {
                return new BinaryTypeDoubleAttribute(attributeArguments.Nullable, attributeArguments.Alignment, attributeArguments.Type);
            }
            else if (propertyType == typeof(byte))
            {
                return new BinaryTypeUIntAttribute(8, attributeArguments.Nullable, attributeArguments.Alignment, attributeArguments.Type);
            }
            else if (propertyType == typeof(UInt16))
            {
                return new BinaryTypeUIntAttribute(16, attributeArguments.Nullable, attributeArguments.Alignment, attributeArguments.Type);
            }
            else if (propertyType == typeof(UInt32))
            {
                return new BinaryTypeUIntAttribute(32, attributeArguments.Nullable, attributeArguments.Alignment, attributeArguments.Type);
            }
            else if (propertyType == typeof(UInt64))
            {
                return new BinaryTypeUIntAttribute(64, attributeArguments.Nullable, attributeArguments.Alignment, attributeArguments.Type);
            }
            else if (propertyType == typeof(sbyte))
            {
                return new BinaryTypeIntAttribute(8, attributeArguments.Nullable, attributeArguments.Alignment, attributeArguments.Type);
            }
            else if (propertyType == typeof(Int16))
            {
                return new BinaryTypeIntAttribute(16, attributeArguments.Nullable, attributeArguments.Alignment, attributeArguments.Type);
            }
            else if (propertyType == typeof(Int32))
            {
                return new BinaryTypeIntAttribute(32, attributeArguments.Nullable, attributeArguments.Alignment, attributeArguments.Type);
            }
            else if (propertyType == typeof(Int64))
            {
                return new BinaryTypeIntAttribute(64, attributeArguments.Nullable, attributeArguments.Alignment, attributeArguments.Type);
            }

            return null;
        }

        #endregion AttributeExtractionLogic
    }
}
