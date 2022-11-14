using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NappiSite.EasyCache.Tests
{
    [TestClass]
    public class CacheProviderFactoryTester
    {
        [TestMethod]
        public void GetCache_DefaultToMemoryCache()
        {
            // arrange 
            
            // act
            var result = CacheProviderFactory.GetCache();

            // assert 
            Assert.IsInstanceOfType(result,typeof(EasyMemoryCache));
        }

        [TestMethod]
        public void GetCache_GivenType_InstantiatesThatType()
        {
            // arrange 

            // act
            var result = CacheProviderFactory.GetCache(typeof(NoCache));

            // assert 
            Assert.IsInstanceOfType(result, typeof(NoCache));
        }

        [TestMethod]
        public void GetProviderType_FromConfig_InstantiatesThatType()
        {
            // arrange 
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            // act
            var result = CacheProviderFactory.GetProviderType();

            // assert 
            Assert.IsInstanceOfType(result, typeof(EasyMemoryCache));
        }
    }
}
