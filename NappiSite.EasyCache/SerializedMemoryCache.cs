using System;

namespace NappiSite.EasyCache
{
    public sealed class SerializedMemoryCache : ICacheProvider
    {
        private static readonly EasyMemoryCache _cache = new EasyMemoryCache();

        public void Insert(string key, object value, DateTimeOffset absoluteExpiration)
        {
            value = FormatValue(value);
            _cache.Insert(key, value,absoluteExpiration);
        }
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

        public void Remove(string key) => _cache.Remove(key);

        public object Get(string key) => !(_cache.Get(key) is byte[] byteArray) ? _cache.Get(key) : SerializationHelper.Deserialize(byteArray);
    }
}
