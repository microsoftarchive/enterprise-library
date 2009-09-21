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
using System.Linq;
using System.Linq.Expressions;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
	/// <summary>
	/// Configuration data defining CacheManagerData. Defines the information needed to properly configure
	/// a CacheManager instance.
	/// </summary>
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
            :base(typeof(CacheManager))
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
		[ConfigurationProperty(expirationPollFrequencyInSecondsProperty, IsRequired = true, DefaultValue=60)]
		public int ExpirationPollFrequencyInSeconds
		{
			get { return (int)base[expirationPollFrequencyInSecondsProperty]; }
			set { base[expirationPollFrequencyInSecondsProperty] = value; }
		}

		/// <summary>
		/// Maximum number of items in cache before an add causes scavenging to take place
		/// </summary>
		[ConfigurationProperty(maximumElementsInCacheBeforeScavengingProperty, IsRequired = true, DefaultValue=1000)]
		public int MaximumElementsInCacheBeforeScavenging
		{
			get { return (int)base[maximumElementsInCacheBeforeScavengingProperty]; }
			set { base[maximumElementsInCacheBeforeScavengingProperty] = value; }
		}

		/// <summary>
		/// Number of items to remove from cache when scavenging
		/// </summary>
		[ConfigurationProperty(numberToRemoveWhenScavengingProperty, IsRequired = true, DefaultValue=10)]
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
        [Reference(typeof(CacheManagerSettings), typeof(CacheStorageData))]
		public string CacheStorage
		{
			get { return (string)base[backingStoreNameProperty]; }
			set { base[backingStoreNameProperty] = value; }
		}


        /// <summary>
        /// Get the set of <see cref="TypeRegistration"/> object needed to
        /// register the CacheManager represented by this config element.
        /// </summary>
        /// <returns>The sequence of <see cref="TypeRegistration"/> objects.</returns>
        public override IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource)
        {
            IEnumerable<TypeRegistration> baseRegistrations = base.GetRegistrations(configurationSource);

            return baseRegistrations.Concat( new TypeRegistration[]{ 
                GetBackgroundScheduler(),
                GetExpirationTask(),
                GetScavengerTask(),
                GetExpirationPollTimer(),
                GetInstrumentationProviderRegistration(configurationSource), 
                GetCacheRegistration() });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override Expression<Func<ICacheManager>> GetCacheManagerCreationExpression()
        {
            return () =>
                new CacheManager( Container.Resolved<Cache>(Name),
                                  Container.Resolved<BackgroundScheduler>(Name),
                                  Container.Resolved<ExpirationPollTimer>(Name));


        }

        private TypeRegistration GetBackgroundScheduler()
        {
            return new TypeRegistration<BackgroundScheduler>(() => new BackgroundScheduler(
                Container.Resolved<ExpirationTask>(Name),
                Container.Resolved<ScavengerTask>(Name),
                Container.Resolved<ICachingInstrumentationProvider>(Name)))
            {
                Name = this.Name
            };

        }

        private TypeRegistration GetExpirationTask()
        {
            return new TypeRegistration<ExpirationTask>(() => new ExpirationTask(
                Container.Resolved<Cache>(Name),
                Container.Resolved<ICachingInstrumentationProvider>(Name)))
                {
                    Name = this.Name
                };
        }

        private TypeRegistration GetScavengerTask()
        {
            return new TypeRegistration<ScavengerTask>(() => new ScavengerTask(
              NumberToRemoveWhenScavenging, 
              MaximumElementsInCacheBeforeScavenging, 
              Container.Resolved<Cache>(Name), 
              Container.Resolved<ICachingInstrumentationProvider>(Name)))
            {
                Name = this.Name
            };
        }

        private TypeRegistration GetExpirationPollTimer()
        {
            return new TypeRegistration<ExpirationPollTimer>(() => new ExpirationPollTimer(ExpirationPollFrequencyInSeconds * 1000))
            {
                Name = this.Name
            };
        }

        private TypeRegistration GetCacheRegistration()
        {
            return new TypeRegistration<Cache>(() => new Cache(
                Container.Resolved<IBackingStore>(CacheStorage),
                Container.Resolved<ICachingInstrumentationProvider>(Name)))
            {
                Name = this.Name
            };
        }

        private TypeRegistration GetInstrumentationProviderRegistration(IConfigurationSource configurationSource)
        {
            var instrumentationSection = InstrumentationConfigurationSection.GetSection(configurationSource);

            return new TypeRegistration<ICachingInstrumentationProvider>(
                () => new CachingInstrumentationProvider(
                    Name,
                    instrumentationSection.PerformanceCountersEnabled,
                    instrumentationSection.EventLoggingEnabled,
                    instrumentationSection.WmiEnabled,
                    instrumentationSection.ApplicationInstanceName))
               {
                   Name = Name
               };
        }
	}
}
