//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design
{
	/// <summary>
	/// Represents the design manager for the exception handling settings configuration section.
	/// </summary>
    public sealed class LoggingConfigurationDesignManager : ConfigurationDesignManager
    {
		/// <summary>
		/// Initialize a new instance of the <see cref="LoggingConfigurationDesignManager"/> class.
		/// </summary>
        public LoggingConfigurationDesignManager()
        {
        }

		/// <summary>
		/// Register the commands and node maps needed for the design manager into the design time.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
        public override void Register(IServiceProvider serviceProvider)
        {
			LoggingCommandRegistrar cmdRegistrar = new LoggingCommandRegistrar(serviceProvider);
			cmdRegistrar.Register();
			LoggingNodeMapRegistrar registrar = new LoggingNodeMapRegistrar(serviceProvider);
			registrar.Register();
        }

		/// <summary>
		/// Opens the logging settings configuration section, builds the design time nodes and adds them to the application node.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <param name="rootNode">The root node of the application.</param>
		/// <param name="section">The <see cref="ConfigurationSection"/> that was opened from the <see cref="IConfigurationSource"/>.</param>
        protected override void OpenCore(IServiceProvider serviceProvider, ConfigurationApplicationNode rootNode, ConfigurationSection section)
        {
            if (null != section)
            {
				LoggingSettingsNodeBuilder builder = new LoggingSettingsNodeBuilder(serviceProvider, (LoggingSettings)section);
                LoggingSettingsNode node = builder.Build();
                SetProtectionProvider(section, node);

                rootNode.AddNode(node);
            }
        }

		/// <summary>
		/// Gets the a <see cref="ConfigurationSectionInfo"/> for the logging settings configuration section.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <returns>A <see cref="ConfigurationSectionInfo"/> for the logging settings configuration section.</returns>
        protected override ConfigurationSectionInfo GetConfigurationSectionInfo(IServiceProvider serviceProvider)
        {
            ConfigurationNode rootNode = ServiceHelper.GetCurrentRootNode(serviceProvider);
            LoggingSettingsNode node = null;
            if (rootNode != null) node = (LoggingSettingsNode)rootNode.Hierarchy.FindNodeByType(rootNode, typeof(LoggingSettingsNode));
            LoggingSettings logggingSection = null;
            if (node != null)
            {
				LoggingSettingsBuilder builder = new LoggingSettingsBuilder(serviceProvider, node);
                logggingSection = builder.Build();
            }
            string protectionProviderName = GetProtectionProviderName(node);

            return new ConfigurationSectionInfo(node, logggingSection, LoggingSettings.SectionName, protectionProviderName);
        }
    }
}
