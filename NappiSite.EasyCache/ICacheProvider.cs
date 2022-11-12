using System;

namespace NappiSite.EasyCache
{
  public interface ICacheProvider
  {
    void Insert(string key, object value, DateTime absoluteExpiration);

    void Insert(string key, object value);

    void Remove(string key);

    object this[string key] { get; set; }

    object Get(string key);
  }
}
