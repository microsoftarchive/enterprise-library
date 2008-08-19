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

using System;
using System.Configuration;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests
{
    [TestClass]
    public class CacheManagerFactoryFixture
    {
        static CacheManagerFactory factory;

        [TestInitialize]
        public void CreateFactory()
        {
            factory = new CacheManagerFactory(TestConfigurationSource.GenerateConfiguration());
        }

        [TestMethod]
        public void CreateNamedCacheInstance()
        {
            ICacheManager cache = factory.Create("InMemoryPersistence");
            Assert.IsNotNull(cache, "Should have created caching instance through factory");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void WillThrowExceptionIfCannotFindCacheInstance()
        {
            factory.Create("ThisIsABadName");

            Assert.Fail("Should have thrown ConfigurationErrorsException");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WillThrowExceptionIfInstanceNameIsNull()
        {
            factory.Create(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WillThrowExceptionIfInstanceNameIsEmptyString()
        {
            factory.Create("");
        }

        [TestMethod]
        [ExpectedException(typeof(BuildFailedException))]
        public void WillThrowExceptionIfNullCacheStorage()
        {
            factory.Create("CacheManagerWithBadCacheStorageInstance");
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void WillThrowExceptionIfCannotCreateNamedStorageType()
        {
            factory.Create("CacheManagerWithBadStoreType");
        }

        [TestMethod]
        public void CallingSameFactoryTwiceReturnsSameInstance()
        {
            ICacheManager cacheOne = factory.Create("InMemoryPersistence");
            ICacheManager cacheTwo = factory.Create("InMemoryPersistence");
            Assert.AreSame(cacheOne, cacheTwo, "CacheManagerFactory should always return the same instance when using the same instance name");
        }

        [TestMethod]
        public void CallingDifferentFactoryTwiceReturnsDifferentInstances()
        {
            ICacheManager cacheOne = factory.Create("InMemoryPersistence");

            CacheManagerFactory secondFactory = new CacheManagerFactory(TestConfigurationSource.GenerateConfiguration());
            ICacheManager cacheTwo = secondFactory.Create("InMemoryPersistence");

            Assert.IsFalse(ReferenceEquals(cacheOne, cacheTwo), "Different factories should always return different instances for same instance name");
        }

        [TestMethod]
        public void CanCreateDefaultCacheManager()
        {
            ICacheManager cacheManager = factory.CreateDefault();
            Assert.IsNotNull(cacheManager);
        }

        [TestMethod]
        public void DefaultCacheManagerAndNamedDefaultInstanceAreSameObject()
        {
            ICacheManager defaultInstance = factory.CreateDefault();
            ICacheManager namedInstance = factory.Create("ShortInMemoryPersistence");

            Assert.AreSame(defaultInstance, namedInstance);
        }

        [TestMethod]
        public void CanCreateDifferentCacheManagerImplementation()
        {
            ICacheManager mockCacheManager = factory.Create("MockManager");
            Assert.IsInstanceOfType(mockCacheManager, typeof(MockCacheManager));
        }
    }
}