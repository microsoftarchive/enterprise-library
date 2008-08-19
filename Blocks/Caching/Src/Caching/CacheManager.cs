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
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
	/// <summary>
	/// This class represents the interface to caching as shown to the user. All caching operations are performed through this class.
	/// </summary>
	[ConfigurationElementType(typeof(CacheManagerData))]
	public class CacheManager : IDisposable, ICacheManager
	{
		private Cache realCache;
		private BackgroundScheduler scheduler;
		private ExpirationPollTimer pollTimer;

		internal CacheManager(Cache realCache, BackgroundScheduler scheduler, ExpirationPollTimer pollTimer)
		{
			this.realCache = realCache;
			this.scheduler = scheduler;
			this.pollTimer = pollTimer;
		}

		/// <summary>
		/// Returns the number of items currently in the cache.
		/// </summary>
		public int Count
		{
			get { return realCache.Count; }
		}

		/// <summary>
		/// Returns true if key refers to item current stored in cache
		/// </summary>
		/// <param name="key">Key of item to check for</param>
		/// <returns>True if item referenced by key is in the cache</returns>
		public bool Contains(string key)
		{
			return realCache.Contains(key);
		}

		/// <summary>
		/// Returns the item identified by the provided key
		/// </summary>
		/// <param name="key">Key to retrieve from cache</param>
		/// <exception cref="ArgumentNullException">Provided key is null</exception>
		/// <exception cref="ArgumentException">Provided key is an empty string</exception>
		/// <remarks>The CacheManager can be configured to use different storage mechanisms in which to store the cache items.
		/// Each of these storage mechanisms can throw exceptions particular to their own implementations.</remarks>
		public object this[string key]
		{
			get { return realCache.GetData(key); }
		}

		/// <summary>
		/// Adds new CacheItem to cache. If another item already exists with the same key, that item is removed before
		/// the new item is added. If any failure occurs during this process, the cache will not contain the item being added. 
		/// Items added with this method will be not expire, and will have a Normal <see cref="CacheItemPriority" /> priority.
		/// </summary>
		/// <param name="key">Identifier for this CacheItem</param>
		/// <param name="value">Value to be stored in cache. May be null.</param>
		/// <exception cref="ArgumentNullException">Provided key is null</exception>
		/// <exception cref="ArgumentException">Provided key is an empty string</exception>
		/// <remarks>The CacheManager can be configured to use different storage mechanisms in which to store the CacheItems.
		/// Each of these storage mechanisms can throw exceptions particular to their own implementations.</remarks>
		public void Add(string key, object value)
		{
			Add(key, value, CacheItemPriority.Normal, null);
		}

		/// <summary>
		/// Adds new CacheItem to cache. If another item already exists with the same key, that item is removed before
		/// the new item is added. If any failure occurs during this process, the cache will not contain the item being added.
		/// </summary>
		/// <param name="key">Identifier for this CacheItem</param>
		/// <param name="value">Value to be stored in cache. May be null.</param>
		/// <param name="scavengingPriority">Specifies the new item's scavenging priority. 
		/// See <see cref="CacheItemPriority" /> for more information.</param>
		/// <param name="refreshAction">Object provided to allow the cache to refresh a cache item that has been expired. May be null.</param>
		/// <param name="expirations">Param array specifying the expiration policies to be applied to this item. May be null or omitted.</param>
		/// <exception cref="ArgumentNullException">Provided key is null</exception>
		/// <exception cref="ArgumentException">Provided key is an empty string</exception>
		/// <remarks>The CacheManager can be configured to use different storage mechanisms in which to store the CacheItems.
		/// Each of these storage mechanisms can throw exceptions particular to their own implementations.</remarks>
		public void Add(string key, object value, CacheItemPriority scavengingPriority, ICacheItemRefreshAction refreshAction, params ICacheItemExpiration[] expirations)
		{
			realCache.Add(key, value, scavengingPriority, refreshAction, expirations);
		}

		/// <summary>
		/// Removes the given item from the cache. If no item exists with that key, this method does nothing.
		/// </summary>
		/// <param name="key">Key of item to remove from cache.</param>
		/// <exception cref="ArgumentNullException">Provided key is null</exception>
		/// <exception cref="ArgumentException">Provided key is an empty string</exception>
		/// <remarks>The CacheManager can be configured to use different storage mechanisms in which to store the CacheItems.
		/// Each of these storage mechanisms can throw exceptions particular to their own implementations.</remarks>
		public void Remove(string key)
		{
			realCache.Remove(key);
		}

		/// <summary>
		/// Returns the value associated with the given key.
		/// </summary>
		/// <param name="key">Key of item to return from cache.</param>
		/// <returns>Value stored in cache</returns>
		/// <exception cref="ArgumentNullException">Provided key is null</exception>
		/// <exception cref="ArgumentException">Provided key is an empty string</exception>
		/// <remarks>The CacheManager can be configured to use different storage mechanisms in which to store the CacheItems.
		/// Each of these storage mechanisms can throw exceptions particular to their own implementations.</remarks>
		public object GetData(string key)
		{
			return realCache.GetData(key);
		}

		/// <summary>
		/// Removes all items from the cache. If an error occurs during the removal, the cache is left unchanged.
		/// </summary>
		/// <remarks>The CacheManager can be configured to use different storage mechanisms in which to store the CacheItems.
		/// Each of these storage mechanisms can throw exceptions particular to their own implementations.</remarks>
		public void Flush()
		{
			realCache.Flush();
		}

		/// <summary>
		/// Not intended for public use. Only public due to requirements of IDisposable. If you call this method, your
		/// cache will be unusable.
		/// </summary>
		public void Dispose()
		{
			GC.SuppressFinalize(this);

			if (scheduler != null)
			{
				scheduler.Stop();
				scheduler = null;
			}
			if (pollTimer != null)
			{
				pollTimer.StopPolling();
				pollTimer = null;
			}
			if (realCache != null)
			{
				realCache.Dispose();
				realCache = null;
			}
		}
	}
}