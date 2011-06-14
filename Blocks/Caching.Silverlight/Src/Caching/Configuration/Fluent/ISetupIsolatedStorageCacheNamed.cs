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
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// Fluent interface used to configure a <see cref="IsolatedStorageCache"/> instance.
    /// </summary>
    /// <seealso cref="IsolatedStorageCache"/>
    /// <seealso cref="IsolatedStorageCacheData"/>
    public interface ISetupIsolatedStorageCacheNamed : IConfigureCaching, IFluentInterface
    {
        /// <summary>
        /// Specifies this <see cref="IsolatedStorageCache"/> should be the caching blocks' default <see cref="ObjectCache"/> instance.
        /// </summary>
        /// <seealso cref="IsolatedStorageCache"/>
        /// <seealso cref="IsolatedStorageCacheData"/>
        ISetupIsolatedStorageCacheNamed SetAsDefault();

        /// <summary>
        /// Returns a fluent interface to further configure the current <see cref="IsolatedStorageCache"/> instance. 
        /// </summary>
        /// <seealso cref="IsolatedStorageCache"/>
        /// <seealso cref="IsolatedStorageCacheData"/>
        ISetupIsolatedStorageCacheNamedOptions WithOptions { get; }
    }

    /// <summary>
    /// Fluent interface used to further configure a <see cref="IsolatedStorageCache"/> instance.
    /// </summary>
    /// <seealso cref="IsolatedStorageCache"/>
    /// <seealso cref="IsolatedStorageCacheData"/>
    public interface ISetupIsolatedStorageCacheNamedOptions : ISetupIsolatedStorageCacheNamed, IFluentInterface
    {
        /// <summary>
        /// Specifies the thresholds for when the scavenging logic should start/stop removing items from this <see cref="IsolatedStorageCache"/> instance.
        /// </summary>
        /// <param name="percentOfQuotaUsedBeforeScavenging">The percentage of quota before scavenging of entries needs to take place.</param>
        /// <param name="percentOfQuotaUsedAfterScavenging">The percentage of quota after scavenging has taken place.</param>
        /// <seealso cref="IsolatedStorageCache"/>
        /// <seealso cref="IsolatedStorageCacheData"/>
        ISetupIsolatedStorageCacheNamedOptions WithScavengingThresholds(int percentOfQuotaUsedBeforeScavenging, int percentOfQuotaUsedAfterScavenging);

        /// <summary>
        /// Specifies the frequency of expiration polling cycle that should be used by this <see cref="InMemoryCache"/> to check for items that expired.
        /// </summary>
        /// <param name="interval">The frequency of expiration polling cycle.</param>
        /// <seealso cref="InMemoryCache"/>
        /// <seealso cref="InMemoryCacheData"/>
        ISetupIsolatedStorageCacheNamedOptions UsingExpirationPollingInterval(TimeSpan interval);

        /// <summary>
        /// Specifies the maximum size in kilobytes that can be used by this <see cref="IsolatedStorageCache"/> to store items.
        /// </summary>
        /// <param name="maxSizeInKilobytes">The maximum size in kilobytes for the cache.</param>
        /// <seealso cref="IsolatedStorageCache"/>
        /// <seealso cref="IsolatedStorageCacheData"/>
        ISetupIsolatedStorageCacheNamedOptions WithMaxSizeInKilobytes(int maxSizeInKilobytes);

        /// <summary>
        /// Specifies the serializer <see cref="Type"/> used for serializing and deserializing the cache entries.
        /// </summary>
        /// <param name="serializerType">The type used for serializing and deserializing the cache entries.</param>
        /// <seealso cref="IsolatedStorageCache"/>
        /// <seealso cref="IsolatedStorageCacheData"/>
        ISetupIsolatedStorageCacheNamedOptions UsingSerializerOfType(Type serializerType);
    }
}
