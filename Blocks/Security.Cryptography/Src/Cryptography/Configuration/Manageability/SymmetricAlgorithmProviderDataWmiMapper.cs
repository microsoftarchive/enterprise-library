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
    /// Represents a mapper of a symmetric algorithm provider configuration data to Wmi.
    /// </summary>
	public static class SymmetricAlgorithmProviderDataWmiMapper
	{
        /// <summary>
        /// Creates the <see cref="SymmetricAlgorithmProviderSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
		public static void GenerateWmiObjects(SymmetricAlgorithmProviderData configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(
				new SymmetricAlgorithmProviderSetting(configurationObject,
					configurationObject.Name,
					configurationObject.AlgorithmType.AssemblyQualifiedName,
					configurationObject.ProtectedKeyFilename,
					configurationObject.ProtectedKeyProtectionScope.ToString()));
		}

		internal static bool SaveChanges(SymmetricAlgorithmProviderSetting setting, ConfigurationElement sourceElement)
		{
			SymmetricAlgorithmProviderData element = (SymmetricAlgorithmProviderData)sourceElement;

			element.AlgorithmTypeName = setting.AlgorithmType;
			element.ProtectedKeyFilename = setting.ProtectedKeyFilename;
			element.ProtectedKeyProtectionScope = ParseHelper.ParseEnum<DataProtectionScope>(setting.ProtectedKeyProtectionScope, false);

			return true;
		}

		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(SymmetricAlgorithmProviderSetting));
		}
	}
}