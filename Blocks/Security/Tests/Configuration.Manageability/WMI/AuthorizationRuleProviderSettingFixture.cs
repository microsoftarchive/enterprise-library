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
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Manageability.Tests.WMI
{
	[TestClass]
	public class AuthorizationRuleProviderSettingFixture
	{
		[TestInitialize]
		public void SetUp()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(AuthorizationRuleProviderSetting));
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
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM AuthorizationRuleProviderSetting")
						.Get().GetEnumerator())
			{
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}

		[TestMethod]
		public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
		{
			string[] rules = new string[] { "r1", "r2" };
			AuthorizationRuleProviderSetting setting = new AuthorizationRuleProviderSetting(null, "name", rules);
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Publish();

			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM AuthorizationRuleProviderSetting")
						.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("name", resultEnumerator.Current.Properties["Name"].Value);
				Assert.ReferenceEquals(rules, resultEnumerator.Current.Properties["Rules"].Value);
				Assert.AreEqual("AuthorizationRuleProviderSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}

		[TestMethod]
		public void CanBindObject()
		{
			string[] rules = new string[] { "r1", "r2" };
			AuthorizationRuleProviderSetting setting = new AuthorizationRuleProviderSetting(null, "name", rules);
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			//setting.Changed += this.Changed;

			setting.Publish();

			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM AuthorizationRuleProviderSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("AuthorizationRuleProviderSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);

				ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
				Assert.IsNotNull(managementObject);

				//should throw 
				managementObject.Put();
			}
		}

		[TestMethod]
		public void SavesChangesToConfigurationObject()
		{
			NamedElementCollection<AuthorizationRuleData> ruleCollection = new NamedElementCollection<AuthorizationRuleData>();
			ruleCollection.Add(new AuthorizationRuleData("att1", "val"));
			ruleCollection.Add(new AuthorizationRuleData("att2", "val"));
			AuthorizationRuleProviderData sourceElement = new AuthorizationRuleProviderData("name");
			sourceElement.Rules.Add(new AuthorizationRuleData("att3", "val")); sourceElement.Rules.Add(new AuthorizationRuleData("att4", "val"));

			List<ConfigurationSetting> settings = new List<ConfigurationSetting>(1);
			AuthorizationRuleProviderDataWmiMapper.GenerateWmiObjects(sourceElement, settings);

			Assert.AreEqual(1, settings.Count);

			AuthorizationRuleProviderSetting setting = settings[0] as AuthorizationRuleProviderSetting;
			Assert.IsNotNull(setting);

			setting.Rules = GenerateRulesArray(ruleCollection);

			setting.Commit();

			Assert.ReferenceEquals(ruleCollection, sourceElement.Rules);
		}

		private string[] GenerateRulesArray(NamedElementCollection<AuthorizationRuleData> rules)
		{
			string[] rulesArray = new string[rules.Count];
			int i = 0;
			foreach (AuthorizationRuleData rule in rules)
			{
				rulesArray[i++]
					= Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.KeyValuePairEncoder.EncodeKeyValuePair(rule.Name, rule.Expression);
			}
			return rulesArray;
		}
	}
}
