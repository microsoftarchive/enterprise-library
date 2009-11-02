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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using System.Linq.Expressions;
using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration
{
	/// <summary>
	/// Configuration data for the Security Cache.
	/// </summary>
    [ResourceDescription(typeof(DesignResources), "CachingStoreProviderDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "CachingStoreProviderDataDisplayName")]
    [AddSateliteProviderCommand(CacheManagerSettings.SectionName)]
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
            Type = typeof(CachingStoreProvider);
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
        /// Gets or sets the Caching Application Block Cache instance name.
		/// </summary>
        /// <value>Caching Application Block Cache Instance Name.</value>
        [ConfigurationProperty(cacheManagerProperty, IsRequired = true)]
        [Reference(typeof(NameTypeConfigurationElementCollection<CacheManagerDataBase, CustomCacheManagerData>), typeof(CacheManagerDataBase))]
        [ResourceDescription(typeof(DesignResources), "CachingStoreProviderDataCacheManagerDescription")]
        [ResourceDisplayName(typeof(DesignResources), "CachingStoreProviderDataCacheManagerDisplayName")]
		public string CacheManager
		{
			get { return (string)this[cacheManagerProperty]; }
			set { this[cacheManagerProperty] = value; }
		}

		/// <summary>
		/// Gets or sets the Sliding Session Expiration duration (in minutes).
		/// </summary>
		/// <value>Sliding Session Expiration duration</value>
        [ConfigurationProperty(slidingExpirationProperty, IsRequired = true, DefaultValue = 10)]
        [ResourceDescription(typeof(DesignResources), "CachingStoreProviderDataSlidingExpirationDescription")]
        [ResourceDisplayName(typeof(DesignResources), "CachingStoreProviderDataSlidingExpirationDisplayName")]
		public int SlidingExpiration
		{
			get { return (int)this[slidingExpirationProperty]; }
			set { this[slidingExpirationProperty] = value; }
		}

		/// <summary>
		/// Gets or sets the Absolute Session Expiration duration (in minutes).
		/// </summary>
		/// <value>Absolute Session Expiration duration</value>
		[ConfigurationProperty(absoluteExpirationProperty, IsRequired = true, DefaultValue=60)]
        [ResourceDescription(typeof(DesignResources), "CachingStoreProviderDataAbsoluteExpirationDescription")]
        [ResourceDisplayName(typeof(DesignResources), "CachingStoreProviderDataAbsoluteExpirationDisplayName")]
		public int AbsoluteExpiration
		{
			get { return (int)this[absoluteExpirationProperty]; }
			set { this[absoluteExpirationProperty] = value; }
		}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource)
        {
            yield return base.GetInstrumentationProviderRegistration(configurationSource);

            yield return new TypeRegistration<ISecurityCacheProvider>(() => new CachingStoreProvider(
                            SlidingExpiration,
                            AbsoluteExpiration,
                            Container.Resolved<ICacheManager>(CacheManager),
                            Container.Resolved<ISecurityCacheProviderInstrumentationProvider>(Name)))
                            {
                                Name = this.Name,
                                Lifetime = TypeRegistrationLifetime.Transient
                            };
        }
	}
}
