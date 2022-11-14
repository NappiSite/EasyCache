using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NappiSite.EasyCache.Tests
{
    [TestClass]
    public class EasyMemoryCacheTester
    {
        [TestMethod]
        public void Insert_ExistsInCache()
        {
            // arrange 
            const string KEY = "testKey";
            var cache = new EasyMemoryCache();

            // act
            cache.Insert(KEY, "Some String",DateTime.Now.AddSeconds(1));
            var result = cache.Get(KEY);

            // assert 
            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual( "Some String", (string)result);
        }

        [TestMethod]
        public void Insert_ExistsInCache_ThenExpires()
        {
            // arrange 
            const string KEY = "testKey2";
            var cache = new EasyMemoryCache();

            // act
            cache.Insert(KEY, "Some String", DateTime.Now.AddMilliseconds(10));
            Thread.Sleep(20);
            var result = cache.Get(KEY);

            // assert 
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Remove_IsNull()
        {
            // arrange 
            const string KEY = "testKey3";
            var cache = new EasyMemoryCache();

            // act
            cache.Insert(KEY, "Some String", DateTime.Now.AddMilliseconds(10));
            cache.Remove(KEY);
            var result = cache.Get(KEY);

            // assert 
            Assert.IsNull(result);
        }
    }
}
