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
    /// Fluent interface used to configure a <see cref="InMemoryCache"/> instance.
    /// </summary>
    /// <seealso cref="InMemoryCache"/>
    /// <seealso cref="InMemoryCacheData"/>
    public interface ISetupInMemoryCacheNamed : IConfigureCaching, IFluentInterface
    {
        /// <summary>
        /// Specifies this <see cref="InMemoryCache"/> should be the caching blocks' default <see cref="ObjectCache"/> instance.
        /// </summary>
        /// <seealso cref="InMemoryCache"/>
        /// <seealso cref="InMemoryCacheData"/>
        ISetupInMemoryCacheNamed SetAsDefault();

        /// <summary>
        /// Returns a fluent interface to further configure the current <see cref="InMemoryCache"/> instance. 
        /// </summary>
        /// <seealso cref="InMemoryCache"/>
        /// <seealso cref="InMemoryCacheData"/>
        ISetupInMemoryCacheNamedOptions WithOptions { get; }
    }

    /// <summary>
    /// Fluent interface used to further configure a <see cref="InMemoryCache"/> instance.
    /// </summary>
    /// <seealso cref="InMemoryCache"/>
    /// <seealso cref="InMemoryCacheData"/>
    public interface ISetupInMemoryCacheNamedOptions : ISetupInMemoryCacheNamed, IFluentInterface
    {
        /// <summary>
        /// Specifies the thresholds for when the scavenging logic should start/stop removing items from this <see cref="InMemoryCache"/> instance.
        /// </summary>
        /// <param name="maxItemsBeforeScavenging">The maximum number of items in cache before an add causes scavenging to take place.</param>
        /// <param name="itemsLeftAfterScavenging">The number of items left in the cache after scavenging has taken place.</param>
        /// <seealso cref="InMemoryCache"/>
        /// <seealso cref="InMemoryCacheData"/>
        ISetupInMemoryCacheNamedOptions WithScavengingThresholds(int maxItemsBeforeScavenging, int itemsLeftAfterScavenging);

        /// <summary>
        /// Specifies the frequency of expiration polling cycle that should be used by this <see cref="InMemoryCache"/> to check for items that expired.
        /// </summary>
        /// <param name="interval">The frequency of expiration polling cycle.</param>
        /// <seealso cref="InMemoryCache"/>
        /// <seealso cref="InMemoryCacheData"/>
        ISetupInMemoryCacheNamedOptions UsingExpirationPollingInterval(TimeSpan interval);
    }
}
