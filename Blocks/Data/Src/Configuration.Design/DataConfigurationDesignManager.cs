//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
	/// <summary>
	/// Represents the design manager for the database settings configuration section.
	/// </summary>    
    public sealed class DataConfigurationDesignManager : ConfigurationDesignManager
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="DataConfigurationDesignManager"/> class.
        /// </summary>
        public DataConfigurationDesignManager()
        {
        }

		/// <summary>
		/// Register the commands and node maps needed for the design manager into the design time.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
        public override void Register(IServiceProvider serviceProvider)
        {
			DataCommandRegistrar registrar = new DataCommandRegistrar(serviceProvider);
			registrar.Register();
        }

		/// <summary>
		/// Opens the database settings configuration section, builds the design time nodes and adds them to the application node.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <param name="rootNode">The root node of the application.</param>
		/// <param name="section">The <see cref="ConfigurationSection"/> that was opened from the <see cref="IConfigurationSource"/>.</param>
		protected override void OpenCore(IServiceProvider serviceProvider, ConfigurationApplicationNode rootNode, ConfigurationSection section)
		{	
			if (null != section)
			{
                DatabaseSectionNodeBuilder builder = new DatabaseSectionNodeBuilder(serviceProvider, (DatabaseSettings)section);
                DatabaseSectionNode node = builder.Build();
                SetProtectionProvider(section, node);
                rootNode.AddNode(node);
			}
		}

		/// <summary>
		/// Gets the a <see cref="ConfigurationSectionInfo"/> for the database settings configuration section.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <returns>A <see cref="ConfigurationSectionInfo"/> for the database settings configuration section.</returns>
		protected override ConfigurationSectionInfo GetConfigurationSectionInfo(IServiceProvider serviceProvider)
		{
			ConfigurationNode rootNode = ServiceHelper.GetCurrentRootNode(serviceProvider);
			DatabaseSectionNode node = null; 
			if (null != rootNode) node = rootNode.Hierarchy.FindNodeByType(rootNode, typeof(DatabaseSectionNode)) as DatabaseSectionNode;
			DatabaseSettings databaseSection = null;
			if (node == null)
			{
				databaseSection = null;
			}
			else
			{
				DatabaseSectionBuilder builder = new DatabaseSectionBuilder(serviceProvider, node);
				databaseSection = builder.Build();
			}
            string protectionProviderName = GetProtectionProviderName(node);

            return new ConfigurationSectionInfo(node, databaseSection, DatabaseSettings.SectionName, protectionProviderName);
		}
	}
}