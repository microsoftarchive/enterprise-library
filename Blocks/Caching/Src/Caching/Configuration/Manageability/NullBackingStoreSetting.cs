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

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability
{
    /// <summary>
    /// Represents the configuration information from a 
    /// <see cref="Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.CacheStorageData"/> instance.
    /// </summary>
    /// <remarks>
    /// Class CacheStorageData is both the root of the cache storage implementation configuration objects and the
    /// configuration object that represents a 
    /// <see cref="Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations.NullBackingStore"/>.
    /// </remarks>
    /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.CacheStorageData"/>
    /// <seealso cref="CacheStorageSetting"/>
    [ManagementEntity]
    public class NullBackingStoreSetting : CacheStorageSetting
    {
        string storageEncryption;

        /// <summary>
        /// Initialize a new instance of the <see cref="NullBackingStoreSetting"/> class with the name and the storage encryption.
        /// </summary>
        /// <param name="name">The name of the backing store.</param>
        /// <param name="storageEncryption">The storage encryption for the backing store.</param>
        public NullBackingStoreSetting(string name,
                                       string storageEncryption)
            : base(name)
        {
            this.storageEncryption = storageEncryption;
        }

        /// <summary>
        /// Gets the name of the optional storage encryption provider for the represented configuration object.
        /// </summary>
        /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.CacheStorageData.StorageEncryption"/>
        [ManagementProbe]
        public string StorageEncryption
        {
            get { return storageEncryption; }
            set { storageEncryption = value; }
        }

        /// <summary>
        /// Returns the <see cref="NullBackingStoreSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <param name="Name">The value for the Name key property.</param>
        /// <returns>The published <see cref="NullBackingStoreSetting"/> instance specified by the values for the key properties,
        /// or <see langword="null"/> if no such an instance is currently published.</returns>
        [ManagementBind]
        public static NullBackingStoreSetting BindInstance(string ApplicationName,
                                                           string SectionName,
                                                           string Name)
        {
            return BindInstance<NullBackingStoreSetting>(ApplicationName, SectionName, Name);
        }

        /// <summary>
        /// Returns an enumeration of the published <see cref="NullBackingStoreSetting"/> instances.
        /// </summary>
        [ManagementEnumerator]
        public static IEnumerable<NullBackingStoreSetting> GetInstances()
        {
            return GetInstances<NullBackingStoreSetting>();
        }
    }
}
