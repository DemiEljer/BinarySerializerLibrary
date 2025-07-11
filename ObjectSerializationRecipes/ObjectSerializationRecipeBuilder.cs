using BinarySerializerLibrary.Attributes;
using BinarySerializerLibrary.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.ObjectSerializationRecipes
{
    /// <summary>
    /// Класс ручного построения рецептов обработки объектов
    /// </summary>
    public class ObjectSerializationRecipeBuilder
    {
        /// <summary>
        /// Тип объекта
        /// </summary>
        public Type ObjectType { get; }
        /// <summary>
        /// Список рецептов свойств
        /// </summary>
        public List<BaseObjectPropertySerializationRecipe> PropertiesRecipes { get; } = new();

        public ObjectSerializationRecipeBuilder(Type objectType)
        {
            if (objectType is null)
            {
                throw new ObjectTypeIsNullException();
            }

            if (ObjectSerializationRecipesMapper.GetFabric().VerifyType(objectType))
            {
                ObjectType = objectType;
            }
            else
            {
                throw new ObjectTypeVerificationFailedException(objectType);
            }
        }
        /// <summary>
        /// Добавить свойство
        /// </summary>
        /// <param name="property"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public ObjectSerializationRecipeBuilder AddProperty(string property, BinaryTypeBaseAttribute attribute) => AddProperty(ObjectType.GetProperty(property), attribute);
        /// <summary>
        /// Добавить свойство
        /// </summary>
        /// <param name="property"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public ObjectSerializationRecipeBuilder AddProperty(PropertyInfo? property, BinaryTypeBaseAttribute attribute)
        {
            if (property is null)
            {
                throw new PropertyIsNullException();
            }

            var propertyRecipe = ObjectSerializationRecipesMapper.GetFabric().CreatePropertyRecipe(ObjectType, property, attribute);

            if (propertyRecipe is not null)
            {
                PropertiesRecipes.Add(propertyRecipe);
            }
            else
            {
                throw new ObjectTypeVerificationFailedException(ObjectType);
            }

            return this;
        }
        /// <summary>
        /// Добавить свойство
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public ObjectSerializationRecipeBuilder AddProperty(string property) => AddProperty(ObjectType.GetProperty(property));
        /// <summary>
        /// Добавить свойство
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public ObjectSerializationRecipeBuilder AddProperty(PropertyInfo? property)
        {
            if (property is null)
            {
                throw new PropertyIsNullException();
            }
            // Поиск атрибута среди указанных при объявлении типа
            var binaryAttribute = property.GetCustomAttributes(true).FirstOrDefault(attribute => attribute is BinaryTypeBaseAttribute) as BinaryTypeBaseAttribute;

            BaseObjectPropertySerializationRecipe? propertyRecipe;
            // В случае, если атрибут найден, то используем его
            if (binaryAttribute is not null)
            {
                propertyRecipe = ObjectSerializationRecipesMapper.GetFabric().CreatePropertyRecipe(ObjectType, property, binaryAttribute);
            }
            // В противном случае выбираем автоматический поиск атрибутов
            else
            {
                propertyRecipe = ObjectSerializationRecipesMapper.GetFabric().CreatePropertyRecipe(ObjectType, property, new BinaryTypeAutoAttribute());
            }

            if (propertyRecipe is not null)
            {
                PropertiesRecipes.Add(propertyRecipe);
            }
            else
            {
                throw new ObjectTypeVerificationFailedException(ObjectType);
            }

            return this;
        }
        /// <summary>
        /// Создать рецепт и добавить его в коллекцию
        /// </summary>
        public void Commit(int typeCode = 0)
        {
            ObjectSerializationRecipesMapper.AddRecipe(ObjectType, new ObjectSerializationRecipe(ObjectType, PropertiesRecipes.ToArray()));
            // Регистрация кода типа
            if (typeCode != 0)
            {
                ObjectTypeMapper.RegisterType(ObjectType, typeCode);
            }
        }
    }
}
