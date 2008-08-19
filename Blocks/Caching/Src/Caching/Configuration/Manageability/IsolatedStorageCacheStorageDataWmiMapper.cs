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
	internal static class IsolatedStorageCacheStorageDataWmiMapper 
	{
        /// <summary>
        /// Creates the <see cref="IsolatedStorageCacheStorageSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
		public static void GenerateWmiObjects(IsolatedStorageCacheStorageData configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(
				new IsolatedStorageCacheStorageSetting(configurationObject.Name,
					configurationObject.PartitionName,
					configurationObject.StorageEncryption));
		}

		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(IsolatedStorageCacheStorageSetting));
		}
	}
}
