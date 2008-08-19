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
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
	sealed class OracleConnectionSettingsBuilder 
	{
		private IConfigurationUIHierarchy hierarchy;
		private OracleConnectionSettings oracleConnectionSettings;

		public OracleConnectionSettingsBuilder(IServiceProvider serviceProvider) 
		{
			this.hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
		}

		public OracleConnectionSettings Build()
		{
			oracleConnectionSettings = new OracleConnectionSettings();
			IList<ConfigurationNode> connections = hierarchy.FindNodesByType(typeof(OracleConnectionElementNode));
			for (int index = 0; index < connections.Count; ++index)
			{
				OracleConnectionData data = new OracleConnectionData();
				data.Name = connections[index].Parent.Name;
				foreach (OraclePackageElementNode node in connections[index].Nodes)
				{
					data.Packages.Add(node.OraclePackageElement);
				}
				oracleConnectionSettings.OracleConnectionsData.Add(data);
			}
			return oracleConnectionSettings;
		}
	}
}
