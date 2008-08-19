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

using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cache.Configuration.Design.Properties;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cache.Configuration.Design
{
	/// <summary>
	/// Represents the design manager for the security cache caching store configuration element.
	/// </summary>
    public sealed class SecurityCacheCachingStoreConfigurationDesignManager : ConfigurationDesignManager
    {
        /// <summary>
        /// <para>Registers the <see cref="CachingStoreProviderNode"/> in the application.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        public override void Register(IServiceProvider serviceProvider)
        {
            SecurityCacheCachingStoreCommandRegistrar commandRegistrar = new SecurityCacheCachingStoreCommandRegistrar(serviceProvider);
            commandRegistrar.Register();

            SecurityCacheCachingStoreNodeMapRegistrar nodeMapRegistrar = new SecurityCacheCachingStoreNodeMapRegistrar(serviceProvider);
            nodeMapRegistrar.Register();
        }

		/// <summary>
		/// Initializes the security caching store and adds it to the exception settings.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <param name="rootNode">The root node of the application.</param>
		/// <param name="section">The <see cref="ConfigurationSection"/> that was opened from the <see cref="IConfigurationSource"/>.</param>
        protected override void OpenCore(IServiceProvider serviceProvider, ConfigurationApplicationNode rootNode, System.Configuration.ConfigurationSection section)
        {			
            IConfigurationUIHierarchy hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
            foreach (CachingStoreProviderNode securityCacheNode in hierarchy.FindNodesByType(typeof(CachingStoreProviderNode)))
            {
                foreach (CacheManagerNode cacheManagerNode in hierarchy.FindNodesByType(typeof(CacheManagerNode)))
                {
                    if (cacheManagerNode.Name == securityCacheNode.cacheManagerName)
                    {
                        securityCacheNode.CacheManager = cacheManagerNode;
                        break;
                    }
                }
            }			
            base.OpenCore(serviceProvider, rootNode, section);
        }
    }
}