using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NappiSite.EasyCache
{
    public interface ICacheManager
    {   
        void Insert(string cacheKey, object value, string tag);
        bool Exists(string cacheKey);
        void Remove(string cacheKey);
        void Update(string cacheKeys, object value);
        object Get(string cacheKey);
        T GetOrAdd<T>(string cacheKey, Func<T> method);
        Task<T> GetOrAddAsync<T>(string cacheKey, Func<Task<T>> method);
        T GetOrAdd<T>(string cacheKey, Func<T> method, string tag);
        T GetOrAdd<T>(Func<T> method, params object[] args);
    }
}
