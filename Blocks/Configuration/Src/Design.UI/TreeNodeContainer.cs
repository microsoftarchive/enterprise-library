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
using System.Collections;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.UI
{
    /// <summary>
    /// Represents a lookup table of ConfigurationTreeNode objects indexed by their contained configuration node's ID.
    /// </summary>
    public class TreeNodeContainer
    {
        readonly Hashtable treeNodes;

        /// <summary>
        /// Initialize a new instance of the <see cref="TreeNodeContainer"/> class.
        /// </summary>
        public TreeNodeContainer()
        {
            treeNodes = new Hashtable();
        }

        /// <summary>
        /// Adds the specified ConfigurationTreeNode to the container.
        /// </summary>
        /// <param name="treeNode">
        /// The <see cref="ConfigurationTreeNode"/> to add.
        /// </param>
        public void AddTreeNode(ConfigurationTreeNode treeNode)
        {
            treeNodes[treeNode.ConfigurationNode.Id] = treeNode;
        }

        /// <summary>
        /// Looks up the ConfigurationTreeNode indexed by the specified ID.
        /// </summary>
        /// <param name="id">
        /// The id to lookup.
        /// </param>
        public ConfigurationTreeNode GetTreeNode(Guid id)
        {
            return (ConfigurationTreeNode)treeNodes[id];
        }
    }
}
