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
	sealed class DatabaseSectionBuilder 
	{
		private DatabaseSectionNode databaseSectionNode;
		private IConfigurationUIHierarchy hierarchy;		
		
		public DatabaseSectionBuilder(IServiceProvider serviceProvider, DatabaseSectionNode databaseSectionNode) 
		{
			this.databaseSectionNode = databaseSectionNode;
			this.hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
		}		
		public DatabaseSettings Build()
		{
			DatabaseSettings settings = new DatabaseSettings();
			if (!this.databaseSectionNode.RequirePermission)	// don't set if false
				settings.SectionInformation.RequirePermission = this.databaseSectionNode.RequirePermission;
			foreach (ProviderMappingNode node in hierarchy.FindNodesByType(databaseSectionNode, typeof(ProviderMappingNode)))
			{
				settings.ProviderMappings.Add(node.ProviderMapping);
			}
			if (null != databaseSectionNode.DefaultDatabase) settings.DefaultDatabase = databaseSectionNode.DefaultDatabase.Name;
			return settings;
		}		
	}
}
