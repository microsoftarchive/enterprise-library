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
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{   
	/// <summary>
	/// Represents the design manager for the configuration sources section.
	/// </summary>
    public sealed class ConfigurationSourceConfigurationDesignManager : ConfigurationDesignManager
    {   
		/// <summary>
		/// Initialize a new instance of the <see cref="ConfigurationSourceConfigurationDesignManager"/> class.
		/// </summary>
		public ConfigurationSourceConfigurationDesignManager()
        {
        }

		/// <summary>
		/// Register the commands and node maps needed for the design manager into the design time.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
        public override void Register(IServiceProvider serviceProvider)
        {
			ConfigurationSourceCommandRegistrar commandRegistrar = new ConfigurationSourceCommandRegistrar(serviceProvider);
			commandRegistrar.Register();

			ConfigurationSourceNodeMapRegistrar nodeMapRegistrar = new ConfigurationSourceNodeMapRegistrar(serviceProvider);
			nodeMapRegistrar.Register();
        }

		/// <summary>
		/// Opens the configuration sources section, builds the design time nodes and adds them to the application node.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <param name="rootNode">The root node of the application.</param>
		/// <param name="section">The <see cref="ConfigurationSection"/> that was opened from the <see cref="IConfigurationSource"/>.</param>
		protected override void OpenCore(IServiceProvider serviceProvider, ConfigurationApplicationNode rootNode, ConfigurationSection section)
		{			
			if (null != section)
			{
				ConfigurationSourceSectionNodeBuilder builder = new ConfigurationSourceSectionNodeBuilder(serviceProvider, (ConfigurationSourceSection)section);
                ConfigurationSourceSectionNode node = builder.Build();
                SetProtectionProvider(section, node);

				rootNode.AddNode(node);
			}				
		}

		/// <summary>
		/// Gets the a <see cref="ConfigurationSectionInfo"/> for the configuration sources.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <returns>A <see cref="ConfigurationSectionInfo"/> for the configuration for the configuration sources.</returns>
		protected override ConfigurationSectionInfo GetConfigurationSectionInfo(IServiceProvider serviceProvider)
		{
			ConfigurationNode rootNode = ServiceHelper.GetCurrentRootNode(serviceProvider);
			ConfigurationSourceSectionNode node = null;
			if (null != rootNode) node = (ConfigurationSourceSectionNode)rootNode.Hierarchy.FindNodeByType(rootNode, typeof(ConfigurationSourceSectionNode));
			ConfigurationSourceSection sourcesSection = null;
			if (node == null)
			{
				sourcesSection = null;
			}
			else
			{

				ConfigurationSourceSectionBuilder builder = new ConfigurationSourceSectionBuilder(node);
				sourcesSection = builder.Build(); ;
			}
            string protectionProviderName = GetProtectionProviderName(node);

            return new ConfigurationSectionInfo(node, sourcesSection, ConfigurationSourceSection.SectionName, protectionProviderName);			
		}

		/// <summary>
		/// Gets a <see cref="FileConfigurationParameter"/> based on the applications configuration file.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <returns>A <see cref="FileConfigurationParameter"/> based on the applications configuration file.</returns>
		protected override IConfigurationParameter GetConfigurationParameter(IServiceProvider serviceProvider)
		{
			return new FileConfigurationParameter(ServiceHelper.GetCurrentRootNode(serviceProvider).ConfigurationFile);
		}

		/// <summary>
		/// Gets a <see cref="FileConfigurationSource"/> based on the applications configuration file.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <returns>A <see cref="FileConfigurationSource"/> based on the applications configuration file.</returns>
		protected override IConfigurationSource GetConfigurationSource(IServiceProvider serviceProvider)
		{
            string configurationFile = ServiceHelper.GetCurrentRootNode(serviceProvider).ConfigurationFile;
            FileConfigurationSource.ResetImplementation(configurationFile, false);

			return new FileConfigurationSource(configurationFile);
		}
    }
}
