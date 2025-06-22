using BinarySerializerLibrary.Attributes;
using BinarySerializerLibrary.Base;
using BinarySerializerLibrary.ObjectSerializationRecipes;
using BinarySerializerLibrary.Serializers;
using System.IO;

namespace BinarySerializerLibrary
{
    public class BinarySerializer
    {
        /// <summary>
        /// Преобразовать объект в массив байт
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="obj"></param>
        /// <param name="exceptionCallback"></param>
        /// <returns></returns>
        public static byte[] SerializeExceptionThrowing<TObject>(TObject? obj)
            where TObject : class
        {
            return SerializeExceptionShielding(obj, (e) => throw e);
        }
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
        public static void SerializeExceptionThrowing<TObject>(BinaryArrayBuilder binaryBuilder, TObject? obj)
            where TObject : class
        {
            SerializeExceptionShielding(binaryBuilder, obj, (e) => throw e);
        }
        /// <summary>
        /// Преобразовать объект в массив байт
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="obj"></param>
        /// <param name="exceptionCallback"></param>
        /// <returns></returns>
        public static void SerializeExceptionShielding<TObject>(BinaryArrayBuilder binaryBuilder, TObject? obj, Action<Exception>? exceptionCallback = null)
            where TObject : class
        {
            if (binaryBuilder is null)
            {
                exceptionCallback?.Invoke(new ArgumentNullException(nameof(binaryBuilder)));

                return;
            }

            // Объект внутреннего построения бинарного массива
            BinaryArrayBuilder internalBinaryBuilder;
            // В случае, если был передан еше не заполненный объект, то используем его
            if (binaryBuilder.RealBitLength == 0)
            {
                internalBinaryBuilder = binaryBuilder;
            }
            else
            {
                internalBinaryBuilder = new BinaryArrayBuilder();
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
        /// <summary>
        /// Получить объект из массива байт
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="content"></param>
        /// <param name="exceptionCallback"></param>
        /// <returns></returns>
        public static TObject? DeserializeExceptionThrowing<TObject>(byte[]? content)
            where TObject : class
        {
            return DeserializeExceptionShielding<TObject>(content, (e) => throw e);
        }
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
        /// Получить объект из массива байт
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="content"></param>
        /// <param name="exceptionCallback"></param>
        /// <returns></returns>
        public static object? DeserializeExceptionThrowing(byte[]? content, Type? objectType)
        {
            return DeserializeExceptionShielding(content, objectType, (e) => throw e);
        }
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
        public static TObject? DeserializeExceptionThrowing<TObject>(BinaryArrayReader binaryReader)
            where TObject : class
        {
            return DeserializeExceptionShielding<TObject>(binaryReader, (e) => throw e);
        }
        /// <summary>
        /// Получить объект с помощью байтового обработчика
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="content"></param>
        /// <param name="exceptionCallback"></param>
        /// <returns></returns>
        public static TObject? DeserializeExceptionShielding<TObject>(BinaryArrayReader binaryReader, Action<Exception>? exceptionCallback = null)
            where TObject : class
        {
            return DeserializeExceptionShielding(binaryReader, typeof(TObject), exceptionCallback) as TObject;
        }
        /// <summary>
        /// Получить объект с помощью байтового обработчика
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="content"></param>
        /// <param name="exceptionCallback"></param>
        /// <returns></returns>
        public static object? DeserializeExceptionThrowing(BinaryArrayReader binaryReader, Type? objectType)
        {
            return DeserializeExceptionShielding(binaryReader, objectType, (e) => throw e);
        }
        /// <summary>
        /// Получить объект с помощью байтового обработчика
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="content"></param>
        /// <param name="exceptionCallback"></param>
        /// <returns></returns>
        public static object? DeserializeExceptionShielding(BinaryArrayReader binaryReader, Type? objectType, Action<Exception>? exceptionCallback = null)
        {
            if (binaryReader is null)
            {
                exceptionCallback?.Invoke(new ArgumentNullException(nameof(binaryReader)));

                return null;
            }

            if (objectType is null)
            {
                exceptionCallback?.Invoke(new ArgumentNullException(nameof(objectType)));

                return null;
            }

            try
            {
                var obj = ObjectSerializer.DeserializeObject(binaryReader, objectType);

                return obj;
            }
            catch (Exception e)
            {
                exceptionCallback?.Invoke(e);

                return null;
            }
        }
        /// <summary>
        /// Подготовить рецепт сериализации типа объекта
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="exceptionCallback"></param>
        public static void CookObjectRecipeExceptionThrowing<ObjectType>()
            where ObjectType : class
        {
            CookObjectRecipeExceptionShielding<ObjectType>((e) => throw e);
        }
        /// <summary>
        /// Подготовить рецепт сериализации типа объекта
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="exceptionCallback"></param>
        public static void CookObjectRecipeExceptionShielding<ObjectType>(Action<Exception>? exceptionCallback = null)
            where ObjectType : class
        {
            try
            {
                ObjectSerializationRecipesMapper.GetRecipe(typeof(ObjectType));
            }
            catch (Exception e)
            {
                exceptionCallback?.Invoke(e);

                return;
            }
        }
        /// <summary>
        /// Подготовить рецепт сериализации типа объекта
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="exceptionCallback"></param>
        public static void CookObjectRecipeExceptionThrowing(Type? objectType)
        {
            CookObjectRecipeExceptionShielding(objectType, (e) => throw e);
        }
        /// <summary>
        /// Подготовить рецепт сериализации типа объекта
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="exceptionCallback"></param>
        public static void CookObjectRecipeExceptionShielding(Type? objectType, Action<Exception>? exceptionCallback = null)
        {
            if (objectType is null)
            {
                exceptionCallback?.Invoke(new ArgumentNullException(nameof(objectType)));

                return;
            }

            try
            {
                ObjectSerializationRecipesMapper.GetRecipe(objectType);
            }
            catch (Exception e)
            {
                exceptionCallback?.Invoke(e);

                return;
            }
        }
    }
}
