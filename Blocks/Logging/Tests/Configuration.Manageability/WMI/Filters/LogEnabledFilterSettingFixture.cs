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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Filters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Tests.WMI.Filters
{
    [TestClass]
    public class LogEnabledFilterSettingFixture
    {
        [TestInitialize]
        public void SetUp()
        {
            ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(LogEnabledFilterSetting));
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
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM LogEnabledFilterSetting")
                        .Get().GetEnumerator())
            {
                Assert.IsFalse(resultEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
        {
            LogEnabledFilterSetting setting = new LogEnabledFilterSetting(null, "name", true);
            setting.ApplicationName = "app";
            setting.SectionName = InstrumentationConfigurationSection.SectionName;
            setting.Publish();

            using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM LogEnabledFilterSetting")
                        .Get().GetEnumerator())
            {
                Assert.IsTrue(resultEnumerator.MoveNext());
                Assert.AreEqual("name", resultEnumerator.Current.Properties["Name"].Value);
                Assert.AreEqual(true, resultEnumerator.Current.Properties["Enabled"].Value);
                Assert.AreEqual("LogEnabledFilterSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
                Assert.IsFalse(resultEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void CanBindObject()
        {
            LogEnabledFilterSetting setting = new LogEnabledFilterSetting(null, "name", true);
            setting.ApplicationName = "app";
            setting.SectionName = InstrumentationConfigurationSection.SectionName;
            //setting.Changed += this.Changed;

            setting.Publish();

            using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM LogEnabledFilterSetting")
                    .Get().GetEnumerator())
            {
                Assert.IsTrue(resultEnumerator.MoveNext());
                Assert.AreEqual("LogEnabledFilterSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);

                ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
                Assert.IsNotNull(managementObject);

                //should throw 
                managementObject.Put();
            }
        }

		[TestMethod]
		public void SavesChangesToConfigurationObject()
		{
			LogEnabledFilterData sourceElement = new LogEnabledFilterData();
			sourceElement.Enabled = true;

			List<ConfigurationSetting> settings = new List<ConfigurationSetting>(1);
			LogEnabledFilterDataWmiMapper.GenerateWmiObjects(sourceElement, settings);

			Assert.AreEqual(1, settings.Count);

			LogEnabledFilterSetting setting = settings[0] as LogEnabledFilterSetting;
			Assert.IsNotNull(setting);

			setting.Enabled = false;

			setting.Commit();

			Assert.AreEqual(false, sourceElement.Enabled);
		}
	}
}
