using System;

namespace NappiSite.EasyCache
{
    public interface ICacheProvider
    {
        void Insert(string key, object value, DateTimeOffset absoluteExpiration);
        void Remove(string key);
        object Get(string key);
    }
}