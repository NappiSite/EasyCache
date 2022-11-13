using System;
using System.Threading.Tasks;

namespace NappiSite.EasyCache
{
    public interface ICacheManager
    {   
        void Insert(string cacheKey, object value);
        bool Exists(string cacheKey);
        void Remove(string cacheKey);
        void Update(string cacheKeys, object value);
        object Get(string cacheKey);
        T GetOrAdd<T>(string cacheKey, Func<T> method);
        Task<T> GetOrAddAsync<T>(string cacheKey, Func<Task<T>> method);
    }
}
