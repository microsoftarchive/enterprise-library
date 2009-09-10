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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// <see cref="ICachingConfigurationCacheManager"/> extension that allows an <see cref="IsolatedStorageBackingStore"/> to be configured.
    /// </summary>
    /// <seealso cref="IsolatedStorageBackingStore"/>
    /// <seealso cref="IsolatedStorageCacheStorageData"/>
    public static class StoreInIsolatedStorageExtension
    {
        /// <summary>
        /// Specifies that current <see cref="CacheManager"/>'s items should be stored using a <see cref="IsolatedStorageBackingStore"/> instance.
        /// </summary>
        /// <param name="backingStoreName">The name of the <see cref="IsolatedStorageBackingStore"/> instance</param>
        /// <param name="context">Fluent interface extension point.</param>
        /// <seealso cref="IsolatedStorageBackingStore"/>
        /// <seealso cref="IsolatedStorageCacheStorageData"/>
        public static IStoreInIsolatedStorage StoreInIsolatedStorage(this ICachingConfigurationCacheManager context, string backingStoreName)
        {
            if (backingStoreName == null) throw new ArgumentException(Resources.EmptyParameterName, "backingStoreName");
            return new StoreInIsolatedStorageBuilder(context, backingStoreName);
        }

        private class StoreInIsolatedStorageBuilder : CacheManagerExtension, IStoreInIsolatedStorage, IStoreInCustomStore, IBackingStoreEncryptItemsUsing
        {
            IsolatedStorageCacheStorageData isolatedStorageData;

            public StoreInIsolatedStorageBuilder(ICachingConfigurationCacheManager context, string backingStoreName)
                :base(context)
            {
                isolatedStorageData = new IsolatedStorageCacheStorageData
                {
                    Name = backingStoreName
                };

                base.AddBackingStoreToCachingConfigurationAndCurrentCacheManager(isolatedStorageData);
            }

            public IStoreInIsolatedStorage UsePartition(string partitionName)
            {
                if (string.IsNullOrEmpty(partitionName)) throw new ArgumentException(Resources.EmptyParameterName, "isolatedStorageData");

                isolatedStorageData.PartitionName = partitionName;
                return this;
            }

            public IBackingStoreEncryptItemsUsing EncryptUsing
            {
                get { return this; }
            }

            public ICachingConfiguration SharedEncryptionProviderNamed(string encryptionProviderName)
            {
                isolatedStorageData.StorageEncryption = encryptionProviderName;

                return this;
            }
        }
    }
}
