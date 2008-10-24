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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Manageability
{
    /// <summary>
    /// Represents a mapper for authorization rule provider configuration to Wmi.
    /// </summary>
	public static class AuthorizationRuleProviderDataWmiMapper
	{
        /// <summary>
        /// Creates the <see cref="AuthorizationRuleProviderSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
		public static void GenerateWmiObjects(AuthorizationRuleProviderData configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			string[] rules = GenerateRulesArray(configurationObject.Rules);

			wmiSettings.Add(new AuthorizationRuleProviderSetting(configurationObject, configurationObject.Name, rules));
		}

		private static string[] GenerateRulesArray(NamedElementCollection<AuthorizationRuleData> rules)
		{
			string[] rulesArray = new string[rules.Count];
			int i = 0;
			foreach (AuthorizationRuleData rule in rules)
			{
				rulesArray[i++]
					= KeyValuePairEncoder.EncodeKeyValuePair(rule.Name, rule.Expression);
			}
			return rulesArray;
		}

		internal static bool SaveChanges(AuthorizationRuleProviderSetting authorizationRuleProviderSetting, ConfigurationElement sourceElement)
		{
			AuthorizationRuleProviderData element = (AuthorizationRuleProviderData)sourceElement;

			element.Rules.Clear();
			NamedElementCollection<AuthorizationRuleData> ruleCollection = GenerateRulesCollection(authorizationRuleProviderSetting.Rules);
			foreach (AuthorizationRuleData rule in ruleCollection)
			{
				element.Rules.Add(rule);
			}
			return true;
		}

		private static NamedElementCollection<AuthorizationRuleData> GenerateRulesCollection(string[] rules)
		{
			NamedElementCollection<AuthorizationRuleData> attributeCollection = new NamedElementCollection<AuthorizationRuleData>();
			foreach (string attribute in rules)
			{
				string[] splittedAttribute = attribute.Split('=');
				attributeCollection.Add(new AuthorizationRuleData(splittedAttribute[0], splittedAttribute[1]));
			}
			return attributeCollection;
		}

		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(AuthorizationRuleProviderSetting));
		}
	}
}
