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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.Configuration
{
    [TestClass]
    public class MockCacheManagerDataFixture
    {
        const string name1 = "name1";
        const string name2 = "name2";
        const string foo1 = "Foo1";
        const string foo2 = "Foo2";

        [TestInitialize]
        public void TestInitialize()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
        }

        [TestMethod]
        public void CanDeserializeSerializedConfiguration()
        {
            MockCacheManagerData data1 = new MockCacheManagerData(name1, foo1);
            MockCacheManagerData data2 = new MockCacheManagerData(name2, foo2);

            CacheManagerSettings settings = new CacheManagerSettings();
            settings.DefaultCacheManager = name1;

            settings.CacheManagers.Add(data1);
            settings.CacheManagers.Add(data2);

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[CacheManagerSettings.SectionName] = settings;
            IConfigurationSource configurationSource
                = ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

            CacheManagerSettings roSettigs = (CacheManagerSettings)configurationSource.GetSection(CacheManagerSettings.SectionName);

            Assert.IsNotNull(roSettigs);
            Assert.AreEqual(2, roSettigs.CacheManagers.Count);
            Assert.AreEqual(name1, roSettigs.DefaultCacheManager);

            data1 = (MockCacheManagerData)roSettigs.CacheManagers.Get(name1);
            Assert.IsNotNull(data1);
            Assert.AreEqual(name1, data1.Name);
            Assert.AreEqual(foo1, data1.Foo);

            data2 = (MockCacheManagerData)roSettigs.CacheManagers.Get(name2);
            Assert.IsNotNull(data2);
            Assert.AreEqual(name2, data2.Name);
            Assert.AreEqual(foo2, data2.Foo);
        }
    }
}