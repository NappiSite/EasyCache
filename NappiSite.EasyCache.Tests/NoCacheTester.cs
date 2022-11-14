using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NappiSite.EasyCache.Tests
{
    [TestClass]
    public  class NoCacheTester
    {
        [TestMethod]
        public void Insert_String_DoesNothing()
        {
            // arrange 
            const string KEY = "testKey";
            var cache = new NoCache();
            const string VAL = "Some String";

            // act
            cache.Insert(KEY, VAL, DateTime.Now.AddSeconds(1));
            var result = cache.Get(KEY);

            // assert 
            Assert.IsNull(result);
        }
    }
}
