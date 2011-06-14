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

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching
{
    ///<summary>Specifies the reason why a cache entry was removed or an entry is about to be removed.</summary>
    public enum CacheEntryRemovedReason
    {
        /// <summary>
        /// A cache entry was removed by using the <see cref="ObjectCache.Remove(string,string)"/>
        /// or <see cref="ObjectCache.Set(string,object,DateTimeOffset,string)"/> method.
        /// </summary>
        Removed = 0,

        /// <summary>
        /// A cache entry was removed because it expired. Expiration can be based on
        /// an absolute time or on a sliding expiration time.
        /// </summary>
        Expired = 1,

        /// <summary>
        /// A cache entry was removed to free memory in the cache. This occurs when a
        /// cache instance approaches cache-specific memory limits, or when a process
        /// or cache instance approaches computer-wide memory limits.
        /// </summary>
        Evicted = 2,
    }
}
