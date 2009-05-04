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

using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.UI
{
    /// <devdoc>
    /// The graphical representation of a ConfigurationNode.
    /// </devdoc>
    public class ConfigurationTreeNode : TreeNode
    {
        private TreeNodeFactory childNodeFactory;
        private ConfigurationNode configurationNode;
        private SortedList children;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configNode"></param>
        /// <param name="childNodeFactory"></param>
        public ConfigurationTreeNode(ConfigurationNode configNode, TreeNodeFactory childNodeFactory) : base(configNode.Name)
        {
            this.childNodeFactory = childNodeFactory;
            configurationNode = configNode;
            this.RegisterEventHandlers();
        }

        /// <devdoc>
        /// Gets the ConfigurationNode associated with this tree node
        /// </devdoc>
        [Browsable(false)]
        public ConfigurationNode ConfigurationNode
        {
            get { return configurationNode; }
        }

        /// <devdoc>
        /// Copies the tree node and the  entire subtree rooted at this tree node.
        /// </devdoc>
        public override object Clone()
        {
            ConfigurationTreeNode clone = (ConfigurationTreeNode) base.Clone();
            clone.configurationNode = configurationNode;
            clone.children = children;
            return clone;
        }

        /// <devdoc>
        /// Creates a ConfigurationTreeNode using the TreeNodeFactory of the current ConfigurationTreeView.
        /// </devdoc>
        protected ConfigurationTreeNode CreateChildNode(ConfigurationNode node)
        {
            ConfigurationTreeNode treeNode = childNodeFactory.Create(node);
            return treeNode;
        }

        private void RegisterEventHandlers()
        {
			configurationNode.ChildAdded += new EventHandler<ConfigurationNodeChangedEventArgs>(this.OnChildAdded);
			configurationNode.ChildRemoved += new EventHandler<ConfigurationNodeChangedEventArgs>(this.OnChildRemoved);
			configurationNode.Renamed += new EventHandler<ConfigurationNodeChangedEventArgs>(this.OnRenamed);
			configurationNode.Removed += new EventHandler<ConfigurationNodeChangedEventArgs>(this.OnRemoved);
			configurationNode.ChildMoved += new EventHandler<ConfigurationNodeMovedEventArgs>(this.OnChildMoved);
        }

        private void OnChildMoved(object sender, ConfigurationNodeMovedEventArgs e)
        {
            TreeNode node = Nodes[e.OldIndex];
            Nodes.RemoveAt(e.OldIndex);
            Nodes.Insert(e.NewIndex, node);
            node.TreeView.SelectedNode = node;
        }

        private void OnChildAdded(object sender, ConfigurationNodeChangedEventArgs e)
        {
            AddChildeNode(e);
            this.Expand();
        }

        private void AddChildeNode(ConfigurationNodeChangedEventArgs e)
        {
            ConfigurationTreeNode newNode = CreateChildNode(e.Node);
            if (newNode.ConfigurationNode.Parent.SortChildren)
            {
                AddSorted(newNode);
            }
            else
            {
                this.Nodes.Add(newNode);    
            }
            foreach (ConfigurationNode child in e.Node.Nodes)
            {
                newNode.OnChildAdded(newNode, new ConfigurationNodeChangedEventArgs(child, e.Node));
            }
        }

        private void OnChildRemoved(object sender, ConfigurationNodeChangedEventArgs e)
        {
            foreach (ConfigurationTreeNode child in Nodes)
            {
                if (e.Node.Id == child.ConfigurationNode.Id)
                {
                    this.Nodes.Remove(child);
                    break;
                }
            }
        }

        private void OnRemoved(object sender, ConfigurationNodeChangedEventArgs e)
        {
            if (this.Parent != null)
            {
                this.Parent.Nodes.Remove(this);
            }
        }

        private void OnRenamed(object sender, ConfigurationNodeChangedEventArgs args)
        {
            Text = args.Node.Name;
            // check to see if we are attached are not attached yet
            if (TreeView == null) return;
            //this.TreeView.FindForm().Refresh();
        }

        private int AddSorted(ConfigurationTreeNode node) 
        {
            int index = 0;
            string nodeDisplayName = node.Text;
            int childCount = Nodes.Count;
            if (childCount > 0) 
            {
                CompareInfo compare = CultureInfo.CurrentCulture.CompareInfo;                
                if (compare.Compare(Nodes[childCount-1].Text, nodeDisplayName) <= 0) 
                {
                    index = childCount;
                } 
                else 
                {                    
                    int firstNode = 0;
                    int lastNode = 0;
                    int compareNode = 0;                   
                    for (firstNode = 0, lastNode = childCount; firstNode < lastNode;) 
                    {
                        compareNode = (firstNode + lastNode) / 2;
                        if (compare.Compare(Nodes[compareNode].Text, nodeDisplayName) <= 0) 
                        {
                            firstNode = compareNode + 1;
                        } 
                        else 
                        {
                            lastNode = compareNode;
                        }                        
                    }
                    index = firstNode;
                }
            }
            Nodes.Insert(index, node);            
            return index;
        }
    }
}
