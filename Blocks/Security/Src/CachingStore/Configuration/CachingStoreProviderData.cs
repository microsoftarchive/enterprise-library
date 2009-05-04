//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration
{
	/// <summary>
	/// Configuration data for the Security Cache.
	/// </summary>
	[Assembler(typeof(CachingStoreProviderAssembler))]
	[ContainerPolicyCreator(typeof(CachingStoreProviderPolicyCreator))]
	public class CachingStoreProviderData : SecurityCacheProviderData
	{
		private const string cacheManagerProperty = "cacheManagerInstanceName";
		private const string slidingExpirationProperty = "defaultSlidingSessionExpirationInMinutes";
		private const string absoluteExpirationProperty = "defaultAbsoluteSessionExpirationInMinutes";

		/// <summary>
		/// <para>Initialize a new instance of the <see cref="CachingStoreProviderData"/> class.</para>
		/// </summary>
		public CachingStoreProviderData()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CachingStoreProviderData"/> class with 
		/// a specified name, sliding expiration, absolute expiration and cache manager instance name. 
		/// </summary>
		/// <param name="name">The name if the <see cref="CachingStoreProvider"/> instance.</param>
		/// <param name="slidingExpiration">The number of minutes between the time the added object was last accessed and when that object expires.</param>
		/// <param name="absoluteExpiration">The number of minutes in which an added object expires and is removed from the cache.</param>
		/// <param name="cacheManager">The name of the CacheManager instance that is used to store cached items.</param>
		public CachingStoreProviderData(string name, int slidingExpiration, int absoluteExpiration, string cacheManager)
			: base(name, typeof(CachingStoreProvider))
		{
			this.CacheManager = cacheManager;
			this.SlidingExpiration = slidingExpiration;
			this.AbsoluteExpiration = absoluteExpiration;
		}

		/// <summary>
		/// Gets or sets the Caching Block Cache instance name.
		/// </summary>
		/// <value>Caching Block Cache Instance Name.</value>
		[ConfigurationProperty(cacheManagerProperty, IsRequired = true)]
		public string CacheManager
		{
			get { return (string)this[cacheManagerProperty]; }
			set { this[cacheManagerProperty] = value; }
		}

		/// <summary>
		/// Gets or sets the Sliding Session Expiration duration (in minutes).
		/// </summary>
		/// <value>Sliding Session Expiration duration</value>
		[ConfigurationProperty(slidingExpirationProperty, IsRequired = true)]
		public int SlidingExpiration
		{
			get { return (int)this[slidingExpirationProperty]; }
			set { this[slidingExpirationProperty] = value; }
		}

		/// <summary>
		/// Gets or sets the Absolute Session Expiration duration (in minutes).
		/// </summary>
		/// <value>Absolute Session Expiration duration</value>
		[ConfigurationProperty(absoluteExpirationProperty, IsRequired = true)]
		public int AbsoluteExpiration
		{
			get { return (int)this[absoluteExpirationProperty]; }
			set { this[absoluteExpirationProperty] = value; }
		}
	}

	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Represents the process to build a <see cref="CachingStoreProvider"/> described by a <see cref="SecurityCacheProviderData"/> configuration object.
	/// </summary>
	/// <remarks>This type is linked to the <see cref="SecurityCacheProviderData"/> type and it is used by the <see cref="SecurityCacheProviderCustomFactory"/> 
	/// to build the specific <see cref="ISecurityCacheProvider"/> object represented by the configuration object.
	/// </remarks>
	public class CachingStoreProviderAssembler : IAssembler<ISecurityCacheProvider, SecurityCacheProviderData>
	{
		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Builds an <see cref="CachingStoreProvider"/> based on an instance of <see cref="SecurityCacheProviderData"/>.
		/// </summary>
		/// <seealso cref="SecurityCacheProviderCustomFactory"/>
		/// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
		/// <param name="objectConfiguration">The configuration object that describes the object to build. Must be an instance of <see cref="CachingStoreProviderData"/>.</param>
		/// <param name="configurationSource">The source for configuration objects.</param>
		/// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
		/// <returns>A fully initialized instance of <see cref="CachingStoreProvider"/>.</returns>
		public ISecurityCacheProvider Assemble(IBuilderContext context,
											   SecurityCacheProviderData objectConfiguration,
											   IConfigurationSource configurationSource,
											   ConfigurationReflectionCache reflectionCache)
		{
			CachingStoreProviderData castedObjectConfiguration
				= (CachingStoreProviderData)objectConfiguration;

			IBuilderContext cacheManagerContext
				= context.CloneForNewBuild(
					NamedTypeBuildKey.Make<ICacheManager>(castedObjectConfiguration.CacheManager), null);

			ICacheManager securityCacheManager
				= (ICacheManager)cacheManagerContext.Strategies.ExecuteBuildUp(cacheManagerContext);

			ISecurityCacheProvider createdObject
				= new CachingStoreProvider(
					castedObjectConfiguration.SlidingExpiration,
					castedObjectConfiguration.AbsoluteExpiration,
					securityCacheManager);

			return createdObject;
		}
	}
}
