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

using System.Collections;

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
	/// <summary>
	/// Represents a cache operation.
	/// </summary>
    public interface ICacheOperations
    {
		/// <summary>
		/// Gets the current cache state.
		/// </summary>
		/// <returns></returns>
        Hashtable CurrentCacheState { get; }

		/// <summary>
		/// Removes a <see cref="CacheItem"/>.
		/// </summary>
		/// <param name="key">The key of the item to remove.</param>
		/// <param name="removalReason">One of the <see cref="CacheItemRemovedReason"/> values.</param>
        void RemoveItemFromCache(string key, CacheItemRemovedReason removalReason);
    }
}