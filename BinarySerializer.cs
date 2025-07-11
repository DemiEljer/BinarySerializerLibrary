using BinarySerializerLibrary.Attributes;
using BinarySerializerLibrary.Base;
using BinarySerializerLibrary.BinaryDataHandlers;
using BinarySerializerLibrary.BinaryDataHandlers.Helpers;
using BinarySerializerLibrary.Exceptions;
using BinarySerializerLibrary.ObjectSerializationRecipes;
using BinarySerializerLibrary.Serializers.ComplexTypes;
using System.IO;

namespace BinarySerializerLibrary
{
    public static class BinarySerializer
    {
        #region Serialize

        /// <summary>
        /// Преобразовать объект в массив байт
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="obj"></param>
        /// <param name="exceptionCallback"></param>
        /// <returns></returns>
        public static byte[] SerializeExceptionThrowing<TObject>(TObject? obj) where TObject : class =>
            SerializeExceptionShielding(obj, (e) => throw e);
        /// <summary>
        /// Преобразовать объект в массив байт
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="obj"></param>
        /// <param name="exceptionCallback"></param>
        /// <returns></returns>
        public static byte[] SerializeExceptionShielding<TObject>(TObject? obj, Action<Exception>? exceptionCallback = null)
            where TObject : class
        {
            var binaryBuilder = new BinaryArrayBuilder();

            SerializeExceptionShielding(binaryBuilder, obj, exceptionCallback);

            return binaryBuilder.GetByteArray();
        }
        /// <summary>
        /// Преобразовать объект в массив байт
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="obj"></param>
        /// <param name="exceptionCallback"></param>
        /// <returns></returns>
        public static void SerializeExceptionThrowing<TObject>(this ABinaryDataWriter binaryBuilder, TObject? obj) where TObject : class =>
            SerializeExceptionShielding(binaryBuilder, obj, (e) => throw e);
        /// <summary>
        /// Преобразовать объект в массив байт
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="obj"></param>
        /// <param name="exceptionCallback"></param>
        /// <returns></returns>
        public static void SerializeExceptionShielding<TObject>(this ABinaryDataWriter binaryBuilder, TObject? obj, Action<Exception>? exceptionCallback = null) where TObject : class =>
            SerializeExceptionShielding(binaryBuilder, obj as object, exceptionCallback);
        /// <summary>
        /// Преобразовать объект в массив байт
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="obj"></param>
        /// <param name="exceptionCallback"></param>
        /// <returns></returns>
        public static byte[] SerializeExceptionThrowing(object? obj) =>
            SerializeExceptionShielding(obj, (e) => throw e);
        /// <summary>
        /// Преобразовать объект в массив байт
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="obj"></param>
        /// <param name="exceptionCallback"></param>
        /// <returns></returns>
        public static byte[] SerializeExceptionShielding(object? obj, Action<Exception>? exceptionCallback = null)
        {
            var binaryBuilder = new BinaryArrayBuilder();

            SerializeExceptionShielding(binaryBuilder, obj, exceptionCallback);

            return binaryBuilder.GetByteArray();
        }
        /// <summary>
        /// Преобразовать объект в массив байт
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="obj"></param>
        /// <param name="exceptionCallback"></param>
        /// <returns></returns>
        public static void SerializeExceptionThrowing(this ABinaryDataWriter binaryBuilder, object? obj) =>
            SerializeExceptionShielding(binaryBuilder, obj, (e) => throw e);
        /// <summary>
        /// Преобразовать объект в массив байт
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="obj"></param>
        /// <param name="exceptionCallback"></param>
        /// <returns></returns>
        public static void SerializeExceptionShielding(this ABinaryDataWriter binaryBuilder, object? obj, Action<Exception>? exceptionCallback = null)
        {
            if (binaryBuilder is null)
            {
                exceptionCallback?.Invoke(new BinaryWriterIsNullException());

                return;
            }

            // Объект внутреннего построения бинарного массива
            ABinaryDataWriter? internalBinaryBuilder;
            // В случае, если был передан еше не заполненный объект, то используем его
            if (binaryBuilder.BytesCount == 0)
            {
                internalBinaryBuilder = binaryBuilder;
            }
            else
            {
                internalBinaryBuilder = Activator.CreateInstance(binaryBuilder.GetType()) as ABinaryDataWriter;                
            }

            if (internalBinaryBuilder is null)
            {
                exceptionCallback?.Invoke(new BinaryWriterIsNullException());

                return;
            }

            try
            {
                ObjectSerializer.SerializeObject(internalBinaryBuilder, obj);
                // В случае, если был создан новый объект, то дополняем основной его содержимым
                if (internalBinaryBuilder != binaryBuilder)
                {
                    binaryBuilder.AppenBuilderAndShiftToEnd(internalBinaryBuilder);
                }
            }
            catch (Exception e)
            {
                internalBinaryBuilder.Clear();

                exceptionCallback?.Invoke(e);
            }
        }

