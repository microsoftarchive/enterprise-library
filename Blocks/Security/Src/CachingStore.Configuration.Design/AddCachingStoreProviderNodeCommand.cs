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
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cache.Configuration.Design
{
    /// <summary>
    /// Adds a security cache provider based on the Caching Application Block. 
    /// </summary>
    public class AddCachingStoreProviderNodeCommand : AddChildNodeCommand
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="AddCachingStoreProviderNodeCommand"/> class with an <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
        public AddCachingStoreProviderNodeCommand(IServiceProvider serviceProvider)
            : base(serviceProvider, typeof(CachingStoreProviderNode)) {}

        /// <summary>
        /// Adds a <see cref="CachingStoreProviderNode"/> and the Caching Application Block if it is not already part of the application.
        /// </summary>
        /// <param name="node">The node to add the <see cref="CachingStoreProviderNode"/> to.</param>
        protected override void ExecuteCore(ConfigurationNode node)
        {
            base.ExecuteCore(node);

            IConfigurationUIHierarchy hierarchy = ServiceHelper.GetCurrentHierarchy(ServiceProvider);
            if (hierarchy.FindNodeByType(typeof(CacheManagerSettingsNode)) == null)
            {
                ConfigurationApplicationNode currentApplicationNode = hierarchy.RootNode;
                if (currentApplicationNode != null)
                {
                    new AddCacheManagerSettingsNodeCommand(ServiceProvider).Execute(currentApplicationNode);

                    CacheManagerNode defaultCacheManager = (CacheManagerNode)hierarchy.FindNodeByType(typeof(CacheManagerNode));
                    if (defaultCacheManager != null && ChildNode != null)
                    {
                        ((CachingStoreProviderNode)ChildNode).CacheManager = defaultCacheManager;
                        hierarchy.SelectedNode = ChildNode;
                    }
                }
            }
        }
    }
}
