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
using System.Collections.Specialized;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	/// <summary>
	/// Provides a service for creating nodes based on data types.
	/// </summary>
    public class NodeCreationService : INodeCreationService
    {
        private Hashtable nodeTypesByDataType;
        private Hashtable namesByNodeType;                
        
		/// <summary>
		/// Initialize a new instance of the <see cref="NodeCreationService"/> class.
		/// </summary>
        public NodeCreationService()
        {
            nodeTypesByDataType = new Hashtable();
            namesByNodeType = new Hashtable();            
        }

		/// <summary>
		/// Creates a <see cref="ConfigurationNode"/> based on the data type.
		/// </summary>
		/// <param name="dataType">
		/// The data type to base the creation upon.
		/// </param>
		/// <returns>
		/// A <see cref="ConfigurationNode"/> based on the data type or <see langword="null"/> if one does not exists.
		/// </returns>
		public ConfigurationNode CreateNodeByDataType(Type dataType)
        {
            return CreateNodeByDataType(dataType, null);
        }

		/// <summary>
		/// Creates a <see cref="ConfigurationNode"/> based on the data type.
		/// </summary>
		/// <param name="dataType">
		/// The data type to base the creation upon.
		/// </param>
		/// <param name="constructorArguments">
		/// The constructor arguments for the node.
		/// </param>
		/// <returns>
		/// A <see cref="ConfigurationNode"/> based on the data type or <see langword="null"/> if one does not exists.
		/// </returns>
        public ConfigurationNode CreateNodeByDataType(Type dataType, object[] constructorArguments)
        {
			if (null == dataType) throw new ArgumentNullException("dataType");
            
            Type nodeType = nodeTypesByDataType[dataType.FullName] as Type;
            if (nodeType == null)
            {
                return null;
            }
            return (ConfigurationNode)Activator.CreateInstance(nodeType,
                                                               BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.CreateInstance,
                                                               null,
                                                               constructorArguments,
                                                               null);
        }

		/// <summary>
		/// Creates a map between the node and the data that it represents.
		/// </summary>
		/// <param name="nodeCreationEntry">
		/// A <see cref="NodeCreationEntry"/> object.
		/// </param>
        public void AddNodeCreationEntry(NodeCreationEntry nodeCreationEntry)
        {
            if (null == nodeCreationEntry) throw new ArgumentNullException("nodeCreationEntry");
            
            string displayName = nodeCreationEntry.DisplayName;
            nodeTypesByDataType[nodeCreationEntry.DataType.FullName] = nodeCreationEntry.NodeType;

            Type baseType = nodeCreationEntry.NodeType.BaseType;    
            Type nodeTypeToStore = nodeCreationEntry.NodeType;
            while(baseType != typeof(ConfigurationNode))
            {
                nodeTypeToStore = baseType;
                baseType = baseType.BaseType;
            }            
            nodeCreationEntry.BaseTypeToCompare = nodeTypeToStore;
            AddNamesForBaseType(nodeTypeToStore.FullName, displayName);
        }

        private void AddNamesForBaseType(string baseTypeName, string displayName)
        {
            if (!namesByNodeType.Contains(baseTypeName))
            {
                namesByNodeType[baseTypeName] = new StringCollection();
            }
            StringCollection names = namesByNodeType[baseTypeName] as StringCollection;
            if (!names.Contains(displayName))
            {
                names.Add(displayName);
            }
        }
    }
}
