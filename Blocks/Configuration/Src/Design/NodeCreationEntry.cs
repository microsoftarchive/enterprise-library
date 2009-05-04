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
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Represents a entry for the <see cref="INodeCreationService"/> that contains the data to create a node.
    /// </summary>
    public class NodeCreationEntry
    {
        private readonly ConfigurationNodeCommand configurationNodeCommand;
        private readonly bool allowMultiple;
        private readonly Type nodeType;
        private readonly string displayName;
        private readonly Type dataType;
        private Type baseTypeToCompare;
        

        /// <summary>
        /// Initialize a new instance of the <see cref="NodeCreationEntry"/> with a command, the node type to create, the configuration data type associated with the node, name to display for this node, and if there can be more than one node of the type.
        /// </summary>
        /// <param name="configurationNodeCommand">The <see cref="ConfigurationNodeCommand"/> used to create the node.</param>
        /// <param name="nodeType">The <see cref="Type"/> of the node to create.</param>
        /// <param name="dataType">The <see cref="Type"/> of the configuration data associated with the <paramref name="nodeType"/>.</param>
        /// <param name="displayName">The name to display in the user interface to create the node.</param>
        /// <param name="allowMultiple">
        /// Determines if more than one of the node type can exist.
        /// </param>
        private NodeCreationEntry(ConfigurationNodeCommand configurationNodeCommand, Type nodeType, Type dataType, string displayName, bool allowMultiple)
        {
            if (null == configurationNodeCommand) throw new ArgumentNullException("configurationNodeCommand");
            if (null == nodeType) throw new ArgumentNullException("nodeType");
            if (null == dataType) throw new ArgumentNullException("dataType");
            if (string.IsNullOrEmpty(displayName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "displayName");

            this.configurationNodeCommand = configurationNodeCommand;
            this.allowMultiple = allowMultiple;
            this.nodeType = nodeType;
            if (!typeof(ConfigurationNode).IsAssignableFrom(nodeType))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentUICulture, Resources.ExceptionTypeNotConfigurationNode, nodeType.FullName), "nodeType");
            }

            this.displayName = displayName;
            this.dataType = dataType;
        }

        /// <summary>
        /// Create a new instance of the <see cref="NodeCreationEntry"/> that allows multiple nodes in the same parent node with a command, the node type to create, the configuration data type associated with the node and name to display for this node.
        /// </summary>
        /// <param name="configurationNodeCommand">The <see cref="ConfigurationNodeCommand"/> used to create the node.</param>
        /// <param name="nodeType">The <see cref="Type"/> of the node to create.</param>
        /// <param name="dataType">The <see cref="Type"/> of the configuration data associated with the <paramref name="nodeType"/>.</param>
        /// <param name="displayName">The name to display in the user interface to create the node.</param>
		/// <returns>The new <see cref="NodeCreationEntry"/> object.</returns>
        public static NodeCreationEntry CreateNodeCreationEntryWithMultiples(ConfigurationNodeCommand configurationNodeCommand, Type nodeType, Type dataType, string displayName)
        {
            return new NodeCreationEntry(configurationNodeCommand, nodeType, dataType, displayName, true);
        }

        /// <summary>
        /// Create a new instance of the <see cref="NodeCreationEntry"/> that does not allows multiple nodes in the same parent node with a command, the node type to create, the configuration data type associated with the node and name to display for this node.
        /// </summary>
        /// <param name="configurationNodeCommand">The <see cref="ConfigurationNodeCommand"/> used to create the node.</param>
        /// <param name="nodeType">The <see cref="Type"/> of the node to create.</param>
        /// <param name="dataType">The <see cref="Type"/> of the configuration data associated with the <paramref name="nodeType"/>.</param>
        /// <param name="displayName">The name to display in the user interface to create the node.</param>
        /// <returns>The new <see cref="NodeCreationEntry"/> object.</returns>
        public static NodeCreationEntry CreateNodeCreationEntryNoMultiples(ConfigurationNodeCommand configurationNodeCommand, Type nodeType, Type dataType, string displayName)
        {
            return new NodeCreationEntry(configurationNodeCommand, nodeType, dataType, displayName, false);
        }

        /// <summary>
        /// Gets the configuration data type associated with the node type.
        /// </summary>
        /// <value>
        /// The configuration data type associated with the node type.
        /// </value>
        public Type DataType
        {
            get { return dataType; }
        }

        /// <summary>
        /// Gets the node type to create.
        /// </summary>
        /// <value>
        /// The node type to create.
        /// </value>
        public Type NodeType
        {
            get { return nodeType; }
        }

        /// <summary>
        /// Gets the display name to show in the user interface that creats the node.
        /// </summary>
        /// <value>
        /// The display name to show in the user interface that creats the node.
        /// </value>
        public string DisplayName
        {
            get { return displayName; }
        }

        /// <summary>
        /// Gets the command used to create the node.
        /// </summary>
        /// <value>
        /// The commad used to create the node.
        /// </value>
        public ConfigurationNodeCommand ConfigurationNodeCommand
        {
            get { return configurationNodeCommand; }
        }

        /// <summary>
        /// Determines if more than one of the node type can exist.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if more than one of the node type can exist; otherwise, <see langword="false"/>.
        /// </value>
        public bool AllowMultiple
        {
            get { return allowMultiple; }
        }

        /// <summary>
        /// Gets or sets the base type of the node type that will be compared to determine if the type can allow multiple.
        /// </summary>
        /// <value>
        /// The base type of the node type that will be compared to determine if the type can allow multiple.
        /// </value>
        internal Type BaseTypeToCompare
        {
            get { return baseTypeToCompare; }
            set { baseTypeToCompare = value; }
        }
    }
}
