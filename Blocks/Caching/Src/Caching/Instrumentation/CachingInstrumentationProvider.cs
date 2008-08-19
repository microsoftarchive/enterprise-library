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

using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using System;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation
{
	/// <summary>
	/// Defines the logical events that can be instrumented for the caching block.
	/// </summary>
	/// <remarks>
	/// The concrete instrumentation is provided by an object bound to the events of the provider. 
	/// The default listener, automatically bound during construction, is <see cref="CachingInstrumentationListener"/>.
	/// </remarks>
	[InstrumentationListener(typeof(CachingInstrumentationListener))]
	public class CachingInstrumentationProvider
	{
		/// <summary>
		/// Occurs when an item from a <see cref="CacheManager"/> is updated.
		/// </summary>
		[InstrumentationProvider("CacheUpdated")]
		public event EventHandler<CacheUpdatedEventArgs> cacheUpdated;

		/// <summary>
		/// Occurs when an item from a <see cref="CacheManager"/> is retrieved.
		/// </summary>
		[InstrumentationProvider("CacheAccessed")]
		public event EventHandler<CacheAccessedEventArgs> cacheAccessed;

		/// <summary>
		/// Occurs when an item from a <see cref="CacheManager"/> is expired.
		/// </summary>
		[InstrumentationProvider("CacheExpired")]
		public event EventHandler<CacheExpiredEventArgs> cacheExpired;

		/// <summary>
		/// Occurs when items from a <see cref="CacheManager"/> are scavanged.
		/// </summary>
		[InstrumentationProvider("CacheScavenged")]
		public event EventHandler<CacheScavengedEventArgs> cacheScavenged;

		/// <summary>
		/// Occurs when a configured callback could not be executed.
		/// </summary>
		[InstrumentationProvider("CacheCallbackFailed")]
		public event EventHandler<CacheCallbackFailureEventArgs> cacheCallbackFailed;

		/// <summary>
		/// Occurs when a failure occurs in a <see cref="CacheManager"/>.
		/// </summary>
		[InstrumentationProvider("CacheFailed")]
		public event EventHandler<CacheFailureEventArgs> cacheFailed;

		/// <summary>
		/// Fires the <see cref="CachingInstrumentationProvider.cacheUpdated"/> event.
		/// </summary>
		/// <param name="updatedEntriesCount">The number of entries updated.</param>
		/// <param name="totalEntriesCount">The total number of entries in cache.</param>
		public void FireCacheUpdated(long updatedEntriesCount, long totalEntriesCount)
		{
			if (cacheUpdated != null) cacheUpdated(this, new CacheUpdatedEventArgs(updatedEntriesCount, totalEntriesCount));
		}

		/// <summary>
		/// Fires the <see cref="CachingInstrumentationProvider.cacheAccessed"/> event.
		/// </summary>
		/// <param name="key">The key which was used to access the cache.</param>
		/// <param name="hit"><code>true</code> if accessing the cache was successful</param>
		public void FireCacheAccessed(string key, bool hit)
		{
			if (cacheAccessed != null) cacheAccessed(this, new CacheAccessedEventArgs(key, hit));
		}

		/// <summary>
		/// Fires the <see cref="CachingInstrumentationProvider.cacheExpired"/> event.
		/// </summary>
		/// <param name="itemsExpired">The number of items that are expired.</param>
		public void FireCacheExpired(long itemsExpired)
		{
			if (cacheExpired != null) cacheExpired(this, new CacheExpiredEventArgs(itemsExpired));
		}

		/// <summary>
		/// Fires the <see cref="CachingInstrumentationProvider.cacheScavenged"/> event.
		/// </summary>
		/// <param name="itemsScavenged">The number of items scavenged from cache.</param>
		public void FireCacheScavenged(long itemsScavenged)
		{
			if (cacheScavenged != null) cacheScavenged(this, new CacheScavengedEventArgs(itemsScavenged));
		}

		/// <summary>
		/// Fires the <see cref="CachingInstrumentationProvider.cacheCallbackFailed"/> event.
		/// </summary>
		/// <param name="key">The key that was used accessing the <see cref="CacheManager"/> when this failure ocurred.</param>
		/// <param name="exception">The exception causing the failure.</param>
		public void FireCacheCallbackFailed(string key, Exception exception)
		{
			if (cacheCallbackFailed != null) cacheCallbackFailed(this, new CacheCallbackFailureEventArgs(key, exception));
		}

		/// <summary>
		/// Fires the <see cref="CachingInstrumentationProvider.cacheFailed"/> event.
		/// </summary>
		/// <param name="errorMessage">The message that describes the failure.</param>
		/// <param name="exception">The message that represents the exception causing the failure.</param>
		public void FireCacheFailed(string errorMessage, Exception exception)
		{
			if (cacheFailed != null) cacheFailed(this, new CacheFailureEventArgs(errorMessage, exception));
		}
	}
}
