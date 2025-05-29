using BinarySerializerLibrary.Attributes;
using BinarySerializerLibrary.Base;
using BinarySerializerLibrary.ObjectSerializationRecipes;
using BinarySerializerLibrary.Serializers;

namespace BinarySerializerLibrary
{
    public class BinarySerializer
    {
        public static byte[] Serialize<TObject>(TObject obj, Action<Exception>? exceptionCallback = null)
            where TObject : class
        {
            var binaryBuilder = new BinaryArrayBuilder();

            try
            {
                ComplexTypeSerializerMapper.SerializeObject(new BinaryTypeObjectAttribute(), obj, binaryBuilder);
            }
            catch (Exception e)
            {
                binaryBuilder.Clear();

                exceptionCallback?.Invoke(e);
            }

            return binaryBuilder.GetByteArray();
        }

        public static TObject? Deserialize<TObject>(byte[]? content, Action<Exception>? exceptionCallback = null)
            where TObject : class
        {
            var binaryReader = new BinaryArrayReader(content);

            try
            {
                return ComplexTypeSerializerMapper.DeserializeObject<TObject>(new BinaryTypeObjectAttribute(), typeof(TObject), binaryReader);
            }
            catch (Exception e)
            {
                exceptionCallback?.Invoke(e);

                return null;
            }
        }
    }
}
