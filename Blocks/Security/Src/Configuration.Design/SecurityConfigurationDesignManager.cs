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
using System.Configuration;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design
{
	/// <summary>
	/// Represents the design manager for the security settings configuration section.
	/// </summary>
    public sealed class SecurityConfigurationDesignManager : ConfigurationDesignManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityConfigurationDesignManager"/> class.
        /// </summary>
        public SecurityConfigurationDesignManager()
        {
        }

		/// <summary>
		/// Register the commands and node maps needed for the design manager into the design time.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
        public override void Register(IServiceProvider serviceProvider)
        {
            CommandRegistrar securityCommandRegistrar = new SecurityCommandRegistrar(serviceProvider);
            securityCommandRegistrar.Register();

            NodeMapRegistrar securityNodeMapRegistrar = new SecurityNodeMapRegistrar(serviceProvider);
            securityNodeMapRegistrar.Register();
        }

		/// <summary>
		/// Opens the security settings configuration section, builds the design time nodes and adds them to the application node.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <param name="rootNode">The root node of the application.</param>
		/// <param name="section">The <see cref="ConfigurationSection"/> that was opened from the <see cref="IConfigurationSource"/>.</param>
		protected override void OpenCore(IServiceProvider serviceProvider, ConfigurationApplicationNode rootNode, ConfigurationSection section)
		{
			if (null != section)
            {
                SecuritySettingsNodeBuilder nodeBuilder = new SecuritySettingsNodeBuilder(serviceProvider, (SecuritySettings)section);
                SecuritySettingsNode node = nodeBuilder.Build();
                SetProtectionProvider(section, node);
				rootNode.AddNode(node);
			}
		}

		/// <summary>
		/// Gets the a <see cref="ConfigurationSectionInfo"/> for the security settings configuration section.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <returns>A <see cref="ConfigurationSectionInfo"/> for the security settings configuration section.</returns>
		protected override ConfigurationSectionInfo GetConfigurationSectionInfo(IServiceProvider serviceProvider)
		{
			ConfigurationNode rootNode = ServiceHelper.GetCurrentRootNode(serviceProvider);
			SecuritySettingsNode node = null; 
			if (null != rootNode) node = rootNode.Hierarchy.FindNodeByType(rootNode, typeof(SecuritySettingsNode)) as SecuritySettingsNode;
			SecuritySettings securitySettings = null;
            if (node != null)
            {
                SecuritySettingsBuilder builder = new SecuritySettingsBuilder(serviceProvider, node);
                securitySettings = builder.Build();
            }
            string protectionProviderName = GetProtectionProviderName(node);

            return new ConfigurationSectionInfo(node, securitySettings, SecuritySettings.SectionName, protectionProviderName);
		}
    }
}
