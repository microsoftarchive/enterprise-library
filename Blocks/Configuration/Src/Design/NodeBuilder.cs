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
using System.Globalization;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Represents a base class to for use when building your node graph from your configuration objects.
    /// </summary>
    public abstract class NodeBuilder
    {
        readonly IErrorLogService errorLogService;
        readonly INodeCreationService nodeCreationService;
        readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Initialize a new instance of the <see cref="NodeBuilder"/> class with a <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
        protected NodeBuilder(IServiceProvider serviceProvider)
        {
            nodeCreationService = ServiceHelper.GetNodeCreationService(serviceProvider);
            errorLogService = ServiceHelper.GetErrorService(serviceProvider);
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Gets a <see cref="INodeCreationService"/>.
        /// </summary>
        /// <value>
        /// A <see cref="INodeCreationService"/>.
        /// </value>
        protected INodeCreationService NodeCreationService
        {
            get { return nodeCreationService; }
        }

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/>.
        /// </summary>
        /// <value>
        /// The <see cref="IServiceProvider"/>.
        /// </value>
        protected IServiceProvider ServiceProvider
        {
            get { return serviceProvider; }
        }

        /// <summary>
        /// Log an error when building the node graph.
        /// </summary>
        /// <param name="node">The node where the error occurred.</param>
        /// <param name="message"></param>
        protected void LogError(ConfigurationNode node,
                                string message)
        {
            errorLogService.LogError(new ConfigurationError(node, message));
        }

        /// <summary>
        /// Load an error when creating a node.
        /// </summary>
        /// <param name="node">The node where the error occurred.</param>
        /// <param name="configType">The type trying to be created.</param>
        protected void LogNodeMapError(ConfigurationNode node,
                                       MemberInfo configType)
        {
            if (null == configType) throw new ArgumentNullException("configType");

            LogError(node, string.Format(CultureInfo.CurrentUICulture, Resources.ExceptionNodeMapNotRegistered, configType.Name));
        }
    }
}
