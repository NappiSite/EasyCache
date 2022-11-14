using System;

namespace NappiSite.EasyCache
{
    internal sealed class SerializedMemoryCache : EasyMemoryCache
    {
        public override void Insert(string key, object value, DateTimeOffset absoluteExpiration)
        {
            value = FormatValue(value);
            base.Insert(key, value,absoluteExpiration);
        }
        
        public override object Get(string key) => !(base.Get(key) is byte[] byteArray) ? base.Get(key) : SerializationHelper.Deserialize(byteArray);

        private static object FormatValue(object value)
        {
            switch (value)
            {
                case ValueType _:
                case string _:           
                    return value;
                default:
                    return SerializationHelper.Serialize(value);                   
            }
        }
    }
}
