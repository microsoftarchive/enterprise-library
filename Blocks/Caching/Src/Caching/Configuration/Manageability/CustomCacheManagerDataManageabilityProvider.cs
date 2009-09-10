//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability
{
    /// <summary>
    /// Provides a default implementation for <see cref="CustomCacheManagerData"/> that
    /// splits policy overrides processing and WMI objects generation, performing appropriate logging of 
    /// policy processing errors.
    /// </summary>
	public class CustomCacheManagerDataManageabilityProvider
		: CustomProviderDataManageabilityProvider<CustomCacheManagerData>
	{
        /// <summary>
        /// Initialize a new instance of the <see cref="CustomCacheManagerDataManageabilityProvider"/> class.
        /// </summary>
		public CustomCacheManagerDataManageabilityProvider()
			: base(Resources.CacheManagerPolicyNameTemplate)
		{
			CustomCacheManagerDataWmiMapper.RegisterWmiTypes();
		}

		internal new void AddAdministrativeTemplateDirectives(AdmContentBuilder contentBuilder,
			ConfigurationElement configurationObject,
			IConfigurationSource configurationSource,
			string parentKey)
		{
			base.AddAdministrativeTemplateDirectives(contentBuilder,
				configurationObject,
				configurationSource,
				parentKey);
		}

		internal new bool OverrideWithGroupPoliciesAndGenerateWmiObjects(ConfigurationElement configurationObject,
			bool readGroupPolicies, IRegistryKey machineKey, IRegistryKey userKey,
			bool generateWmiObjects, ICollection<ConfigurationSetting> wmiSettings)
		{
			return base.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, readGroupPolicies, machineKey, userKey, generateWmiObjects,
				wmiSettings);
		}

        /// <summary>
        /// Creates the <see cref="ConfigurationSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
        protected override void GenerateWmiObjects(CustomCacheManagerData configurationObject, 
			ICollection<ConfigurationSetting> wmiSettings)
		{
			CustomCacheManagerDataWmiMapper.GenerateWmiObjects(configurationObject, wmiSettings);
		}
	}
}
