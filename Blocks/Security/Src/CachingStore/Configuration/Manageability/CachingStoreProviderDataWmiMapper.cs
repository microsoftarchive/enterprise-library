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

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration.Manageability
{
    /// <summary>
    /// Represents a mapper for caching store provider configuration to Wmi.
    /// </summary>
	public static partial class CachingStoreProviderDataWmiMapper
	{
        /// <summary>
        /// Creates the <see cref="CachingStoreProviderSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
		public static void GenerateWmiObjects(CachingStoreProviderData configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(
				new CachingStoreProviderSetting(configurationObject,
					configurationObject.Name,
					configurationObject.CacheManager,
					configurationObject.AbsoluteExpiration,
					configurationObject.SlidingExpiration));
		}

		internal static bool SaveChanges(CachingStoreProviderSetting cachingStoreProviderSetting, ConfigurationElement sourceElement)
		{
			CachingStoreProviderData element = (CachingStoreProviderData)sourceElement;

			element.AbsoluteExpiration = cachingStoreProviderSetting.AbsoluteExpiration;
			element.CacheManager = cachingStoreProviderSetting.CacheManager;
			element.SlidingExpiration = cachingStoreProviderSetting.SlidingExpiration;

			return true;
		}

		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(CachingStoreProviderSetting));
		}
	}
}