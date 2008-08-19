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
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;
using Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
	/// <summary>
	/// Represents the task to start scavenging items in a <see cref="CacheManager"/>.
	/// </summary>
	public class ScavengerTask
	{
		private CacheCapacityScavengingPolicy scavengingPolicy;
		private readonly int numberToRemoveWhenScavenging;
		private ICacheOperations cacheOperations;
		private CachingInstrumentationProvider instrumentationProvider;

		/// <summary>
		/// Initialize a new instance of the <see cref="ScavengerTask"/> with a <see cref="CacheManager"/> name, a <see cref="CachingConfigurationView"/>, the <see cref="CacheCapacityScavengingPolicy"/> and the <see cref="ICacheOperations"/>.
		/// </summary>
		/// <param name="numberToRemoveWhenScavenging">The number of items that should be removed from the cache when scavenging.</param>
		/// <param name="scavengingPolicy">The <see cref="CacheCapacityScavengingPolicy"/> to use.</param>
		/// <param name="cacheOperations">The <see cref="ICacheOperations"/> to perform.</param>
		/// <param name="instrumentationProvider">An instrumentation provider.</param>
		public ScavengerTask(int numberToRemoveWhenScavenging,
							   CacheCapacityScavengingPolicy scavengingPolicy,
							   ICacheOperations cacheOperations,
							   CachingInstrumentationProvider instrumentationProvider)
		{
			this.numberToRemoveWhenScavenging = numberToRemoveWhenScavenging;
			this.scavengingPolicy = scavengingPolicy;
			this.cacheOperations = cacheOperations;
			this.instrumentationProvider = instrumentationProvider;
		}

		/// <summary>
		/// Performs the scavenging.
		/// </summary>
		public void DoScavenging()
		{
			if (NumberOfItemsToBeScavenged == 0) return;

			Hashtable liveCacheRepresentation = cacheOperations.CurrentCacheState;

			int currentNumberItemsInCache = liveCacheRepresentation.Count;

			if (scavengingPolicy.IsScavengingNeeded(currentNumberItemsInCache))
			{
				ResetScavengingFlagInCacheItems(liveCacheRepresentation);
				SortedList scavengableItems = SortItemsForScavenging(liveCacheRepresentation);
				RemoveScavengableItems(scavengableItems);
			}
		}

		private static void ResetScavengingFlagInCacheItems(Hashtable liveCacheRepresentation)
		{
			foreach (CacheItem cacheItem in liveCacheRepresentation.Values)
			{
				lock (cacheItem)
				{
					cacheItem.MakeEligibleForScavenging();
				}
			}
		}

		private static SortedList SortItemsForScavenging(Hashtable unsortedItemsInCache)
		{
			return new SortedList(unsortedItemsInCache, new PriorityDateComparer(unsortedItemsInCache));
		}

		private int NumberOfItemsToBeScavenged
		{
			get { return this.numberToRemoveWhenScavenging; }
		}

		private void RemoveScavengableItems(SortedList scavengableItems)
		{
			int scavengedItemCount = 0;
			foreach (CacheItem scavengableItem in scavengableItems.Values)
			{
				bool wasRemoved = RemoveItemFromCache(scavengableItem);
				if (wasRemoved)
				{
					scavengedItemCount++;
					if (scavengedItemCount == NumberOfItemsToBeScavenged)
					{
						break;
					}
				}
			}

			instrumentationProvider.FireCacheScavenged(scavengedItemCount);
		}

		private bool RemoveItemFromCache(CacheItem itemToRemove)
		{
			lock (itemToRemove)
			{
				if (itemToRemove.EligibleForScavenging)
				{
					try
					{
						cacheOperations.RemoveItemFromCache(itemToRemove.Key, CacheItemRemovedReason.Scavenged);
						return true;
					}
					catch (Exception e)
					{
						instrumentationProvider.FireCacheFailed(Resources.FailureToRemoveCacheItemInBackground, e);
					}
				}
			}

			return false;
		}
	}
}