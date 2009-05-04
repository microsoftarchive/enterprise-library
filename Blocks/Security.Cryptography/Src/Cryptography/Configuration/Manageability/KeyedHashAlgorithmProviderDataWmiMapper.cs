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
    /// Represents a keyed hash algorithm provider configuration mapper to Wmi.
    /// </summary>
	public static class KeyedHashAlgorithmProviderDataWmiMapper
	{
        /// <summary>
        /// Creates the <see cref="KeyedHashAlgorithmProviderSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
		public static void GenerateWmiObjects(KeyedHashAlgorithmProviderData configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(
				new KeyedHashAlgorithmProviderSetting(configurationObject,
					configurationObject.Name,
					configurationObject.AlgorithmType.AssemblyQualifiedName,
					configurationObject.ProtectedKeyFilename,
					configurationObject.ProtectedKeyProtectionScope.ToString(),
					configurationObject.SaltEnabled));
		}

		internal static bool SaveChanges(KeyedHashAlgorithmProviderSetting setting, ConfigurationElement sourceElement)
		{
			KeyedHashAlgorithmProviderData element = (KeyedHashAlgorithmProviderData)sourceElement;

			element.AlgorithmTypeName = setting.AlgorithmType;
			element.ProtectedKeyFilename = setting.ProtectedKeyFilename;
			element.ProtectedKeyProtectionScope = ParseHelper.ParseEnum<DataProtectionScope>(setting.ProtectedKeyProtectionScope, false);
			element.SaltEnabled = setting.SaltEnabled;

			return true;
		}

		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(KeyedHashAlgorithmProviderSetting));
		}
	}
}
