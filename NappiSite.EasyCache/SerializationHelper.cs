using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NappiSite.EasyCache
{
    internal static class SerializationHelper
    {
        public static byte[] Serialize(object obj)
        {
            using (var serializationStream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(serializationStream, obj);
                return serializationStream.ToArray();
            }
        }

        public static object Deserialize(byte[] obj)
        {
            using (var serializationStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                serializationStream.Write(obj, 0, obj.Length);
                serializationStream.Position = 0L;
                return binaryFormatter.Deserialize(serializationStream);
            }
        }
    }
}