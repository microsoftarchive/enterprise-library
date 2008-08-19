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

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Expirations
{
    /// <summary>
    /// This class reflects an expiration policy of never being expired.
    /// </summary>
    [Serializable]
    public class NeverExpired : ICacheItemExpiration
    {
        /// <summary>
        /// Always returns false
        /// </summary>
        /// <returns>False always</returns>
        public bool HasExpired()
        {
            return false;
        }

        /// <summary>
        /// Not used
        /// </summary>
        public void Notify()
        {
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="owningCacheItem">Not used</param>
        public void Initialize(CacheItem owningCacheItem)
        {
        }
    }
}