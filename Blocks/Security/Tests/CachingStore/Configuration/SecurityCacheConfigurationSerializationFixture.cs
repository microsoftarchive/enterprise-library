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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration.Tests
{
    [TestClass]
    public class SecurityCacheConfigurationSerializationFixture
    {
        const string securityCacheName1 = "authorization1";
        const string manager1 = "manager";
        const int sliding1 = 100;
        const int absolute1 = 101;

        [TestInitialize]
        public void TestInitialize()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
        }

        [TestMethod]
        public void CanDeserializeSerializedConfiguration()
        {
            SecuritySettings settings = new SecuritySettings();

            CachingStoreProviderData securityCacheData1 = new CachingStoreProviderData(securityCacheName1, sliding1, absolute1, manager1);

            settings.SecurityCacheProviders.Add(securityCacheData1);
            settings.DefaultSecurityCacheProviderName = securityCacheName1;

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[SecuritySettings.SectionName] = settings;
            IConfigurationSource configurationSource
                = ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

            SecuritySettings roSettigs = (SecuritySettings)configurationSource.GetSection(SecuritySettings.SectionName);

            Assert.IsNotNull(roSettigs);
            Assert.AreEqual(1, roSettigs.SecurityCacheProviders.Count);

            Assert.IsNotNull(roSettigs.SecurityCacheProviders.Get(securityCacheName1));
            Assert.AreSame(typeof(CachingStoreProviderData), roSettigs.SecurityCacheProviders.Get(securityCacheName1).GetType());
            Assert.AreEqual(absolute1, ((CachingStoreProviderData)roSettigs.SecurityCacheProviders.Get(securityCacheName1)).AbsoluteExpiration);
            Assert.AreEqual(manager1, ((CachingStoreProviderData)roSettigs.SecurityCacheProviders.Get(securityCacheName1)).CacheManager);
            Assert.AreEqual(sliding1, ((CachingStoreProviderData)roSettigs.SecurityCacheProviders.Get(securityCacheName1)).SlidingExpiration);
        }
    }
}