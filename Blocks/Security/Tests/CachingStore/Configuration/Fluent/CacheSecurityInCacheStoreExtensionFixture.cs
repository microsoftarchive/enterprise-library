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
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Tests.Configuration.Fluent
{
    public abstract class Given_SecuritySettingsInConfigurationSourceBuilder : ArrangeActAssert
    {
        IConfigurationSourceBuilder configurationSourceBuilder;
        protected IConfigureSecuritySettings ConfigureSecuritySettings;

        protected override void Arrange()
        {
            configurationSourceBuilder = new ConfigurationSourceBuilder();
            ConfigureSecuritySettings = configurationSourceBuilder.ConfigureSecurity();
        }

        protected SecuritySettings GetSecuritySettings()
        {
            IConfigurationSource source = new DictionaryConfigurationSource();
            configurationSourceBuilder.UpdateConfigurationWithReplace(source);

            return (SecuritySettings)source.GetSection(SecuritySettings.SectionName);
        }
    }


    [TestClass]
    public class When_AddingCachingStoreSecurityCacheProviderPassingNullName : Given_SecuritySettingsInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_CacheSecurityInCacheStoreNamed_ThrowsArgumentException()
        {
            ConfigureSecuritySettings.CacheSecurityInCacheStoreNamed(null);
        }
    }

    [TestClass]
    public class When_AddingCachingStoreSecurityCacheProviderToConfigurationSourceBuilder : Given_SecuritySettingsInConfigurationSourceBuilder
    {
        ICacheSecurityInCacheStore cacheSecurityInfo;

        protected override void Act()
        {
            cacheSecurityInfo = base.ConfigureSecuritySettings.CacheSecurityInCacheStoreNamed("caching store");
        }

        [TestMethod]
        public void Then_SecurityCacheStoreProviderIsContainedInSecurityConfiguration()
        {
            Assert.IsTrue(GetSecuritySettings().SecurityCacheProviders.OfType<CachingStoreProviderData>().Any());
        }

        [TestMethod]
        public void Then_SecurityCacheStoreProviderHasAppropriateName()
        {
            var securityCacheStore = GetSecuritySettings().SecurityCacheProviders.OfType<CachingStoreProviderData>().First();
            Assert.AreEqual("caching store", securityCacheStore.Name);
        }

        [TestMethod]
        public void Then_SecurityCacheStoreHasCorrectType()
        {
            var securityCacheStore = GetSecuritySettings().SecurityCacheProviders.OfType<CachingStoreProviderData>().First();
            Assert.AreEqual(typeof(CachingStoreProvider), securityCacheStore.Type);
        }

        [TestMethod]
        public void Then_SecurityCacheStoreHasAbsoluteExpirationSetTo60Minutes()
        {
            var securityCacheStore = GetSecuritySettings().SecurityCacheProviders.OfType<CachingStoreProviderData>().First();
            Assert.AreEqual(60, securityCacheStore.AbsoluteExpiration);
        }

        [TestMethod]
        public void Then_SecurityCacheStoreHasSlidingExpirationSetTo10Minutes()
        {
            var securityCacheStore = GetSecuritySettings().SecurityCacheProviders.OfType<CachingStoreProviderData>().First();
            Assert.AreEqual(10, securityCacheStore.SlidingExpiration);
        }

        [TestMethod]
        public void Then_CanAddAnotherSecurityCacheStore()
        {
            cacheSecurityInfo.CacheSecurityInCacheStoreNamed("another store");

            var securityCacheStores = GetSecuritySettings().SecurityCacheProviders;
            Assert.AreEqual(2, securityCacheStores.Count());
        }
    }

    public abstract class Given_SecurityCacheProviderInConfigurationSourceBuilder : Given_SecuritySettingsInConfigurationSourceBuilder
    {
        protected ICacheSecurityInCacheStore cachingStoreProvider;
        protected override void Arrange()
        {
            base.Arrange();

            cachingStoreProvider = ConfigureSecuritySettings.CacheSecurityInCacheStoreNamed("cache store");
        }

        protected CachingStoreProviderData GetCachingStoreProvider()
        {
            return GetSecuritySettings().SecurityCacheProviders.Where(x => x.Name == "cache store").Cast<CachingStoreProviderData>().First();
        }
    }

    [TestClass]
    public class When_SettingAbsoluteExpirationOnCachingStoreProvider : Given_SecurityCacheProviderInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            cachingStoreProvider.WithOptions.AbsoluteExpiration(TimeSpan.FromDays(2));
        }

        [TestMethod]
        public void Then_CachingStoreProviderHasAbsoluteExpirationInMinutes()
        {
            Assert.AreEqual(TimeSpan.FromDays(2).Minutes, GetCachingStoreProvider().AbsoluteExpiration);
        }
    }

    [TestClass]
    public class When_SettingCacheManagerOnCachingStoreProvider : Given_SecurityCacheProviderInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            cachingStoreProvider.WithOptions.UseSharedCacheManager("cache manager");
        }

        [TestMethod]
        public void Then_CachingStoreProviderHasAbsoluteExpirationInMinutes()
        {
            Assert.AreEqual("cache manager", GetCachingStoreProvider().CacheManager);
        }
    }

    [TestClass]
    public class When_SettingNullCacheManagerOnCachingStoreProvider : Given_SecurityCacheProviderInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_UseSharedCacheManager_ThrowsArgumentException()
        {
            cachingStoreProvider.WithOptions.UseSharedCacheManager(null);
        }
    }


    [TestClass]
    public class When_SettingSlidingExpirationOnCachingStoreProvider : Given_SecurityCacheProviderInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            cachingStoreProvider.WithOptions.SlidingExpiration(TimeSpan.FromDays(0.5));
        }

        [TestMethod]
        public void Then_CachingStoreProviderHasSlidingxpirationInMinutes()
        {
            Assert.AreEqual(TimeSpan.FromDays(0.5).Minutes, GetCachingStoreProvider().SlidingExpiration);
        }
    }


    [TestClass]
    public class When_CallingIsDefaultOnCachingStoreProvider : Given_SecurityCacheProviderInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            cachingStoreProvider.SetAsDefault();
        }

        [TestMethod]
        public void Then_SecurityConfigurationHasDefaultCachingStore()
        {
            Assert.AreEqual(GetCachingStoreProvider().Name, GetSecuritySettings().DefaultSecurityCacheProviderName);
        }
    }
}
