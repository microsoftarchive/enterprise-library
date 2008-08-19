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
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	/// <summary>
	/// Provides hierarchy management for each configuration application.
	/// </summary>
    public interface IConfigurationUIHierarchy : IDisposable
    {
		/// <summary>
		/// When implemented by a class, occurs after the hierarchy is saved.
		/// </summary>
        event EventHandler<HierarchySavedEventArgs> Saved;

		/// <summary>
		/// When implemented by a class, gets a unique id for the hierarchy.
		/// </summary>
		/// <value>
		/// A unique id for the hierarchy.
		/// </value>
        Guid Id { get; }

		/// <summary>
		/// When implemented by a class, gets the root node for the hierarchy.
		/// </summary>
		/// <value>
		/// The root node for the hierarchy.
		/// </value>
		ConfigurationApplicationNode RootNode { get; }

		/// <summary>
		/// When implemented by a class, gets or sets the currently selected node in the hierarchy.
		/// </summary>
		/// <value>
		/// The currently selected node in the hierarchy
		/// </value>
		ConfigurationNode SelectedNode { get; set; }

		/// <summary>
		/// When implemented by a class, gets the <see cref="IStorageService"/> for the current hierarchy.
		/// </summary>
		/// <value>
		/// The <see cref="IStorageService"/> for the current hierarchy.
		/// </value>
		IStorageService StorageService { get; }

		/// <summary>
		/// When implemented by a class, gets the <see cref="IConfigurationSource"/> for the current hierarchy.
		/// </summary>
		/// <value>
		/// The <see cref="IConfigurationSource"/> for the current hierarchy.
		/// </value>
        IConfigurationSource ConfigurationSource { get; set;}

		/// <summary>
		/// When implemented by a class, gets the <see cref="IConfigurationParameter"/> for the current hierarchy.
		/// </summary>
		/// <value>
		/// The <see cref="IConfigurationParameter"/> for the current hierarchy.
		/// </value>
        IConfigurationParameter ConfigurationParameter { get; set;}

		/// <summary>
		/// When implemented by a class, finds a node via it's path.
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
		ConfigurationNode FindNodeByPath(string path);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configurationNodeId"></param>
        /// <returns></returns>
        ConfigurationNode FindNodeById(Guid configurationNodeId);

		/// <summary>
		/// When implemented by a class, finds nodes by their <see cref="Type"/>.
		/// </summary>
		/// <param name="type">
		/// The <see cref="Type"/> of the node.
		/// </param>
		/// <returns>
		/// The nodes found.
		/// </returns>
		IList<ConfigurationNode> FindNodesByType(Type type);

		/// <summary>
		/// When implemented by a class, finds nodes by their <see cref="Type"/>.
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
		IList<ConfigurationNode> FindNodesByType(ConfigurationNode parent, Type type);

		/// <summary>
		/// When implemented by a class, finds a node by it's <see cref="Type"/>.
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
		ConfigurationNode FindNodeByType(Type type);

		/// <summary>
		/// When implemented by a class, finds nodes by their <see cref="Type"/>.
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
		ConfigurationNode FindNodeByType(ConfigurationNode parent, Type type);

		/// <summary>
		/// When implemented by a class, finds nodes by their <seealso cref="ConfigurationNode.Name"/>.
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
		ConfigurationNode FindNodeByName(ConfigurationNode parent, string name);

		/// <summary>
		/// When implemented by a class, determines if a node type exists in the hierarchy.
		/// </summary>
		/// <param name="nodeType">
		/// The <see cref="Type"/> of the node to find.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the type is contained in the hierarchy; otherwise <see langword="false"/>.
		/// </returns>
        bool ContainsNodeType(Type nodeType);

		/// <summary>
		/// When implemented by a class, determines if a node type exists in the hierarchy.
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
		bool ContainsNodeType(ConfigurationNode parent, Type nodeType);

		/// <summary>
		/// When implemented by a class, add a node to the hierarchy.
		/// </summary>
		/// <param name="node">
		/// The node to add to the hierarchy.
		/// </param>
		void AddNode(ConfigurationNode node);

		/// <summary>
		/// When implemented by a class, remove a node from the hierarchy.
		/// </summary>
		/// <param name="node">
		/// The node to remove.
		/// </param>
		void RemoveNode(ConfigurationNode node);

		/// <summary>
		/// When implemented by a class, renames a node in the hierarchy.
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
		void RenameNode(ConfigurationNode node, string oldName, string newName);		

		/// <summary>
		/// When implemented by a class, save the application and all it's configuration.
		/// </summary>
        void Save();

        
		/// <summary>
		/// When implemented by a class, opens the application and loads it's configuration.
		/// </summary>
        void Open();

		/// <summary>
		/// When implemented by a class, loads all available configuration available for the application.
		/// </summary>
        void Load();

		/// <summary>
		/// When implemented by a class, builds an <see cref="IConfigurationSource"/> from the configuration settings of the current application configuration node.
		/// </summary>
		/// <returns>The <see cref="IConfigurationSource"/> based on the current application..</returns>
		IConfigurationSource BuildConfigurationSource();

		/// <summary>
		/// When implemented by a class, gets the requested service.
		/// </summary>
		/// <param name="serviceType">The type of service to retrieve.</param>
		/// <returns>An instance of the service if it could be found, or a null reference (Nothing in Visual Basic) if it could not be found.</returns>
        object GetService(Type serviceType);
    }    
}