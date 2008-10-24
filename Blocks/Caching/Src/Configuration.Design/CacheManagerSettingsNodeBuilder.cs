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
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design
{	
	class CacheManagerSettingsNodeBuilder : NodeBuilder
	{
		private CacheManagerSettings cacheManagerSettings;						

		public CacheManagerSettingsNodeBuilder(IServiceProvider serviceProvider, CacheManagerSettings cacheManagerSettings) : base(serviceProvider)
		{
			this.cacheManagerSettings = cacheManagerSettings;			
		}

		public CacheManagerSettingsNode Build()
		{
			CacheManagerSettingsNode rootNode = new CacheManagerSettingsNode();
			CacheManagerCollectionNode node = new CacheManagerCollectionNode();
			foreach (CacheManagerDataBase data in cacheManagerSettings.CacheManagers)
			{
				CacheManagerBaseNode cacheManagerBaseNode = (CacheManagerBaseNode)NodeCreationService.CreateNodeByDataType(data.GetType(), new object[] { data });
				if (cacheManagerBaseNode == null)
				{
					LogNodeMapError(rootNode, data.GetType());
				}
				else
				{
					node.AddNode(cacheManagerBaseNode);
					if (cacheManagerBaseNode.Name == cacheManagerSettings.DefaultCacheManager)
						rootNode.DefaultCacheManager = cacheManagerBaseNode;
					
					if (cacheManagerBaseNode is CacheManagerNode)
					{	
						// special case for CacheManagerNode
						CreateStorageNode(cacheManagerBaseNode, ((CacheManagerData)data).CacheStorage);
					}
				}
			}			
			rootNode.AddNode(node);
			rootNode.RequirePermission = cacheManagerSettings.SectionInformation.RequirePermission;
			return rootNode;
		}


		private void CreateStorageNode(CacheManagerBaseNode cacheManagerNode,string cacheStorageName)
		{
			if (string.IsNullOrEmpty(cacheStorageName)) return;

			CacheStorageData cacheStorageData = cacheManagerSettings.BackingStores.Get(cacheStorageName);
			if (null == cacheStorageData) 
			{
				LogError(cacheManagerNode, string.Format(CultureInfo.CurrentUICulture, Resources.ExceptionNoStorageProviderDefined, cacheStorageName));
				return;
			}
			if (cacheStorageData.TypeName != null &&
                cacheStorageData.TypeName.StartsWith(typeof(NullBackingStore).FullName)) return; // special case

			ConfigurationNode storageNode = NodeCreationService.CreateNodeByDataType(cacheStorageData.GetType(), new object[] { cacheStorageData });
			if (null == storageNode)
			{
				LogNodeMapError(cacheManagerNode, cacheStorageData.GetType());
				return;
			}
			cacheManagerNode.AddNode(storageNode);
			CreateEncryptionNode(storageNode, cacheStorageData.StorageEncryption);						
		}

		private void CreateEncryptionNode(ConfigurationNode storageNode, string storageEncryption)
		{
			if (string.IsNullOrEmpty(storageEncryption)) return;

			StorageEncryptionProviderData encryptionProviderData = cacheManagerSettings.EncryptionProviders.Get(storageEncryption);
			if (null == encryptionProviderData)
			{
				LogError(storageNode, string.Format(CultureInfo.CurrentUICulture, Resources.ExceptionNoEncrypitonProviderDefined, storageEncryption));
				return;
			}

			ConfigurationNode encyrptionNode = NodeCreationService.CreateNodeByDataType(encryptionProviderData.GetType(), new object[] { encryptionProviderData });
			if (null == encyrptionNode)
			{
				LogNodeMapError(storageNode, encryptionProviderData.GetType());
				return;
			}
			storageNode.AddNode(encyrptionNode);
		}
	}
}
