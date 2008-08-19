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
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
	/// <summary>
	/// Represents a task to perform expiration on cached items.
	/// </summary>
    public class ExpirationTask
    {
        private ICacheOperations cacheOperations;
		private CachingInstrumentationProvider instrumentationProvider;

		/// <summary>
		/// Initialize an instance of the <see cref="ExpirationTask"/> class with an <see cref="ICacheOperations"/> object.
		/// </summary>
		/// <param name="cacheOperations">An <see cref="ICacheOperations"/> object.</param>
		/// <param name="instrumentationProvider">An instrumentation provider.</param>
		public ExpirationTask(ICacheOperations cacheOperations, CachingInstrumentationProvider instrumentationProvider)
        {
            this.cacheOperations = cacheOperations;
			this.instrumentationProvider = instrumentationProvider;
        }

		/// <summary>
		/// Perform the cacheItemExpirations.
		/// </summary>
        public void DoExpirations()
        {
            Hashtable liveCacheRepresentation = cacheOperations.CurrentCacheState;
            MarkAsExpired(liveCacheRepresentation);
            PrepareForSweep();
            int expiredItemsCount = SweepExpiredItemsFromCache(liveCacheRepresentation);
			
			if(expiredItemsCount > 0) instrumentationProvider.FireCacheExpired(expiredItemsCount);
        }

		/// <summary>
		/// Mark each <see cref="CacheItem"/> as expired. 
		/// </summary>
		/// <param name="liveCacheRepresentation">The set of <see cref="CacheItem"/> objects to expire.</param>
		/// <returns>
		/// The number of items marked.
		/// </returns>
        public virtual int MarkAsExpired(Hashtable liveCacheRepresentation)
        {
            int markedCount = 0;
            foreach (CacheItem cacheItem in liveCacheRepresentation.Values)
            {
                lock (cacheItem)
                {
                    if (cacheItem.HasExpired())
                    {
                        markedCount++;
                        cacheItem.WillBeExpired = true;
                    }
                }
            }

            return markedCount;
        }

		/// <summary>
		/// Sweep and remove the <see cref="CacheItem"/>s.
		/// </summary>
		/// <param name="liveCacheRepresentation">
		/// The set of <see cref="CacheItem"/> objects to remove.
		/// </param>
		public virtual int SweepExpiredItemsFromCache(Hashtable liveCacheRepresentation)
        {
			int expiredItems = 0;

            foreach (CacheItem cacheItem in liveCacheRepresentation.Values)
            {
				if (RemoveItemFromCache(cacheItem))
					expiredItems++;
            }

			return expiredItems;
        }

		/// <summary>
		/// Prepare to sweep the <see cref="CacheItem"/>s.
		/// </summary>
        public virtual void PrepareForSweep()
        {
        }
		
		private bool RemoveItemFromCache(CacheItem itemToRemove)
        {
			bool expired = false;

            lock (itemToRemove)
            {
                if (itemToRemove.WillBeExpired)
                {
					try
					{
						expired = true;
						cacheOperations.RemoveItemFromCache(itemToRemove.Key, CacheItemRemovedReason.Expired);
					}
					catch (Exception e)
					{
						instrumentationProvider.FireCacheFailed(Resources.FailureToRemoveCacheItemInBackground, e);
					}                    
                }
            }

			return expired;
        }
    }
}