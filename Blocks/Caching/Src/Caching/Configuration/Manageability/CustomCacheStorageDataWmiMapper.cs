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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability
{
	internal static class CustomCacheStorageDataWmiMapper
	{
        /// <summary>
        /// Creates the <see cref="CustomCacheStorageSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="data">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
		public static void GenerateWmiObjects(CustomCacheStorageData data,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(
				new CustomCacheStorageSetting(data.Name,
					data.Type.AssemblyQualifiedName,
					CustomDataWmiMapperHelper.GenerateAttributesArray(data.Attributes)));
		}

		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(CustomCacheStorageSetting));
		}
	}
}
