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
    /// Represents a mapper for custom hash provider configuration to Wmi.
    /// </summary>
	public static class CustomHashProviderDataWmiMapper
	{
        /// <summary>
        /// Creates the <see cref="CustomHashProviderSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="data">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
		public static void GenerateWmiObjects(CustomHashProviderData data,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(
				new CustomHashProviderSetting(data,
					data.Name,
					data.Type.AssemblyQualifiedName,
					CustomDataWmiMapperHelper.GenerateAttributesArray(data.Attributes)));
		}

		///<summary>
		/// Save the changes to the provider.
		///</summary>
		///<param name="setting">The settings to save.</param>
		///<param name="sourceElement">The source element.</param>
		///<returns>true if it was saved successfully; otherwise, false.</returns>
		public static bool SaveChanges(CustomHashProviderSetting setting, ConfigurationElement sourceElement)
		{
			CustomHashProviderData element = (CustomHashProviderData)sourceElement;

			element.TypeName = setting.ProviderType;
			CustomDataWmiMapperHelper.UpdateAttributes(setting.Attributes, element.Attributes);

			return true;
		}

		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(CustomHashProviderSetting));
		}
	}
}
