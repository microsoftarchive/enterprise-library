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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design
{
	sealed class CategoryTraceSourceCollectionNodeBuilder : NodeBuilder
	{
		private NamedElementCollection<TraceSourceData> traceSources;
		private CategoryTraceSourceCollectionNode node;
		private TraceListenerCollectionNode listeners;

		public CategoryTraceSourceCollectionNodeBuilder(IServiceProvider serviceProvider, NamedElementCollection<TraceSourceData> traceSources, TraceListenerCollectionNode listeners)
			: base(serviceProvider)
		{
			this.traceSources = traceSources;
			this.listeners = listeners;
		}

		public CategoryTraceSourceCollectionNode Build()
		{
			node = new CategoryTraceSourceCollectionNode();
			traceSources.ForEach(new Action<TraceSourceData>(CreateTraceSourceNode));
			return node;
		}

		private void CreateTraceSourceNode(TraceSourceData traceSourceData)
		{
			CategoryTraceSourceNode traceSourceNode = new CategoryTraceSourceNode(traceSourceData);
			foreach (TraceListenerReferenceData traceListener in traceSourceData.TraceListeners)
			{
				TraceListenerReferenceNode referenceNode = new TraceListenerReferenceNode(traceListener);
				foreach (TraceListenerNode traceListenerNode in listeners.Nodes)
				{
					if (traceListenerNode.Name == referenceNode.Name)
					{
						referenceNode.ReferencedTraceListener = traceListenerNode;
					}
				}
				traceSourceNode.AddNode(referenceNode);
			}
			node.AddNode(traceSourceNode);
		}
	}
}
