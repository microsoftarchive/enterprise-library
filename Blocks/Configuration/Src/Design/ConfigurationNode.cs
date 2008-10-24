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
using System.Reflection;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	/// <summary>
	/// Represents a designtime node for a particular configuration element. This class is abstract.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes"), Image(typeof(ConfigurationNode))]
	[System.ComponentModel.DesignerCategory("Code")]	
	[DefaultProperty("Name")]
	public abstract class ConfigurationNode : IComponent, IComparable
	{	
		private static readonly object renamingEvent = new object();
		private static readonly object renamedEvent = new object();
        private static readonly object childRemovingEvent = new object();
		private static readonly object childRemovedEvent = new object();
        private static readonly object childAddingEvent = new object();
        private static readonly object childAddedEvent = new object();
        private static readonly object childMovedEvent = new object();		
		private static readonly object removingEvent = new object();
		private static readonly object removedEvent = new object();
		private static readonly object disposedEvent = new object();

		private Guid id;
		private ConfigurationNode parent;
		private int index;
		private int childCount;
		private ConfigurationNode[] childNodes;
		private ConfigurationNodeCollection nodes;
		private IConfigurationUIHierarchy hierarchy;		
		internal Dictionary<string, ConfigurationNode> childNodeLookup;
		internal Dictionary<Guid, ConfigurationNode> childNodeById;
		private string nodeName;
		private ISite site;
		private EventHandlerList handlerList;

		/// <summary>
		/// Occurs after the name of the current node changes.
		/// </summary>
		public event EventHandler<ConfigurationNodeChangedEventArgs> Renaming
		{
			add { if (null != handlerList) handlerList.AddHandler(renamingEvent, value); }
			remove { if (null != handlerList) handlerList.RemoveHandler(renamingEvent, value); }
		}

		/// <summary>
		/// Occurs after the name of the current node changes.
		/// </summary>
		public event EventHandler<ConfigurationNodeChangedEventArgs> Renamed
		{
			add { if (null != handlerList) handlerList.AddHandler(renamedEvent, value); }
			remove { if (null != handlerList) handlerList.RemoveHandler(renamedEvent, value); }
		}


		/// <summary>
		/// Occurs before a child node of the current node is removed.
		/// </summary>
		public event EventHandler<ConfigurationNodeChangedEventArgs> ChildRemoving
        {
			add { if (null != handlerList) handlerList.AddHandler(childRemovingEvent, value); }
			remove { if (null != handlerList) handlerList.RemoveHandler(childRemovingEvent, value); }
        }

		/// <summary>
		/// Occurs after a child node of the current node is removed.
		/// </summary>
		public event EventHandler<ConfigurationNodeChangedEventArgs> ChildRemoved
		{
			add { handlerList.AddHandler(childRemovedEvent, value); }
			remove { handlerList.RemoveHandler(childRemovedEvent, value); }
		}

		/// <summary>
		/// Occurs after a child node is added to the current node.
		/// </summary>
		public event EventHandler<ConfigurationNodeChangedEventArgs> ChildAdding
		{
			add { if (null != handlerList) handlerList.AddHandler(childAddingEvent, value); }
			remove { if (null != handlerList) handlerList.RemoveHandler(childAddingEvent, value); }
		}

		/// <summary>
		/// Occurs after a child node is added to the current node.
		/// </summary>
		public event EventHandler<ConfigurationNodeChangedEventArgs> ChildAdded
		{
			add { if (null != handlerList) handlerList.AddHandler(childAddedEvent, value); }
			remove { if (null != handlerList) handlerList.RemoveHandler(childAddedEvent, value); }
		}		

		/// <summary>
		/// Occurs after a child node is moved from its parent's node collection.
		/// </summary>
        public event EventHandler<ConfigurationNodeMovedEventArgs> ChildMoved
        {
			add { if (null != handlerList) handlerList.AddHandler(childMovedEvent, value); }
			remove { if (null != handlerList) handlerList.RemoveHandler(childMovedEvent, value); }
        }

		/// <summary>
		/// Occurs before the current node is removed from its parent's node collection.
		/// </summary>
		public event EventHandler<ConfigurationNodeChangedEventArgs> Removing
        {
			add { if (null != handlerList) handlerList.AddHandler(removingEvent, value); }
			remove { if (null != handlerList) handlerList.RemoveHandler(removingEvent, value); }
        }


		/// <summary>
		/// Occurs after the current node is removed from its parent's node collection.
		/// </summary>
		public event EventHandler<ConfigurationNodeChangedEventArgs> Removed
		{
			add { if (null != handlerList) handlerList.AddHandler(removedEvent, value); }
			remove { if (null != handlerList) handlerList.RemoveHandler(removedEvent, value); }
		}

		/// <summary>
		/// Occurs after the current node is disposed.
		/// </summary>
		public event EventHandler Disposed
		{
			add { if (null != handlerList) handlerList.AddHandler(disposedEvent, value); }
			remove { if (null != handlerList) handlerList.RemoveHandler(disposedEvent, value); }
		}

		/// <summary>
		/// Initialize a new instance of the <see cref="ConfigurationNode"/> class.
		/// </summary>
		protected ConfigurationNode()
			: this(string.Empty)
		{
		}

		/// <summary>
		/// Initialize a new instance of the <see cref="ConfigurationNode"/> class with a name.
		/// </summary>		
		/// <param name="name">The name of the node.</param>
		protected ConfigurationNode(string name)
		{
			id = Guid.NewGuid();
			childNodeLookup = new Dictionary<string, ConfigurationNode>();
			childNodeById = new Dictionary<Guid, ConfigurationNode>();
			nodeName = name;
            index = -1;
			handlerList = new EventHandlerList();
		}

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="ConfigurationNode "/> and optionally releases the managed resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="ConfigurationNode"/> and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">
		/// <see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				EventHandler handler = (EventHandler)handlerList[disposedEvent];
				if (handler != null) handler(this, EventArgs.Empty);
				if (handlerList != null) handlerList.Dispose();
				handlerList = null;
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="ISite"/> for the node.
		/// </summary>
		/// <value>
		/// The <see cref="ISite"/> for the node.
		/// </value>
		[Browsable(false)]
		public ISite Site
		{
			get
			{
				return site;
			}
			set
			{			
				site = value;
			}
		}

		/// <summary>
		/// Gets the position of this node in the <seealso cref="Nodes"/>.
		/// </summary>
		/// <value>
		/// The position of this node in the <seealso cref="Nodes"/>.
		/// </value>
		[Browsable(false)]
		public int Index
		{
			get { return index; }
		}

		/// <summary>
		/// Gets the collection of <see cref="ConfigurationNode"/> objects assigned to the current configuration node.
		/// </summary>
		/// <value>
		/// The collection of <see cref="ConfigurationNode"/> objects assigned to the current configuration node.
		/// </value>
		[Browsable(false)]
		public IReadOnlyCollection<ConfigurationNode> Nodes
		{
			get
			{
				if (nodes == null)
				{
					nodes = new ConfigurationNodeCollection(this);
				}				
				return nodes;
			}
		}

		/// <summary>
		/// Gets or sets the name of the node.
		/// </summary>
		/// <value>
		/// The name of the node.
		/// </value>
		[SRCategory("CategoryName", typeof(Resources))]
		[SRDescription("NodeNameDescription", typeof(Resources))]
		[Required]
		public virtual string Name
		{
			get
			{
				return GetName();
			}
			set 
			{				
				Rename(value, true); 
			}
		}

		/// <summary>
		/// Gets the node immediately preceding this node.
		/// </summary>
		/// <value>
		/// The preceding <see cref="ConfigurationNode"/>. If there is no preceding node, a <see langword="null"/> reference (Nothing in Visual Basic) is returned.
		/// </value>
		[Browsable(false)]
		public ConfigurationNode PreviousSibling
		{
			get
			{
				if (Index > 0)
				{
					int previousIndex = Index - 1;
					return parent.childNodes[previousIndex];
				}
				else
				{
					return null;
				}
			}
		}

		/// <summary>
		/// Gets the node immediately following this node.
		/// </summary>
		/// <value>
		/// The next <see cref="ConfigurationNode"/>. If there is no next node, a <see langword="null"/> reference (Nothing in Visual Basic) is returned.
		/// </value>
		[Browsable(false)]
		public ConfigurationNode NextSibling
		{
			get
			{
				int nextIndex = Index + 1;
				// check if this is the last child
				if (parent != null && nextIndex != parent.childCount)
				{
					return parent.childNodes[nextIndex];
				}
				else
				{
					return null;
				}
			}
		}

		/// <summary>
		/// Gets the first child node in the node collection.
		/// </summary>
		/// <value>
		/// The first child <see cref="ConfigurationNode"/> in the <seealso cref="Nodes"/> collection.
		/// </value>
		[Browsable(false)]
		public ConfigurationNode FirstNode
		{
			get
			{
				if (childCount > 0)
				{
					return childNodes[0];
				}
				return null;
			}
		}

		/// <summary>
		/// Gets the first child node in the node collection.
		/// </summary>
		/// <value>
		/// The first child <see cref="ConfigurationNode"/> in the <seealso cref="Nodes"/> collection.
		/// </value>
		[Browsable(false)]
		public ConfigurationNode LastNode
		{
			get
			{
				if (childCount > 0)
				{
					return childNodes[childCount - 1];
				}
				return null;
			}
		}

		/// <summary>
		/// Gets the parent configuration node of the the current configuration node.
		/// </summary>
		/// <value>
		/// The parent configuration node of the the current configuration node.
		/// </value>
		[Browsable(false)]
		public ConfigurationNode Parent
		{
			get { return parent; }
		}

		/// <summary>
		/// Gets the path from the root node to the current node.
		/// </summary>
		/// <value>
		/// The path from the root node to the current node.
		/// </value>
		[Browsable(false)]
		public string Path
		{
			get
			{
				StringBuilder path = new StringBuilder();
				GetFullPath(path, System.IO.Path.AltDirectorySeparatorChar);
				return path.ToString();
			}
		}

		/// <summary>
		/// Gets a runtime-generated, non-persisted unique identifier for this node.
		/// </summary>
		/// <value>
		/// A runtime-generated, non-persisted unique identifier for this node.
		/// </value>
		[Browsable(false)]
		public Guid Id
		{
			get { return id; }
		}

		/// <summary>
		/// Gets or sets the <see cref="IConfigurationUIHierarchy"/> containing this node.
		/// </summary>
		/// <value>
		/// The <see cref="IConfigurationUIHierarchy"/> containing this node. The default is <see langword="null"/>.
		/// </value>
		[Browsable(false)]
		public IConfigurationUIHierarchy Hierarchy
		{
			get { return hierarchy; }
			set { hierarchy = value; }
		}

		/// <summary>
		/// Gets if children added to the node are sorted.
		/// </summary>
		/// <value>
		/// <see langword="true"/> if nodes add are sorted; otherwise <see langword="false"/>. The default is <see langword="true"/>.
		/// </value>
		[Browsable(false)]
		public virtual bool SortChildren
		{
			get { return true; }
		}

		/// <summary>
		/// Moves the specified child node immediately after the specified reference node.
		/// </summary>
		/// <param name="childNode">The existing child node to move.</param>
		/// <param name="siblingNode">The existing child node after which the <i>childNode</i> will be placed.</param>
		public void MoveAfter(ConfigurationNode childNode, ConfigurationNode siblingNode)
		{
			if (childNode == null) throw new ArgumentNullException("childNode");
			if (siblingNode == null) throw new ArgumentNullException("siblingNode");

			if ((childNode.Parent != (ConfigurationNode)this) || (siblingNode.Parent != (ConfigurationNode)this))
			{
				throw new InvalidOperationException(Resources.ExceptionOnlyReorderSiblings);
			}
			if (siblingNode.NextSibling != childNode)
			{				
				MoveNode(siblingNode.Index, (ConfigurationNode)childNode);				
			}
		}

		/// <summary>
		/// Moves the specified node immediately before the specified reference node.
		/// </summary>
		/// <param name="childNode">The existing child node to move.</param>
		/// <param name="siblingNode">The existing child node before which the <i>childNode</i> will be placed.</param>
		public void MoveBefore(ConfigurationNode childNode, ConfigurationNode siblingNode)
		{
			if (childNode == null) throw new ArgumentNullException("childNode");
			if (siblingNode == null) throw new ArgumentNullException("siblingNode");

			if (childNode.Parent != (ConfigurationNode)this || siblingNode.Parent != (ConfigurationNode)this)
			{
				throw new InvalidOperationException(Resources.ExceptionOnlyReorderSiblings);
			}
			// check if the node's are already ordered in this way
			if (siblingNode.PreviousSibling != childNode)
			{				
				MoveNode(siblingNode.Index, (ConfigurationNode)childNode);
			}
		}

		/// <summary>
		/// Creates and returns a string representation of the current node.
		/// </summary>
		/// <returns>A string representation of the current node.</returns>
		public override string ToString()
		{
			return this.Name;
		}

		/// <summary>
		/// Removes the current node from its parent node.
		/// </summary>
		public void Remove()
		{
			RemoveNode(this);
		}

		/// <summary>
		/// Perform custom validation for this node.
		/// </summary>
		/// <param name="errors">The list of errors to add any validation errors.</param>
		public virtual void Validate(IList<ValidationError> errors)
		{			
		}

		/// <summary>
		/// Compares the specified node to the current node based on the value of the <seealso cref="Name"/> property.
		/// </summary>
		/// <param name="obj">An object to compare.></param>
		/// <returns>
		/// A signed integer that indicates the relative order of this node and the node being compared.
		/// </returns>
		int IComparable.CompareTo(object obj)
		{
			ConfigurationNode compareNode = obj as ConfigurationNode;
			if (compareNode == null)
			{
				return -1;
			}
			return id.CompareTo(compareNode.Id);
		}

		/// <summary>
		/// Compares the specified node to the current node based on the value of the <seealso cref="Name"/> property.
		/// </summary>
		/// <param name="node">A <see cref="ConfigurationNode"/> to compare.</param>
		/// <returns>
		/// A signed integer that indicates the relative order of this node and the node being compared.
		/// </returns>
		public int CompareTo(ConfigurationNode node)
		{
			return ((IComparable)this).CompareTo(node);
		}

		/// <summary>
		/// Add a node to the current node's collection.
		/// </summary>
		/// <param name="configurationNode">The node to add.</param>
		/// <returns>The index of the added node.</returns>
		public int AddNode(ConfigurationNode configurationNode)
		{
			if (childNodeById.ContainsKey(configurationNode.Id)) throw new ArgumentException(Resources.ExceptionNodeAlreadyInCollection, "configurationNode");

			EnsureNodeCapacity();
			int newIndex = -1;
			configurationNode.parent = this;
			OnChildAdding(new ConfigurationNodeChangedEventArgs(configurationNode, this));
			UpdateHierarchy(configurationNode);
			childNodeLookup.Add(configurationNode.Name, configurationNode);
			childNodeById.Add(configurationNode.Id, configurationNode);
			if (SortChildren)
			{
				newIndex = AddSorted(configurationNode);
			}
			else
			{
				newIndex = AddUnsorted(configurationNode);
			}

			OnChildAdded(new ConfigurationNodeChangedEventArgs(configurationNode, this));			
			return newIndex;
		}		

		/// <summary>
		/// Remove a specific node from the current node's collection.
		/// </summary>
		/// <param name="configurationNode">The node to remove.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
		public void RemoveNode(ConfigurationNode configurationNode)
		{
			if (null == configurationNode) throw new ArgumentNullException("configurationNode");

			configurationNode.RemoveThisNode(ParentNodeNotification.Notify);
		}

		/// <summary>
		/// Clear the <see cref="Nodes"/> collection.
		/// </summary>
		public void ClearChildNodes()
		{
			while (childCount > 0)
			{
				childNodes[childCount - 1].RemoveThisNode(ParentNodeNotification.Notify);
			}			
			childNodes = null;
		}

		/// <summary>
		/// Rename the current node.
		/// </summary>
		/// <param name="newName">The new node name.</param>
		protected internal void Rename(string newName)
        {
			string currentName = GetName();	
			if (!ShouldRename(newName, currentName)) return;

			DoRename(newName, currentName);
        }

		/// <summary>
		/// Rename the current node and determine if the parent node should be notified and events should be fired..
		/// </summary>
		/// <param name="newName">The new node name.</param>
		/// <param name="notify"><c>true</c> if the parent node should be notified and the <see cref="ChildAdded"/> and <see cref="ChildAdding"/> events are fired; otherwise, <c>false</c>.</param>
		protected internal void Rename(string newName, bool notify)
		{
			if (string.IsNullOrEmpty(newName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "newName");

			string currentName = GetName();			
			if (!ShouldRename(newName, currentName)) return;
			
            if (parent != null && notify)
			{
				if (parent.childNodeLookup.ContainsKey(newName))
				{
					throw new InvalidOperationException(string.Format(Resources.Culture, Resources.ExceptionNodeNameAlreadyExists, newName));
				}
				parent.childNodeLookup.Remove(currentName);
				parent.childNodeLookup.Add(newName, this);
			}
            if (notify) OnRenaming(new ConfigurationNodeChangedEventArgs(this, parent));
			DoRename(newName, currentName);
			if (notify) OnRenamed(new ConfigurationNodeChangedEventArgs(this, parent));
		}

		/// <summary>
		/// <para>Raises the <seealso cref="ChildRemoving"/> event.</para>
		/// </summary>
		/// <param name="e"><para>A <see cref="ConfigurationNodeChangedEventArgs"/> that contains the event data.</para></param>
		protected virtual void OnChildRemoving(ConfigurationNodeChangedEventArgs e)
		{
			if (handlerList != null)
			{
				EventHandler<ConfigurationNodeChangedEventArgs> handler = (EventHandler<ConfigurationNodeChangedEventArgs>)handlerList[childRemovingEvent];
				if (handler != null)
				{
					handler(this, e);
				}
			}
		}

		/// <summary>
		/// <para>Raises the <seealso cref="ChildRemoved"/> event.</para>
		/// </summary>
		/// <param name="e"><para>A <see cref="ConfigurationNodeChangedEventArgs"/> that contains the event data.</para></param>
		protected virtual void OnChildRemoved(ConfigurationNodeChangedEventArgs e)
		{
			if (handlerList != null)
			{
				EventHandler<ConfigurationNodeChangedEventArgs> handler = (EventHandler<ConfigurationNodeChangedEventArgs>)handlerList[childRemovedEvent];
				if (handler != null)
				{
					handler(this, e);
				}
			}
		}

		/// <summary>
		/// <para>Raises the <seealso cref="Removing"/> event.</para>
		/// </summary>
		/// <param name="e"><para>A <see cref="ConfigurationNodeChangedEventArgs"/> that contains the event data.</para></param>
		protected virtual void OnRemoving(ConfigurationNodeChangedEventArgs e)
		{
			if (handlerList != null)
			{
				EventHandler<ConfigurationNodeChangedEventArgs> handler = (EventHandler<ConfigurationNodeChangedEventArgs>)handlerList[removingEvent];
				if (handler != null)
				{
					handler(this, e);
				}
			}
		}

		/// <summary>
		/// <para>Raises the <seealso cref="Removed"/> event.</para>
		/// </summary>
		/// <param name="e"><para>A <see cref="ConfigurationNodeChangedEventArgs"/> that contains the event data.</para></param>
		protected virtual void OnRemoved(ConfigurationNodeChangedEventArgs e)
		{
			if (handlerList != null)
			{
				EventHandler<ConfigurationNodeChangedEventArgs> handler = (EventHandler<ConfigurationNodeChangedEventArgs>)handlerList[removedEvent];
				if (handler != null)
				{
					handler(this, e);
				}
			}
		}

		/// <summary>
		/// <para>Raises the <seealso cref="ChildMoved"/> event.</para>
		/// </summary>
		/// <param name="e"><para>A <see cref="ConfigurationNodeChangedEventArgs"/> that contains the event data.</para></param>
		protected virtual void OnChildMoved(ConfigurationNodeMovedEventArgs e)
		{
			if (handlerList != null)
			{
				EventHandler<ConfigurationNodeMovedEventArgs> handler = (EventHandler<ConfigurationNodeMovedEventArgs>)handlerList[childMovedEvent];
				if (handler != null)
				{
					handler(this, e);
				}
			}
		}

		/// <summary>
		/// <para>Raises the <seealso cref="Renaming"/> event.</para>
		/// </summary>
		/// <param name="e"><para>A <see cref="ConfigurationNodeChangedEventArgs"/> that contains the event data.</para></param>
		protected virtual void OnRenaming(ConfigurationNodeChangedEventArgs e)
		{
			if (handlerList != null)
			{
				EventHandler<ConfigurationNodeChangedEventArgs> handler = (EventHandler<ConfigurationNodeChangedEventArgs>)handlerList[renamingEvent];
				if (handler != null)
				{
					handler(this, e);
				}
			}
		}

		/// <summary>
		/// <para>Raises the <seealso cref="Renamed"/> event.</para>
		/// </summary>
		/// <param name="e"><para>A <see cref="ConfigurationNodeChangedEventArgs"/> that contains the event data.</para></param>
		protected virtual void OnRenamed(ConfigurationNodeChangedEventArgs e)
		{
			if (handlerList != null)
			{
				EventHandler<ConfigurationNodeChangedEventArgs> handler = (EventHandler<ConfigurationNodeChangedEventArgs>)handlerList[renamedEvent];
				if (handler != null)
				{
					handler(this, e);
				}
			}
		}

		/// <summary>
		/// <para>Raises the <seealso cref="ChildAdding"/> event.</para>
		/// </summary>
		/// <param name="e"><para>A <see cref="ConfigurationNodeChangedEventArgs"/> that contains the event data.</para></param>
		protected virtual void OnChildAdding(ConfigurationNodeChangedEventArgs e)
		{
			if (handlerList != null)
			{
				EventHandler<ConfigurationNodeChangedEventArgs> handler = (EventHandler<ConfigurationNodeChangedEventArgs>)handlerList[childAddingEvent];
				if (handler != null)
				{
					handler(this, e);
				}
			}
		}

		/// <summary>
		/// <para>Raises the <seealso cref="ChildAdded"/> event.</para>
		/// </summary>
		/// <param name="e"><para>A <see cref="ConfigurationNodeChangedEventArgs"/> that contains the event data.</para></param>
		protected virtual void OnChildAdded(ConfigurationNodeChangedEventArgs e)
		{
			if (handlerList != null)
			{
				EventHandler<ConfigurationNodeChangedEventArgs> handler = (EventHandler<ConfigurationNodeChangedEventArgs>)handlerList[childAddedEvent];
				if (handler != null)
				{
					handler(this, e);
				}
			}
		}

		internal int ChildCount
		{
			get { return childCount; }
		}

		internal ConfigurationNode[] ChildNodes
		{
			get { return childNodes; }
		}

		internal void MoveNode(int moveIndex, ConfigurationNode childNode)
		{			
            int childNodeIndex = childNode.Index;
            childNodes[childNodeIndex] = null;
            for (int idx = childNodeIndex; idx < childCount - 1; idx++)
            {
                childNodes[idx] = childNodes[idx + 1];
                childNodes[idx].index = idx;
            }
            childCount--;
            InsertNodeAt(moveIndex, (ConfigurationNode)childNode);

            OnChildMoved(new ConfigurationNodeMovedEventArgs(childNode, this, childNodeIndex, moveIndex));
		}

		internal void UpdateHierarchy(ConfigurationNode configurationNode)
		{
			if (hierarchy != null)
			{
				hierarchy.AddNode(configurationNode);
				UpdateHierarchyRecursive(hierarchy, configurationNode);
			}
		}

		internal void InsertNodeAt(int newIndex, ConfigurationNode node)
		{
			EnsureNodeCapacity();
			node.parent = this;
			node.index = newIndex;
			for (int count = childCount; count > newIndex; --count)
			{
				(childNodes[count] = childNodes[count - 1]).index = count;
			}
			childNodes[newIndex] = node;
			childCount++;
		}

		internal void EnsureNodeCapacity()
		{
			if (childNodes == null)
			{
				childNodes = new ConfigurationNode[4];
			}
			else if (childCount == childNodes.Length)
			{
				ConfigurationNode[] resizeArray = new ConfigurationNode[childCount * 2];
				Array.Copy(childNodes, 0, resizeArray, 0, childCount);
				childNodes = resizeArray;
			}
		}

		internal void RemoveThisNode(ParentNodeNotification notification)
		{
			OnRemoving(new ConfigurationNodeChangedEventArgs(this, parent));

			for (int i = 0; i < childCount; i++)
			{
				childNodes[i].RemoveThisNode(ParentNodeNotification.DoNotNotify);
			}
			// since the name is based on site have to store to really remove it
			string nameToRemove = this.Name;
			RemoveNodeFromChildLookup(nameToRemove);
			hierarchy.RemoveNode(this);
			if ((notification == ParentNodeNotification.Notify) && (parent != null))
			{
				parent.OnChildRemoving(new ConfigurationNodeChangedEventArgs(this, null));
				parent.childNodes[index] = null;
				for (int i = index; i < parent.childCount - 1; ++i)
				{
					parent.childNodes[i] = parent.childNodes[i + 1];
					parent.childNodes[i].index = i;
				}
				parent.childCount--;
				parent.OnChildRemoved(new ConfigurationNodeChangedEventArgs(this, null));
			}
			parent = null;
			OnRemoved(new ConfigurationNodeChangedEventArgs(this, parent));
		}

		private void DoRename(string newName, string currentName)
		{
			nodeName = newName;
			if (null != Site) Site.Name = newName;
			if (null != hierarchy) hierarchy.RenameNode(this, currentName, newName);
		}

		private static bool ShouldRename(string newName, string currentName)
		{			
			if (newName == currentName)
			{
				return false;
			}
			return true;
		}

		private int AddUnsorted(ConfigurationNode configurationNode)
		{
            InsertNodeAt(childCount, configurationNode);			
			return configurationNode.index;
		}

		private int AddSorted(ConfigurationNode node)
		{
			int newIndex = 0;
			string nodeDisplayName = node.Name;
			if (childCount > 0)
			{
				CompareInfo compare = CultureInfo.CurrentCulture.CompareInfo;
				if (compare.Compare(childNodes[childCount - 1].Name, nodeDisplayName) <= 0)
				{
					newIndex = childCount;
				}
				else
				{
					int firstNode = 0;
					int lastNode = 0;
					int compareNode = 0;
					for (firstNode = 0, lastNode = childCount; firstNode < lastNode; )
					{
						compareNode = (firstNode + lastNode) / 2;
						if (compare.Compare(childNodes[compareNode].Name, nodeDisplayName) <= 0)
						{
							firstNode = compareNode + 1;
						}
						else
						{
							lastNode = compareNode;
						}
					}
					newIndex = firstNode;
				}
			}
			InsertNodeAt(newIndex, node);
			return newIndex;
		}		
		

		private void RemoveNodeFromChildLookup(string nameToRemove)
		{
			if (parent != null) parent.childNodeLookup.Remove(nameToRemove);
		}

		private string GetName()
		{
			if (Site == null)
			{
				return nodeName;
			}
			return Site.Name;
		}
		

		private void UpdateHierarchyRecursive(IConfigurationUIHierarchy hierarchyToUpdate, ConfigurationNode node)
		{
			for (int idx = 0; idx < node.ChildCount; ++idx)
			{
				if (node.childNodes[idx].Hierarchy == null || node.childNodes[idx].Hierarchy.Id != hierarchyToUpdate.Id)
				{
					//node.childNodes[index].Hierarchy = hierarchy;
					hierarchyToUpdate.AddNode(node.childNodes[idx]);
				}				
				UpdateHierarchyRecursive(hierarchyToUpdate, node.childNodes[idx]);
			}
		}

		private void GetFullPath(StringBuilder path, char pathSeparator)
		{
			if (parent != null)
			{
				parent.GetFullPath(path, pathSeparator);
				path.Append(pathSeparator);
			}
			path.Append(this.Name);
		}
	}
}
