//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.AppSettings.Configuration.Design
{
    /// <summary>
    /// Represents the design manager for the <see cref="AppSettingsSection"/>.
    /// </summary>
    public class AppSettingsConfigurationDesignManager : ConfigurationDesignManager
    {
        private const string appSettingsSectionName = "appSettings";

        /// <summary>
        /// Initialize a new instance of the <see cref="AppSettingsConfigurationDesignManager"/> class.
        /// </summary>
        public AppSettingsConfigurationDesignManager()
        {
        }

        /// <summary>
        /// Registers the commands for the configurationnodes to be used in the designtime.
        /// </summary>
        /// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
        public override void Register(IServiceProvider serviceProvider)
        {
            AppSettingsCommandRegistrar registrar = new AppSettingsCommandRegistrar(serviceProvider);
            registrar.Register();
        }

        /// <summary>
        /// Gets the a <see cref="ConfigurationSectionInfo"/> for the appSettings section.
        /// </summary>
        /// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
        /// <returns>A <see cref="ConfigurationSectionInfo"/> for the configuration for the configuration sources.</returns>
        protected override ConfigurationSectionInfo GetConfigurationSectionInfo(IServiceProvider serviceProvider)
        {
            ConfigurationNode rootNode = ServiceHelper.GetCurrentRootNode(serviceProvider);
            AppSettingsNode node = null;
            if (null != rootNode) node = rootNode.Hierarchy.FindNodeByType(rootNode, typeof(AppSettingsNode)) as AppSettingsNode;
            AppSettingsSection appSettingsSection = null;
            if (node == null)
            {
                appSettingsSection = null;
            }
            else
            {
                AppSettingsBuilder builder = new AppSettingsBuilder(node);
                appSettingsSection = builder.Build();
            }
            string protectionProviderName = GetProtectionProviderName(node);

            return new ConfigurationSectionInfo(node, appSettingsSection, appSettingsSectionName, protectionProviderName);
        }

        /// <summary>
        /// Opens the appSettings section, builds the design time nodes and adds them to the application node.
        /// </summary>
        /// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
        /// <param name="rootNode">The root node of the application.</param>
        /// <param name="section">The <see cref="ConfigurationSection"/> that was opened from the <see cref="IConfigurationSource"/>.</param>
        protected override void OpenCore(IServiceProvider serviceProvider, ConfigurationApplicationNode rootNode, System.Configuration.ConfigurationSection section)
        {
            if (null != section)
            {
                AppSettingsNodeBuilder builder = new AppSettingsNodeBuilder(serviceProvider, (AppSettingsSection)section);
                AppSettingsNode node = builder.Build();
                SetProtectionProvider(section, node);

                rootNode.AddNode(node);
            }
        }

        /// <summary>
        /// Gets a <see cref="FileConfigurationSource"/> based on the applications configuration file.
        /// </summary>
        /// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
        /// <returns>A <see cref="FileConfigurationSource"/> based on the applications configuration file.</returns>
        protected override IConfigurationSource GetConfigurationSource(IServiceProvider serviceProvider)
        {
            return new FileConfigurationSource(ServiceHelper.GetCurrentRootNode(serviceProvider).ConfigurationFile, false);
        }
    }
}
