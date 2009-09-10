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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Fluent
{
    /// <summary>
    /// Fluent interface used to further configure a <see cref="CacheManager"/> instance.
    /// </summary>
    /// <seealso cref="CacheManager"/>
    /// <seealso cref="CacheManagerData"/>
    public interface ICachingConfigurationCacheManagerOptions : ICachingConfigurationCacheManager
    {
        /// <summary>
        /// Specifies the time interval, in seconds, that should be waited to see whether cache items should be expired. <br/>
        /// The default interval is 60 seconds.
        /// </summary>
        /// <param name="pollExperitionSeconds">The time interval, in seconds, that should be waited to see whether cache items should be expired.</param>
        /// <returns>Fluent interface that can be used to further configure this <see cref="CacheManager"/>.</returns>
        /// <seealso cref="CacheManager"/>
        /// <seealso cref="CacheManagerData"/>
        ICachingConfigurationCacheManagerOptions PollWhetherItemsAreExpiredIntervalSeconds(int pollExperitionSeconds);

        /// <summary>
        /// Specifies the maximum numer of cache items after which scavenging will be performed. <br/>
        /// The default maximum number of cache items is 1000.
        /// </summary>
        /// <param name="numberOfElementsBeforeScavenging">The maximum numer of cache items after which scavenging will be performed.</param>
        /// <returns>Fluent interface that can be used to further configure this <see cref="CacheManager"/>.</returns>
        /// <seealso cref="CacheManager"/>
        /// <seealso cref="CacheManagerData"/>
        ICachingConfigurationCacheManagerOptions StartScavengingAfterItemCount(int numberOfElementsBeforeScavenging);

        /// <summary>
        /// Specifies the number of cache items that should be removed when scavenging cache items. <br/>
        /// The default number of cache items that should be removed is 10.
        /// </summary>
        /// <param name="numberOfElementsBeforeScavenging">The number of cache items that should be removed when scavenging cache items.</param>
        /// <returns>Fluent interface that can be used to further configure this <see cref="CacheManager"/>.</returns>
        /// <seealso cref="CacheManager"/>
        /// <seealso cref="CacheManagerData"/>
        ICachingConfigurationCacheManagerOptions WhenScavengingRemoveItemCount(int numberOfElementsBeforeScavenging);

    }
}
