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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
	sealed class ConnectionStringsSectionBuilder 
	{
		private ConnectionStringsSectionNode connectionStringsSectionNode;		
		private ConnectionStringsSection connectionStringsSection;
		IConfigurationUIHierarchy hierarchy;
		
		public ConnectionStringsSectionBuilder(IServiceProvider serviceProvider, ConnectionStringsSectionNode connectionStringsSectionNode) 
		{
			this.connectionStringsSectionNode = connectionStringsSectionNode;
			hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
		}

		public ConnectionStringsSection Build()
		{
			connectionStringsSection = new ConnectionStringsSection();
			foreach (ConnectionStringSettingsNode node in connectionStringsSectionNode.Nodes)
			{
				BuildConnectionString(node);
			}
			return connectionStringsSection;
		}

		private void BuildConnectionString(ConnectionStringSettingsNode connectionStringNode)
		{
			connectionStringsSection.ConnectionStrings.Add(new ConnectionStringSettings(connectionStringNode.Name,
				connectionStringNode.ConnectionString, connectionStringNode.ProviderName));
		}
	}
}
