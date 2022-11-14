using System;
using System.Configuration;

namespace NappiSite.EasyCache
{
    internal static class CacheProviderFactory
    {
        public static ICacheProvider GetCache()
        {
            var t = GetProviderType();
            return GetCache(t);
        }

        internal static ICacheProvider GetCache(Type type)
        {
            return GetProvider(type) ?? new EasyMemoryCache();
        }

        private static ICacheProvider GetProvider(Type t)
        {
            if (t == null) return null;

            return Activator.CreateInstance(t) as ICacheProvider;
        }

        internal static Type GetProviderType()
        {
            var cacheType = ConfigurationManager.AppSettings["easyCache.cacheProviderType"];
            return string.IsNullOrEmpty(cacheType) ? null : Type.GetType(cacheType, true);
        }
    }
}
