using System;
using System.Collections.Specialized;
using System.Configuration;

namespace NappiSite.EasyCache
{
  public sealed class CacheFactory
  {
    public ICacheProvider GetCache() => GetCache(CacheFactory.GetProviderType());

    public ICacheProvider GetCache(Type type) => CacheFactory.GetProvider(type) ?? (ICacheProvider) new MemoryCache();

    public ICacheProvider GetCache<T>() where T : ICacheProvider => this.GetCache(typeof (T));

    private static ICacheProvider GetProvider(Type t) => (object) t == null ? (ICacheProvider) null : Activator.CreateInstance(t) as ICacheProvider;

    private static Type GetProviderType()
    {
      string typeName = (ConfigurationManager.GetSection("easyCacheSettings") as NameValueCollection)["cacheProviderType"];
      return !string.IsNullOrEmpty(typeName) ? Type.GetType(typeName, true) : (Type) null;
    }
  }
}
