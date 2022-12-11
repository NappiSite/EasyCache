using System;
using System.Threading.Tasks;

namespace NappiSite.EasyCache
{
    public interface ICacheManager
    {
        void Insert(string cacheKey, object value, DateTimeOffset absoluteExpiration);
        bool Exists(string cacheKey);
        void Remove(string cacheKey);
        void Update(string cacheKeys, object value, DateTimeOffset absoluteExpiration);
        object Get<T>(string cacheKey);
        T GetOrAdd<T>(string cacheKey, Func<T> method, DateTimeOffset absoluteExpiration);
        Task<T> GetOrAddAsync<T>(string cacheKey, Func<Task<T>> method, DateTimeOffset absoluteExpiration);
    }
}