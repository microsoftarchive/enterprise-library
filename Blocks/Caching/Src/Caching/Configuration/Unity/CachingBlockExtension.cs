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

using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Unity
{
	/// <summary>
	/// A <see cref="UnityContainerExtension"/> that registers the policies necessary
	/// to create <see cref="ICacheManager"/> instances described in the standard
	/// configuration file.
	/// </summary>
	public class CachingBlockExtension : EnterpriseLibraryBlockExtension
	{
		/// <summary>
		/// Initialize this extension by adding the Enterprise Library Caching Block's policies.
		/// </summary>
		protected override void Initialize()
		{
			CacheManagerSettings settings
				= (CacheManagerSettings)ConfigurationSource.GetSection(CacheManagerSettings.SectionName);

			if (settings == null)
			{
				return;
			}

			CreateProvidersPolicies<IBackingStore, CacheStorageData>(
				Context.Policies,
				null,
				settings.BackingStores,
				ConfigurationSource);

			CreateProvidersPolicies<IStorageEncryptionProvider, StorageEncryptionProviderData>(
				Context.Policies,
				null,
				settings.EncryptionProviders,
				ConfigurationSource);

			CreateProvidersPolicies<ICacheManager, CacheManagerDataBase>(
				Context.Policies,
				settings.DefaultCacheManager,
				settings.CacheManagers,
				ConfigurationSource);

            CreateCacheManagerLifetimePolicies(
                Context.Policies,
                Context.Container,
                settings.CacheManagers);
        }

        private static void CreateCacheManagerLifetimePolicies(IPolicyList policyList, IUnityContainer container, IEnumerable<CacheManagerDataBase> cacheManagers)
        {
            foreach (var cacheManagerData in cacheManagers)
            {
                container.RegisterType(cacheManagerData.Type, cacheManagerData.Name, new ContainerControlledLifetimeManager());
            }
        }
	}
}
