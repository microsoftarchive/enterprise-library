//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using System.Configuration;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration.Manageability
{
	/// <summary>
	/// Represents the configuration information from a 
	/// <see cref="Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration.CachingStoreProviderData"/> instance.
	/// </summary>
	/// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration.CachingStoreProviderData"/>
	/// <seealso cref="NamedConfigurationSetting"/>
	/// <seealso cref="ConfigurationSetting"/>	
	[ManagementEntity]
	public partial class CachingStoreProviderSetting : SecurityCacheProviderSetting
	{
		private int absoluteExpiration;
		private string cacheManager;
		private int slidingExpiration;

        /// <summary>
        /// Initialize a new instance of the <see cref="CachingStoreProviderSetting"/> class with the configuraiton source element,
        /// the name of the store, the name of the cache manager, the absolute expiration and the sliding expiration.
        /// </summary>
        /// <param name="sourceElement">The configuration source element.</param>
        /// <param name="name">The name of the source.</param>
        /// <param name="cacheManager">The name of the cache manager.</param>
        /// <param name="absoluteExpiration">The absolute expiration.</param>
        /// <param name="slidingExpiration">The sliding expiration.</param>
		public CachingStoreProviderSetting(ConfigurationElement sourceElement,
			string name,
			string cacheManager,
			int absoluteExpiration,
			int slidingExpiration)
			: base(sourceElement, name)
		{
			this.cacheManager = cacheManager;
			this.absoluteExpiration = absoluteExpiration;
			this.slidingExpiration = slidingExpiration;
		}

		/// <summary>
		/// Gets the absolute expiration for the represented configuration object.
		/// </summary>
		/// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration.CachingStoreProviderData.AbsoluteExpiration">
		/// CachingStoreProviderData.AbsoluteExpiration</seealso>
		[ManagementConfiguration]
		public int AbsoluteExpiration
		{
			get { return absoluteExpiration; }
			set { absoluteExpiration = value; }
		}

		/// <summary>
		/// Gets the name of the cache manager for the represented configuration object.
		/// </summary>
		/// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration.CachingStoreProviderData.CacheManager">
		/// CachingStoreProviderData.CacheManager</seealso>
		[ManagementConfiguration]
		public string CacheManager
		{
			get { return cacheManager; }
			set { cacheManager = value; }
		}

		/// <summary>
		/// Gets the sliding expiration for the represented configuration object.
		/// </summary>
		/// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration.CachingStoreProviderData.SlidingExpiration">
		/// CachingStoreProviderData.SlidingExpiration</seealso>
		[ManagementConfiguration]
		public int SlidingExpiration
		{
			get { return slidingExpiration; }
			set { slidingExpiration = value; }
		}

        /// <summary>
        /// Returns an enumeration of the published <see cref="CachingStoreProviderSetting"/> instances.
        /// </summary>
		[ManagementEnumerator]
		public static IEnumerable<CachingStoreProviderSetting> GetInstances()
		{
			return NamedConfigurationSetting.GetInstances<CachingStoreProviderSetting>();
		}

        /// <summary>
        /// Returns the <see cref="CachingStoreProviderSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <param name="Name">The value for the Name key property.</param>
        /// <returns>The published <see cref="CachingStoreProviderSetting"/> instance specified by the values for the key properties,
        /// or <see langword="null"/> if no such an instance is currently published.</returns>
		[ManagementBind]
		public static CachingStoreProviderSetting BindInstance(string ApplicationName, string SectionName, string Name)
		{
			return NamedConfigurationSetting.BindInstance<CachingStoreProviderSetting>(ApplicationName, SectionName, Name);
		}

        /// <summary>
        /// Saves the changes on the <see cref="CachingStoreProviderSetting"/> to its corresponding configuration object.
        /// </summary>
        /// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
        /// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
		protected override bool SaveChanges(ConfigurationElement sourceElement)
		{
			return CachingStoreProviderDataWmiMapper.SaveChanges(this, sourceElement);
		}
	}
}