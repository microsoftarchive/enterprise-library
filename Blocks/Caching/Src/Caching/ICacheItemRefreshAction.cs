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

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
    /// <summary>
    /// This interface defines the contract that must be implemented to create an object that can be used to refresh 
    /// an expired item from the cache. The implementing class must be serializable. Care must be taken when implementing 
    /// this interface not to create an object that maintains too much state about its environment, as all portions of its
    /// environment will be serialized as well, creating possibly a huge object graph.
    /// </summary>
    public interface ICacheItemRefreshAction
    {
        /// <summary>
        /// Called when an item expires from the cache. This method can be used to notify an application that
        /// the expiration occured, cause the item to be refetched and refreshed from its original location, or 
        /// perform any other application-specific action. 
        /// </summary>
        /// <param name="removedKey">Key of item removed from cache. Will never be null.</param>
        /// <param name="expiredValue">Value from cache item that was just expired</param>
        /// <param name="removalReason">Reason the item was removed from the cache. See <see cref="CacheItemRemovedReason"/></param>
        /// <remarks>This method should catch and handle any exceptions thrown during its operation. No exceptions should leak
        /// out of it.</remarks>
        void Refresh(string removedKey, object expiredValue, CacheItemRemovedReason removalReason);
    }
}