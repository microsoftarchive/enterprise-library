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
    /// Represents a mapper for security cache provider configuraiton to Wmi.
    /// </summary>
	public static class CustomSecurityCacheProviderDataWmiMapper
	{
        /// <summary>
        /// Creates the <see cref="CustomSecurityCacheProviderSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="data">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
		public static void GenerateWmiObjects(CustomSecurityCacheProviderData data,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(
				new CustomSecurityCacheProviderSetting(data,
					data.Name,
					data.Type.AssemblyQualifiedName,
					CustomDataWmiMapperHelper.GenerateAttributesArray(data.Attributes)));
		}

		///<summary>
		/// Save the changes from configuration to Wmi.
		///</summary>
		///<param name="setting">The configuration settings.</param>
		///<param name="sourceElement">The configuration source element.</param>
		///<returns>true if the changes were successful; otherwise, false.</returns>
		public static bool SaveChanges(CustomSecurityCacheProviderSetting setting, ConfigurationElement sourceElement)
		{
			CustomSecurityCacheProviderData element = (CustomSecurityCacheProviderData)sourceElement;

			element.TypeName = setting.ProviderType;
			CustomDataWmiMapperHelper.UpdateAttributes(setting.Attributes, element.Attributes);

			return true;
		}

		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(CustomSecurityCacheProviderSetting));
		}
	}
}
