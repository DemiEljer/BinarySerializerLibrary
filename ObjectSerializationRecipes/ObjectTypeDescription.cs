using BinarySerializerLibrary.Attributes;
using BinarySerializerLibrary.Enums;
using BinarySerializerLibrary.Exceptions;
using BinarySerializerLibrary.ObjectSerializationRecipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary
{
    /// <summary>
    /// Описание типа 
    /// </summary>
    [BinarySerializableObject]
    public class ObjectTypeDescription
    {
        /// <summary>
        /// Имя типа
        /// </summary>
        [BinaryTypeString()]
        public string? TypeName { get; set; }
        /// <summary>
        /// Код типа
        /// </summary>
        [BinaryTypeInt(30)]
        public int TypeCode { get; set; }
        /// <summary>
        /// Список свйоств
        /// </summary>
        [BinaryTypeString(Enums.BinaryArgumentTypeEnum.Array)]
        public string[]? PropertiesSequence { get; set; }

        static ObjectTypeDescription()
        {
            // Ручное создание рецепта сериализации объекта
            var objectType = typeof(ObjectTypeDescription);

            var typeNameProperty = objectType.GetProperty("TypeName");
            var typeCodeProperty = objectType.GetProperty("TypeCode");
            var propertiesProperty = objectType.GetProperty("PropertiesSequence");

            if (typeNameProperty is not null
                && typeCodeProperty is not null
                && propertiesProperty is not null)
            {
                ObjectSerializationRecipesMapper.AddRecipe(typeof(ObjectTypeDescription))
                    .AddProperty(typeNameProperty, new BinaryTypeStringAttribute())
                    .AddProperty(typeCodeProperty, new BinaryTypeIntAttribute(30))
                    .AddProperty(propertiesProperty, new BinaryTypeStringAttribute(Enums.BinaryArgumentTypeEnum.Array))
                    .Commit();
            }
            else
            {
                throw new TypeDescriptionCreationException(objectType);
            }
        }

        public ObjectTypeDescription() { }

        public ObjectTypeDescription(ObjectSerializationRecipe recipe)
        {
            if (recipe is null)
            {
                throw new TypeDescriptionCreationException(recipe?.ObjectType);
            }

            TypeName = recipe.ObjectType.FullName;
            TypeCode = ObjectTypeMapper.GetCode(recipe.ObjectType);
            PropertiesSequence = recipe.PropertiesRecipes.Select(property => property.Property.Name).ToArray();
        }
        /// <summary>
        /// Применить описание типа
        /// </summary>
        public void ApplyDescription()
        {
            var typeRecipe = ObjectSerializationRecipesMapper.ObjectsRecipes.FirstOrDefault(recipe => recipe.ObjectType.FullName == TypeName);

            if (typeRecipe is not null)
            {
                if (!typeRecipe.TryToResetPropertiesSequence(PropertiesSequence))
                {
                    throw new TypeDescriptionApplyingException(this);
                }

                ObjectTypeMapper.RegisterType(typeRecipe.ObjectType, TypeCode);
            }
            else
            {
                throw new TypeDescriptionApplyingException(this);
            }
        }
        /// <summary>
        /// Преобразовать 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"@{TypeName} {TypeCode} {string.Join(" ", (PropertiesSequence is null ? Array.Empty<string>() : PropertiesSequence))}";
        }
        /// <summary>
        /// Преобразовать последовательность описаний в строку
        /// </summary>
        /// <param name="descriptions"></param>
        /// <returns></returns>
        public static string SequenceToString(IEnumerable<ObjectTypeDescription> descriptions)
        {
            if (descriptions is null)
            {
                return "";
            }
            return string.Join("\r\n", descriptions.Select(d => d.ToString()));
        }
        /// <summary>
        /// Распарсить последовательность описаний типа из строки
        /// </summary>
        /// <param name="descriptionsContent"></param>
        /// <returns></returns>
        public static IEnumerable<ObjectTypeDescription?> ParseSequence(string descriptionsContent, Action<Exception>? exceptionCallback = null)
        {
            return descriptionsContent.Split("@").Where(descriptionContent => !string.IsNullOrEmpty(descriptionContent)).Select(descriptionContent =>
            {
                try
                {
                    return Parse(descriptionContent);
                }
                catch (Exception e)
                {
                    if (exceptionCallback is null)
                    {
                        throw;
                    }
                    else
                    {
                        exceptionCallback.Invoke(e);

                        return null;
                    }
                }
            });
        }
        /// <summary>
        /// Распарсить объект описания типа из строки
        /// </summary>
        /// <param name="descriptionContent"></param>
        /// <returns></returns>
        /// <exception cref="TypeDescriptionParsingException"></exception>
        public static ObjectTypeDescription Parse(string descriptionContent)
        {
            try
            {
                var descriptionElements = descriptionContent.Split("@").Last().Trim().Split(" ").ToArray();

                ObjectTypeDescription description = new ObjectTypeDescription()
                {
                    TypeName = descriptionElements[0]
                    , TypeCode = int.Parse(descriptionElements[1])
                    , PropertiesSequence = descriptionElements.TakeLast(descriptionElements.Length - 2).ToArray()
                };

                return description;
            }
            catch
            {
                throw new TypeDescriptionParsingException(descriptionContent);
            }
        }
    }
}
