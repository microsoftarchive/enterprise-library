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
    /// Represents a mapper for custom symmetric cryptography providers to Wmi.
    /// </summary>
	public static class CustomSymmetricCryptoProviderDataWmiMapper
	{
        /// <summary>
        /// Creates the <see cref="CustomSymmetricCryptoProviderSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="data">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
		public static void GenerateWmiObjects(CustomSymmetricCryptoProviderData data,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(
				new CustomSymmetricCryptoProviderSetting(data,
					data.Name,
					data.Type.AssemblyQualifiedName,
					CustomDataWmiMapperHelper.GenerateAttributesArray(data.Attributes)));
		}

		internal static bool SaveChanges(CustomSymmetricCryptoProviderSetting setting, ConfigurationElement sourceElement)
		{
			CustomSymmetricCryptoProviderData element = (CustomSymmetricCryptoProviderData)sourceElement;

			element.TypeName = setting.ProviderType;
			CustomDataWmiMapperHelper.UpdateAttributes(setting.Attributes, element.Attributes);

			return true;
		}

		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(CustomSymmetricCryptoProviderSetting));
		}
	}
}
