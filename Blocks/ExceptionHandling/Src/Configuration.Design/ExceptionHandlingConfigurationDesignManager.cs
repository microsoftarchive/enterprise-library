//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
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
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design
{
	/// <summary>
	/// Represents the design manager for the exception handling settings configuration section.
	/// </summary>    
    public sealed class ExceptionHandlingConfigurationDesignManager : ConfigurationDesignManager
    {
		/// <summary>
		/// Initialize a new instance of the <see cref="ExceptionHandlingConfigurationDesignManager"/> class.
		/// </summary>
        public ExceptionHandlingConfigurationDesignManager()
        {
        }

		/// <summary>
		/// Register the commands and node maps needed for the design manager into the design time.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
        public override void Register(IServiceProvider serviceProvider)
        {
			ExceptionHandlingCommandRegistrar commandRegistrar = new ExceptionHandlingCommandRegistrar(serviceProvider);
			commandRegistrar.Register();
			ExceptionHandlingNodeMapRegistrar nodeMapRegistrar = new ExceptionHandlingNodeMapRegistrar(serviceProvider);
			nodeMapRegistrar.Register();
        }

		/// <summary>
		/// Opens the exception handling settings configuration section, builds the design time nodes and adds them to the application node.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <param name="rootNode">The root node of the application.</param>
		/// <param name="section">The <see cref="ConfigurationSection"/> that was opened from the <see cref="IConfigurationSource"/>.</param>
		protected override void OpenCore(IServiceProvider serviceProvider, ConfigurationApplicationNode rootNode, ConfigurationSection section)
		{
			if (null != section)
			{
				ExceptionHandlingSettingsNodeBuilder builder = new ExceptionHandlingSettingsNodeBuilder(serviceProvider, (ExceptionHandlingSettings)section);
                ExceptionHandlingSettingsNode node = builder.Build();
                SetProtectionProvider(section, node);

				rootNode.AddNode(node);
			}
		}

		/// <summary>
		/// Gets the a <see cref="ConfigurationSectionInfo"/> for the exception handling settings configuration section.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <returns>A <see cref="ConfigurationSectionInfo"/> for the exception handling settings configuration section.</returns>
		protected override ConfigurationSectionInfo GetConfigurationSectionInfo(IServiceProvider serviceProvider)
		{
			ConfigurationNode rootNode = ServiceHelper.GetCurrentRootNode(serviceProvider);
			ExceptionHandlingSettingsNode node = null; 
			if (null != rootNode) node = rootNode.Hierarchy.FindNodeByType(rootNode, typeof(ExceptionHandlingSettingsNode)) as ExceptionHandlingSettingsNode;
			ExceptionHandlingSettings exceptionHandlingConfiguration = null;
			if (node == null)
			{
				exceptionHandlingConfiguration = null ;
			}
			else
			{
				ExceptionHandlingSettingsBuilder builder = new ExceptionHandlingSettingsBuilder(serviceProvider, node);
				exceptionHandlingConfiguration = builder.Build();
			}
            string protectionProviderName = GetProtectionProviderName(node);

            return new ConfigurationSectionInfo(node, exceptionHandlingConfiguration, ExceptionHandlingSettings.SectionName, protectionProviderName);
		}
    }
}