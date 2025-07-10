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

            ObjectType = objectType;
        }
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

            ObjectSerializationRecipeFabric.CreatePropertyRecipe(ObjectType, property, attribute);

            return this;
        }
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

            ObjectSerializationRecipeFabric.CreatePropertyRecipe(ObjectType, property, new BinaryTypeAutoAttribute());

            return this;
        }
        /// <summary>
        /// Создать рецепт и добавить его в коллекцию
        /// </summary>
        public void Commit()
        {
            ObjectSerializationRecipesMapper.AddRecipe(ObjectType, new ObjectSerializationRecipe(ObjectType, PropertiesRecipes.ToArray()));
        }
    }
}
