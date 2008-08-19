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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability
{
    /// <summary>
    /// Represents a mapper for <see cref="CachingBlockSetting"/> configuration to Wmi.
    /// </summary>
	public static class CacheManagerSettingsWmiMapper
	{
        /// <summary>
        /// Creates the <see cref="CachingBlockSetting"/> instances that describe the configurationObject.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
		public static void GenerateWmiObjects(CacheManagerSettings configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(new CachingBlockSetting(configurationObject, configurationObject.DefaultCacheManager));
		}

		/// <summary>
        /// Creates the <see cref="CachingBlockSetting"/> instances that describe the configurationObject.
		/// </summary>
        /// <param name="data">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
		public static void GenerateCacheManagerWmiObjects(CacheManagerDataBase data,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			if (data is CacheManagerData)
			{
				CacheManagerData cacheManagerData = (CacheManagerData)data;

				wmiSettings.Add(
					new CacheManagerSetting(data.Name,
								cacheManagerData.CacheStorage,
								cacheManagerData.ExpirationPollFrequencyInSeconds,
								cacheManagerData.MaximumElementsInCacheBeforeScavenging,
								cacheManagerData.NumberToRemoveWhenScavenging));
			}
			else
			{
				wmiSettings.Add(new UnknownCacheManagerSetting(data.Name));
			}
		}

		/// <summary>
        /// Save chages made to a <see cref="CachingBlockSetting" /> instance/>
		/// </summary>
        /// <param name="cachingBlockSetting">The configuration object.</param>
		/// <param name="sourceElement">The parent configuration object.</param>
		public static bool SaveChanges(CachingBlockSetting cachingBlockSetting, ConfigurationElement sourceElement)
		{
			CacheManagerSettings settings = (CacheManagerSettings)sourceElement;

			settings.DefaultCacheManager = cachingBlockSetting.DefaultCacheManager;

			return true;
		}

		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(CachingBlockSetting),
				typeof(CacheManagerSetting),
				typeof(UnknownCacheManagerSetting));
		}
	}
}
