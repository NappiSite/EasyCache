using System;
using System.Runtime.Caching;

namespace NappiSite.EasyCache
{
    internal sealed class EasyMemoryCache : ICacheProvider
    {
        private static readonly MemoryCache _cache = MemoryCache.Default;

        public void Insert(string key, object value, DateTimeOffset absoluteExpiration)
        {
            var item = new CacheItem(key, value);
            var policy = new CacheItemPolicy() { AbsoluteExpiration = absoluteExpiration };
            _cache.Add(item, policy);
        }

        public void Remove(string key) => _cache.Remove(key);

        public object Get(string key) => _cache.Get(key);
    }
}
