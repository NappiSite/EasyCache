using System;

namespace NappiSite.EasyCache
{
    public sealed class SerializedMemoryCache : ICacheProvider
    {
        private static readonly MemoryCache _cache = new MemoryCache();

        public void Insert(string key, object value, DateTime absoluteExpiration)
        {
            value = FormatValue(value);
            _cache.Insert(key, value,absoluteExpiration);
        }

        public void Insert(string key, object value)
        {
            value = FormatValue(value);
            _cache.Insert(key, value);
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

        public object this[string key]
        {
            get => Get(key);
            set => Insert(key, value);
        }

        public object Get(string key) => !(_cache.Get(key) is byte[] byteArray) ? _cache.Get(key) : SerializationHelper.Deserialize(byteArray);


    }
}
