using System;
using System.Runtime.Caching;

namespace NappiSite.EasyCache
{
    internal class EasyMemoryCache : ICacheProvider
    {
        private static readonly MemoryCache _cache = MemoryCache.Default;

        public virtual void Insert(string key, object value, DateTimeOffset absoluteExpiration)
        {
            var item = new CacheItem(key, value);
            var policy = new CacheItemPolicy { AbsoluteExpiration = absoluteExpiration };
            _cache.Set(item, policy);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public virtual object Get(string key)
        {
            return _cache.Get(key);
        }
    }
}