using System;
using System.Collections.Specialized;
using System.Configuration;

namespace NappiSite.EasyCache
{
    public sealed class CacheProviderFactory
    {
        public ICacheProvider GetCache()
        {
            var t = GetProviderType();
            return GetCache(t);
        }

        private static ICacheProvider GetCache(Type type)
        {
            return GetProvider(type) ?? new MemoryCache();
        }

        public ICacheProvider GetCache<T>()
            where T : ICacheProvider
        {
            return GetCache(typeof(T));
        }

        private static ICacheProvider GetProvider(Type t)
        {
            if (t == null) return null;

            return Activator.CreateInstance(t) as ICacheProvider;
        }

        private static Type GetProviderType()
        {
            var cacheType = ConfigurationManager.AppSettings["cacheProviderType"];
            return string.IsNullOrEmpty(cacheType) ? null : Type.GetType(cacheType, true);
        }
    }
}
