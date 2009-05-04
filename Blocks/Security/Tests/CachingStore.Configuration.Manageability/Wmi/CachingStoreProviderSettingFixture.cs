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

using System.Collections.Generic;
using System.Management;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration.Manageability;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Manageability.Tests.WMI
{
    [TestClass]
    public class CachingStoreProviderSettingFixture
    {

        [TestInitialize]
        public void SetUp()
        {
            ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(CachingStoreProviderSetting));
            NamedConfigurationSetting.ClearPublishedInstances();
        }

        [TestCleanup]
        public void TearDown()
        {
            ManagementEntityTypesRegistrar.UnregisterAll();
            NamedConfigurationSetting.ClearPublishedInstances();
        }

        [TestMethod]
        public void WmiQueryReturnsEmptyResultIfNoPublishedInstances()
        {
            using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM CachingStoreProviderSetting")
                        .Get().GetEnumerator())
            {
                Assert.IsFalse(resultEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
        {
            CachingStoreProviderSetting setting = new CachingStoreProviderSetting(null, "name", "CacheManager", 1, 2);
            setting.ApplicationName = "app";
            setting.SectionName = InstrumentationConfigurationSection.SectionName;
            setting.Publish();

            using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM CachingStoreProviderSetting")
                        .Get().GetEnumerator())
            {
                Assert.IsTrue(resultEnumerator.MoveNext());
                Assert.AreEqual("name", resultEnumerator.Current.Properties["Name"].Value);
                Assert.AreEqual("CacheManager", resultEnumerator.Current.Properties["CacheManager"].Value);
                Assert.AreEqual(1, resultEnumerator.Current.Properties["AbsoluteExpiration"].Value);
                Assert.AreEqual(2, resultEnumerator.Current.Properties["SlidingExpiration"].Value);
                Assert.AreEqual("CachingStoreProviderSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
                Assert.IsFalse(resultEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void CanBindObject()
        {
            CachingStoreProviderSetting setting = new CachingStoreProviderSetting(null, "name", "CacheManager", 1, 2);
            setting.ApplicationName = "app";
            setting.SectionName = InstrumentationConfigurationSection.SectionName;
            //setting.Changed += this.Changed;

            setting.Publish();

            using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM CachingStoreProviderSetting")
                    .Get().GetEnumerator())
            {
                Assert.IsTrue(resultEnumerator.MoveNext());
                Assert.AreEqual("CachingStoreProviderSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);

                ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
                Assert.IsNotNull(managementObject);

                //should throw 
                managementObject.Put();
            }
        }

        [TestMethod]
        public void SavesChangesToConfigurationObject()
        {
            CachingStoreProviderData sourceElement = new CachingStoreProviderData("name", 1, 2, "CacheManager");

            List<ConfigurationSetting> settings = new List<ConfigurationSetting>(1);
			CachingStoreProviderDataWmiMapper.GenerateWmiObjects(sourceElement, settings);

            Assert.AreEqual(1, settings.Count);

            CachingStoreProviderSetting setting = settings[0] as CachingStoreProviderSetting;
            Assert.IsNotNull(setting);

            setting.SlidingExpiration = 56;
            setting.CacheManager = "ChangedCacheManager";
            setting.AbsoluteExpiration = 65;

            setting.Commit();

            Assert.AreEqual(56, sourceElement.SlidingExpiration);
            Assert.AreEqual("ChangedCacheManager", sourceElement.CacheManager);
            Assert.AreEqual(65, sourceElement.AbsoluteExpiration);
        }
    }
}
