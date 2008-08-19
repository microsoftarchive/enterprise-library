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
using Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Unity
{
	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Represents the process to create the container policies required to create a <see cref="CacheManager"/>.
	/// </summary>
	public class CacheManagerPolicyCreator : IContainerPolicyCreator
	{
		void IContainerPolicyCreator.CreatePolicies(
			IPolicyList policyList,
			string instanceName,
			ConfigurationElement configurationObject,
			IConfigurationSource configurationSource)
		{
			// this policy creator will not create policies to support the build plan
			// dynamic method generation.
			// instead, a fixed build plan policy is set

			CacheManagerData castConfigurationObject = (CacheManagerData)configurationObject;

			// local vars to avoid getting the configuration object in the delegate's closure
			var cacheManagerName = instanceName;
			var cacheStorageName = castConfigurationObject.CacheStorage;
			var maximumElementsInCacheBeforeScavenging = castConfigurationObject.MaximumElementsInCacheBeforeScavenging;
			var numberToRemoveWhenScavenging = castConfigurationObject.NumberToRemoveWhenScavenging;
			var expirationPollFrequencyInSeconds = castConfigurationObject.ExpirationPollFrequencyInSeconds;

			policyList.Set<IBuildPlanPolicy>(
				new DelegateBuildPlanPolicy(
					context =>
					{
						IBuilderContext backingStoreContext 
							= context.CloneForNewBuild(NamedTypeBuildKey.Make<IBackingStore>(cacheStorageName), null);
						IBackingStore backingStore = (IBackingStore)context.Strategies.ExecuteBuildUp(backingStoreContext);

						CachingInstrumentationProvider instrumentationProvider
							= CreateInstrumentationProvider(cacheManagerName, configurationSource);

						return new CacheManagerFactoryHelper().BuildCacheManager(
							cacheManagerName,
							backingStore,
							maximumElementsInCacheBeforeScavenging,
							numberToRemoveWhenScavenging,
							expirationPollFrequencyInSeconds,
							instrumentationProvider);
					}),
				NamedTypeBuildKey.Make<CacheManager>(cacheManagerName));
		}

		private readonly static ConfigurationReflectionCache reflectionCache = new ConfigurationReflectionCache();
		private static CachingInstrumentationProvider CreateInstrumentationProvider(string name, IConfigurationSource configurationSource)
		{
			CachingInstrumentationProvider instrumentationProvider = new CachingInstrumentationProvider();
			new InstrumentationAttachmentStrategy()
				.AttachInstrumentation(name, instrumentationProvider, configurationSource, reflectionCache);

			return instrumentationProvider;
		}
	}
}
