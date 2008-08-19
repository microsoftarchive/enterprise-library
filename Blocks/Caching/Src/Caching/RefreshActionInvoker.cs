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
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
    /// <summary>
    /// Purpose of this class is to encapsulate the behavior of how ICacheItemRefreshActions
    /// are invoked in the background.
    /// </summary>
    public static class RefreshActionInvoker
    {
        /// <summary>
        /// Invokes the refresh action on a thread pool thread
        /// </summary>
        /// <param name="removedCacheItem">Cache item being removed. Must never be null.</param>
		/// <param name="removalReason">The reason the item was removed.</param>	
		/// <param name="instrumentationProvider">The instrumentation provider.</param>
		public static void InvokeRefreshAction(CacheItem removedCacheItem, CacheItemRemovedReason removalReason, CachingInstrumentationProvider instrumentationProvider)
        {
            if (removedCacheItem.RefreshAction == null)
            {
                return;
            }

			try
			{
                RefreshActionData refreshActionData =
                    new RefreshActionData(removedCacheItem.RefreshAction, removedCacheItem.Key, removedCacheItem.Value, removalReason, instrumentationProvider);
                refreshActionData.InvokeOnThreadPoolThread();
			}
			catch (Exception e)
			{
				instrumentationProvider.FireCacheFailed(Resources.FailureToSpawnUserSpecifiedRefreshAction, e);
			}            
        }

        private class RefreshActionData
        {
            private ICacheItemRefreshAction refreshAction;
            private string keyToRefresh;
            private object removedData;
            private CacheItemRemovedReason removalReason;
			private CachingInstrumentationProvider instrumentationProvider;

			public RefreshActionData(ICacheItemRefreshAction refreshAction, string keyToRefresh, object removedData, CacheItemRemovedReason removalReason, CachingInstrumentationProvider instrumentationProvider)
            {
                this.refreshAction = refreshAction;
                this.keyToRefresh = keyToRefresh;
                this.removalReason = removalReason;
                this.removedData = removedData;
				this.instrumentationProvider = instrumentationProvider;
            }

            public ICacheItemRefreshAction RefreshAction
            {
                get { return refreshAction; }
            }

            public string KeyToRefresh
            {
                get { return keyToRefresh; }
            }

            public CacheItemRemovedReason RemovalReason
            {
                get { return removalReason; }
            }

            public object RemovedData
            {
                get { return removedData; }
            }

			public CachingInstrumentationProvider InstrumentationProvider
			{
				get { return instrumentationProvider; }
			}

            public void InvokeOnThreadPoolThread()
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadPoolRefreshActionInvoker));
            }

            private void ThreadPoolRefreshActionInvoker(object notUsed)
            {
				try
				{
                    RefreshAction.Refresh(KeyToRefresh, RemovedData, RemovalReason);
				}
				catch (Exception e)
				{
					InstrumentationProvider.FireCacheCallbackFailed(KeyToRefresh, e);
				}
            }
        }
    }
}
