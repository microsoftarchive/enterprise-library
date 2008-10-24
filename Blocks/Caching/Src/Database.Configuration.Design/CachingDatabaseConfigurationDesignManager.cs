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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.Design
{
	/// <summary>
	/// Represents the design manager for the database cache storage.
	/// </summary>
	public sealed class CachingDatabaseConfigurationDesignManager : ConfigurationDesignManager
    {
		/// <summary>
		/// Register the commands and node maps needed for the design manager into the design time.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
        public override void Register(IServiceProvider serviceProvider)
        {
            CommandRegistrar commandRegistrar = new DataCacheStorageCommandRegistrar(serviceProvider);
            commandRegistrar.Register();

			NodeMapRegistrar nodeMapRegistrar = new DataCacheStorageNodeMapRegistrar(serviceProvider);
            nodeMapRegistrar.Register();
        }

		/// <summary>
		/// Initializes the data cache storage and adds it to the caching settings.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <param name="rootNode">The root node of the application.</param>
		/// <param name="section">The <see cref="ConfigurationSection"/> that was opened from the <see cref="IConfigurationSource"/>.</param>
		protected override void OpenCore(IServiceProvider serviceProvider, ConfigurationApplicationNode rootNode, ConfigurationSection section)
		{			
			// since logging and exception handling gets loaded before us then we can search for any logging handlers and set the trace source
			IConfigurationUIHierarchy hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
			foreach (DataCacheStorageNode dataCacheStorageNode in hierarchy.FindNodesByType(typeof(DataCacheStorageNode)))
			{
				foreach (ConnectionStringSettingsNode connectionNode in hierarchy.FindNodesByType(typeof(ConnectionStringSettingsNode)))
				{
					if (connectionNode.Name == dataCacheStorageNode.connectionStringName)
					{
						dataCacheStorageNode.DatabaseInstance = connectionNode;
						break;
					}
				}
			}			
		}		
    }
}
