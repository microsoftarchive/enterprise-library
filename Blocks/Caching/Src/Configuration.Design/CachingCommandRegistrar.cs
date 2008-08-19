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
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design
{
	class CachingCommandRegistrar : CommandRegistrar
	{
		public CachingCommandRegistrar(IServiceProvider serviceProvider) : base(serviceProvider)
		{
		}

		public override void Register()
		{
			AddCacheSettingsCommand();
			AddDefaultCommands(typeof(CacheManagerSettingsNode));

			AddCacheManagerCommand();
			AddDefaultCommands(typeof(CacheManagerNode));

			AddCustomCacheManagerCommand();
			AddDefaultCommands(typeof(CustomCacheManagerNode));

			AddIsolatedStorageCommand();
			AddDefaultCommands(typeof(IsolatedStorageCacheStorageNode));

			AddCustomStorageCommand();
			AddDefaultCommands(typeof(CustomCacheStorageNode));
		}
		
		private void AddCustomStorageCommand()
		{
			AddSingleChildNodeCommand(Resources.CustomStorageUICommandText,
				Resources.CustomStorageUICommandLongText, typeof(CustomCacheStorageNode),
				typeof(CacheStorageNode), typeof(CacheManagerNode));
		}

		private void AddIsolatedStorageCommand()
		{
			AddSingleChildNodeCommand(Resources.IsolatedStorageUICommandText,
				Resources.IsolatedStorageUICommandLongText, typeof(IsolatedStorageCacheStorageNode),
				typeof(CacheStorageNode), typeof(CacheManagerNode));
		}

		private void AddCacheManagerCommand()
		{
			AddMultipleChildNodeCommand(Resources.CacheManagerMenuText,
				Resources.CacheManagerStatusText,
				typeof(CacheManagerNode), typeof(CacheManagerCollectionNode));
		}

		private void AddCustomCacheManagerCommand()
		{
			AddMultipleChildNodeCommand(Resources.CustomCacheManagerMenuText,
				Resources.CustomCacheManagerStatusText,
				typeof(CustomCacheManagerNode), typeof(CacheManagerCollectionNode));
		}

		private void AddCacheSettingsCommand()
		{
			ConfigurationUICommand cmd = ConfigurationUICommand.CreateSingleUICommand(ServiceProvider, 
				Resources.CachingSettingsUICommandText,
				Resources.CachingSettingsUICommandLongText,
				new AddCacheManagerSettingsNodeCommand(ServiceProvider),
                typeof(CacheManagerSettingsNode));
			AddUICommand(cmd, typeof(ConfigurationApplicationNode));
		}
	}
}
