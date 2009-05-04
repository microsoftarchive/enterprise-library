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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Manageability
{
    /// <summary>
    /// Represents a mapper from cryptography configuration to Wmi.
    /// </summary>
	public static class CryptographySettingsWmiMapper
	{
        /// <summary>
        /// Creates the <see cref="CryptographyBlockSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
		public static void GenerateWmiObjects(CryptographySettings configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(new CryptographyBlockSetting(configurationObject,
				configurationObject.DefaultHashProviderName,
				configurationObject.DefaultSymmetricCryptoProviderName));
		}

		internal static bool SaveChanges(CryptographyBlockSetting configurationSetting, ConfigurationElement sourceElement)
		{
			CryptographySettings section = (CryptographySettings)sourceElement;

			section.DefaultHashProviderName = configurationSetting.DefaultHashProvider;
			section.DefaultSymmetricCryptoProviderName = configurationSetting.DefaultSymmetricCryptoProvider;

			return true;
		}

		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(CryptographyBlockSetting));
		}
	}
}