        #endregion Serialize

        #region Deserialize

        /// <summary>
        /// Получить объект из массива байт
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="content"></param>
        /// <param name="exceptionCallback"></param>
        /// <returns></returns>
        public static TObject? DeserializeExceptionThrowing<TObject>(byte[]? content) where TObject : class =>
            DeserializeExceptionShielding<TObject>(content, (e) => throw e);
        /// <summary>
        /// Получить объект из массива байт
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="content"></param>
        /// <param name="exceptionCallback"></param>
        /// <returns></returns>
        public static TObject? DeserializeExceptionShielding<TObject>(byte[]? content, Action<Exception>? exceptionCallback = null)
            where TObject : class
        {
            var binaryReader = new BinaryArrayReader(content);

            return DeserializeExceptionShielding<TObject>(binaryReader, exceptionCallback);
        }
        /// <summary>
        /// Получить объект с помощью байтового обработчика
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="content"></param>
        /// <param name="exceptionCallback"></param>
        /// <returns></returns>
        public static TObject? DeserializeExceptionThrowing<TObject>(this ABinaryDataReader binaryReader) where TObject : class =>
            DeserializeExceptionShielding<TObject>(binaryReader, (e) => throw e);
        /// <summary>
        /// Получить объект с помощью байтового обработчика
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="content"></param>
        /// <param name="exceptionCallback"></param>
        /// <returns></returns>
        public static TObject? DeserializeExceptionShielding<TObject>(this ABinaryDataReader binaryReader, Action<Exception>? exceptionCallback = null) where TObject : class =>
             DeserializeExceptionShielding(binaryReader, typeof(TObject), exceptionCallback) as TObject;
        /// <summary>
        /// Получить объект из массива байт
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="content"></param>
        /// <param name="exceptionCallback"></param>
        /// <returns></returns>
        public static object? DeserializeExceptionThrowing(byte[]? content, Type? objectType) => 
            DeserializeExceptionShielding(content, objectType, (e) => throw e);
        /// <summary>
        /// Получить объект из массива байт
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="content"></param>
        /// <param name="exceptionCallback"></param>
        /// <returns></returns>
        public static object? DeserializeExceptionShielding(byte[]? content, Type? objectType, Action<Exception>? exceptionCallback = null)
        {
            var binaryReader = new BinaryArrayReader(content);

            return DeserializeExceptionShielding(binaryReader, objectType, exceptionCallback);
        }
        /// <summary>
        /// Получить объект с помощью байтового обработчика
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="content"></param>
        /// <param name="exceptionCallback"></param>
        /// <returns></returns>
        public static object? DeserializeExceptionThrowing(this ABinaryDataReader binaryReader, Type? objectType) => 
            DeserializeExceptionShielding(binaryReader, objectType, (e) => throw e);
        /// <summary>
        /// Получить объект с помощью байтового обработчика
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="content"></param>
        /// <param name="exceptionCallback"></param>
        /// <returns></returns>
        public static object? DeserializeExceptionShielding(this ABinaryDataReader binaryReader, Type? objectType, Action<Exception>? exceptionCallback = null)
        {
            if (binaryReader is null)
            {
                exceptionCallback?.Invoke(new BinaryReaderIsNullException());

                return null;
            }

