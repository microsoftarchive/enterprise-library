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

using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability.Tests
{
    [TestClass]
    public class CacheManagerSettingsWmiMapperFixture
    {
        [TestCleanup]
        public void TearDown()
        {
            // preventive unregister to work around WMI.NET 2.0 issues with appdomain unloading
            ManagementEntityTypesRegistrar.UnregisterAll();
        }

        [TestMethod]
        public void WmiCacheManagerSettingIsCreated()
        {
            CacheManagerData data = new CacheManagerData();
            data.Name = "cache manager 1";
            data.CacheStorage = "storage";
            data.NumberToRemoveWhenScavenging = 100;
            data.MaximumElementsInCacheBeforeScavenging = 200;
            data.ExpirationPollFrequencyInSeconds = 60;

            List<ConfigurationSetting> wmiSettings = new List<ConfigurationSetting>();

            CacheManagerSettingsWmiMapper.GenerateCacheManagerWmiObjects(data, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.IsInstanceOfType(wmiSettings[0], typeof(CacheManagerSetting));

            CacheManagerSetting setting = (CacheManagerSetting)wmiSettings[0];
            Assert.AreEqual("cache manager 1", setting.Name);
            Assert.AreEqual("storage", setting.CacheStorage);
            Assert.AreEqual(100, setting.NumberToRemoveWhenScavenging);
            Assert.AreEqual(200, setting.MaximumElementsInCacheBeforeScavenging);
            Assert.AreEqual(60, setting.ExpirationPollFrequencyInSeconds);
        }

        [TestMethod]
        public void WmiUnknownCacheManagerSettingIsCreated()
        {
            CustomCacheManagerData data = new CustomCacheManagerData();
            data.Name = "cache manager 1";

            List<ConfigurationSetting> wmiSettings = new List<ConfigurationSetting>();

            CacheManagerSettingsWmiMapper.GenerateCacheManagerWmiObjects(data, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.IsInstanceOfType(wmiSettings[0], typeof(UnknownCacheManagerSetting));

            UnknownCacheManagerSetting setting = (UnknownCacheManagerSetting)wmiSettings[0];
            Assert.AreEqual("cache manager 1", setting.Name);
        }
    }
}