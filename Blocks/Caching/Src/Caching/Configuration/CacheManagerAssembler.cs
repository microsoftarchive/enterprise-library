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

using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Represents the process to build an <see cref="CacheManager"/> described by a <see cref="CacheManagerData"/> configuration object.
	/// </summary>
	/// <remarks>This type is linked to the <see cref="CacheManagerDataBase"/> type and it is used by the <see cref="CacheManagerCustomFactory"/> 
	/// to build the specific <see cref="ICacheManager"/> object represented by the configuration object.
	/// </remarks>	
	public class CacheManagerAssembler : IAssembler<ICacheManager, CacheManagerDataBase>
	{
		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Builds a <see cref="CacheManager"/> based on an instance of <see cref="CacheManagerData"/>.
		/// </summary>
		/// <seealso cref="CacheManagerCustomFactory"/>
		/// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
		/// <param name="objectConfiguration">The configuration object that describes the object to build. Must be an instance of <see cref="CacheManagerData"/>.</param>
		/// <param name="configurationSource">The source for configuration objects.</param>
		/// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
		/// <returns>A fully initialized instance of <see cref="CacheManager"/>.</returns>
		public ICacheManager Assemble(IBuilderContext context, CacheManagerDataBase objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			CacheManagerData cacheManagerData = (CacheManagerData)objectConfiguration;

			IBackingStore backingStore
				= BackingStoreCustomFactory.Instance.Create(context, cacheManagerData.CacheStorage, configurationSource, reflectionCache);

			CachingInstrumentationProvider instrumentationProvider = CreateInstrumentationProvider(cacheManagerData.Name, configurationSource, reflectionCache);

			CacheManager createdObject
				= new CacheManagerFactoryHelper().BuildCacheManager(
					cacheManagerData.Name,
					backingStore,
					cacheManagerData.MaximumElementsInCacheBeforeScavenging,
					cacheManagerData.NumberToRemoveWhenScavenging,
					cacheManagerData.ExpirationPollFrequencyInSeconds,
					instrumentationProvider);

			return createdObject;
		}

		private static CachingInstrumentationProvider CreateInstrumentationProvider(string name, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			CachingInstrumentationProvider instrumentationProvider = new CachingInstrumentationProvider();
			new InstrumentationAttachmentStrategy().AttachInstrumentation(name, instrumentationProvider, configurationSource, reflectionCache);

			return instrumentationProvider;
		}
	}

	/// <summary>
	/// Public for testing purposes
	/// </summary>
	public class CacheManagerFactoryHelper
	{
		/// <summary>
		/// Made public for testing purposes.
		/// </summary>
		public CacheManager BuildCacheManager(
			string cacheManagerName,
			IBackingStore backingStore,
			int maximumElementsInCacheBeforeScavenging,
			int numberToRemoveWhenScavenging,
			int expirationPollFrequencyInSeconds,
			CachingInstrumentationProvider instrumentationProvider)
		{
			CacheCapacityScavengingPolicy scavengingPolicy =
				new CacheCapacityScavengingPolicy(maximumElementsInCacheBeforeScavenging);

			Cache cache = new Cache(backingStore, scavengingPolicy, instrumentationProvider);

			ExpirationPollTimer timer = new ExpirationPollTimer();
			ExpirationTask expirationTask = CreateExpirationTask(cache, instrumentationProvider);
			ScavengerTask scavengerTask = new ScavengerTask(numberToRemoveWhenScavenging, scavengingPolicy, cache, instrumentationProvider);
			BackgroundScheduler scheduler = new BackgroundScheduler(expirationTask, scavengerTask, instrumentationProvider);
			cache.Initialize(scheduler);

			scheduler.Start();
			timer.StartPolling(new TimerCallback(scheduler.ExpirationTimeoutExpired), expirationPollFrequencyInSeconds * 1000);

			return new CacheManager(cache, scheduler, timer);
		}

		/// <summary>
		/// Made protected for testing purposes.
		/// </summary>
		/// <param name="cacheOperations">For testing only.</param>
		/// <param name="instrumentationProvider">For testing only.</param>
		/// <returns>For testing only.</returns>
		public virtual ExpirationTask CreateExpirationTask(ICacheOperations cacheOperations, CachingInstrumentationProvider instrumentationProvider)
		{
			return new ExpirationTask(cacheOperations, instrumentationProvider);
		}
	}
}
