using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
[assembly: InternalsVisibleTo("NappiSite.EasyCache.Tests")]

namespace NappiSite.EasyCache
{
    public class Cache : ICacheManager
    {
        private const int DEFAULT_EXPIRATION_SECONDS = 3600;
        private readonly ICacheProvider cache;
        private static readonly object syncLock = new object();

        private static class TypeCache<T>
        {
            internal static string CacheKeyFormat { get; set; }
        }

        internal static readonly ConcurrentDictionary<string, object> cacheLocks =
            new ConcurrentDictionary<string, object>();

        public static Cache Default { get; } = new Cache(new CacheProviderFactory().GetCache());

        public Cache(ICacheProvider cacheProvider)
        {
            cache = cacheProvider;
        }

        internal ICacheProvider GetCache()
        {
            return cache;
        }

        public static string GetCacheKey(string prefix, string uniqueValue)
        {
            return $"{prefix}_{uniqueValue}".ToLower();
        }     

        public void Update(string cacheKey, object value)
        {
            var existingValue = cache.Get(cacheKey);
            if (existingValue != value)
            {
                if (value == null)
                    Remove(cacheKey);
                else
                    Insert(cacheKey, value);
            }
        }

        public void Insert(string cacheKey, object value, string tag = null)
        {
            if (value == null) return;

            var absoluteExpiration = GetAbsoluteExpiration();
      
             cache.Insert(cacheKey, value, absoluteExpiration);         
        }

        public bool Exists(string cacheKey)
        {
            return (GetByKey(cacheKey) != null);
        }

        public void Remove(string cacheKey)
        {
            cache.Remove(cacheKey);
        }

        private static DateTime GetAbsoluteExpiration()
        {
            return DateTime.Now.AddSeconds(DEFAULT_EXPIRATION_SECONDS);
        }


        public object Get(string cacheKey)
        {
            return (cache[cacheKey] is NullObject) ? null : cache[cacheKey];
        }

        private object GetByKey(string cacheKey)
        {
            return cache[cacheKey];
        }

        public T GetOrAdd<T>(string cacheKey, Func<T> method)
        {
            return GetOrAdd(cacheKey, method, null);
        }

        public async Task<T> GetOrAddAsync<T>(string cacheKey, Func<Task<T>> method)
        {
            var obj = GetByKey(cacheKey);
            if (obj == null)
            {
                obj = await method();
                Insert(cacheKey, obj ?? new NullObject());
            }

            return obj is T ? (T)obj : default;
        }

        public T GetOrAdd<T>(string cacheKey, Func<T> method, string tag)
        {
            var obj = GetByKey(cacheKey);
            if (obj == null)
            {
                lock (cacheLocks.GetOrAdd(cacheKey, new object()))
                {
                    obj = GetByKey(cacheKey);
                    if (obj == null)
                    {
                        obj = method.Invoke();
                        Insert(cacheKey, obj ?? new NullObject(), tag);
                    }
                }

                cacheLocks.TryRemove(cacheKey, out System.Object o);
            }

            return obj is T ? (T)obj : default(T);
        }

        public T GetOrAdd<T>(Func<T> method, params object[] args)
        {
            var objectType = typeof(T);
            var key = string.Format(GenerateCacheKeyFormat(objectType), args);
            return GetOrAdd(key, method);
        }

        public static string GenerateCacheKeyFormat(Type type)
        {
            return $"cm_{type.FullName.ToLower(CultureInfo.CurrentCulture)}_{{0}}";
        }

        public static string GenerateCacheKeyFormat<T>()
        {
            if (TypeCache<T>.CacheKeyFormat == null)
            {
                lock (syncLock)
                {
                    if (TypeCache<T>.CacheKeyFormat == null)
                    {
                        var type = typeof(T);
                        TypeCache<T>.CacheKeyFormat = $"cm_{type.FullName.ToLower(CultureInfo.CurrentCulture)}_{{0}}";
                    }
                }
            }
            return TypeCache<T>.CacheKeyFormat;
        }

        [Serializable]
        private class NullObject
        {
        }
    }
}
