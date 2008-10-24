//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests
{
    [TestClass]
    public class CacheFactoryFixture
    {
        [TestMethod]
        public void GetDefaultCacheManagerTest()
        {
            ICacheManager cacheManager = CacheFactory.GetCacheManager();
            Assert.IsNotNull(cacheManager);
        }

        [TestMethod]
        public void GetCacheManagerTest()
        {
            ICacheManager cacheManager = CacheFactory.GetCacheManager("InIsoStorePersistenceWithNullEncryption");
            Assert.IsNotNull(cacheManager);
        }

        [TestMethod]
        public void GetMockManagerTest()
        {
            MockCacheManager cacheManager = CacheFactory.GetCacheManager("MockManager") as MockCacheManager;
            Assert.IsNotNull(cacheManager);
        }
    }
}
