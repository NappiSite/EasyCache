using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Text;

namespace NappiSite.EasyCache
{
  public static class Cache
  {
    private static readonly ICacheProvider cache = new CacheFactory().GetCache();
    private static readonly int defaultExpirationSeconds = Cache.GetDefaultExpirationSeconds();

    private static int GetDefaultExpirationSeconds()
    {
      int result;
      return ConfigurationManager.GetSection("easyCacheSettings") is NameValueCollection section && int.TryParse(section["defaultExpirationSeconds"], out result) ? result : 0;
    }

    public static string GetCacheKey(string cacheKey, params object[] args)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendFormat("ez_{0}", (object) cacheKey);
      foreach (object obj in args)
        stringBuilder.AppendFormat("_{0}", obj);
      return stringBuilder.ToString();
    }

    public static void Add(string cacheKey, object objectToCache) => Cache.Add(cacheKey, objectToCache, 0);

    public static void Add(string cacheKey, object objectToCache, int expirationSeconds)
    {
      if (objectToCache == null)
      {
        Cache.Remove(cacheKey);
      }
      else
      {
        if (expirationSeconds <= 0)
          expirationSeconds = Cache.defaultExpirationSeconds;
        if (expirationSeconds > 0)
          Cache.cache.Insert(cacheKey, objectToCache, DateTime.Now.AddSeconds((double) expirationSeconds));
        else
          Cache.cache.Insert(cacheKey, objectToCache);
      }
    }

    public static bool Exists(string cacheKey) => Cache.Get(cacheKey) != null;

    public static bool Exists<T>(string cacheKey) where T : class => (object) (Cache.Get(cacheKey) as T) != null;

    public static void Remove(string cacheKey) => Cache.cache.Remove(cacheKey);

    public static object Get(string cacheKey) => Cache.cache[cacheKey];

    public static T Get<T>(string cacheKey, Func<T> method)
    {
      object objectToCache = Cache.Get(cacheKey);
      if (objectToCache == null)
      {
        objectToCache = (object) method();
        Cache.Add(cacheKey, objectToCache);
      }
      return (T) objectToCache;
    }
  }
}
