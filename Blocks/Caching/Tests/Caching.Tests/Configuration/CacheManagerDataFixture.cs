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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations.Tests;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Tests
{
    [TestClass]
    public class CacheManagerDataFixture
    {
        const string name1 = "name1";
        const string name2 = "name2";
        const string storageName = "cache storage";

        const int pollFrequency1 = 11;
        const int itemsBeforeScavenge1 = 12;
        const int itemsToScavenge1 = 13;

        const int pollFrequency2 = 21;
        const int itemsBeforeScavenge2 = 22;
        const int itemsToScavenge2 = 23;

        [TestInitialize]
        public void TestInitialize()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
        }

        [TestMethod]
        public void CanDeserializeSerializedConfiguration()
        {
            CacheManagerData data1 = new CacheManagerData(name1, pollFrequency1, itemsBeforeScavenge1, itemsToScavenge1, storageName);
            CacheManagerData data2 = new CacheManagerData(name2, pollFrequency2, itemsBeforeScavenge2, itemsToScavenge2, storageName);

            CacheManagerSettings settings = new CacheManagerSettings();
            settings.DefaultCacheManager = name1;

            settings.CacheManagers.Add(data1);
            settings.CacheManagers.Add(data2);

            // needed to save configuration
            settings.BackingStores.Add(new CustomCacheStorageData("foo", typeof(MockCustomStorageBackingStore)));

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[CacheManagerSettings.SectionName] = settings;
            IConfigurationSource configurationSource
                = ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

            CacheManagerSettings roSettigs = (CacheManagerSettings)configurationSource.GetSection(CacheManagerSettings.SectionName);

            Assert.IsNotNull(roSettigs);
            Assert.AreEqual(2, roSettigs.CacheManagers.Count);
            Assert.AreEqual(name1, roSettigs.DefaultCacheManager);

            data1 = (CacheManagerData)roSettigs.CacheManagers.Get(name1);
            Assert.IsNotNull(data1);
            Assert.AreEqual(name1, data1.Name);
            Assert.AreEqual(pollFrequency1, data1.ExpirationPollFrequencyInSeconds);
            Assert.AreEqual(itemsBeforeScavenge1, data1.MaximumElementsInCacheBeforeScavenging);
            Assert.AreEqual(itemsToScavenge1, data1.NumberToRemoveWhenScavenging);
            Assert.AreEqual(storageName, data1.CacheStorage);

            data2 = (CacheManagerData)roSettigs.CacheManagers.Get(name2);
            Assert.IsNotNull(data2);
            Assert.AreEqual(name2, data2.Name);
            Assert.AreEqual(pollFrequency2, data2.ExpirationPollFrequencyInSeconds);
            Assert.AreEqual(itemsBeforeScavenge2, data2.MaximumElementsInCacheBeforeScavenging);
            Assert.AreEqual(itemsToScavenge2, data2.NumberToRemoveWhenScavenging);
            Assert.AreEqual(storageName, data2.CacheStorage);
        }
    }
}
