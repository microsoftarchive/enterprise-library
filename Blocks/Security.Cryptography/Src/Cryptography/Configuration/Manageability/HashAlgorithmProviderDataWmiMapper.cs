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
    /// Represents a mapper for hash algorithm provider configuration to Wmi.
    /// </summary>
	public static class HashAlgorithmProviderDataWmiMapper
	{
        /// <summary>
        /// Creates the <see cref="HashAlgorithmProviderSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
		public static void GenerateWmiObjects(HashAlgorithmProviderData configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(
				new HashAlgorithmProviderSetting(configurationObject,
					configurationObject.Name,
					configurationObject.AlgorithmType.AssemblyQualifiedName,
					configurationObject.SaltEnabled));
		}

		internal static bool SaveChanges(HashAlgorithmProviderSetting setting, ConfigurationElement sourceElement)
		{
			HashAlgorithmProviderData element = (HashAlgorithmProviderData)sourceElement;

			element.AlgorithmTypeName = setting.AlgorithmType;
			element.SaltEnabled = setting.SaltEnabled;

			return true;
		}

		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(HashAlgorithmProviderSetting));
		}
	}
}
