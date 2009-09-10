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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using System.Collections.Specialized;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.Configuration.Fluent
{
    [TestClass]
    public class When_CallingForCustomCachgeManagerNamedOnCachingConfigurationWithNullName : Given_CachingSettingsInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_ThrowsArgumentException()
        {
            base.CachingConfiguration.ForCustomCacheManagerNamed(null, GetType(), new NameValueCollection());
        }
    }

    [TestClass]
    public class When_CallingForCustomCachgeManagerNamedOnCachingConfigurationWithNullType : Given_CachingSettingsInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_ThrowsArgumentNullException()
        {
            base.CachingConfiguration.ForCustomCacheManagerNamed("name", null, new NameValueCollection());
        }
    }

    [TestClass]
    public class When_CallingForCustomCachgeManagerNamedOnCachingConfigurationWithNulAttributes : Given_CachingSettingsInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_ThrowsArgumentNullException()
        {
            base.CachingConfiguration.ForCustomCacheManagerNamed("name", GetType(), null);
        }
    }

    [TestClass]
    public class When_CallingForCustomCachgeManagerNamedOnCachingConfiguration : Given_CachingSettingsInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.CachingConfiguration.ForCustomCacheManagerNamed("custom cache manager name", typeof(TestCustomCacheManager));
        }

        [TestMethod]
        public void Then_CustomCacheManagerConfigurationIsContainedInCachingSettings()
        {
            var cachingSettings = GetCacheManagerSettings();
            Assert.IsTrue(cachingSettings.CacheManagers.OfType<CustomCacheManagerData>().Any());
        }


        [TestMethod]
        public void Then_CustomCacheManagerConfigurationHasAppropriateName()
        {
            var cachingSettings = GetCacheManagerSettings();
            var cacheManager = cachingSettings.CacheManagers.OfType<CustomCacheManagerData>().First();

            Assert.AreEqual("custom cache manager name", cacheManager.Name);
        }
        [TestMethod]
        public void Then_CustomCacheManagerConfigurationHasAppropriateType()
        {
            var cachingSettings = GetCacheManagerSettings();
            var cacheManager = cachingSettings.CacheManagers.OfType<CustomCacheManagerData>().First();

            Assert.AreEqual(typeof(TestCustomCacheManager), cacheManager.Type);
        }

        [TestMethod]
        public void Then_CustomCacheManagerConfigurationHasNoAttributes()
        {
            var cachingSettings = GetCacheManagerSettings();
            var cacheManager = cachingSettings.CacheManagers.OfType<CustomCacheManagerData>().First();

            Assert.AreEqual(0, cacheManager.Attributes.Count);
        }
    }
    
    [TestClass]
    public class When_CallingForCustomCachgeManagerNamedOnCachingConfigurationGeneric : Given_CachingSettingsInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.CachingConfiguration.ForCustomCacheManagerNamed<TestCustomCacheManager>("custom cache manager name");
        }

        [TestMethod]
        public void Then_CustomCacheManagerConfigurationIsContainedInCachingSettings()
        {
            var cachingSettings = GetCacheManagerSettings();
            Assert.IsTrue(cachingSettings.CacheManagers.OfType<CustomCacheManagerData>().Any());
        }


        [TestMethod]
        public void Then_CustomCacheManagerConfigurationHasAppropriateName()
        {
            var cachingSettings = GetCacheManagerSettings();
            var cacheManager = cachingSettings.CacheManagers.OfType<CustomCacheManagerData>().First();

            Assert.AreEqual("custom cache manager name", cacheManager.Name);
        }
        [TestMethod]
        public void Then_CustomCacheManagerConfigurationHasAppropriateType()
        {
            var cachingSettings = GetCacheManagerSettings();
            var cacheManager = cachingSettings.CacheManagers.OfType<CustomCacheManagerData>().First();

            Assert.AreEqual(typeof(TestCustomCacheManager), cacheManager.Type);
        }

        [TestMethod]
        public void Then_CustomCacheManagerConfigurationHasNoAttributes()
        {
            var cachingSettings = GetCacheManagerSettings();
            var cacheManager = cachingSettings.CacheManagers.OfType<CustomCacheManagerData>().First();

            Assert.AreEqual(0, cacheManager.Attributes.Count);
        }
    }
    
    [TestClass]
    public class When_CallingForCustomCachgeManagerNamedOnCachingConfigurationPassingAttributesGeneric : Given_CachingSettingsInConfigurationSourceBuilder
    {
        NameValueCollection attributes;

        protected override void Act()
        {
            attributes = new NameValueCollection();
            attributes.Add("key1", "value1");
            attributes.Add("key2", "value2");

            base.CachingConfiguration.ForCustomCacheManagerNamed<TestCustomCacheManager>("custom cache manager name", attributes);
        }

        [TestMethod]
        public void Then_CustomCacheManagerConfigurationIsContainedInCachingSettings()
        {
            var cachingSettings = GetCacheManagerSettings();
            Assert.IsTrue(cachingSettings.CacheManagers.OfType<CustomCacheManagerData>().Any());
        }


        [TestMethod]
        public void Then_CustomCacheManagerConfigurationHasAppropriateName()
        {
            var cachingSettings = GetCacheManagerSettings();
            var cacheManager = cachingSettings.CacheManagers.OfType<CustomCacheManagerData>().First();

            Assert.AreEqual("custom cache manager name", cacheManager.Name);
        }
        [TestMethod]
        public void Then_CustomCacheManagerConfigurationHasAppropriateType()
        {
            var cachingSettings = GetCacheManagerSettings();
            var cacheManager = cachingSettings.CacheManagers.OfType<CustomCacheManagerData>().First();

            Assert.AreEqual(typeof(TestCustomCacheManager), cacheManager.Type);
        }

        [TestMethod]
        public void Then_CustomCacheManagerConfigurationHasAppropriateAttributes()
        {
            var cachingSettings = GetCacheManagerSettings();
            var cacheManager = cachingSettings.CacheManagers.OfType<CustomCacheManagerData>().First();

            Assert.AreEqual(attributes.Count, cacheManager.Attributes.Count);
            foreach (string attKey in attributes)
            {
                Assert.AreEqual(attributes[attKey], cacheManager.Attributes[attKey]);
            }
        }
    }
    
    [TestClass]
    public class When_CallingForCustomCachgeManagerNamedOnCachingConfigurationPassingAttributes : Given_CachingSettingsInConfigurationSourceBuilder
    {
        NameValueCollection attributes;

        protected override void Act()
        {
            attributes = new NameValueCollection();
            attributes.Add("key1", "value1");
            attributes.Add("key2", "value2");

            base.CachingConfiguration.ForCustomCacheManagerNamed("custom cache manager name", typeof(TestCustomCacheManager), attributes);
        }

        [TestMethod]
        public void Then_CustomCacheManagerConfigurationIsContainedInCachingSettings()
        {
            var cachingSettings = GetCacheManagerSettings();
            Assert.IsTrue(cachingSettings.CacheManagers.OfType<CustomCacheManagerData>().Any());
        }


        [TestMethod]
        public void Then_CustomCacheManagerConfigurationHasAppropriateName()
        {
            var cachingSettings = GetCacheManagerSettings();
            var cacheManager = cachingSettings.CacheManagers.OfType<CustomCacheManagerData>().First();

            Assert.AreEqual("custom cache manager name", cacheManager.Name);
        }
        [TestMethod]
        public void Then_CustomCacheManagerConfigurationHasAppropriateType()
        {
            var cachingSettings = GetCacheManagerSettings();
            var cacheManager = cachingSettings.CacheManagers.OfType<CustomCacheManagerData>().First();

            Assert.AreEqual(typeof(TestCustomCacheManager), cacheManager.Type);
        }

        [TestMethod]
        public void Then_CustomCacheManagerConfigurationHasAppropriateAttributes()
        {
            var cachingSettings = GetCacheManagerSettings();
            var cacheManager = cachingSettings.CacheManagers.OfType<CustomCacheManagerData>().First();

            Assert.AreEqual(attributes.Count, cacheManager.Attributes.Count);
            foreach (string attKey in attributes)
            {
                Assert.AreEqual(attributes[attKey], cacheManager.Attributes[attKey]);
            }
        }
    }

    [TestClass]
    public class When_CallingUseAsDefaultCacheOnCustomCacheManager : Given_CachingSettingsInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.CachingConfiguration
                .ForCustomCacheManagerNamed("custom cache manager name", typeof(TestCustomCacheManager))
                .UseAsDefaultCache();
        }

        [TestMethod]
        public void Then_CachingConfigurationHasCustomCacheAsDefaultCacheManager()
        {
            Assert.AreEqual("custom cache manager name", base.GetCacheManagerSettings().DefaultCacheManager);
        }
    }

    class TestCustomCacheManager : ICacheManager
    {
        public void Add(string key, object value)
        {
            throw new NotImplementedException();
        }

        public void Add(string key, object value, CacheItemPriority scavengingPriority, ICacheItemRefreshAction refreshAction, params ICacheItemExpiration[] expirations)
        {
            throw new NotImplementedException();
        }

        public bool Contains(string key)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public void Flush()
        {
            throw new NotImplementedException();
        }

        public object GetData(string key)
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            throw new NotImplementedException();
        }

        public object this[string key]
        {
            get { throw new NotImplementedException(); }
        }
    }
}