            if (objectType is null)
            {
                exceptionCallback?.Invoke(new ObjectTypeIsNullException());

                return null;
            }

            try
            {
                return ObjectSerializer.DeserializeObject(binaryReader, objectType);
            }
            catch (Exception e)
            {
                exceptionCallback?.Invoke(e);

                return null;
            }
        }
        /// <summary>
        /// Получить объект из массива байт
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="content"></param>
        /// <param name="exceptionCallback"></param>
        /// <returns></returns>
        public static object? AutoDeserializeExceptionThrowing(byte[]? content) => 
            AutoDeserializeExceptionShielding(content, (e) => throw e);
        /// <summary>
        /// Получить объект из массива байт
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="content"></param>
        /// <param name="exceptionCallback"></param>
        /// <returns></returns>
        public static object? AutoDeserializeExceptionShielding(byte[]? content, Action<Exception>? exceptionCallback = null)
        {
            var binaryReader = new BinaryArrayReader(content);

            return AutoDeserializeExceptionShielding(binaryReader, exceptionCallback);
        }
        /// <summary>
        /// Получить объект с помощью байтового обработчика
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="content"></param>
        /// <param name="exceptionCallback"></param>
        /// <returns></returns>
        public static object? AutoDeserializeExceptionThrowing(this ABinaryDataReader binaryReader) => 
            AutoDeserializeExceptionShielding(binaryReader, (e) => throw e);
        /// <summary>
        /// Получить объект с помощью байтового обработчика
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="content"></param>
        /// <param name="exceptionCallback"></param>
        /// <returns></returns>
        public static object? AutoDeserializeExceptionShielding(this ABinaryDataReader binaryReader, Action<Exception>? exceptionCallback = null)
        {
            if (binaryReader is null)
            {
                exceptionCallback?.Invoke(new BinaryReaderIsNullException());

                return null;
            }

            try
            {
                return ObjectSerializer.AutoDeserializeObject(binaryReader);
            }
            catch (Exception e)
            {
                exceptionCallback?.Invoke(e);

                return null;
            }
        }
        /// <summary>
        /// Проверить, что размер сериализованного объекта может быть прочитан
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <returns></returns>
        public static bool CheckIfSerializedObjectSizeCanBeRead(this ABinaryDataReader binaryReader) => 
            BinaryDataLengthParameterHelpers.CheckIfBinaryCollectionSizeCanBeRead(binaryReader);
        /// <summary>
        /// Прочитать размер сериализованного объекта в байтах
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <returns></returns>
        public static int? GetSerializedObjectSize(this ABinaryDataReader binaryReader) => 
            BinaryDataLengthParameterHelpers.UnpackBinaryCollectionSizeWithoutShifting(binaryReader);
        /// <summary>
        /// Проверить, что сериализованный объект может быть прочитан
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <returns></returns>
        public static bool CheckIfSerializedObjectCanBeRead(this ABinaryDataReader binaryReader)
        {
            if (binaryReader is null)
            {
                return false;
            }

            var objectSerializedCollectionSize = BinaryDataLengthParameterHelpers.UnpackBinaryCollectionSizeWithoutShifting(binaryReader);

            if (objectSerializedCollectionSize is null)
            {
                return false;
            }
            else if ((binaryReader.BitIndex % 8) == 0)
            {
                return (binaryReader.ByteIndex + objectSerializedCollectionSize) <= binaryReader.BytesCount;
            }
            else
            {
                return (binaryReader.ByteIndex + objectSerializedCollectionSize + 1) <= binaryReader.BytesCount;
            }
        }

        #endregion Deserialize

        #region CookRecipe

