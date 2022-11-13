using System;

namespace NappiSite.EasyCache
{
    public class NoCache : ICacheProvider
    {
        public void Insert(string key, object value)
        {
            //Do nothing
        }

        public void Insert(string key, object value, DateTime absoluteExpiration)
        {
            //Do nothing
        }

        public void Remove(string key)
        {
            //Do nothing
        }

        public object this[string key]
        {
            get { return Get(key); }
            set
            {
                //Do nothing
            }
        }

        public object Get(string key)
        {
            return null;
        }
    }
}
