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
	public class CryptographyBlockSettingFixture
	{
		[TestInitialize]
		public void SetUp()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(CryptographyBlockSetting));
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
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM CryptographyBlockSetting")
						.Get().GetEnumerator())
			{
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}

		[TestMethod]
		public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
		{
			CryptographyBlockSetting setting = new CryptographyBlockSetting(null, "defaultHashProvider", "DefaultSymmetricCryptoProvider");
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Publish();

			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM CryptographyBlockSetting")
						.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("DefaultSymmetricCryptoProvider", resultEnumerator.Current.Properties["DefaultSymmetricCryptoProvider"].Value);
				Assert.AreEqual("defaultHashProvider", resultEnumerator.Current.Properties["DefaultHashProvider"].Value);
				Assert.AreEqual("CryptographyBlockSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}

		[TestMethod]
		public void CanBindObject()
		{
			CryptographyBlockSetting setting = new CryptographyBlockSetting(null, "defaultHashProvider", "defaultSymetricCryptoProvider");
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			//setting.Changed += this.Changed;

			setting.Publish();

			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM CryptographyBlockSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("CryptographyBlockSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);

				ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
				Assert.IsNotNull(managementObject);

				//should throw 
				managementObject.Put();
			}
		}

		[TestMethod]
		public void SavesChangesToConfigurationObject()
		{
			CryptographySettings sourceElement = new CryptographySettings();
			sourceElement.DefaultHashProviderName = "foo";
			sourceElement.DefaultSymmetricCryptoProviderName = "bar";

			List<ConfigurationSetting> settings = new List<ConfigurationSetting>(1);
			CryptographySettingsWmiMapper.GenerateWmiObjects(sourceElement, settings);

			Assert.AreEqual(1, settings.Count);

			CryptographyBlockSetting setting = settings[0] as CryptographyBlockSetting;
			Assert.IsNotNull(setting);

			setting.DefaultHashProvider = "foobar";
			setting.DefaultSymmetricCryptoProvider = "barfoo";

			setting.Commit();

			Assert.AreEqual("foobar", sourceElement.DefaultHashProviderName);
			Assert.AreEqual("barfoo", sourceElement.DefaultSymmetricCryptoProviderName);
		}
	}
}
