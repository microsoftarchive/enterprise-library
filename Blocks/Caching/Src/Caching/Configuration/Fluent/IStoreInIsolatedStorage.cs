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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// Fluent interface used to configure a <see cref="IsolatedStorageBackingStore"/> instance.
    /// </summary>
    /// <seealso cref="IsolatedStorageBackingStore"/>
    /// <seealso cref="IsolatedStorageCacheStorageData"/>
    public interface IStoreInIsolatedStorage : ICachingConfiguration, IFluentInterface
    {
        /// <summary>
        /// Specifies the which partition should be used for the <see cref="IsolatedStorageBackingStore"/> instance being configured.
        /// </summary>
        /// <param name="partitionName">The name of the partition that should be used.</param>
        /// <returns>A fluent interface that can be used to further configure the current <see cref="IsolatedStorageBackingStore"/> instance.</returns>
        /// <seealso cref="IsolatedStorageBackingStore"/>
        /// <seealso cref="IsolatedStorageCacheStorageData"/>
        IStoreInIsolatedStorage UsePartition(string partitionName);

        /// <summary>
        /// Returns a fluent interface that can be used to set up encryption for the current <see cref="IsolatedStorageBackingStore"/> instance.
        /// </summary>
        /// <seealso cref="IsolatedStorageBackingStore"/>
        /// <seealso cref="IsolatedStorageCacheStorageData"/>
        IBackingStoreEncryptItemsUsing EncryptUsing { get; }

    }
}
