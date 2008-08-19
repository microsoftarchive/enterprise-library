//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design
{
	sealed class LogFilterCollectionNodeBuilder : NodeBuilder
	{
		private NameTypeConfigurationElementCollection<LogFilterData, CustomLogFilterData> logFilters;
		private LogFilterCollectionNode node;

		public LogFilterCollectionNodeBuilder(IServiceProvider serviceProvider, NameTypeConfigurationElementCollection<LogFilterData, CustomLogFilterData> logFilters)
			: base(serviceProvider)
		{
			this.logFilters = logFilters;
		}

		public LogFilterCollectionNode Build()
		{
			node = new LogFilterCollectionNode();
			logFilters.ForEach(new Action<LogFilterData>(CreateLogFilterNode));
			return node;
		}

		private void CreateLogFilterNode(LogFilterData logFilterData)
		{
			LogFilterNode logFilterNode = NodeCreationService.CreateNodeByDataType(logFilterData.GetType(), new object[] { logFilterData }) as LogFilterNode;
			if (null == logFilterNode)
			{
				LogNodeMapError(node, logFilterData.GetType());
				return;
			}
			node.AddNode(logFilterNode);
		}
	}
}
