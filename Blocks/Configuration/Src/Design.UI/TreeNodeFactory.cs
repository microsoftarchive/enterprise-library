//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
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
    /// Represents a factory for creating ConfigurationTreeNode objects.
    /// </summary>
    public class TreeNodeFactory
    {
        ConfigurationNodeImageContainer imageContainer;
        TreeNodeContainer treeNodeContainer;
        Hashtable treeNodeLookup;

        /// <summary>
        /// Initialize a new instance of the <see cref="TreeNodeFactory"/> class.
        /// </summary>
        public TreeNodeFactory()
        {
            treeNodeContainer = new TreeNodeContainer();
            treeNodeLookup = new Hashtable();
            treeNodeLookup.Add(typeof(ConfigurationNode), typeof(ConfigurationTreeNode));
        }

        /// <summary>
        /// Creates a new intance of a ConfigurationTreeNode class and sets its ImageIndex and SelectedImageIndex property values.
        /// </summary>
        /// <param name="node">
        /// The parent node.
        /// </param>
        public ConfigurationTreeNode Create(ConfigurationNode node)
        {
            Type nodeType = node.GetType();
            Type treeNodeType = GetTreeNodeType(nodeType);
            ConfigurationTreeNode treeNode = (ConfigurationTreeNode)Activator.CreateInstance(treeNodeType, new object[] { node, this });
            if (imageContainer != null)
            {
                treeNode.ImageIndex = imageContainer.GetImageIndex(nodeType);
                treeNode.SelectedImageIndex = imageContainer.GetSelectedImageIndex(nodeType);
            }
            if (treeNodeContainer != null)
            {
                treeNodeContainer.AddTreeNode(treeNode);
            }
            return treeNode;
        }

        /// <summary>
        /// Looks up the ConfigurationTreeNode indexed by the specified ID.
        /// </summary>
        /// <param name="id">
        /// The id to look up.
        /// </param>
        public ConfigurationTreeNode GetTreeNode(Guid id)
        {
            return treeNodeContainer.GetTreeNode(id);
        }

        Type GetTreeNodeType(Type nodeType)
        {
            Type t = (Type)treeNodeLookup[nodeType];
            if (t != null)
            {
                return t;
            }
            else
            {
                return GetTreeNodeType(nodeType.BaseType);
            }
        }

        /// <summary>
        /// Sets the image container for the node.
        /// </summary>
        /// <param name="imageContainer"></param>
        public void SetImageContainer(ConfigurationNodeImageContainer imageContainer)
        {
            this.imageContainer = imageContainer;
        }
    }
}