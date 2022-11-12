using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NappiSite.EasyCache
{
  internal static class SerializationHelper
  {
    public static byte[] Serialize(object obj)
    {
      using (MemoryStream serializationStream = new MemoryStream())
      {
        new BinaryFormatter().Serialize((Stream) serializationStream, obj);
        return serializationStream.ToArray();
      }
    }

    public static object Deserialize(byte[] obj)
    {
      using (MemoryStream serializationStream = new MemoryStream())
      {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        serializationStream.Write(obj, 0, obj.Length);
        serializationStream.Position = 0L;
        return binaryFormatter.Deserialize((Stream) serializationStream);
      }
    }
  }
}
