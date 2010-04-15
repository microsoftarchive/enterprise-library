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
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Database.Tests
{
    [TestClass]
    public class DataCacheStorageDataFixture
    {
        const string name1 = "name1";
        const string encryption1 = "encryption1";
        const string partition1 = "partition1";
        const string database1 = "database1";

        [TestInitialize]
        public void TestInitialize()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
        }

        [TestMethod]
        [DeploymentItem("test.exe.config")]
        public void CanDeserializeSerializedConfiguration()
        {
            CacheManagerSettings settings = new CacheManagerSettings();

            DataCacheStorageData data1 = new DataCacheStorageData(name1, database1, partition1);
            settings.BackingStores.Add(data1);

            // needed to save configuration
            settings.CacheManagers.Add(new CacheManagerData("foo", 0, 0, 0, "storage"));

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[CacheManagerSettings.SectionName] = settings;
            IConfigurationSource configurationSource
                = ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

            CacheManagerSettings roSettigs = (CacheManagerSettings)configurationSource.GetSection(CacheManagerSettings.SectionName);

            Assert.IsNotNull(roSettigs);
            Assert.AreEqual(1, roSettigs.BackingStores.Count);

            Assert.IsNotNull(roSettigs.BackingStores.Get(name1));
            Assert.AreSame(typeof(DataCacheStorageData), roSettigs.BackingStores.Get(name1).GetType());
            Assert.AreEqual(name1, roSettigs.BackingStores.Get(name1).Name);
            Assert.AreEqual(database1, ((DataCacheStorageData)roSettigs.BackingStores.Get(name1)).DatabaseInstanceName);
            Assert.AreEqual(partition1, ((DataCacheStorageData)roSettigs.BackingStores.Get(name1)).PartitionName);
            //Assert.AreEqual(encryption1, ((DataCacheStorageData)roSettigs.BackingStores.Get(name1)).StorageEncryption);
        }
    }
}
