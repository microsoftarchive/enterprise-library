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
using System.Configuration;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Manageability
{
    /// <summary>
    /// Represents a mapper for dpapi provider configuration to Wmi.
    /// </summary>
	public static class DpapiSymmetricCryptoProviderDataWmiMapper
	{
        /// <summary>
        /// Creates the <see cref="DpapiSymmetricCryptoProviderSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
		public static void GenerateWmiObjects(DpapiSymmetricCryptoProviderData configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(
				new DpapiSymmetricCryptoProviderSetting(configurationObject,
					configurationObject.Name,
					configurationObject.Scope.ToString()));
		}

		internal static bool SaveChanges(DpapiSymmetricCryptoProviderSetting setting, ConfigurationElement sourceElement)
		{
			DpapiSymmetricCryptoProviderData element = (DpapiSymmetricCryptoProviderData)sourceElement;

			element.Scope = ParseHelper.ParseEnum<DataProtectionScope>(setting.Scope, false);

			return true;
		}

		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(DpapiSymmetricCryptoProviderSetting));
		}
	}
}
