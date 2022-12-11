using System;

namespace NappiSite.EasyCache
{
    public class NoCache : ICacheProvider
    {
        public void Insert(string key, object value, DateTimeOffset absoluteExpiration)
        {
            //Do nothing
        }

        public void Remove(string key)
        {
            //Do nothing
        }

        public object Get(string key)
        {
            return null;
        }
    }
}