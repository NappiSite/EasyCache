using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NappiSite.EasyCache.Tests
{
    [TestClass]
    public class CacheTester
    {
        [TestMethod]
        public void CacheLocksIsZeroAfterAddingItems()
        {
            // arrange 
            var cacheManager = new Cache(new MemoryCache());
            var preTestDictionaryCount = Cache.cacheLocks.Count;

            // act
            cacheManager.GetOrAdd("key", () => "d", "tag");

            // assert 
            Assert.AreEqual(0, preTestDictionaryCount);
            Assert.AreEqual(0, Cache.cacheLocks.Count);
        }
    }
}