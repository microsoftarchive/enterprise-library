//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
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

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Manageability.Tests.WMI
{
	[TestClass]
	public class CustomHashProviderSettingFixture
	{
		[TestInitialize]
		public void SetUp()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(CustomHashProviderSetting));
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
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM CustomHashProviderSetting")
						.Get().GetEnumerator())
			{
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}

		[TestMethod]
		public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
		{
			string[] attributes = new string[] { "att1", "att2" };
			CustomHashProviderSetting setting = new CustomHashProviderSetting(null, "name", "ProviderType", attributes);
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Publish();

			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM CustomHashProviderSetting")
						.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("name", resultEnumerator.Current.Properties["Name"].Value);
				Assert.AreEqual("ProviderType", resultEnumerator.Current.Properties["ProviderType"].Value);
				Assert.ReferenceEquals(attributes, resultEnumerator.Current.Properties["Attributes"].Value);
				Assert.AreEqual("CustomHashProviderSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}

		[TestMethod]
		public void CanBindObject()
		{
			string[] attributes = new string[] { "att1", "att2" };
			CustomHashProviderSetting setting = new CustomHashProviderSetting(null, "name", "ProviderType", attributes);
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			//setting.Changed += this.Changed;

			setting.Publish();

			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM CustomHashProviderSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("CustomHashProviderSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);

				ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
				Assert.IsNotNull(managementObject);

				//should throw 
				managementObject.Put();
			}
		}

		[TestMethod]
		public void SavesChangesToConfigurationObject()
		{
			CustomHashProviderData sourceElement = new CustomHashProviderData("name",
				typeof(bool));
			sourceElement.Attributes.Add("att3", "val3");
			sourceElement.Attributes.Add("att4", "val4");
			sourceElement.Attributes.Add("att5", "val5");

			List<ConfigurationSetting> settings = new List<ConfigurationSetting>(1);
			CustomHashProviderDataWmiMapper.GenerateWmiObjects(sourceElement, settings);

			Assert.AreEqual(1, settings.Count);

			CustomHashProviderSetting setting = settings[0] as CustomHashProviderSetting;
			Assert.IsNotNull(setting);

			setting.Attributes = new string[] { "att1=val1", "att2=val2" };
			setting.ProviderType = typeof(int).AssemblyQualifiedName;

			setting.Commit();

			Assert.AreEqual(2, sourceElement.Attributes.Count);
			Assert.AreEqual("val1", sourceElement.Attributes["att1"]);
			Assert.AreEqual("val2", sourceElement.Attributes["att2"]);
			Assert.AreEqual(typeof(int), sourceElement.Type);
		}
	}
}
