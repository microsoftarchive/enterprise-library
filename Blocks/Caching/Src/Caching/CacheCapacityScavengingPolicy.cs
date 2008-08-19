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
	/// Cache scavanging policy based on capacity.
	/// </summary>
	public class CacheCapacityScavengingPolicy
	{
		private readonly int maximumElementsInCacheBeforeScavenging;

		/// <summary>
		/// Initialize a new instance of the <see cref="CacheCapacityScavengingPolicy"/> class with the name of the cache manager and the proxy to the configuration data.
		/// </summary>
		/// <param name="maximumElementsInCacheBeforeScavenging">The proxy to the latest configuration data.</param>
		public CacheCapacityScavengingPolicy(int maximumElementsInCacheBeforeScavenging)
		{
			this.maximumElementsInCacheBeforeScavenging = maximumElementsInCacheBeforeScavenging;
		}

		/// <summary>
		/// Gets the maximum items to allow before scavenging.
		/// </summary>
		/// <value>
		/// The maximum items to allow before scavenging.
		/// </value>
		public int MaximumItemsAllowedBeforeScavenging
		{
			get { return this.maximumElementsInCacheBeforeScavenging; }
		}

		/// <summary>
		/// Determines if scavanging is needed.
		/// </summary>
		/// <param name="currentCacheItemCount">The current number of <see cref="CacheItem"/> objects in the cache.</param>
		/// <returns>
		/// <see langword="true"/> if scavenging is needed; otherwise, <see langword="false"/>.
		/// </returns>
		public bool IsScavengingNeeded(int currentCacheItemCount)
		{
			return currentCacheItemCount > MaximumItemsAllowedBeforeScavenging;
		}
	}
}