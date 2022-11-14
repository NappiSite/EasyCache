using System.Collections.Concurrent;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NappiSite.EasyCache.Tests
{
    [TestClass]
    public class CacheTester
    {
        [TestMethod]
        public void GetOrAdd_NewValue_AddsNew()
        {
            // arrange 
            var cacheManager = new Cache();
            var key = Guid.NewGuid().ToString();
            const string EXPECTED = "This is a real value";
            var expiration = DateTime.Now.AddSeconds(1);

            // act
            var result = cacheManager.GetOrAdd(key, () => EXPECTED,expiration);
            
            // assert 
            Assert.AreEqual(EXPECTED,result);
        }

        [TestMethod]
        public async void GetOrAddAsync_NewValue_AddsNew()
        {
            // arrange 
            var cacheManager = new Cache();
            var key = Guid.NewGuid().ToString();
            const string EXPECTED = "This is a real value";
            var expiration = DateTime.Now.AddSeconds(1);

            // act
            var result = await cacheManager.GetOrAddAsync(key, () => Task.FromResult(EXPECTED),expiration);
            
            // assert 
            Assert.AreEqual(EXPECTED,result);
        }

        [TestMethod]
        public async Task GetOrAddAsync_PreExisting_ReturnsSame()
        {
            // arrange 
            var cacheManager = new Cache();
            var key = Guid.NewGuid().ToString();
            const string EXPECTED = "This is a real value";
            const string NOT_EXPECTED = "This is not a real value";
            var expiration = DateTime.Now.AddSeconds(1);

            // act
            var result1 = await cacheManager.GetOrAddAsync(key, () => Task.FromResult(EXPECTED),expiration);
            var result2 = await cacheManager.GetOrAddAsync(key, () => Task.FromResult(NOT_EXPECTED),expiration);
            
            // assert 
            Assert.AreEqual(EXPECTED,result1);
            Assert.AreSame(result1,result2);
        }

        [TestMethod]
        public void GetOrAdd_PreExisting_ReturnsSame()
        {
            // arrange 
            var cacheManager = new Cache();
            var key = Guid.NewGuid().ToString();
            const string EXPECTED = "This is a real value";
            const string NOT_EXPECTED = "This is not a real value";
            var expiration = DateTime.Now.AddSeconds(1);

            // act
            var result1 = cacheManager.GetOrAdd(key, () => EXPECTED,expiration);
            var result2 = cacheManager.GetOrAdd(key, () => NOT_EXPECTED,expiration);
            
            // assert 
            Assert.AreEqual(EXPECTED,result1);
            Assert.AreSame(result1,result2);
        }

        [TestMethod]
        public void GetOrAdd_Expires_ReturnsDifferent()
        {
            // arrange 
            var cacheManager = new Cache();
            var key = Guid.NewGuid().ToString();
            const string EXPECTED = "This is a real value";
            const string NOT_EXPECTED = "This is not a real value";
            var expiration = DateTime.Now.AddMilliseconds(1);

            // act
            var result1 = cacheManager.GetOrAdd(key, () => EXPECTED,expiration);
            Thread.Sleep(5);
            var result2 = cacheManager.GetOrAdd(key, () => NOT_EXPECTED,expiration);
            
            // assert 
            Assert.AreEqual(EXPECTED,result1);
            Assert.AreEqual(NOT_EXPECTED,result2);
            Assert.AreNotSame(result1,result2);
        }

        [TestMethod]
        public async Task GetOrAddAsync_Expires_ReturnsDifferent()
        {
            // arrange 
            var cacheManager = new Cache();
            var key = Guid.NewGuid().ToString();
            const string EXPECTED = "This is a real value";
            const string NOT_EXPECTED = "This is not a real value";
            var expiration = DateTime.Now.AddMilliseconds(1);

            // act
            var result1 = await cacheManager.GetOrAddAsync(key, () => Task.FromResult(EXPECTED),expiration);
            await Task.Delay(5);
            var result2 = await cacheManager.GetOrAddAsync(key, () => Task.FromResult(NOT_EXPECTED),expiration);
            
            // assert 
            Assert.AreEqual(EXPECTED,result1);
            Assert.AreEqual(NOT_EXPECTED,result2);
            Assert.AreNotSame(result1,result2);
        }

        [TestMethod]
        public void GetOrAdd_Parallel_HonorsLock()
        {
            // arrange 
            var cacheManager = new Cache();
            var key = Guid.NewGuid().ToString();
            var expiration = DateTime.Now.AddSeconds(1);
            var sameKeyCounter = new ConcurrentBag<int>();
            var uniqueKeyCounter = new ConcurrentBag<int>();
            var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9,10 };
            // act
            Parallel.ForEach(list, i =>
            {
                cacheManager.GetOrAdd(key, () =>
                {
                    sameKeyCounter.Add(1);
                    return i;
                }, expiration);

                cacheManager.GetOrAdd(i.ToString(), () =>
                {
                    uniqueKeyCounter.Add(1);
                    return i;
                }, expiration);

            });

            // assert 
            Assert.AreEqual(1,sameKeyCounter.Count);
            Assert.AreEqual(10,uniqueKeyCounter.Count);
        }

        [TestMethod]
        public void GetOrAddAsync_Parallel_HonorsLock()
        {
            // arrange 
            var cacheManager = new Cache();
            var key = Guid.NewGuid().ToString();
            var expiration = DateTime.Now.AddSeconds(1);
            var sameKeyCounter = new ConcurrentBag<int>();
            var uniqueKeyCounter = new ConcurrentBag<int>();
            var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9,10 };
            // act
            Parallel.ForEach(list, async i =>
            {
                await cacheManager.GetOrAddAsync(key, () =>
                {
                    sameKeyCounter.Add(1);
                    return Task.FromResult(i);
                }, expiration);

                await cacheManager.GetOrAddAsync($"{key}{i}", () =>
                {
                    uniqueKeyCounter.Add(1);
                    return Task.FromResult(i);
                }, expiration);

            });

            // assert 
            Assert.AreEqual(1,sameKeyCounter.Count);
            Assert.AreEqual(10,uniqueKeyCounter.Count);
        }

        [TestMethod]
        public void Get_IsSame()
        {
            // arrange 
            var cacheManager = new Cache();
            var key = Guid.NewGuid().ToString();
            const string EXPECTED = "This is a real value";
            var expiration = DateTime.Now.AddSeconds(1);

            // act
            var result1 = cacheManager.GetOrAdd(key, () => EXPECTED,expiration);
            var result2 = cacheManager.Get<string>(key);
            
            // assert 
            Assert.AreSame(result1,result2);
        }

        
        [TestMethod]
        public void Update_IsSame()
        {
            // arrange 
            var cacheManager = new Cache();
            var key = Guid.NewGuid().ToString();
            const string VALUE = "This is a real value";
            const string NEW_VALUE = "This is not a real value";
            var expiration = DateTime.Now.AddSeconds(1);

            // act
            cacheManager.GetOrAdd(key, () => VALUE,expiration);
            cacheManager.Update(key,NEW_VALUE,expiration);
            var result2 = cacheManager.Get<string>(key);

            // assert 
            Assert.AreSame(NEW_VALUE,result2);
        }

        [TestMethod]
        public void Exists_IsCorrect()
        {
            // arrange 
            var cacheManager = new Cache();
            var key = Guid.NewGuid().ToString();
            const string VALUE = "This is a real value";
            var expiration = DateTime.Now.AddSeconds(1);

            // act
            cacheManager.GetOrAdd(key, () => VALUE,expiration);
            var result=cacheManager.Exists(key);
            var result2 = cacheManager.Exists($"{key}1");

            // assert 
           Assert.IsTrue(result);
           Assert.IsFalse(result2);
        }

        [TestMethod]
        public void Remove_IsGone()
        {
            // arrange 
            var cacheManager = new Cache();
            var key = Guid.NewGuid().ToString();
            const string VALUE = "This is a real value";
            var expiration = DateTime.Now.AddSeconds(1);

            // act
            cacheManager.GetOrAdd(key, () => VALUE,expiration);
            var result=cacheManager.Exists(key);
            cacheManager.Remove(key);
            var result2 = cacheManager.Exists(key);

            // assert 
            Assert.IsTrue(result);
            Assert.IsFalse(result2);
        }
    }
    
}