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
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability
{
	/// <summary>
	/// Represents the configuration information from a 
	/// <see cref="Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.IsolatedStorageCacheStorageData"/> instance.
	/// </summary>
	/// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.IsolatedStorageCacheStorageData"/>
	/// <seealso cref="CacheStorageSetting"/>
	[ManagementEntity]
	public class IsolatedStorageCacheStorageSetting : CacheStorageSetting
	{
		private string partitionName;
		private string storageEncryption;

        /// <summary>
        /// Initialize a new instance of the <see cref="IsolatedStorageCacheStorageSetting"/> class with the name,
        /// partition name and the storage encryption.
        /// </summary>
        /// <param name="name">The name of the cache storage.</param>
        /// <param name="partitionName">The partition name.</param>
        /// <param name="storageEncryption">The storage encryption for the store.</param>
		public IsolatedStorageCacheStorageSetting(string name, string partitionName, string storageEncryption)
			: base(name)
		{
			this.partitionName = partitionName;
			this.storageEncryption = storageEncryption;
		}

		/// <summary>
		/// Gets the name of partition for the represented configuration object.
		/// </summary>
		/// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.IsolatedStorageCacheStorageData.PartitionName">IsolatedStorageCacheStorageData.PartitionName</seealso>
		[ManagementProbe]
		public string PartitionName
		{
			get { return partitionName; }
			set { partitionName = value; }
		}

		/// <summary>
		/// Gets the name of the optional storage encryption provider for the represented configuration object.
		/// </summary>
		/// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.CacheStorageData.StorageEncryption">Inherited CacheStorageData.StorageEncryption</seealso>
		[ManagementProbe]
		public string StorageEncryption
		{
			get { return storageEncryption; }
			set { storageEncryption = value; }
		}


        /// <summary>
        /// Returns an enumeration of the published <see cref="IsolatedStorageCacheStorageSetting"/> instances.
        /// </summary>
		[ManagementEnumerator]
		public static IEnumerable<IsolatedStorageCacheStorageSetting> GetInstances()
		{
			return NamedConfigurationSetting.GetInstances<IsolatedStorageCacheStorageSetting>();
		}

        /// <summary>
        /// Returns the <see cref="IsolatedStorageCacheStorageSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <param name="Name">The value for the Name key property.</param>
        /// <returns>The published <see cref="IsolatedStorageCacheStorageSetting"/> instance specified by the values for the key properties,
        /// or <see langword="null"/> if no such an instance is currently published.</returns>
		[ManagementBind]
		public static IsolatedStorageCacheStorageSetting BindInstance(string ApplicationName, string SectionName, string Name)
		{
			return NamedConfigurationSetting.BindInstance<IsolatedStorageCacheStorageSetting>(ApplicationName, SectionName, Name);
		}
	}
}