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
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
	/// <summary>
	/// Represents the design manager for the connection strings section.
	/// </summary>    
    public sealed class ConnectionStringsConfigurationDesignManager : ConfigurationDesignManager
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="ConnectionStringsConfigurationDesignManager"/> class.
        /// </summary>
		public ConnectionStringsConfigurationDesignManager()
        {
        }

		/// <summary>
		/// Register the commands and node maps needed for the design manager into the design time.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
        public override void Register(IServiceProvider serviceProvider)
        {
			ConnectionStringsCommandRegistrar registrar = new ConnectionStringsCommandRegistrar(serviceProvider);
			registrar.Register();
        }

		/// <summary>
		/// Opens the connection strings section, builds the design time nodes and adds them to the application node.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <param name="rootNode">The root node of the application.</param>
		/// <param name="section">The <see cref="ConfigurationSection"/> that was opened from the <see cref="IConfigurationSource"/>.</param>
		protected override void OpenCore(IServiceProvider serviceProvider, ConfigurationApplicationNode rootNode, ConfigurationSection section)
		{			
			if (null != section)				
			{
				string defaultDatabase = string.Empty;
				DatabaseSettings databaseSection = DatabaseSettings.GetDatabaseSettings(GetConfigurationSource(serviceProvider));
				if (null != databaseSection) defaultDatabase = databaseSection.DefaultDatabase;				
				DatabaseSectionNode node = rootNode.Hierarchy.FindNodeByType(typeof(DatabaseSectionNode)) as DatabaseSectionNode;
				if (null == node) 
				{
					AddDatabaseSectionNodeCommand dbCmd = new AddDatabaseSectionNodeCommand(serviceProvider, false);
					dbCmd.Execute(rootNode);
					node = dbCmd.ChildNode as DatabaseSectionNode;
					Debug.Assert(node != null);
				}
				ConnectionStringsSectionNodeBuilder builder = new ConnectionStringsSectionNodeBuilder(serviceProvider, (ConnectionStringsSection)section, defaultDatabase, node);
				node.AddNode(builder.Build());
			}
		}

		/// <summary>
		/// Gets the a <see cref="ConfigurationSectionInfo"/> for the connection strings section.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <returns>A <see cref="ConfigurationSectionInfo"/> for the connection strings section.</returns>
		protected override ConfigurationSectionInfo GetConfigurationSectionInfo(IServiceProvider serviceProvider)
		{
			ConfigurationNode rootNode = ServiceHelper.GetCurrentRootNode(serviceProvider);
			ConnectionStringsSectionNode node = null;
            DatabaseSectionNode databaseSectionNode = null;
            if (null != rootNode)
            {
                node = rootNode.Hierarchy.FindNodeByType(rootNode, typeof(ConnectionStringsSectionNode)) as ConnectionStringsSectionNode;
                databaseSectionNode = rootNode.Hierarchy.FindNodeByType(rootNode, typeof(DatabaseSectionNode)) as DatabaseSectionNode;
            }

			ConnectionStringsSection connectionStrings = null;
			if (node == null)
			{
				connectionStrings = null;
			}
			else
			{
				ConnectionStringsSectionBuilder builder = new ConnectionStringsSectionBuilder(serviceProvider, node);
				connectionStrings = builder.Build();
			}
            string protectionProviderName = GetProtectionProviderName(databaseSectionNode);

            return new ConfigurationSectionInfo(node, connectionStrings, "connectionStrings", protectionProviderName);
		}
	}	
}
