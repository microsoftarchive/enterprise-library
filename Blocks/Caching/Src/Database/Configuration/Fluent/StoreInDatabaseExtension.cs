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
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Database;
using Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// <see cref="ICachingConfigurationCacheManager"/> extension that allows an <see cref="DataBackingStore"/> to be configured.
    /// </summary>
    /// <seealso cref="DataBackingStore"/>
    /// <seealso cref="DataCacheStorageData"/>
    public static class StoreInDatabaseExtension
    {
        /// <summary>
        /// Specifies that current <see cref="CacheManager"/>'s items should be stored using a <see cref="DataBackingStore"/> instance.
        /// </summary>
        /// <param name="backingStoreName">The name of the <see cref="DataBackingStore"/> instance.</param>
        /// <param name="context">Fluent interface extension point.</param>
        /// <seealso cref="DataBackingStore"/>
        /// <seealso cref="DataCacheStorageData"/>
        public static IStoreInDatabase StoreCacheMangerItemsInDatabase(this ICachingConfigurationCacheManager context, string backingStoreName)
        {
            if (String.IsNullOrEmpty(backingStoreName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "backingStoreName");
            return new StoreInDatabase(context, backingStoreName);
        }

        private class StoreInDatabase : CacheManagerExtension, IStoreInDatabase, IBackingStoreEncryptItemsUsing 
        {
            DataCacheStorageData dataCacheStore;

            public StoreInDatabase(ICachingConfigurationCacheManager context, string backingStoreName)
                : base(context)
            {
                dataCacheStore = new DataCacheStorageData
                {
                    Name = backingStoreName
                };

                base.AddBackingStoreToCachingConfigurationAndCurrentCacheManager(dataCacheStore);
            }

            public IStoreInDatabase UseSharedDatabaseNamed(string connectionStringName)
            {
                if (String.IsNullOrEmpty(connectionStringName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "connectionStringName");

                dataCacheStore.DatabaseInstanceName = connectionStringName;

                return this;
            }

            public IStoreInDatabase UsePartition(string partitionName)
            {
                if (String.IsNullOrEmpty(partitionName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "partitionName");

                dataCacheStore.PartitionName = partitionName;

                return this;
            }


            public IBackingStoreEncryptItemsUsing EncryptUsing
            {
                get{ return this; }
            }

            public ICachingConfiguration SharedEncryptionProviderNamed(string encryptionProviderName)
            {
                dataCacheStore.StorageEncryption = encryptionProviderName;
                
                return this;
            }
        }
    }
}
