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
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design
{
	class CacheManagerSettingsBuilder
	{
		private CacheManagerSettingsNode cacheSettingsNode;
		private IConfigurationUIHierarchy hierarchy;
		private CacheManagerSettings cacheConfiguration;

		public CacheManagerSettingsBuilder(IServiceProvider serviceProvider, CacheManagerSettingsNode cacheSettingsNode)
		{
			this.cacheSettingsNode = cacheSettingsNode;
			hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
			cacheConfiguration = new CacheManagerSettings();
		}

		public CacheManagerSettings Build()
		{
			cacheConfiguration.DefaultCacheManager = cacheSettingsNode.DefaultCacheManager.Name;
			if (!cacheSettingsNode.RequirePermission)	// don't set if false
				cacheConfiguration.SectionInformation.RequirePermission = cacheSettingsNode.RequirePermission;

			BuildStorageEncryptionProviders();

			BuildCacheStorageProviders();

			BuildCacheManagers();

			return cacheConfiguration;
		}

		private void BuildCacheManagers()
		{
			foreach (CacheManagerBaseNode managerNode in hierarchy.FindNodesByType(cacheSettingsNode, typeof(CacheManagerBaseNode)))
			{
				CacheManagerDataBase cacheManagerDataBase = managerNode.CacheManagerData;

				// special case for CacheManagerData, set up the referenced storage as child nodes
				if (cacheManagerDataBase is CacheManagerData)
				{
					CacheManagerData cacheManagerData = (CacheManagerData)cacheManagerDataBase;

					CacheStorageNode storageNodeForManager = (CacheStorageNode)hierarchy.FindNodeByType(managerNode, typeof(CacheStorageNode));
					cacheManagerData.CacheStorage = (storageNodeForManager == null) ? Resources.NullStorageName : storageNodeForManager.Name;

					if (cacheManagerData.CacheStorage == Resources.NullStorageName && !cacheConfiguration.BackingStores.Contains(Resources.NullStorageName))
					{
						cacheConfiguration.BackingStores.Add(new CacheStorageData(Resources.NullStorageName, typeof(NullBackingStore)));
					}
				}

				cacheConfiguration.CacheManagers.Add(cacheManagerDataBase);
			}
		}

		private void BuildCacheStorageProviders()
		{
			foreach (CacheStorageNode cacheStorageNode in hierarchy.FindNodesByType(cacheSettingsNode, typeof(CacheStorageNode)))
			{
				CacheStorageData cacheStorageData = cacheStorageNode.CacheStorageData;
				CacheStorageEncryptionNode encryptionNodeForStorage = (CacheStorageEncryptionNode)hierarchy.FindNodeByType(cacheStorageNode, typeof(CacheStorageEncryptionNode));
				cacheStorageData.StorageEncryption = (encryptionNodeForStorage == null) ? string.Empty : encryptionNodeForStorage.Name;

				cacheConfiguration.BackingStores.Add(cacheStorageData);
			}
		}

		private void BuildStorageEncryptionProviders()
		{
			foreach (CacheStorageEncryptionNode cacheStorageEncryptionNode in hierarchy.FindNodesByType(cacheSettingsNode, typeof(CacheStorageEncryptionNode)))
			{
				cacheConfiguration.EncryptionProviders.Add(cacheStorageEncryptionNode.StorageEncryptionProviderData);
			}
		}
	}
}
