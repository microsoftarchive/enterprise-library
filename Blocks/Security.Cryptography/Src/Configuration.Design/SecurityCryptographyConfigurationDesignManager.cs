//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// Represents the block manager for the Authentication 
    /// configuration data.
    /// </summary>
    public class SecurityCryptographyConfigurationDesignManager : ConfigurationDesignManager
    {

        /// <summary>
        /// Saves the cryptogrpahy configuration to an configuration source and stores protected keys.
        /// </summary>
        /// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
        public override void Save(IServiceProvider serviceProvider)
        {
            base.Save(serviceProvider);

            IConfigurationUIHierarchy hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);

            foreach (ICryptographicKeyConfigurationNode nodeWithKey in hierarchy.FindNodesByType(typeof(KeyedHashAlgorithmProviderNode)))
            {
                SaveProtectedKey(nodeWithKey, serviceProvider);
            }

            foreach (ICryptographicKeyConfigurationNode nodeWithKey in hierarchy.FindNodesByType(typeof(SymmetricAlgorithmProviderNode)))
            {
                SaveProtectedKey(nodeWithKey, serviceProvider);
            }
        }

        private static void SaveProtectedKey(ICryptographicKeyConfigurationNode nodeWithKey, IServiceProvider serviceProvider)
        {
            ProtectedKeySettings keySettings = nodeWithKey.KeySettings;
			if (keySettings.ProtectedKey == null)
			{
				return;
			}

            using (Stream keyOutput = File.OpenWrite(keySettings.Filename))
            {
                KeyManager.Write(keyOutput, keySettings.ProtectedKey.EncryptedKey, keySettings.Scope);
            }
        }
        /// <summary>
        /// Registers the caching design manager into the environment.
        /// </summary>
        /// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
        public override void Register(IServiceProvider serviceProvider)
        {
            CryptographyNodeMapRegistrar nodeRegistrar = new CryptographyNodeMapRegistrar(serviceProvider);
            nodeRegistrar.Register();
            CryptographyCommandRegistrar cmdRegistrar = new CryptographyCommandRegistrar(serviceProvider);
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
                CryptographyManagerSettingsNodeBuilder builder = new CryptographyManagerSettingsNodeBuilder(serviceProvider, (CryptographySettings)section);
                CryptographySettingsNode node = builder.Build();
                SetProtectionProvider(section, node);

                rootNode.AddNode(node);
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
            CryptographySettingsNode node = null;
            if (rootNode != null) node = (CryptographySettingsNode)rootNode.Hierarchy.FindNodeByType(rootNode, typeof(CryptographySettingsNode));
            CryptographySettings cryptoSection = null;
            if (node == null)
            {
                cryptoSection = null;
            }
            else
            {
                CryptographyManagerSettingsBuilder builder = new CryptographyManagerSettingsBuilder(serviceProvider, node);
                cryptoSection = builder.Build();
            }
            string protectionProviderName = GetProtectionProviderName(node);

            return new ConfigurationSectionInfo(node, cryptoSection, CryptographyConfigurationView.SectionName, protectionProviderName);
        }
    }
}
