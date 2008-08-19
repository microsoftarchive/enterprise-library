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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;


namespace Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.Design
{
    /// <summary>
    /// Represents the design manager for the symmetric cache store encryption provider.
    /// </summary>
    public class CachingCryptographyConfigurationDesignManager : ConfigurationDesignManager
    {
        /// <summary>
        /// Register the commands and node maps needed for the design manager into the design time.
        /// </summary>
        /// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
        public override void Register(IServiceProvider serviceProvider)
        {
            CommandRegistrar commandRegistrar = new SymmetricStorageEncryptionCommandRegistrar(serviceProvider);
            commandRegistrar.Register();

            NodeMapRegistrar nodeMapRegistrar = new SymmetricStorageEncryptionNodeMapRegistrar(serviceProvider);
            nodeMapRegistrar.Register();
        }

        /// <summary>
        /// Initializes the symmetric cache store encryption designtime and adds it to the caching settings.
        /// </summary>
        /// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
        /// <param name="rootNode">The root node of the application.</param>
        /// <param name="section">The <see cref="ConfigurationSection"/> that was opened from the <see cref="IConfigurationSource"/>.</param>
        protected override void OpenCore(IServiceProvider serviceProvider, ConfigurationApplicationNode rootNode, ConfigurationSection section)
        {
            IConfigurationUIHierarchy hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
            foreach (SymmetricStorageEncryptionProviderNode symmetricStorageEncryptionNode in hierarchy.FindNodesByType(typeof(SymmetricStorageEncryptionProviderNode)))
            {
                foreach (SymmetricCryptoProviderNode symmetricCryptoProviderNode in hierarchy.FindNodesByType(typeof(SymmetricCryptoProviderNode)))
                {
                    if (symmetricCryptoProviderNode.Name == symmetricStorageEncryptionNode.symmetricCryptoProviderNodeName)
                    {
                        symmetricStorageEncryptionNode.SymmetricInstance = symmetricCryptoProviderNode;
                        break;
                    }
                }
            }		
        }		
    }
}
