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
	public class SymmetricAlgorithmProviderSettingFixture
	{
		[TestInitialize]
		public void SetUp()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(SymmetricAlgorithmProviderSetting));
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
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM SymmetricAlgorithmProviderSetting")
						.Get().GetEnumerator())
			{
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}

		[TestMethod]
		public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
		{
			SymmetricAlgorithmProviderSetting setting = new SymmetricAlgorithmProviderSetting(null,
																				"name", "AlgorithmType",
																				"ProtectedKeyFilename", "ProtectedKeyProtectionScope");
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Publish();

			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM SymmetricAlgorithmProviderSetting")
						.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("name", resultEnumerator.Current.Properties["Name"].Value);
				Assert.AreEqual("AlgorithmType", resultEnumerator.Current.Properties["AlgorithmType"].Value);
				Assert.AreEqual("ProtectedKeyFilename", resultEnumerator.Current.Properties["ProtectedKeyFilename"].Value);
				Assert.AreEqual("ProtectedKeyProtectionScope", resultEnumerator.Current.Properties["ProtectedKeyProtectionScope"].Value);
				Assert.AreEqual("SymmetricAlgorithmProviderSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}

		[TestMethod]
		public void CanBindObject()
		{
			SymmetricAlgorithmProviderSetting setting = new SymmetricAlgorithmProviderSetting(null,
																				"name", typeof(bool).AssemblyQualifiedName,
																				"ProtectedKeyFilename", "LocalMachine");
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			//setting.Changed += this.Changed;

			setting.Publish();

			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM SymmetricAlgorithmProviderSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("SymmetricAlgorithmProviderSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);

				ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
				Assert.IsNotNull(managementObject);

				//should throw 
				managementObject.Put();
			}
		}

		[TestMethod]
		public void SavesChangesToConfigurationObject()
		{
			SymmetricAlgorithmProviderData sourceElement = new SymmetricAlgorithmProviderData("name",
				typeof(bool),
				"file name",
				DataProtectionScope.CurrentUser);

			List<ConfigurationSetting> settings = new List<ConfigurationSetting>(1);
			SymmetricAlgorithmProviderDataWmiMapper.GenerateWmiObjects(sourceElement, settings);

			Assert.AreEqual(1, settings.Count);

			SymmetricAlgorithmProviderSetting setting = settings[0] as SymmetricAlgorithmProviderSetting;
			Assert.IsNotNull(setting);

			setting.AlgorithmType = typeof(int).AssemblyQualifiedName;
			setting.ProtectedKeyFilename = "overriden file name";
			setting.ProtectedKeyProtectionScope = DataProtectionScope.LocalMachine.ToString();

			setting.Commit();

			Assert.AreEqual(typeof(int), sourceElement.AlgorithmType);
			Assert.AreEqual("overriden file name", sourceElement.ProtectedKeyFilename);
			Assert.AreEqual(DataProtectionScope.LocalMachine, sourceElement.ProtectedKeyProtectionScope);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void SettingInvalidEnumThrows()
		{
			SymmetricAlgorithmProviderSetting setting = new SymmetricAlgorithmProviderSetting(null, "name", "", "", "");

			setting.ProtectedKeyProtectionScope = "foo";
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void SettingInvalidTypeThrows()
		{
			SymmetricAlgorithmProviderSetting setting = new SymmetricAlgorithmProviderSetting(null, "name", "", "", "");

			setting.AlgorithmType = "foo";
		}
	}
}
