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

using System;
using System.Collections.Generic;
using System.Management;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Manageability.Tests.WMI
{
	[TestClass]
	public class DpapiSymmetricCryptoProviderSettingFixture
	{
		[TestInitialize]
		public void SetUp()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(DpapiSymmetricCryptoProviderSetting));
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
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM DpapiSymmetricCryptoProviderSetting")
						.Get().GetEnumerator())
			{
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}

		[TestMethod]
		public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
		{
			DpapiSymmetricCryptoProviderSetting setting = new DpapiSymmetricCryptoProviderSetting(null, "name", "Scope");
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Publish();

			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM DpapiSymmetricCryptoProviderSetting")
						.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("name", resultEnumerator.Current.Properties["Name"].Value);
				Assert.AreEqual("Scope", resultEnumerator.Current.Properties["Scope"].Value);
				Assert.AreEqual("DpapiSymmetricCryptoProviderSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}

		[TestMethod]
		public void CanBindObject()
		{
			DpapiSymmetricCryptoProviderSetting setting = new DpapiSymmetricCryptoProviderSetting(null, "name", "CurrentUser");
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			//setting.Changed += this.Changed;

			setting.Publish();

			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM DpapiSymmetricCryptoProviderSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("DpapiSymmetricCryptoProviderSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);

				ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
				Assert.IsNotNull(managementObject);

				//should throw 
				managementObject.Put();
			}
		}

		[TestMethod]
		public void SavesChangesToConfigurationObject()
		{
			DpapiSymmetricCryptoProviderData sourceElement = new DpapiSymmetricCryptoProviderData("name", DataProtectionScope.CurrentUser);

			List<ConfigurationSetting> settings = new List<ConfigurationSetting>(1);
			DpapiSymmetricCryptoProviderDataWmiMapper.GenerateWmiObjects(sourceElement, settings);

			Assert.AreEqual(1, settings.Count);

			DpapiSymmetricCryptoProviderSetting setting = settings[0] as DpapiSymmetricCryptoProviderSetting;
			Assert.IsNotNull(setting);

			setting.Scope = DataProtectionScope.LocalMachine.ToString();

			setting.Commit();

			Assert.AreEqual(DataProtectionScope.LocalMachine, sourceElement.Scope);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void SettingInvalidEnumThrows()
		{
			DpapiSymmetricCryptoProviderSetting setting = new DpapiSymmetricCryptoProviderSetting(null, "name", "");

			setting.Scope = "foo";
		}
	}
}
