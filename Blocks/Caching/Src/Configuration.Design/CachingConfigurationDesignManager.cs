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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design
{
    /// <summary>
    /// Represents the design manager that will register the design time information for the Caching Application Block.
    /// </summary>
    public sealed class CachingConfigurationDesignManager : ConfigurationDesignManager
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="CachingConfigurationDesignManager"/> class.
        /// </summary>
        public CachingConfigurationDesignManager()
        {
        }

        /// <summary>
        /// Registers the caching design manager into the environment.
        /// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
        public override void Register(IServiceProvider serviceProvider)
        {
			CachingNodeMapRegistrar nodeRegistrar = new CachingNodeMapRegistrar(serviceProvider);
			nodeRegistrar.Register();
			CachingCommandRegistrar cmdRegistrar = new CachingCommandRegistrar(serviceProvider);
			cmdRegistrar.Register();
        }

		/// <summary>
		/// Opens the caching configuration from an application configuration file.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <param name="rootNode">The <see cref="ConfigurationApplicationNode"/> of the hierarchy.</param>
		/// <param name="section">The caching configuration section or null if no section was found.</param>
		protected override void OpenCore(IServiceProvider serviceProvider, ConfigurationApplicationNode rootNode, ConfigurationSection section)
		{
			if (null != section)
			{
				CacheManagerSettingsNodeBuilder builder = new CacheManagerSettingsNodeBuilder(serviceProvider, (CacheManagerSettings)section);
                CacheManagerSettingsNode cacheManagerNode = builder.Build();
                SetProtectionProvider(section, cacheManagerNode);

				rootNode.AddNode(cacheManagerNode);
			}
		}

		/// <summary>
		/// Gets a <see cref="ConfigurationSectionInfo"/> object containing the Caching Block's configuration information.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <returns>A <see cref="ConfigurationSectionInfo"/> object containing the Caching Block's configuration information.</returns>
		protected override ConfigurationSectionInfo GetConfigurationSectionInfo(IServiceProvider serviceProvider)
		{
			ConfigurationNode rootNode = ServiceHelper.GetCurrentRootNode(serviceProvider);
			CacheManagerSettingsNode node = null;
			if (rootNode != null) node = (CacheManagerSettingsNode)rootNode.Hierarchy.FindNodeByType(rootNode, typeof(CacheManagerSettingsNode));
			CacheManagerSettings cacheSection = null;
			if (node == null)
			{
                cacheSection = null;
			}
			else
			{
				CacheManagerSettingsBuilder builder = new CacheManagerSettingsBuilder(serviceProvider, node);
				cacheSection = builder.Build();
			}
            string protectionProviderName = GetProtectionProviderName(node);

            return new ConfigurationSectionInfo(node, cacheSection, CacheManagerSettings.SectionName, protectionProviderName);
		}
    }
}
