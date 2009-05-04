//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using System.Management;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Wmi
{
	[TestClass]
	public class InstrumentationSettingFixture
	{
		[TestInitialize]
		public void SetUp()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof (InstrumentationSetting));
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
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM InstrumentationSetting")
					.Get().GetEnumerator())
			{
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}

		[TestMethod]
		public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
		{
			InstrumentationSetting setting = new InstrumentationSetting(null, true, false, true);
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Publish();

			using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM InstrumentationSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual(true, resultEnumerator.Current.Properties["EventLoggingEnabled"].Value);
				Assert.AreEqual(false, resultEnumerator.Current.Properties["PerformanceCountersEnabled"].Value);
				Assert.AreEqual(true, resultEnumerator.Current.Properties["WmiEnabled"].Value);
				Assert.AreEqual("InstrumentationSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}

		[TestMethod]
		public void CanBindObject()
		{
			InstrumentationSetting setting = new InstrumentationSetting(null, true, false, true);
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			//setting.Changed += this.Changed;

			setting.Publish();

			using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM InstrumentationSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("InstrumentationSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);

				ManagementObject managementObject = (ManagementObject) resultEnumerator.Current;
				Assert.IsNotNull(managementObject);

				// would throw if bind didn't work
				managementObject.Put();
			}
		}

		[TestMethod]
		public void SavesChangesToConfigurationObject()
		{
			InstrumentationConfigurationSection sourceElement = new InstrumentationConfigurationSection(false, true, false);

			List<ConfigurationSetting> settings = new List<ConfigurationSetting>(1);
			InstrumentationConfigurationSectionWmiMapper.GenerateWmiObjects(sourceElement, settings);

			Assert.AreEqual(1, settings.Count);

			InstrumentationSetting setting = (InstrumentationSetting) settings[0];
			Assert.IsNotNull(setting);

			setting.EventLoggingEnabled = true;
			setting.PerformanceCountersEnabled = false;
			setting.WmiEnabled = true;

			setting.Commit();

			Assert.AreEqual(true, sourceElement.EventLoggingEnabled);
			Assert.AreEqual(false, sourceElement.PerformanceCountersEnabled);
			Assert.AreEqual(true, sourceElement.WmiEnabled);
		}
	}
}