        /// <summary>
        /// Подготовить рецепт сериализации типа объекта
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="exceptionCallback"></param>
        public static ObjectSerializationRecipe? CookObjectRecipeExceptionThrowing<ObjectType>() where ObjectType : class =>
             CookObjectRecipeExceptionShielding<ObjectType>((e) => throw e);
        /// <summary>
        /// Подготовить рецепт сериализации типа объекта
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="exceptionCallback"></param>
        public static ObjectSerializationRecipe? CookObjectRecipeExceptionShielding<ObjectType>(Action<Exception>? exceptionCallback = null) where ObjectType : class =>
            CookObjectRecipeExceptionShielding(typeof(ObjectType), exceptionCallback);
        /// <summary>
        /// Подготовить рецепт сериализации типа объекта
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="exceptionCallback"></param>
        public static ObjectSerializationRecipe? CookObjectRecipeExceptionThrowing(Type? objectType) => CookObjectRecipeExceptionShielding(objectType, (e) => throw e);
        /// <summary>
        /// Подготовить рецепт сериализации типа объекта
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="exceptionCallback"></param>
        public static ObjectSerializationRecipe? CookObjectRecipeExceptionShielding(Type? objectType, Action<Exception>? exceptionCallback = null)
        {
            if (objectType is null)
            {
                exceptionCallback?.Invoke(new ObjectTypeIsNullException());

                return null;
            }

            try
            {
                return ObjectSerializationRecipesMapper.GetOrCreateRecipe(objectType);
            }
            catch (Exception e)
            {
                exceptionCallback?.Invoke(e);

                return null;
            }
        }
        /// <summary>
        /// Получить рецепт сериализации объекта
        /// </summary>
        /// <typeparam name="ObjectType"></typeparam>
        /// <param name="exceptionCallback"></param>
        /// <returns></returns>
        public static ObjectSerializationRecipe? GetObjectRecipe<ObjectType>(Action<Exception>? exceptionCallback = null)
            where ObjectType : class
        {
            return GetObjectRecipeExceptionShielding(typeof(ObjectType));
        }
        /// <summary>
        /// Получить рецепт сериализации объекта
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static ObjectSerializationRecipe? GetObjectRecipeExceptionThrowing(Type objectType)
        {
            return GetObjectRecipeExceptionShielding(objectType, (e) => throw e);
        }
        /// <summary>
        /// Получить рецепт сериализации объекта
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="exceptionCallback"></param>
        /// <returns></returns>
        public static ObjectSerializationRecipe? GetObjectRecipeExceptionShielding(Type objectType, Action<Exception>? exceptionCallback = null)
        {
            if (objectType is null)
            {
                exceptionCallback?.Invoke(new ObjectTypeIsNullException());

                return null;
            }

            return ObjectSerializationRecipesMapper.GetRecipe(objectType);
        }
        /// <summary>
        /// Получить все рецепты сериализации объектов, созданные в данный момент времени
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ObjectSerializationRecipe> GetAllObjectsRecipes() => ObjectSerializationRecipesMapper.ObjectsRecipes;
        /// <summary>
        /// Создать рецепт сериализации объекта вручную
        /// </summary>
        /// <typeparam name="ObjectType"></typeparam>
        /// <returns></returns>
        public static ObjectSerializationRecipeBuilder CreateObjectRecipeExceptionThrowing<ObjectType>() => 
            CreateObjectRecipeExceptionThrowing(typeof(ObjectType));
        /// <summary>
        /// Создать рецепт сериализации объекта вручную
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static ObjectSerializationRecipeBuilder CreateObjectRecipeExceptionThrowing(Type objectType)
        {
            if (objectType is null)
            {
                throw new ObjectTypeIsNullException();
            }

            return ObjectSerializationRecipesMapper.AddRecipe(objectType);
        }

        #endregion CookRecipe

        #region TypesRegistration

