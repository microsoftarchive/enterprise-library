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
	/// Represents the design manager for the instrumentation section.
	/// </summary>
    public sealed class InstrumentationConfigurationDesignManager : ConfigurationDesignManager
    {   
		/// <summary>
		/// Initialize a new instance of the <see cref="InstrumentationConfigurationDesignManager"/> class.
		/// </summary>
        public InstrumentationConfigurationDesignManager()
        {
        }

		/// <summary>
		/// Register the commands and node maps needed for the design manager into the design time.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
        public override void Register(IServiceProvider serviceProvider)
        {
			InstrumenationSectionCommandRegistrar registrar = new InstrumenationSectionCommandRegistrar(serviceProvider);
			registrar.Register();
        }

		/// <summary>
		/// Opens the instrumenation section, builds the design time nodes and adds them to the application node.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <param name="rootNode">The root node of the application.</param>
		/// <param name="section">The <see cref="ConfigurationSection"/> that was opened from the <see cref="IConfigurationSource"/>.</param>
		protected override void OpenCore(IServiceProvider serviceProvider, ConfigurationApplicationNode rootNode, ConfigurationSection section)
		{
			if (null != section)
			{
                InstrumentationNode node = new InstrumentationNode((InstrumentationConfigurationSection)section);
                SetProtectionProvider(section, node);

				rootNode.AddNode(node);
			}				
		}

		/// <summary>
		/// Gets the a <see cref="ConfigurationSectionInfo"/> for the instrumentation section.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <returns>A <see cref="ConfigurationSectionInfo"/> for the configuration for the configuration sources.</returns>
		protected override ConfigurationSectionInfo GetConfigurationSectionInfo(IServiceProvider serviceProvider)
		{
			ConfigurationNode rootNode = ServiceHelper.GetCurrentRootNode(serviceProvider);
			InstrumentationNode node = null;
			if (null != rootNode) node = rootNode.Hierarchy.FindNodeByType(rootNode, typeof(InstrumentationNode)) as InstrumentationNode;
			InstrumentationConfigurationSection instrumentationSection = null;
			if (node == null)
			{
				instrumentationSection = null;
			}
			else
			{
				instrumentationSection = node.InstrumentationConfigurationSection;
            } 
            string protectionProviderName = GetProtectionProviderName(node);

			return new ConfigurationSectionInfo(node, instrumentationSection, InstrumentationConfigurationSection.SectionName, protectionProviderName);
		}
	}
}
