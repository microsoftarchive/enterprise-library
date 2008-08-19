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
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;
using System.Runtime.InteropServices;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
	/// <summary>
	/// Represents the design manager for the oracle connection configuration section.
	/// </summary>    
    public sealed class OracleConnectionConfigurationDesignManager : ConfigurationDesignManager
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="OracleConnectionConfigurationDesignManager"/> class.
        /// </summary>
		public OracleConnectionConfigurationDesignManager()
        {
        }

		/// <summary>
		/// Register the commands and node maps needed for the design manager into the design time.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
        public override void Register(IServiceProvider serviceProvider)
        {
			OracleConnectionCommandRegistrar registrar = new OracleConnectionCommandRegistrar(serviceProvider);
			registrar.Register();
        }

		/// <summary>
		/// Opens the oracle connection configuration section, builds the design time nodes and adds them to the application node.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <param name="rootNode">The root node of the application.</param>
		/// <param name="section">The <see cref="ConfigurationSection"/> that was opened from the <see cref="IConfigurationSource"/>.</param>
		protected override void OpenCore(IServiceProvider serviceProvider, ConfigurationApplicationNode rootNode, ConfigurationSection section)
		{
			if (null != section)
			{
				OracleConnectionNodeBuilder builder = new OracleConnectionNodeBuilder(serviceProvider, (OracleConnectionSettings)section);
				builder.Build();
			}
		}

		/// <summary>
		/// Gets the a <see cref="ConfigurationSectionInfo"/> for the oracle connection configuration section.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <returns>A <see cref="ConfigurationSectionInfo"/> for the oracle connection configuration section.</returns>
		protected override ConfigurationSectionInfo GetConfigurationSectionInfo(IServiceProvider serviceProvider)
        {
            ConfigurationNode rootNode = ServiceHelper.GetCurrentRootNode(serviceProvider);
            DatabaseSectionNode databaseSectionNode = null;
            if (rootNode != null)
            {
                databaseSectionNode = rootNode.Hierarchy.FindNodeByType(rootNode, typeof(DatabaseSectionNode)) as DatabaseSectionNode;
            }
            OracleConnectionSettings oracleConnectionSection = null;
            IList<ConfigurationNode> connections = rootNode.Hierarchy.FindNodesByType(typeof(OracleConnectionElementNode));
            if (connections.Count == 0)
            {
                oracleConnectionSection = null;
            }
            else
            {
                OracleConnectionSettingsBuilder builder = new OracleConnectionSettingsBuilder(serviceProvider);
                oracleConnectionSection = builder.Build();
            }
            string protectionProviderName = GetProtectionProviderName(databaseSectionNode);

            return new ConfigurationSectionInfo(rootNode, oracleConnectionSection, OracleConnectionSettings.SectionName, protectionProviderName);
        }		
	}
}