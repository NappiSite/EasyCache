using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
[assembly: InternalsVisibleTo("NappiSite.EasyCache.Tests")]

namespace NappiSite.EasyCache
{
    public class Cache : ICacheManager
    {
        private readonly ICacheProvider _cache;
        private static readonly ConcurrentDictionary<string, object> _cacheLocks = new ConcurrentDictionary<string, object>();
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _asyncCacheLocks = new ConcurrentDictionary<string, SemaphoreSlim>();

        public Cache() : this(CacheProviderFactory.GetCache())
        {

        }

        internal Cache(ICacheProvider cacheProvider)
        {
            _cache = cacheProvider;
        }

        public void Update(string cacheKey, object value,DateTimeOffset absoluteExpiration)
        {
            var existingValue = _cache.Get(cacheKey);
            if (existingValue != value)
            {
                if (value == null)
                    Remove(cacheKey);
                else
                    Insert(cacheKey, value, absoluteExpiration);
            }
        }

        public void Insert(string cacheKey, object value, DateTimeOffset absoluteExpiration)
        {
            if (value == null) return;
            
            _cache.Insert(cacheKey, value, absoluteExpiration);
        }

        public bool Exists(string cacheKey)
        {
            return (_cache.Get(cacheKey) != null);
        }

        public void Remove(string cacheKey)
        {
            _cache.Remove(cacheKey);
        }
        public object Get<T>(string cacheKey)
        {
            var obj = (_cache.Get(cacheKey) is NullObject) ? null : _cache.Get(cacheKey);
            return obj is T o ? o : default;
        }
        
        public T GetOrAdd<T>(string cacheKey, Func<T> method,DateTimeOffset absoluteExpiration)
        {
            var obj = _cache.Get(cacheKey);
            if (obj == null)
            {
                try
                {
                    lock (_cacheLocks.GetOrAdd(cacheKey, new object()))
                    {
                        obj = _cache.Get(cacheKey);
                        if (obj == null)
                        {
                            obj = method.Invoke();
                            Insert(cacheKey, obj ?? new NullObject(), absoluteExpiration);
                        }
                    }
                }
                finally
                {
                    _cacheLocks.TryRemove(cacheKey, out _);
                }
            }

            return obj is T o ? o : default;
        }

        public async Task<T> GetOrAddAsync<T>(string cacheKey, Func<Task<T>> method,DateTimeOffset absoluteExpiration)
        {
            var obj = _cache.Get(cacheKey);
            if (obj == null)
            {
                var lck = _asyncCacheLocks.GetOrAdd(cacheKey, new SemaphoreSlim(1, 1));
                try
                {
                    await lck.WaitAsync();

                    obj = _cache.Get(cacheKey);
                    if (obj == null)
                    {
                        obj = await method();
                        Insert(cacheKey, obj ?? new NullObject(), absoluteExpiration);
                    }
                }
                finally
                {
                    lck?.Release();
                    _asyncCacheLocks.TryRemove(cacheKey, out _);
                }
            }

            return obj is T o ? o : default;
        }

        [Serializable]
        private class NullObject
        {
        }
    }
}
