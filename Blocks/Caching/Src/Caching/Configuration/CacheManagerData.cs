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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
	/// <summary>
	/// Configuration data defining CacheManagerData. Defines the information needed to properly configure
	/// a CacheManager instance.
	/// </summary>
	[Assembler(typeof(CacheManagerAssembler))]
	[ContainerPolicyCreator(typeof(CacheManagerPolicyCreator))]
	public class CacheManagerData : CacheManagerDataBase
	{
		private const string expirationPollFrequencyInSecondsProperty = "expirationPollFrequencyInSeconds";
		private const string maximumElementsInCacheBeforeScavengingProperty = "maximumElementsInCacheBeforeScavenging";
		private const string numberToRemoveWhenScavengingProperty = "numberToRemoveWhenScavenging";
		private const string backingStoreNameProperty = "backingStoreName";

		/// <summary>
		/// Initialize a new instance of the <see cref="CacheManagerData"/> class.
		/// </summary>
		public CacheManagerData()
		{
		}

		/// <summary>
		/// Initialize a new instance of the <see cref="CacheManagerData"/> class.
		/// </summary>
		/// <param name="name">
		/// The name of the <see cref="CacheManagerData"/>.
		/// </param>
		/// <param name="expirationPollFrequencyInSeconds">
		/// Frequency in seconds of expiration polling cycle
		/// </param>
		/// <param name="maximumElementsInCacheBeforeScavenging">
		/// Maximum number of items in cache before an add causes scavenging to take place
		/// </param>
		/// <param name="numberToRemoveWhenScavenging">
		/// Number of items to remove from cache when scavenging
		/// </param>
		/// <param name="cacheStorage">
		/// CacheStorageData object from configuration describing how data is stored 
		/// in the cache.
		/// </param>
		public CacheManagerData(string name, int expirationPollFrequencyInSeconds, int maximumElementsInCacheBeforeScavenging, int numberToRemoveWhenScavenging, string cacheStorage)
			: base(name, typeof(CacheManager))
		{
			this.ExpirationPollFrequencyInSeconds = expirationPollFrequencyInSeconds;
			this.MaximumElementsInCacheBeforeScavenging = maximumElementsInCacheBeforeScavenging;
			this.NumberToRemoveWhenScavenging = numberToRemoveWhenScavenging;
			this.CacheStorage = cacheStorage;
		}

		/// <summary>
		/// Frequency in seconds of expiration polling cycle
		/// </summary>
		[ConfigurationProperty(expirationPollFrequencyInSecondsProperty, IsRequired = true)]
		public int ExpirationPollFrequencyInSeconds
		{
			get { return (int)base[expirationPollFrequencyInSecondsProperty]; }
			set { base[expirationPollFrequencyInSecondsProperty] = value; }
		}

		/// <summary>
		/// Maximum number of items in cache before an add causes scavenging to take place
		/// </summary>
		[ConfigurationProperty(maximumElementsInCacheBeforeScavengingProperty, IsRequired = true)]
		public int MaximumElementsInCacheBeforeScavenging
		{
			get { return (int)base[maximumElementsInCacheBeforeScavengingProperty]; }
			set { base[maximumElementsInCacheBeforeScavengingProperty] = value; }
		}

		/// <summary>
		/// Number of items to remove from cache when scavenging
		/// </summary>
		[ConfigurationProperty(numberToRemoveWhenScavengingProperty, IsRequired = true)]
		public int NumberToRemoveWhenScavenging
		{
			get { return (int)base[numberToRemoveWhenScavengingProperty]; }
			set { base[numberToRemoveWhenScavengingProperty] = value; }
		}

		/// <summary>
		/// CacheStorageData object from configuration describing how data is stored 
		/// in the cache.
		/// </summary>
		[ConfigurationProperty(backingStoreNameProperty, IsRequired = true)]
		public string CacheStorage
		{
			get { return (string)base[backingStoreNameProperty]; }
			set { base[backingStoreNameProperty] = value; }
		}
	}
}