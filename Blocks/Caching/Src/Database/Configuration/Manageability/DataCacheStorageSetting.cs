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
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.Manageability
{
    /// <summary>
    /// Represents the configuration information from a 
    /// <see cref="Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.DataCacheStorageData"/> instance.
    /// </summary>
    /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.DataCacheStorageData"/>
    /// <seealso cref="CacheStorageSetting"/>
    [ManagementEntity]
    public class DataCacheStorageSetting : CacheStorageSetting
    {
        string databaseInstanceName;
        string partitionName;
        string storageEncryption;

        /// <summary>
        /// Initialize a new instance of the <see cref="DataCacheStorageSetting"/> with the name of the storage,
        /// the instance name, the partition name and the storage encryption.
        /// </summary>
        /// <param name="name">The name of the data storage.</param>
        /// <param name="databaseInstanceName">The database instance name.</param>
        /// <param name="partitionName">The partion name.</param>
        /// <param name="storageEncryption">The storage encryption.</param>
        public DataCacheStorageSetting(string name,
                                       string databaseInstanceName,
                                       string partitionName,
                                       string storageEncryption)
            : base(name)
        {
            this.databaseInstanceName = databaseInstanceName;
            this.partitionName = partitionName;
            this.storageEncryption = storageEncryption;
        }

        /// <summary>
        /// Gets the name of database for the represented configuration object.
        /// </summary>
        /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.DataCacheStorageData.DatabaseInstanceName">DataCacheStorageData.DatabaseInstanceName</seealso>
        [ManagementProbe]
        public string DatabaseInstanceName
        {
            get { return databaseInstanceName; }
            set { databaseInstanceName = value; }
        }

        /// <summary>
        /// Gets the name of partition for the represented configuration object.
        /// </summary>
        /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.DataCacheStorageData.PartitionName">DataCacheStorageData.PartitionName</seealso>
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
        /// Returns the <see cref="DataCacheStorageSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <param name="Name">The value for the Name key property.</param>
        /// <returns>The published <see cref="DataCacheStorageSetting"/> instance specified by the values for the key properties,
        /// or <see langword="null"/> if no such an instance is currently published.</returns>
        [ManagementBind]
        public static DataCacheStorageSetting BindInstance(string ApplicationName,
                                                           string SectionName,
                                                           string Name)
        {
            return BindInstance<DataCacheStorageSetting>(ApplicationName, SectionName, Name);
        }

        /// <summary>
        /// Returns an enumeration of the published <see cref="DataCacheStorageSetting"/> instances.
        /// </summary>
        [ManagementEnumerator]
        public static IEnumerable<DataCacheStorageSetting> GetInstances()
        {
            return GetInstances<DataCacheStorageSetting>();
        }
    }
}
