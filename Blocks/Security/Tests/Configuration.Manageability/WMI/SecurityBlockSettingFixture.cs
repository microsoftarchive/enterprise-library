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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Manageability.Tests.WMI
{
    [TestClass]
    public class SecurityBlockSettingFixture
    {
        [TestInitialize]
        public void SetUp()
        {
            ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(SecurityBlockSetting));
            ConfigurationSectionSetting.ClearPublishedInstances();
        }

        [TestCleanup]
        public void TearDown()
        {
            ManagementEntityTypesRegistrar.UnregisterAll();
            ConfigurationSectionSetting.ClearPublishedInstances();
        }

        [TestMethod]
        public void WmiQueryReturnsEmptyResultIfNoPublishedInstances()
        {
            using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM SecurityBlockSetting")
                        .Get().GetEnumerator())
            {
                Assert.IsFalse(resultEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
        {
            SecurityBlockSetting setting = new SecurityBlockSetting(null, "defaultAuthorizationProvider", "defaultSecurityCacheProvider");
            setting.ApplicationName = "app";
            setting.SectionName = InstrumentationConfigurationSection.SectionName;
            setting.Publish();

            using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM SecurityBlockSetting")
                        .Get().GetEnumerator())
            {
                Assert.IsTrue(resultEnumerator.MoveNext());
                Assert.AreEqual("defaultAuthorizationProvider", resultEnumerator.Current.Properties["DefaultAuthorizationProvider"].Value);
                Assert.AreEqual("defaultSecurityCacheProvider", resultEnumerator.Current.Properties["DefaultSecurityCacheProvider"].Value);
                Assert.AreEqual("SecurityBlockSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
                Assert.IsFalse(resultEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void CanBindObject()
        {
            SecurityBlockSetting setting = new SecurityBlockSetting(null, "defaultAuthorizationProvider", "defaultSecurityCacheProvider");
            setting.ApplicationName = "app";
            setting.SectionName = InstrumentationConfigurationSection.SectionName;
            //setting.Changed += this.Changed;

            setting.Publish();

            using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM SecurityBlockSetting")
                    .Get().GetEnumerator())
            {
                Assert.IsTrue(resultEnumerator.MoveNext());
                Assert.AreEqual("SecurityBlockSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);

                ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
                Assert.IsNotNull(managementObject);

                //should throw 
                managementObject.Put();
            }
        }

        [TestMethod]
        public void SavesChangesToConfigurationObject()
        {
            SecuritySettings sourceElement = new SecuritySettings();//
            sourceElement.DefaultAuthorizationProviderName = "defaultAuthorizationProvider";
            sourceElement.DefaultSecurityCacheProviderName = "defaultSecurityCacheProvider";

            List<ConfigurationSetting> settings = new List<ConfigurationSetting>(1);
			SecuritySettingsWmiMapper.GenerateWmiObjects(sourceElement, settings);

            Assert.AreEqual(1, settings.Count);

            SecurityBlockSetting setting = settings[0] as SecurityBlockSetting;
            Assert.IsNotNull(setting);

            setting.DefaultSecurityCacheProvider = "MODS";
            setting.DefaultAuthorizationProvider = "MODA";

            setting.Commit();

            Assert.AreEqual("MODS", sourceElement.DefaultSecurityCacheProviderName);
            Assert.AreEqual("MODA", sourceElement.DefaultAuthorizationProviderName);
        }
    }
}
