//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Tests.WMI
{
    [TestClass]
    public class LoggingBlockSettingFixture
    {
        [TestInitialize]
        public void SetUp()
        {
            ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(LoggingBlockSetting));
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
            using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM LoggingBlockSetting")
                    .Get().GetEnumerator())
            {
                Assert.IsFalse(resultEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
        {
            LoggingBlockSetting setting = new LoggingBlockSetting(null, "defaultCategory", false, true, false);
            setting.ApplicationName = "app";
            setting.SectionName = InstrumentationConfigurationSection.SectionName;
            setting.Publish();

            using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM LoggingBlockSetting")
                    .Get().GetEnumerator())
            {
                Assert.IsTrue(resultEnumerator.MoveNext());
                Assert.AreEqual("defaultCategory", resultEnumerator.Current.Properties["DefaultCategory"].Value);
                Assert.AreEqual(false, resultEnumerator.Current.Properties["LogWarningWhenNoCategoriesMatch"].Value);
                Assert.AreEqual(true, resultEnumerator.Current.Properties["TracingEnabled"].Value);
                Assert.AreEqual(false, resultEnumerator.Current.Properties["RevertImpersonation"].Value);
                Assert.AreEqual("LoggingBlockSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
                Assert.IsFalse(resultEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void CanBindObject()
        {
            LoggingBlockSetting setting = new LoggingBlockSetting(null, "defaultCategory", false, true, false);
            setting.ApplicationName = "app";
            setting.SectionName = InstrumentationConfigurationSection.SectionName;
            //setting.Changed += this.Changed;

            setting.Publish();

            using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM LoggingBlockSetting")
                    .Get().GetEnumerator())
            {
                Assert.IsTrue(resultEnumerator.MoveNext());
                Assert.AreEqual("LoggingBlockSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);

                ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
                Assert.IsNotNull(managementObject);

                //should throw 
                managementObject.Put();
            }
        }

        [TestMethod]
        public void SavesChangesToConfigurationObject()
        {
            LoggingSettings sourceElement = new LoggingSettings();
            sourceElement.DefaultCategory = "foo";
            sourceElement.LogWarningWhenNoCategoriesMatch = false;
            sourceElement.TracingEnabled = true;
            sourceElement.RevertImpersonation = false;

            List<ConfigurationSetting> settings = new List<ConfigurationSetting>(1);
            LoggingSettingsWmiMapper.GenerateWmiObjects(sourceElement, settings);

            Assert.AreEqual(1, settings.Count);

            LoggingBlockSetting setting = settings[0] as LoggingBlockSetting;
            Assert.IsNotNull(setting);

            setting.DefaultCategory = "foobar";
            setting.LogWarningWhenNoCategoriesMatch = true;
            setting.TracingEnabled = false;
            setting.RevertImpersonation = true;

            setting.Commit();

            Assert.AreEqual("foobar", sourceElement.DefaultCategory);
            Assert.AreEqual(true, sourceElement.LogWarningWhenNoCategoriesMatch);
            Assert.AreEqual(false, sourceElement.TracingEnabled);
            Assert.AreEqual(true, sourceElement.RevertImpersonation);
        }
    }
}
