//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests.Configuration.Fluent
{

    [TestClass]
    public class When_ConfiguringCustomSecurityCacheProvider : Given_SecuritySettingsInConfigurationSourceBuilder
    {
        ICacheSecurityInCustomStore customCacheStore;

        protected override void Act()
        {
            customCacheStore = ConfigureSecuritySettings.CacheSecurityInCustomStoreNamed("custom provider", typeof(CustomSecurityCacheProvider));
        }

        [TestMethod]
        public void Then_CustomSecurityCacheProviderDataIsAddedToSecurityConfiguration()
        {
            Assert.AreEqual(1, SecurityCacheProviders.OfType<CustomSecurityCacheProviderData>().Count());
        }

        [TestMethod]
        public void Then_CustomSecurityCacheProviderDataHasAppropriateName()
        {
            var CustomSecurityCacheProvider = SecurityCacheProviders.OfType<CustomSecurityCacheProviderData>().First();
            Assert.AreEqual("custom provider", CustomSecurityCacheProvider.Name);
        }

        [TestMethod]
        public void Then_CustomSecurityCacheProviderDataHasAppropriateType()
        {
            var CustomSecurityCacheProvider = SecurityCacheProviders.OfType<CustomSecurityCacheProviderData>().First();
            Assert.AreEqual(typeof(CustomSecurityCacheProvider), CustomSecurityCacheProvider.Type);
        }

        [TestMethod]
        public void Then_CannAddAnotherCustomSecurityCacheProviderr()
        {
            customCacheStore.CacheSecurityInCustomStoreNamed<CustomSecurityCacheProvider>("another");
            Assert.AreEqual(2, SecurityCacheProviders.Count());
        }
    }

    [TestClass]
    public class When_ConfiguringCustomSecurityCacheProviderGeneric : Given_SecuritySettingsInConfigurationSourceBuilder
    {
        ICacheSecurityInCustomStore cacheInCustomStore;

        protected override void Act()
        {
            cacheInCustomStore = ConfigureSecuritySettings.CacheSecurityInCustomStoreNamed<CustomSecurityCacheProvider>("custom provider");
        }

        [TestMethod]
        public void Then_CustomSecurityCacheProviderDataIsAddedToSecurityConfiguration()
        {
            Assert.AreEqual(1, SecurityCacheProviders.OfType<CustomSecurityCacheProviderData>().Count());
        }

        [TestMethod]
        public void Then_CustomSecurityCacheProviderDataHasAppropriateName()
        {
            var CustomSecurityCacheProvider = SecurityCacheProviders.OfType<CustomSecurityCacheProviderData>().First();
            Assert.AreEqual("custom provider", CustomSecurityCacheProvider.Name);
        }

        [TestMethod]
        public void Then_CustomSecurityCacheProviderDataHasAppropriateType()
        {
            var CustomSecurityCacheProvider = SecurityCacheProviders.OfType<CustomSecurityCacheProviderData>().First();
            Assert.AreEqual(typeof(CustomSecurityCacheProvider), CustomSecurityCacheProvider.Type);
        }

        [TestMethod]
        public void Then_CannAddAnotherCustomSecurityCacheProviderr()
        {
            cacheInCustomStore.CacheSecurityInCustomStoreNamed<CustomSecurityCacheProvider>("another");
            Assert.AreEqual(2, SecurityCacheProviders.Count());
        }
    }

    [TestClass]
    public class When_ConfiguringCustomSecurityCacheProviderPassingAttributes : Given_SecuritySettingsInConfigurationSourceBuilder
    {
        ICacheSecurityInCustomStore cacheInCustomStore;
        NameValueCollection attributes;

        protected override void Act()
        {
            attributes = new NameValueCollection();
            attributes.Add("key1", "value1");
            attributes.Add("key2", "value2");

            cacheInCustomStore = ConfigureSecuritySettings.CacheSecurityInCustomStoreNamed("custom provider", typeof(CustomSecurityCacheProvider), attributes);
        }

        [TestMethod]
        public void Then_CustomSecurityCacheProviderDataIsAddedToSecurityConfiguration()
        {
            Assert.AreEqual(1, SecurityCacheProviders.OfType<CustomSecurityCacheProviderData>().Count());
        }

        [TestMethod]
        public void Then_CustomSecurityCacheProviderDataHasAppropriateName()
        {
            var CustomSecurityCacheProvider = SecurityCacheProviders.OfType<CustomSecurityCacheProviderData>().First();
            Assert.AreEqual("custom provider", CustomSecurityCacheProvider.Name);
        }

        [TestMethod]
        public void Then_CustomSecurityCacheProviderDataHasAppropriateType()
        {
            var CustomSecurityCacheProvider = SecurityCacheProviders.OfType<CustomSecurityCacheProviderData>().First();
            Assert.AreEqual(typeof(CustomSecurityCacheProvider), CustomSecurityCacheProvider.Type);
        }

        [TestMethod]
        public void Then_CustomSecurityCacheProviderDataHasAppropriateAttributes()
        {
            var CustomSecurityCacheProvider = SecurityCacheProviders.OfType<CustomSecurityCacheProviderData>().First();
            Assert.AreEqual(attributes.Count, CustomSecurityCacheProvider.Attributes.Count);
            foreach (string key in attributes)
            {
                Assert.AreEqual(attributes[key], CustomSecurityCacheProvider.Attributes[key]);
            }
        }

        [TestMethod]
        public void Then_CannAddAnotherCustomSecurityCacheProviderr()
        {
            cacheInCustomStore.CacheSecurityInCustomStoreNamed<CustomSecurityCacheProvider>("another");
            Assert.AreEqual(2, SecurityCacheProviders.Count());
        }
    }


    [TestClass]
    public class When_ConfiguringCustomSecurityCacheProviderPassingAttributesGeneric : Given_SecuritySettingsInConfigurationSourceBuilder
    {
        ICacheSecurityInCustomStore cacheInCustomStore;
        NameValueCollection attributes;

        protected override void Act()
        {
            attributes = new NameValueCollection();
            attributes.Add("key1", "value1");
            attributes.Add("key2", "value2");

            cacheInCustomStore = ConfigureSecuritySettings.CacheSecurityInCustomStoreNamed<CustomSecurityCacheProvider>("custom provider", attributes);
        }

        [TestMethod]
        public void Then_CustomSecurityCacheProviderDataIsAddedToSecurityConfiguration()
        {
            Assert.AreEqual(1, SecurityCacheProviders.OfType<CustomSecurityCacheProviderData>().Count());
        }

        [TestMethod]
        public void Then_CustomSecurityCacheProviderDataHasAppropriateName()
        {
            var CustomSecurityCacheProvider = SecurityCacheProviders.OfType<CustomSecurityCacheProviderData>().First();
            Assert.AreEqual("custom provider", CustomSecurityCacheProvider.Name);
        }

        [TestMethod]
        public void Then_CustomSecurityCacheProviderDataHasAppropriateType()
        {
            var CustomSecurityCacheProvider = SecurityCacheProviders.OfType<CustomSecurityCacheProviderData>().First();
            Assert.AreEqual(typeof(CustomSecurityCacheProvider), CustomSecurityCacheProvider.Type);
        }

        [TestMethod]
        public void Then_CustomSecurityCacheProviderDataHasAppropriateAttributes()
        {
            var CustomSecurityCacheProvider = SecurityCacheProviders.OfType<CustomSecurityCacheProviderData>().First();
            Assert.AreEqual(attributes.Count, CustomSecurityCacheProvider.Attributes.Count);
            foreach (string key in attributes)
            {
                Assert.AreEqual(attributes[key], CustomSecurityCacheProvider.Attributes[key]);
            }
        }

        [TestMethod]
        public void Then_CannAddAnotherCustomSecurityCacheProviderr()
        {
            cacheInCustomStore.CacheSecurityInCustomStoreNamed<CustomSecurityCacheProvider>("another");
            Assert.AreEqual(2, SecurityCacheProviders.Count());
        }
    }


    [TestClass]
    public class When_CallingSetAsDefaultOnCustomSecurityCacheProvider : Given_SecuritySettingsInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            var cacheInCustomStore = ConfigureSecuritySettings.CacheSecurityInCustomStoreNamed<CustomSecurityCacheProvider>("custom provider");
            cacheInCustomStore.SetAsDefault();
        }

        [TestMethod]
        public void Then_SecurityConfigurationHasDefaultSecurityCacheProvider()
        {
            Assert.AreEqual("custom provider", GetSecuritySettings().DefaultSecurityCacheProviderName);
        }
    }


    [TestClass]
    public class When_ConfiguringCustomSecurityCacheProviderPassingNullForName : Given_SecuritySettingsInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_CacheSecurityInCustomStoreNamed_ThrowsArgumentException()
        {
            ConfigureSecuritySettings.CacheSecurityInCustomStoreNamed(null, typeof(CustomSecurityCacheProvider));
        }
    }

    [TestClass]
    public class When_ConfiguringCustomSecurityCacheProviderPassingNullForType : Given_SecuritySettingsInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_CacheSecurityInCustomStoreNamed_ThrowsArgumentNullException()
        {
            ConfigureSecuritySettings.CacheSecurityInCustomStoreNamed("custom cache", null);
        }
    }

    [TestClass]
    public class When_ConfiguringCustomSecurityCacheProviderPassingWrongType : Given_SecuritySettingsInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_CacheSecurityInCustomStoreNamed_ThrowsArgumentException()
        {
            ConfigureSecuritySettings.CacheSecurityInCustomStoreNamed("custom cache", typeof(object));
        }
    }

    [TestClass]
    public class When_ConfiguringCustomSecurityCacheProviderPassingNullForAttributes : Given_SecuritySettingsInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_CacheSecurityInCustomStoreNamed_ThrowsArgumentNullException()
        {
            ConfigureSecuritySettings.CacheSecurityInCustomStoreNamed("custom cache", typeof(CustomSecurityCacheProvider), null);
        }
    }

}
