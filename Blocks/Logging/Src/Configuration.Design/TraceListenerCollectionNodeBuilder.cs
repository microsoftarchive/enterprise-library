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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design
{
	sealed class TraceListenerCollectionNodeBuilder : NodeBuilder
	{
		private TraceListenerDataCollection listeners;
		private TraceListenerCollectionNode node;
		private FormatterCollectionNode formatters;

		public TraceListenerCollectionNodeBuilder(IServiceProvider serviceProvider, TraceListenerDataCollection listeners, FormatterCollectionNode formatters)
			: base(serviceProvider)
		{
			this.listeners = listeners;
			this.formatters = formatters;
		}

		public TraceListenerCollectionNode Build()
		{
			node = new TraceListenerCollectionNode();
			listeners.ForEach(new Action<TraceListenerData>(CreateTraceListenerNode));
			return node;
		}

		private void CreateTraceListenerNode(TraceListenerData traceListenerData)
		{
			TraceListenerNode traceListenerNode = NodeCreationService.CreateNodeByDataType(traceListenerData.GetType(), new object[] { traceListenerData }) as TraceListenerNode;
			traceListenerNode.Filter = traceListenerData.Filter;
			traceListenerNode.TraceOutputOptions = traceListenerData.TraceOutputOptions;
			if (null == traceListenerNode)
			{
				LogNodeMapError(node, traceListenerData.GetType());
				return;
			}
			traceListenerNode.SetFormatter(formatters);
			node.AddNode(traceListenerNode);
		}
	}
}