        /// <summary>
        /// Зарегистрировать тип для логики автоматической сериализации
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="typeCode"></param>
        public static void RegisterTypeForAutoSerializationExceptionThrowing<ObjectType>(int typeCode) =>
            RegisterTypeForAutoSerializationExceptionShielding(typeof(ObjectType), typeCode, (e) => throw e);
        /// <summary>
        /// Зарегистрировать тип для логики автоматической сериализации
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="typeCode"></param>
        public static void RegisterTypeForAutoSerializationExceptionThrowing(Type objectType, int typeCode) =>
            RegisterTypeForAutoSerializationExceptionShielding(objectType, typeCode, (e) => throw e);
        /// <summary>
        /// Зарегистрировать тип для логики автоматической сериализации
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="typeCode"></param>
        public static void RegisterTypeForAutoSerializationExceptionShielding<ObjectType>(int typeCode, Action<Exception>? exceptionCallback = null)
        {
            RegisterTypeForAutoSerializationExceptionShielding(typeof(ObjectType), typeCode, exceptionCallback);
        }    
        /// <summary>
        /// Зарегистрировать тип для логики автоматической сериализации
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="typeCode"></param>
        public static void RegisterTypeForAutoSerializationExceptionShielding(Type objectType, int typeCode, Action<Exception>? exceptionCallback = null)
        {
            try
            {
                ObjectTypeMapper.RegisterType(objectType, typeCode);
            }
            catch (Exception e)
            {
                exceptionCallback?.Invoke(e);
            }
        }
        /// Получить коллекцию зарегистрированных типов для автоматической сериализации
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ObjectTypeCodePair> GetRegisteredTypesForAutoSerialization() => ObjectTypeMapper.ObjectTypes;

        #endregion TypesRegistration

        #region TypesDescriptions

        /// <summary>
        /// Получить коллекцию описаний зарегистрированных обрабатываемых типов
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ObjectTypeDescription> GetTypesDescriptions() => 
            GetAllObjectsRecipes().Select(recipe => new ObjectTypeDescription(recipe));
        /// <summary>
        /// Получить коллекцию описаний зарегистрированных обрабатываемых типов в строковом представлении
        /// </summary>
        /// <returns></returns>
        public static string GetTypesDescriptionsString() => ObjectTypeDescription.SequenceToString(GetTypesDescriptions());
        /// <summary>
        /// Получить коллекцию описаний зарегистрированных обрабатываемых типов в бинарном представлении
        /// </summary>
        /// <returns></returns>
        public static byte[] GetTypesDescriptionsBinary()
        {
            BinaryArrayBuilder ab = new();

            foreach (var description in GetTypesDescriptions())
            {
                SerializeExceptionThrowing(ab, description);
            }

            return ab.GetByteArray();
        }
        /// <summary>
        /// Применить описания типов
        /// </summary>
        /// <param name="content"></param>
        public static void ApplyTypesDescriptionsExceptionThrowing(string content) => 
            ApplyTypesDescriptionsExceptionShielding(content, (e) => throw e);
        /// <summary>
        /// Применить описания типов
        /// </summary>
        /// <param name="content"></param>
        public static void ApplyTypesDescriptionsExceptionShielding(string content, Action<Exception>? exceptionCallback = null)
        {
            foreach (var description in ObjectTypeDescription.ParseSequence(content, exceptionCallback))
            {
                if (description is null)
                {
                    continue;
                }

                try
                {
                    description.ApplyDescription();
                }
                catch (Exception e)
                {
                    exceptionCallback?.Invoke(e);
                }
            }
        }
        /// <summary>
        /// Применить описания типов
        /// </summary>
        /// <param name="binaryData"></param>
        public static void ApplyTypesDescriptionsExceptionThrowing(byte[] binaryData) => 
            ApplyTypesDescriptionsExceptionShielding(binaryData, (e) => throw e);
        /// <summary>
        /// Применить описания типов
        /// </summary>
        /// <param name="binaryData"></param>
        public static void ApplyTypesDescriptionsExceptionShielding(byte[] binaryData, Action<Exception>? exceptionCallback = null)
        {
            BinaryArrayReader ar = new BinaryArrayReader(binaryData);

            while (!ar.IsEndOfCollection)
            {
                try
                {
                    var description = DeserializeExceptionThrowing<ObjectTypeDescription>(ar);

                    description?.ApplyDescription();
                }
                catch (Exception e)
                {
                    exceptionCallback?.Invoke(e);
                }
            }
        }

        #endregion TypesDescriptions
    }
}
