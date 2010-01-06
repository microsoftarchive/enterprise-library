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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration
{
	/// <summary>
	/// Configuration information for DataCacheStorageData. This class represents the extra information, over and
    /// above what is defined in <see cref="CacheStorageData" />, needed to connect caching to the Data Access Application Block.
	/// </summary>
    [ResourceDescription(typeof(DesignResources), "DataCacheStorageDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "DataCacheStorageDataDisplayName")]
    [AddSateliteProviderCommand("connectionStrings", typeof(DatabaseSettings), "DefaultDatabase", "DatabaseInstanceName")]
    [System.ComponentModel.Browsable(true)]
    public class DataCacheStorageData : CacheStorageData
    {
        private const string databaseInstanceNameProperty = "databaseInstanceName";
        private const string partitionNameProperty = "partitionName";

        /// <overloads>
        /// Initializes an instance of a <see cref="DataCacheStorageData"/> class.
        /// </overloads>
        /// <summary>
        /// Initializes an instance of a <see cref="DataCacheStorageData"/> class.
        /// </summary>
        public DataCacheStorageData() : base(typeof(DataBackingStore))
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="DataCacheStorageData"/> class with a name, database instance name, and partion name.
        /// </summary>
        /// <param name="name">
        /// The name of the provider.
        /// </param>
        /// <param name="databaseInstanceName">
        /// Name of the database instance to use for storage. Instance must be defined in Data configuration.
        /// </param>
        /// <param name="partitionName">
        /// Name of the particular section inside of a database used to store this provider's data. This 
        /// field allows different applications to share the same database safely, preventing any modification of 
        /// one application's data by a provider from another application.
        /// </param>
        public DataCacheStorageData(string name, string databaseInstanceName, string partitionName)
            : base(name, typeof(DataBackingStore))
        {
            this.DatabaseInstanceName = databaseInstanceName;
            this.PartitionName = partitionName;
        }

        /// <summary>
        /// Name of the database instance to use for storage. Instance must be defined in Data configuration.
        /// </summary>
        [ConfigurationProperty(databaseInstanceNameProperty, IsRequired = true)]
        [ResourceDescription(typeof(DesignResources), "DataCacheStorageDataDatabaseInstanceNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "DataCacheStorageDataDatabaseInstanceNameDisplayName")]
        [Reference(typeof(ConnectionStringSettingsCollection), typeof(ConnectionStringSettings))]
        public string DatabaseInstanceName
        {
            get { return (string)base[databaseInstanceNameProperty]; }
            set { base[databaseInstanceNameProperty] = value; }
        }

        /// <summary>
        /// Name of the particular section inside of a database used to store this provider's data. This 
        /// field allows different applications to share the same database safely, preventing any modification of 
        /// one application's data by a provider from another application.
        /// </summary>
        [ConfigurationProperty(partitionNameProperty, IsRequired = true)]
        [ResourceDescription(typeof(DesignResources), "DataCacheStorageDataPartitionNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "DataCacheStorageDataPartitionNameDisplayName")]
        public string PartitionName
        {
            get { return (string)base[partitionNameProperty]; }
            set { base[partitionNameProperty] = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<TypeRegistration> GetRegistrations()
        {
            yield return new TypeRegistration<IBackingStore>(
                () => new DataBackingStore(Container.Resolved<Data.Database>(DatabaseInstanceName), 
                                            PartitionName, 
                                            Container.ResolvedIfNotNull<IStorageEncryptionProvider>(StorageEncryption)))
                {
                    Name = this.Name
                };
        }
    }
}
