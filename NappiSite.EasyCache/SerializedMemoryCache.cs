using System;
using System.Runtime.Caching;
using System.Xml.Schema;

namespace NappiSite.EasyCache
{
    public sealed class SerializedMemoryCache : ICacheProvider
    {
        private static readonly MemoryCache _cache = new MemoryCache();

        public void Insert(string key, object value, DateTime absoluteExpiration)
        {
            value = GetValue(value);
            _cache.Insert(key, value,absoluteExpiration);
        }

        private static object GetValue(object value)
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
            set => _cache.Insert(key, value);
        }

        public object Get(string key) => !(_cache.Get(key) is byte[] numArray) ? _cache.Get(key) : SerializationHelper.Deserialize(numArray);

        public void Insert(string key, object value)
        {
            value = GetValue(value);
            _cache.Insert(key, value);
        }
    }
}
