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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	sealed class ConfigurationSourceSectionBuilder 
	{
		private ConfigurationSourceSectionNode configurationSourceSectionNode;

		public ConfigurationSourceSectionBuilder(ConfigurationSourceSectionNode configurationSourceSectionNode) 
		{
			this.configurationSourceSectionNode = configurationSourceSectionNode;			
		}

		public ConfigurationSourceSection Build()
		{
			ConfigurationSourceSection section = new ConfigurationSourceSection();
			if (!configurationSourceSectionNode.RequirePermission)	// don't set if false
				section.SectionInformation.RequirePermission = configurationSourceSectionNode.RequirePermission;
			section.SelectedSource = configurationSourceSectionNode.SelectedSource.Name;
			for (int index = 0; index < configurationSourceSectionNode.Nodes.Count; ++index)
			{
				ConfigurationSourceElementNode sourceNode = (ConfigurationSourceElementNode)configurationSourceSectionNode.Nodes[index];
				section.Sources.Add(sourceNode.ConfigurationSourceElement);
			}
			return section;  
		}
	}
}
