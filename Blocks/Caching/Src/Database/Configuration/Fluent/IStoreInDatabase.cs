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
using Microsoft.Practices.EnterpriseLibrary.Caching.Database;
using Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// Fluent interface used to configure a <see cref="DataBackingStore"/> instance.
    /// </summary>
    /// <seealso cref="DataBackingStore"/>
    /// <seealso cref="DataCacheStorageData"/>
    public interface IStoreInDatabase : ICachingConfiguration, IFluentInterface
    {
        /// <summary>
        /// Specifies the name of the database, or connection string, that should be used to store cache items.
        /// </summary>
        /// <param name="connectionStringName">The name of the database, or connection string, that should be used to store cache items.</param>
        /// <seealso cref="DataBackingStore"/>
        /// <seealso cref="DataCacheStorageData"/>
        IStoreInDatabase UseSharedDatabaseNamed(string connectionStringName);

        /// <summary>
        /// Specifies the which partition should be used for the <see cref="DataBackingStore"/> instance being configured.
        /// </summary>
        /// <param name="partitionName">The name of the partition that should be used.</param>
        /// <returns>A fluent interface that can be used to further configure the current <see cref="DataBackingStore"/> instance.</returns>
        /// <seealso cref="DataBackingStore"/>
        /// <seealso cref="DataCacheStorageData"/>
        IStoreInDatabase UsePartition(string partitionName);

        /// <summary>
        /// Returns a fluent interface that can be used to set up encryption for the current <see cref="DataBackingStore"/> instance.
        /// </summary>
        /// <seealso cref="DataBackingStore"/>
        /// <seealso cref="DataCacheStorageData"/>
        IBackingStoreEncryptItemsUsing EncryptUsing { get; }

    }
}
