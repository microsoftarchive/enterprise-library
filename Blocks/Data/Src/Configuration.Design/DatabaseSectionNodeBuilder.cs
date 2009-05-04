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
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
	sealed class DatabaseSectionNodeBuilder : NodeBuilder
	{
		private DatabaseSettings databaseSettings;

		public DatabaseSectionNodeBuilder(IServiceProvider serviceProvider, DatabaseSettings databaseSettings)
			: base(serviceProvider)
		{
			this.databaseSettings = databaseSettings;
		}

		public DatabaseSectionNode Build()
		{
			DatabaseSectionNode node = new DatabaseSectionNode();
			ProviderMappingsNode mappingsNode = new ProviderMappingsNode();
			foreach (DbProviderMapping mapping in databaseSettings.ProviderMappings)
			{
				mappingsNode.AddNode(new ProviderMappingNode(mapping));
			}
			node.AddNode(mappingsNode);
			node.RequirePermission = databaseSettings.SectionInformation.RequirePermission;
			return node;
		}
	}
}
