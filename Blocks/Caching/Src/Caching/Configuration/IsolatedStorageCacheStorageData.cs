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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;


namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
    /// <summary>
    /// Configuration data defining IsolatedStorageCacheStorageData. This configuration section adds the name
    /// of the Isolated Storage area to use to store data.
    /// </summary>    
    [ResourceDescription(typeof(DesignResources), "IsolatedStorageCacheStorageDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "IsolatedStorageCacheStorageDataDisplayName")]
    [System.ComponentModel.Browsable(true)]
	public class IsolatedStorageCacheStorageData : CacheStorageData
    {
		private const string partitionNameProperty = "partitionName";

        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatedStorageCacheStorageData"/> class.
        /// </summary>
        public IsolatedStorageCacheStorageData() : base(typeof(IsolatedStorageBackingStore))
        {
        }        

        /// <summary>
		/// Initialize a new instance of the <see cref="IsolatedStorageCacheStorageData"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the <see cref="IsolatedStorageCacheStorageData"/>.
        /// </param>
        /// <param name="storageEncryption">
        /// Storage Encryption data defined in configuration
        /// </param>
        /// <param name="partitionName">
        /// Name of the Isolated Storage area to use.
        /// </param>
        public IsolatedStorageCacheStorageData(string name, string storageEncryption, string partitionName) : base(name, typeof(IsolatedStorageBackingStore), storageEncryption)
        {
            PartitionName = partitionName;
        }

        /// <summary>
        /// Name of the Isolated Storage area to use.
        /// </summary>
        [ConfigurationProperty(partitionNameProperty, IsRequired= true)]
        [ResourceDescription(typeof(DesignResources), "IsolatedStorageCacheStorageDataPartitionNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "IsolatedStorageCacheStorageDataPartitionNameDisplayName")]
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
                () => new IsolatedStorageBackingStore(PartitionName,
                    Container.ResolvedIfNotNull<IStorageEncryptionProvider>(StorageEncryption)))
                    {
                        Name = Name
                    };
        }
    }
}
