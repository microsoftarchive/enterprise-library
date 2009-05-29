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
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.ServiceLocation;

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
        [ExpectedException(typeof(ActivationException))]
        public void WillThrowExceptionIfCannotFindCacheInstance()
        {
            factory.Create("ThisIsABadName");

            Assert.Fail("Should have thrown ConfigurationErrorsException");
        }

        [TestMethod]
        [ExpectedException(typeof(ActivationException))]
        public void WillThrowExceptionIfCannotFindSection()
        {
            CacheManagerFactory factory2 = new CacheManagerFactory(new DictionaryConfigurationSource());

            factory2.Create("some name");

            Assert.Fail("Should have thrown ConfigurationErrorsException");
        }

        [TestMethod]
        [ExpectedException(typeof(ActivationException))]
        public void WillThrowExceptionIfCannotDefault()
        {
            DictionaryConfigurationSource source = new DictionaryConfigurationSource();
            source.Add(CacheManagerSettings.SectionName, new CacheManagerSettings());
            CacheManagerFactory factory2 = new CacheManagerFactory(source);

            factory2.CreateDefault();

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
        [ExpectedException(typeof(ActivationException))]
        public void WillThrowExceptionIfNullCacheStorage()
        {
            factory.Create("CacheManagerWithBadCacheStorageInstance");
        }

        [TestMethod]
        [ExpectedException(typeof(ActivationException))]
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
