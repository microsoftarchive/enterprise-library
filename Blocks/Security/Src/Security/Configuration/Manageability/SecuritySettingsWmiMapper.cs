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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Manageability
{
    /// <summary>
    /// Represents a mapper for the security configuration to Wmi.
    /// </summary>
	public static class SecuritySettingsWmiMapper
	{
        /// <summary>
        /// Creates the <see cref="SecurityBlockSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
		public static void GenerateWmiObjects(SecuritySettings configurationObject, ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(
				new SecurityBlockSetting(configurationObject, configurationObject.DefaultAuthorizationProviderName,
					configurationObject.DefaultSecurityCacheProviderName));
		}

		internal static bool SaveChanges(SecurityBlockSetting securityBlockSetting, ConfigurationElement sourceElement)
		{
			SecuritySettings section = (SecuritySettings)sourceElement;

			section.DefaultSecurityCacheProviderName = securityBlockSetting.DefaultSecurityCacheProvider;
			section.DefaultAuthorizationProviderName = securityBlockSetting.DefaultAuthorizationProvider;

			return true;
		}

		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(SecurityBlockSetting));
		}
	}
}
