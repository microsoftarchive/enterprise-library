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

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching
{
    /// <summary>
    /// Specifies a priority setting that is used to decide whether to evict a cache entry.
    /// </summary>
    public enum CacheItemPriority
    {
        /// <summary>
        /// Indicates that there is no priority for removing the cache entry.
        /// </summary>
        Default,

        /// <summary>
        /// Indicates that a cache entry should never be removed from the cache.
        /// </summary>
        NotRemovable
    }
}
