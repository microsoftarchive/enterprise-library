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
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design
{
    /// <summary>
    /// Adds the Logging Application Block to the current application.
    /// </summary>
    public class AddLoggingSettingsNodeCommand : AddChildNodeCommand
    {
        private IServiceProvider serviceProvider;

        /// <summary>
        /// Initialize a new instance of the <see cref="AddLoggingSettingsNodeCommand"/> class with an <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
        public AddLoggingSettingsNodeCommand(IServiceProvider serviceProvider)
            : base(serviceProvider, typeof(LoggingSettingsNode))
        {
            this.serviceProvider = serviceProvider;
        }
        /// <summary>
        /// Adds the <see cref="LoggingSettingsNode"/> and adds the default nodes.
        /// </summary>
        /// <param name="node">The <see cref="LoggingSettingsNode"/> added.</param>
        protected override void ExecuteCore(ConfigurationNode node)
        {
            base.ExecuteCore(node);
            LoggingSettingsNode loggingNode = ChildNode as LoggingSettingsNode;
            if (loggingNode == null) return;

            TextFormatterNode defaultTextFormatterNode = new TextFormatterNode();
            FormattedEventLogTraceListenerNode defaultTraceListenerNode = new FormattedEventLogTraceListenerNode();
            CategoryTraceSourceNode generalCategoryNode
                = new CategoryTraceSourceNode(new TraceSourceData(Resources.TraceSourceCategoryGeneral, SourceLevels.All));

            loggingNode.AddNode(new LogFilterCollectionNode());

            CategoryTraceSourceCollectionNode categoryTraceSourcesNode = new CategoryTraceSourceCollectionNode();
            TraceListenerReferenceNode generalCategoryListenerRef
                = new TraceListenerReferenceNode(new TraceListenerReferenceData(defaultTraceListenerNode.Name));
            categoryTraceSourcesNode.AddNode(generalCategoryNode);
            generalCategoryNode.AddNode(generalCategoryListenerRef);
            generalCategoryListenerRef.ReferencedTraceListener = defaultTraceListenerNode;
            loggingNode.AddNode(categoryTraceSourcesNode);

            SpecialTraceSourcesNode specialTraceSourcesNode = new SpecialTraceSourcesNode();
            ErrorsTraceSourceNode errorsTraceSourcesNode = new ErrorsTraceSourceNode(new TraceSourceData());
            TraceListenerReferenceNode errorsTraceListenerReferenceNode = new TraceListenerReferenceNode();
            errorsTraceSourcesNode.AddNode(errorsTraceListenerReferenceNode);
            errorsTraceListenerReferenceNode.ReferencedTraceListener = defaultTraceListenerNode;
            specialTraceSourcesNode.AddNode(errorsTraceSourcesNode);
            specialTraceSourcesNode.AddNode(new NotProcessedTraceSourceNode(new TraceSourceData()));
            specialTraceSourcesNode.AddNode(new AllTraceSourceNode(new TraceSourceData()));
            loggingNode.AddNode(specialTraceSourcesNode);

            TraceListenerCollectionNode traceListenerCollectionNode = new TraceListenerCollectionNode();
            traceListenerCollectionNode.AddNode(defaultTraceListenerNode);
            defaultTraceListenerNode.Formatter = defaultTextFormatterNode;
            loggingNode.AddNode(traceListenerCollectionNode);

            FormatterCollectionNode formattersNode = new FormatterCollectionNode();
            formattersNode.AddNode(defaultTextFormatterNode);
            loggingNode.AddNode(formattersNode);


            loggingNode.DefaultCategory = generalCategoryNode;
            loggingNode.LogWarningWhenNoCategoriesMatch = true;
            loggingNode.TracingEnabled = true;
            loggingNode.RevertImpersonation = true;

            ServiceHelper.GetUIService(serviceProvider).RefreshPropertyGrid();
        }
    }
}