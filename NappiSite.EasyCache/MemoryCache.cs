using System;
using System.Runtime.Caching;

namespace NappiSite.EasyCache
{
    public sealed class MemoryCache : ICacheProvider
    {
        private static readonly System.Runtime.Caching.MemoryCache _cache = System.Runtime.Caching.MemoryCache.Default;

        public void Insert(string key, object value, DateTime absoluteExpiration)
        {
            var item = new CacheItem(key, value);
            var policy = new CacheItemPolicy() { AbsoluteExpiration = absoluteExpiration };
            _cache.Add(item, policy);
        }

        public void Insert(string key, object value) => new CacheItem(key, value);

        public void Remove(string key) => _cache.Remove(key);

        public object this[string key]
        {
            get => Get(key);
            set => Insert(key, value);
        }

        public object Get(string key) => _cache.Get(key);
    }
}
