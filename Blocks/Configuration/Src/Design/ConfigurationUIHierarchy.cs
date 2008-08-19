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
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using System.Reflection;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	/// <summary>
	/// Provides hierarchy management for each configuration application.
	/// </summary>
    public sealed class ConfigurationUIHierarchy : IConfigurationUIHierarchy, IContainer
    {
        private static object hierarchySaved = new object();

		private int siteCount;
		private ISite[] sites;
		private ConfigurationApplicationNode rootNode;
		private ConfigurationNode selectedNode;
		private Dictionary<Guid, ConfigurationNode> nodesById;
		private Dictionary<Guid, Dictionary<string, ConfigurationNode>> nodesByName;
        private bool loaded;
        private IServiceProvider serviceProvider;        
		private ConfigurationDesignManagerDomain configDomain;		
        private EventHandlerList handlerList;        
		private Dictionary<Guid, NodeTypeEntryArrayList> nodesByType;
		private StorageService storageSerivce;
		private object syncObj = new Object();
        private IConfigurationSource configurationSource;
        private IConfigurationParameter configurationParameter;

		/// <summary>
		/// Occurs after the hierarchy is saved.
		/// </summary>
		public event EventHandler<HierarchySavedEventArgs> Saved
        {
            add { handlerList.AddHandler(hierarchySaved, value); }
            remove { handlerList.RemoveHandler(hierarchySaved, value); }
        }        
        
		/// <summary>
		/// Initialize a new instance of the <see cref="ConfigurationUIHierarchy"/> class.
		/// </summary>
		/// <param name="rootNode">The root node of the hierarchy.</param>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		public ConfigurationUIHierarchy(ConfigurationApplicationNode rootNode, IServiceProvider serviceProvider)
        {
			if (rootNode == null) throw new ArgumentNullException("rootNode");
            if (serviceProvider == null) throw new ArgumentNullException("serviceProvider");

			this.storageSerivce = new StorageService();			
			this.configDomain = new ConfigurationDesignManagerDomain(serviceProvider);			
            this.serviceProvider = serviceProvider;                        
            nodesByType = new Dictionary<Guid,NodeTypeEntryArrayList>();
			nodesById = new Dictionary<Guid, ConfigurationNode>();
			nodesByName = new Dictionary<Guid, Dictionary<string, ConfigurationNode>>();
            handlerList = new EventHandlerList();
            this.rootNode = rootNode;
			this.storageSerivce.ConfigurationFile = this.rootNode.ConfigurationFile;
			this.rootNode.Renamed += new EventHandler<ConfigurationNodeChangedEventArgs>(OnConfigurationFileChanged);
            selectedNode = rootNode;
			AddNode(rootNode);
			if (null != rootNode.FirstNode) rootNode.UpdateHierarchy(rootNode.FirstNode); 
        }

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="ConfigurationUIHierarchy"/> and optionally releases the managed resources.
		/// </summary>
        public void Dispose()
        {
			if (handlerList != null) handlerList.Dispose();
			lock (syncObj)
			{
				while (siteCount > 0)
				{
					ISite site = sites[--siteCount];
					site.Component.Site = null;
					site.Component.Dispose();
				}
				sites = null;
				handlerList = null;
				nodesByName = null;
				nodesById = null;
				nodesByType = null;
				rootNode = null;
				selectedNode = null;
			}			
            GC.SuppressFinalize(this);
        }
		
		/// <summary>
		/// Gets a unique id for the hierarchy.
		/// </summary>
		/// <value>
		/// A unique id for the hierarchy.
		/// </value>
        public Guid Id
        {
            get
            {
                if (rootNode != null)
                {
                    return rootNode.Id;
                }
                return Guid.Empty;
            }
        }

		/// <summary>
		/// Gets the root node for the hierarchy.
		/// </summary>
		/// <value>
		/// The root node for the hierarchy.
		/// </value>
		public ConfigurationApplicationNode RootNode
        {
            get { return rootNode; }			
        }

		/// <summary>
		/// Gets or sets the currently selected node in the hierarchy.
		/// </summary>
		/// <value>
		/// The currently selected node in the hierarchy
		/// </value>
		public ConfigurationNode SelectedNode
        {
            get { return selectedNode; }
            set { selectedNode = value; }
        }

		/// <summary>
		/// Gets the <see cref="IStorageService"/> for the current hierarchy.
		/// </summary>
		/// <value>
		/// The <see cref="IStorageService"/> for the current hierarchy.
		/// </value>
		public IStorageService StorageService
		{
			get { return storageSerivce; }
		}

		/// <summary>
		/// Gets the <see cref="IConfigurationSource"/> for the current hierarchy.
		/// </summary>
		/// <value>
		/// The <see cref="IConfigurationSource"/> for the current hierarchy.
		/// </value>
		public IConfigurationSource ConfigurationSource
		{
			get
			{
                if (configurationSource != null) return configurationSource;

				ConfigurationSourceSectionNode node = GetConfigurationSourceNode();
                if (null == node || node.ConfigurationSource == null)
                {
                    FileConfigurationSource.ResetImplementation(rootNode.ConfigurationFile, false);
                    return new FileConfigurationSource(rootNode.ConfigurationFile);
                }
				return node.ConfigurationSource;
			}
            set
            {
                configurationSource = value;
            }
		}

		/// <summary>
		/// Gets the <see cref="IConfigurationParameter"/> for the current hierarchy.
		/// </summary>
		/// <value>
		/// the <see cref="IConfigurationParameter"/> for the current hierarchy.
		/// </value>
		public IConfigurationParameter ConfigurationParameter
		{
			get 
			{
                if (configurationParameter != null) return configurationParameter;

				ConfigurationSourceSectionNode node = GetConfigurationSourceNode();
                if (null == node || node.ConfigurationSource == null) return new FileConfigurationParameter(rootNode.ConfigurationFile);
				return node.ConfigurationParameter;
			}
            set
            {
                configurationParameter = value;
            }
		}

		/// <summary>
		/// Finds a node via it's path.
		/// </summary>
		/// <param name="path">
		/// The path to the node.
		/// </param>
		/// <returns>
		/// The node if found or <see langword="null"/> if not found.
		/// </returns>
		/// <remarks>
		/// Use the <seealso cref="ConfigurationNode.Path"/> property get the path to the node.
		/// </remarks>
		public ConfigurationNode FindNodeByPath(string path)
        {
            return innerFindNodeByPath(RootNode, path);
        }

        private ConfigurationNode innerFindNodeByPath(ConfigurationNode contextNode, string path)
        {
            if (contextNode.Path.Equals(path))
            {
                return contextNode;
            }

            if (path.StartsWith(contextNode.Path))
            {
                foreach (ConfigurationNode node in contextNode.Nodes)
                {
                    ConfigurationNode foundNode = innerFindNodeByPath(node, path);
                    if (foundNode != null)
                    {
                        return foundNode;
                    }
                }
            }
            return null;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configurationNodeId"></param>
        /// <returns></returns>
        public ConfigurationNode FindNodeById(Guid configurationNodeId)
        {
            ConfigurationNode node;
            nodesById.TryGetValue(configurationNodeId, out node);

            return node;
        }

		/// <summary>
		/// Finds nodes by their <see cref="Type"/>.
		/// </summary>
		/// <param name="type">
		/// The <see cref="Type"/> of the node.
		/// </param>
		/// <returns>
		/// The nodes found.
		/// </returns>
		public IList<ConfigurationNode> FindNodesByType(Type type)
        {
			return FindNodesByType(null, type);
        }

		/// <summary>
		/// Finds nodes by their <see cref="Type"/>.
		/// </summary>
		/// <param name="parent">
		/// The parent to start the search.
		/// </param>
		/// <param name="type">
		/// The <see cref="Type"/> of the node.
		/// </param>
		/// <returns>
		/// The nodes found.
		/// </returns>
		public IList<ConfigurationNode> FindNodesByType(ConfigurationNode parent, Type type)
        {
            if (null == type) throw new ArgumentNullException("type");

			List<ConfigurationNode> nodes = new List<ConfigurationNode>();
            if (parent == null && type == rootNode.GetType())
            {
                nodes.Add(rootNode);
            }
            DoSearch(parent, type, nodes);
			return nodes;
        }

		/// <summary>
		/// Finds a node by it's <see cref="Type"/>.
		/// </summary>
		/// <param name="type">
		/// The <see cref="Type"/> of the node.
		/// </param>
		/// <returns>
		/// The node if found or <see langword="null"/> if not found.
		/// </returns>
		/// <remarks>
		/// If this is more than one type in the hierachy, this function will only find the first one found.
		/// </remarks>
		public ConfigurationNode FindNodeByType(Type type)
        {
            return FindNodeByType(null, type);
        }

		/// <summary>
		/// Finds nodes by their <see cref="Type"/>.
		/// </summary>
		/// <param name="parent">
		/// The parent to start the search.
		/// </param>
		/// <param name="type">
		/// The <see cref="Type"/> of the node.
		/// </param>
		/// <returns>
		/// The node if found or <see langword="null"/> if not found.
		/// </returns>
		/// <remarks>
		/// If this is more than one type in the hierachy, this function will only find the first one found.
		/// </remarks>
		public ConfigurationNode FindNodeByType(ConfigurationNode parent, Type type)
        {
			IList<ConfigurationNode> nodes = FindNodesByType(parent, type);
            if (nodes.Count > 0)
            {
                return nodes[0];
            }
            return null;
        }

		/// <summary>
		/// Finds nodes by their <seealso cref="ConfigurationNode.Name"/>.
		/// </summary>
		/// <param name="parent">
		/// The parent to start the search.
		/// </param>
		/// <param name="name">
		/// The name of the node.
		/// </param>
		/// <returns>
		/// The node if found or <see langword="null"/> if not found.
		/// </returns>
		public ConfigurationNode FindNodeByName(ConfigurationNode parent, string name)
        {
            if (null == parent) throw new ArgumentNullException("parent");

            if (!nodesByName.ContainsKey(parent.Id))
            {
                return null;
            }
			Dictionary<string, ConfigurationNode> childs = nodesByName[parent.Id];
			if (childs.ContainsKey(name))
				return childs[name];
			foreach (ConfigurationNode childNode in childs.Values)
			{
				ConfigurationNode node = FindNodeByName(childNode, name);
				if (null != node) return node;
			}			
			return null;
        }

		/// <summary>
		/// Determines if a node type exists in the hierarchy.
		/// </summary>
		/// <param name="nodeType">
		/// The <see cref="Type"/> of the node to find.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the type is contained in the hierarchy; otherwise <see langword="false"/>.
		/// </returns>
        public bool ContainsNodeType(Type nodeType)
        {
			ConfigurationNode node = FindNodeByType(nodeType);
            return node != null;
        }

		/// <summary>
		/// Determines if a node type exists in the hierarchy.
		/// </summary>
		/// <param name="parent">
		/// The parent to start the search.
		/// </param>
		/// <param name="nodeType">
		/// The <see cref="Type"/> of the node to find.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the type is contained in the hierarchy; otherwise <see langword="false"/>.
		/// </returns>
		public bool ContainsNodeType(ConfigurationNode parent, Type nodeType)
        {
			ConfigurationNode node = FindNodeByType(parent, nodeType);
            return node != null;
        }

		/// <summary>
		/// Add a node to the hierarchy.
		/// </summary>
		/// <param name="node">
		/// The node to add to the hierarchy.
		/// </param>
		public void AddNode(ConfigurationNode node)
        {
            if (node == null) throw new ArgumentNullException("node");
			if (nodesById.ContainsKey(node.Id)) return; // we already own this one

            SetUniqueComponentName(node);
			AddSite(node, node.Name);			
            node.Hierarchy = this;
            string nodeTypeName = node.GetType().FullName;
            if (!nodesByType.ContainsKey(node.Id))
            {
                nodesByType[node.Id] = new NodeTypeEntryArrayList();
            }
            if (node.Parent != null && nodesByType.ContainsKey(node.Parent.Id))
            {
                NodeTypeEntryArrayList childTypes = nodesByType[node.Parent.Id];
                NodeTypeEntry entry = new NodeTypeEntry(node, nodeTypeName);
                if (!childTypes.Contains(entry))
                {
                    childTypes.Add(entry);
                    AddBaseTypes(node, childTypes);
                }
            }
			nodesById[node.Id] = node;
			AddNodeByName(node);
        }

		/// <summary>
		/// Remove a node from the hierarchy.
		/// </summary>
		/// <param name="node">
		/// The node to remove.
		/// </param>
		public void RemoveNode(ConfigurationNode node)
        {
			string nodeTypeName = node.GetType().FullName;
            nodesByType.Remove(node.Id);
            if (node.Parent != null)
            {
                NodeTypeEntryArrayList childTypes = nodesByType[node.Parent.Id];
                NodeTypeEntry entryToRemove = new NodeTypeEntry(node, nodeTypeName);
                if (childTypes.Contains(entryToRemove))
                {
                    childTypes.Remove(entryToRemove);
                    RemoveBaseTypes(node, childTypes);
                }
            }
            node.Hierarchy = null;
            nodesById.Remove(node.Id);
            RemoveNodeByName(node);
            if (node.Id == rootNode.Id) rootNode = null;
        }

		/// <summary>
		/// Renames a node in the hierarchy.
		/// </summary>
		/// <param name="node">
		/// The node to be renamed.
		/// </param>
		/// <param name="oldName">
		/// The old name.
		/// </param>
		/// <param name="newName">
		/// The new name.
		/// </param>
		public void RenameNode(ConfigurationNode node, string oldName, string newName)
        {
            if (node.Parent == null)
            {
                return;
            }

            if (!nodesByName.ContainsKey(node.Parent.Id))
            {
                return;

            }
			Dictionary<string, ConfigurationNode> childNodes = nodesByName[node.Parent.Id];
            childNodes.Remove(oldName);
            childNodes.Add(newName, node);
        }		

		/// <summary>
		/// Save the application and all it's configuration.
		/// </summary>
        public void Save()
        {
			this.configDomain.Save();
			IErrorLogService errorLogService = ServiceHelper.GetErrorService(serviceProvider);			
			if (errorLogService.ConfigurationErrorCount > 0) return;
            OnSaved(new HierarchySavedEventArgs(this));
        }


		/// <summary>
		/// Opens the application and loads it's configuration.
		/// </summary>
        public void Open()
        {
            if (!loaded)
            {
                Load();
            }
			rootNode.ClearChildNodes();
			configDomain.Open();			
        }

		/// <summary>
		/// Loads all available configuration available for the application.
		/// </summary>
        public void Load()
        {
			if (!loaded)
			{
				this.configDomain.Load();
			}
			this.configDomain.Register();
            loaded = true;
        }

		/// <summary>
		/// Builds an <see cref="IConfigurationSource"/> from the configuration settings of the current application configuration node.
		/// </summary>
		/// <returns>The <see cref="IConfigurationSource"/> based on the current application..</returns>
		public IConfigurationSource BuildConfigurationSource()
		{
			return configDomain.BuildConfigurationSource();						
		}

		/// <summary>
		/// Gthe requested service.
		/// </summary>
		/// <param name="serviceType">The type of service to retrieve.</param>
		/// <returns>An instance of the service if it could be found, or a null reference (Nothing in Visual Basic) if it could not be found.</returns>
		public object GetService(Type serviceType)
        {
			if (serviceType == typeof(Container))
			{
				return this;
			}
            return serviceProvider.GetService(serviceType);
        }

		/// <summary>
		/// Adds the specified <see cref="IComponent"/> to the <see cref="IContainer"/>, and assigns a name to the component. 
		/// </summary>
		/// <param name="component">The <see cref="IComponent"/> to add.</param>
		/// <param name="name">The name to assign to the <see cref="IComponent"/>.</param>
		void IContainer.Add(IComponent component, string name)
		{
			ConfigurationNode node = (ConfigurationNode)component;
			node.Name = name;
			AddNode(node);			
		}

		/// <summary>
		/// Adds the specified <see cref="IComponent"/> to the <see cref="IContainer"/>.
		/// </summary>
		/// <param name="component">The <see cref="IComponent"/> to add.</param>
		void IContainer.Add(IComponent component)
		{
			AddNode((ConfigurationNode)component);
		}		

		/// <summary>
		/// Gets all the components in the <see cref="IContainer"/>.
		/// </summary>
		/// <value>
		/// A collection of <see cref="IComponent"/> objects that represents all the components in the <see cref="IContainer"/>.
		/// </value>
		ComponentCollection IContainer.Components
		{
			get
			{
				IComponent[] compoenents = new IComponent[nodesById.Values.Count];
				int index = 0;
				foreach (ConfigurationNode node in nodesById.Values)
				{
					compoenents[index++] = (IComponent)node;
				}
				return new ComponentCollection(compoenents);
			}
		}

		/// <summary>
		/// Removes a component from the <see cref="IContainer"/>. 
		/// </summary>
		/// <param name="component">The <see cref="IComponent"/> to remove.</param>
		void IContainer.Remove(IComponent component)
		{			
			RemoveNode((ConfigurationNode)component);
		}

		private void OnConfigurationFileChanged(object sender, ConfigurationNodeChangedEventArgs e)
		{
			storageSerivce.ConfigurationFile = ((ConfigurationApplicationNode)e.Node).ConfigurationFile;
		}

		private ConfigurationSourceSectionNode GetConfigurationSourceNode()
		{
			ConfigurationSourceSectionNode node = FindNodeByType(typeof(ConfigurationSourceSectionNode)) as ConfigurationSourceSectionNode;
			return node;
		}


		private void AddSite(ConfigurationNode node, string name)
		{
			if (node.Site != null) return;

			if (sites == null)
			{
				sites = new ISite[4];
			}
			else
			{
				if (sites.Length == siteCount)
				{
					ISite[] newSites = new ISite[siteCount * 2];
					Array.Copy(sites, 0, newSites, 0, siteCount);
					sites = newSites;
				}
			}

			ISite nodeSite = new Site(this, node, name);
			sites[siteCount++] = nodeSite;
			node.Site = nodeSite;						
		}

		private void SetUniqueComponentName(ConfigurationNode node)
		{	
			INodeNameCreationService service = ServiceHelper.GetNameCreationService(serviceProvider);
            string nameToUse = node.Name;
            if (string.IsNullOrEmpty(nameToUse)) nameToUse = node.GetType().Name;			
			ConfigurationNode parent = GetParentToBeUsed(node);
            string newName = service.GetUniqueName(nameToUse, node, parent);
            if (node.Name != newName)
            {
                node.Rename(newName, false);
            }
		}

		private ConfigurationNode GetParentToBeUsed(ConfigurationNode node)
		{			
			PropertyInfo info = node.GetType().GetProperty("Name");
			object [] attributes = info.GetCustomAttributes(typeof(UniqueNameAttribute), true);
			if (attributes.Length == 0) return node.Parent;			
			UniqueNameAttribute attr = (UniqueNameAttribute)attributes[0];
			return FindNodeByType(rootNode, attr.ContainerType);
		}

		

		private void DoSearch(ConfigurationNode parent, Type type, List<ConfigurationNode> nodes)
        {
            if (parent == null) parent = rootNode; //return;
			ConfigurationNode searchNode = parent;
            SearchNodeForType(searchNode, type, nodes);
			foreach (ConfigurationNode childNode in searchNode.Nodes)
            {
                DoSearch(childNode, type, nodes);
            }
            searchNode = searchNode.NextSibling;
        }

		private void SearchNodeForType(ConfigurationNode searchNode, Type type, List<ConfigurationNode> nodes)
        {
			ConfigurationNode[] childNodes;
            NodeTypeEntryArrayList childNodesByType = nodesByType[searchNode.Id];
            if (childNodesByType == null) return;
            childNodes = childNodesByType.FindAllEntryTypeMatchs(type.FullName);
			foreach (ConfigurationNode node in childNodes)
            {
                // since the same instance can be in multiple places... we have to make sure
                // we don't have it already
                if (!nodes.Contains(node))
                {
                    nodes.Add(node);
                }

            }
        }

		private void AddNodeByName(ConfigurationNode node)
        {
            if (node.Parent == null)
            {
                return;
            }
			Dictionary<string, ConfigurationNode> childNodes = null;
            if (!nodesByName.ContainsKey(node.Parent.Id))
            {
				childNodes = new Dictionary<string, ConfigurationNode>();
                nodesByName[node.Parent.Id] = childNodes;
            }
            else
            {
                childNodes = nodesByName[node.Parent.Id];
            }            
            childNodes.Add(node.Name, node);
        }

		private void RemoveNodeByName(ConfigurationNode node)
        {
				

            if (node.Parent == null)
            {
                return;
            }

            if (!nodesByName.ContainsKey(node.Parent.Id))
            {
                return;
            }

			if (nodesByName.ContainsKey(node.Id))
			{
				nodesByName.Remove(node.Id);
			}

			Dictionary<string, ConfigurationNode> childNodes = nodesByName[node.Parent.Id];
            childNodes.Remove(node.Name);
        }

		private static void AddBaseTypes(ConfigurationNode node, ArrayList tableToAddTypes)
        {
            Type searchType = node.GetType().BaseType;
			while (searchType != null && searchType != typeof(ConfigurationNode))
            {
                tableToAddTypes.Add(new NodeTypeEntry(node, searchType.FullName));
                searchType = searchType.BaseType;
            }
        }

		private static void RemoveBaseTypes(ConfigurationNode node, NodeTypeEntryArrayList tableToAddTypes)
        {
            Type searchType = node.GetType().BaseType;
			while (searchType != null)
            {
                NodeTypeEntry entry = new NodeTypeEntry(node, searchType.FullName);
                tableToAddTypes.Remove(entry);
                searchType = searchType.BaseType;
            }
        }		

        private void OnSaved(HierarchySavedEventArgs e)
        {
			EventHandler<HierarchySavedEventArgs> handler = (EventHandler<HierarchySavedEventArgs>)handlerList[hierarchySaved];
            if (handler != null)
            {
                handler(this, e);
            }
        }

		private class Site : ISite
		{
			private readonly IComponent component;
			private readonly ConfigurationUIHierarchy container;
			private string name;

			public Site(ConfigurationUIHierarchy container, IComponent component, string name)
			{
				this.component = component;
				this.container = container;
				this.name = name;
			}			

			public IComponent Component
			{
				get { return component; }
			}

			public IContainer Container
			{
				get { return container; }
			}

			public bool DesignMode
			{
				get { return false; }
			}

			public string Name
			{
				get { return name; }
				set { name = value; }
			}

			public object GetService(Type serviceType)
			{
				if (serviceType != typeof(ISite))
				{
					return container.GetService(serviceType);
				}
				return this;
			}
		}

        private class NodeTypeEntryArrayList : ArrayList
        {
            public override bool Contains(object item)
            {
                NodeTypeEntry nodeTypeEntry = (NodeTypeEntry)item;
                foreach (NodeTypeEntry entry in this)
                {
                    if (nodeTypeEntry == entry)
                    {
                        return true;
                    }
                }
                return false;
            }

            public ConfigurationNode[] FindAllEntryTypeMatchs(string typeName)
            {
                ArrayList list = new ArrayList(this.Count);
                foreach (NodeTypeEntry entry in this)
                {
                    if (entry.TypeName == typeName)
                    {
                        list.Add(entry.Node);
                    }
                }
				return (ConfigurationNode[])list.ToArray(typeof(ConfigurationNode));
            }
        }

        private class NodeTypeEntry
        {
			private readonly ConfigurationNode node;
            private readonly string typeName;

			public NodeTypeEntry(ConfigurationNode node, string typeName)
            {
                this.node = node;
                this.typeName = typeName;
            }

			public static bool operator ==(NodeTypeEntry lhs, NodeTypeEntry rhs)
			{
				return lhs.Equals(rhs);
			}

			public static bool operator !=(NodeTypeEntry lhs, NodeTypeEntry rhs)
			{
				return !lhs.Equals(rhs);
			}

			public ConfigurationNode Node
            {
                get { return node; }
            }

            public string TypeName
            {
                get { return typeName; }
            }

			public override bool Equals(object obj)
			{
				NodeTypeEntry entry = (NodeTypeEntry)obj;
				return node.Id == entry.node.Id && typeName == entry.typeName;
			}

			public override int GetHashCode()
			{
				return node.Id.GetHashCode() ^ typeName.GetHashCode();
			}
        }		
	}
}