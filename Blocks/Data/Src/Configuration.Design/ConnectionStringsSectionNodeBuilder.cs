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

using System;using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{	
	sealed class ConnectionStringsSectionNodeBuilder : NodeBuilder
	{
		private ConnectionStringsSection connectionStringSections;
		private ConnectionStringsSectionNode node;		
		private DatabaseSectionNode databaseSectionNode;
		private string defaultDatabaseName;

		public ConnectionStringsSectionNodeBuilder(IServiceProvider serviceProvider, ConnectionStringsSection conectionStringsSection, string defaultDatabaseName, DatabaseSectionNode databaseSectionNode)
			: base(serviceProvider)
		{
			this.connectionStringSections = conectionStringsSection;
			this.databaseSectionNode = databaseSectionNode;
			this.defaultDatabaseName = defaultDatabaseName;
		}

		public ConnectionStringsSectionNode Build()
		{
			node = new ConnectionStringsSectionNode();			
			foreach (ConnectionStringSettings connectionString in connectionStringSections.ConnectionStrings)
			{
				BuildConnectionStringNode(connectionString);
			}						
			return node;
		}		

		private void BuildConnectionStringNode(ConnectionStringSettings connectionString)
		{
			ConnectionStringSettingsNode connectionStringNode = new ConnectionStringSettingsNode(connectionString);
			if (connectionStringNode.Name == defaultDatabaseName) databaseSectionNode.DefaultDatabase = connectionStringNode;
			node.AddNode(connectionStringNode);
		}		
	}
}
