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

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Expirations.Tests
{
    /// <summary>
    /// This class reflects an expiration policy of always being expired.
    /// </summary>
    [Serializable]
    public class AlwaysExpired : ICacheItemExpiration
    {
        /// <summary>
        /// Always returns true.
        /// </summary>
        /// <returns>True always</returns>
        public bool HasExpired()
        {
            return true;
        }

        /// <summary>
        ///  Not used
        /// </summary>
        public void Notify()
        {
        }

        /// <summary>
        ///  Not used
        /// </summary>
        /// <param name="owningCacheItem">Not used</param>
        public void Initialize(CacheItem owningCacheItem)
        {
        }
    }
}

