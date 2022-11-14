using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Serialization;

namespace NappiSite.EasyCache.Tests
{
    [TestClass]
    public class SerializedMemoryCacheTester
    {
        [TestMethod]
        public void Insert_string_ExistsInCache()
        {
            // arrange 
            const string KEY = "testKey";
            var cache = new SerializedMemoryCache();
            const string VAL = "Some String";

            // act

            cache.Insert(KEY, VAL, DateTime.Now.AddSeconds(1));
            var result = cache.Get(KEY);

            // assert 
            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual(VAL, (string)result);
            Assert.AreSame(VAL, result);
        }

        [TestMethod]
        public void Insert_Object_ExistsInCache()
        {
            // arrange 
            const string KEY = "testKey4";
            var cache = new SerializedMemoryCache();
            var val = new DummyObject("Some String");

            // act
            cache.Insert(KEY, val, DateTime.Now.AddSeconds(1));
            var result = cache.Get(KEY);

            // assert 
            Assert.AreNotSame(val, result);
        }

        [TestMethod]
        public void Insert_NotSerializable_Throws()
        {
            // arrange 
            const string KEY = "testKey4";
            var cache = new SerializedMemoryCache();
            var val = new {Name="Some String"};

            // act
            Assert.ThrowsException<SerializationException>(()=>cache.Insert(KEY, val, DateTime.Now.AddSeconds(1)));

        }

        [TestMethod]
        public void Insert_ExistsInCache_ThenExpires()
        {
            // arrange 
            const string KEY = "testKey2";
            var cache = new SerializedMemoryCache();

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
            var cache = new SerializedMemoryCache();

            // act
            cache.Insert(KEY, "Some String", DateTime.Now.AddMilliseconds(10));
            cache.Remove(KEY);
            var result = cache.Get(KEY);

            // assert 
            Assert.IsNull(result);
        }
    }
}
