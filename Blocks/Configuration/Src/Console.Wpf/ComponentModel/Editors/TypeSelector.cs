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
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;


namespace Console.Wpf.ComponentModel.Editors
{
    /// <summary>
    /// Represents a selector for types
    /// </summary>
    public class TypeSelector
    {
        readonly Type baseType_;
        readonly Type configurationType;
        readonly Type currentType;
        TreeNode currentTypeTreeNode;
        readonly TypeSelectorIncludes flags;
        readonly bool includeAbstractTypes;
        readonly bool includeAllInterfaces;
        readonly bool includeBaseType;
        readonly bool includeNonPublicTypes;
        readonly Hashtable loadedAssemblies;
        TreeNode rootNode;
        readonly TreeView treeView;
        private string nameFilter;

        /// <summary>
        /// Initialize a new instance of the <see cref="TypeSelector"/> class with the current type, the base type
        /// and a <see cref="TreeView"/> to display the types.
        /// </summary>
        /// <param name="currentType">The current type.</param>
        /// <param name="baseType">The base type for the current type.</param>
        /// <param name="treeView">A <see cref="TreeView"/> to display the types.</param>
        public TypeSelector(Type currentType,
                            Type baseType,
                            TreeView treeView)
            : this(currentType, baseType, TypeSelectorIncludes.None, null, treeView) { }

        /// <summary>
        /// Initialize a new instance of the <see cref="TypeSelector"/> class with the current type, the base type
        /// a flag for what types to include, and a <see cref="TreeView"/> to display the types.
        /// </summary>
        /// <param name="currentType">The current type.</param>
        /// <param name="baseType">The base type for the current type.</param>
        /// <param name="flags">One of the <see cref="TypeSelectorIncludes"/> values.</param>
        /// <param name="treeView">A <see cref="TreeView"/> to display the types.</param>
        public TypeSelector(Type currentType,
                            Type baseType,
                            TypeSelectorIncludes flags,
                            TreeView treeView)
            : this(currentType, baseType, flags, null, treeView) { }

        /// <summary>
        /// Initialize a new instance of the <see cref="TypeSelector"/> class with the current type, the base type
        /// a flag for what types to include, a configuration type and a <see cref="TreeView"/> to display the types.
        /// </summary>
        /// <param name="currentType">The current type.</param>
        /// <param name="baseType">The base type for the current type.</param>
        /// <param name="flags">One of the <see cref="TypeSelectorIncludes"/> values.</param>
        /// <param name="configurationType">The configurtion type.</param>
        /// <param name="treeView">A <see cref="TreeView"/> to display the types.</param>
        public TypeSelector(Type currentType,
                            Type baseType,
                            TypeSelectorIncludes flags,
                            Type configurationType,
                            TreeView treeView)
        {
            if (treeView == null)
            {
                throw new ArgumentNullException("treeView");
            }
            if (baseType == null)
            {
                throw new ArgumentNullException("baseType");
            }
            loadedAssemblies = new Hashtable();
            this.configurationType = configurationType;
            this.treeView = treeView;
            this.currentType = currentType;
            baseType_ = baseType;
            this.flags = flags;
            includeAbstractTypes = IsSet(TypeSelectorIncludes.AbstractTypes);
            includeAllInterfaces = IsSet(TypeSelectorIncludes.Interfaces);
            includeNonPublicTypes = IsSet(TypeSelectorIncludes.NonpublicTypes);
            includeBaseType = IsSet(TypeSelectorIncludes.BaseType);
            this.nameFilter = string.Empty;

            LoadTypes(baseType);
        }

        /// <summary>
        /// The root node of the tree.
        /// </summary>
        public TreeNode AssembliesRootNode
        {
            get { return rootNode; }
        }

        /// <summary>
        /// The type to verify.
        /// </summary>
        public Type TypeToVerify
        {
            get { return baseType_; }
        }

        bool AddTypesToTreeView(TreeNodeTable nodeTable,
                                Assembly assembly)
        {
            if (nodeTable.AssemblyNode.Nodes.Count > 0)
            {
                rootNode.Nodes.Add(nodeTable.AssemblyNode);
                rootNode.ExpandAll();
                loadedAssemblies[assembly.FullName] = 1;
                return true;
            }
            else
            {
                return false;
            }
        }

        bool IsSet(TypeSelectorIncludes compareFlag)
        {
            return ((flags & compareFlag) == compareFlag);
        }

        /// <summary>
        /// Determines if a type is valid.
        /// </summary>
        /// <param name="selectedType">The selected type.</param>
        /// <returns>true if the type is valid; otherwise, false.</returns>
        public bool IsTypeValid(Type selectedType)
        {
            bool valid;

            if (includeAllInterfaces && selectedType.IsInterface)
            {
                valid = true;
            }
            else if (selectedType == baseType_)
            {
                valid = includeBaseType;
            }
            else
            {
                valid = baseType_.IsAssignableFrom(selectedType);

                if (valid)
                {
                    if ((!includeAbstractTypes) && (selectedType.IsAbstract || selectedType.IsInterface))
                    {
                        valid = false;
                    }
                }

                if (valid)
                {
                    if (!(selectedType.IsPublic) && !(selectedType.IsNestedPublic) && (!includeNonPublicTypes))
                    {
                        valid = false;
                    }
                }

                if (valid && configurationType != null)
                {
                    object[] configurationElementTypeAttributes = selectedType.GetCustomAttributes(typeof(ConfigurationElementTypeAttribute), true);
                    if (configurationElementTypeAttributes.Length == 0)
                    {
                        valid = false;
                    }
                    else
                    {
                        ConfigurationElementTypeAttribute configElementTypeAttribute = (ConfigurationElementTypeAttribute)configurationElementTypeAttributes[0];
                        if (configurationType != configElementTypeAttribute.ConfigurationType)
                        {
                            valid = false;
                        }
                    }
                }
            }
            return valid;
        }

        /// <summary>
        /// Loads the tree based on an assembly.
        /// </summary>
        /// <param name="assembly">The assembly to use to load the tree.</param>
        /// <returns>true if the tree contains the assembly; otherwise false.</returns>
        public bool LoadTreeView(Assembly assembly)
        {
            if (loadedAssemblies.Contains(assembly.FullName))
            {
                return true;
            }

            TreeNodeTable nodeTable = new TreeNodeTable(assembly);
            Type[] types = LoadTypesFromAssembly(assembly);
            if (types == null)
            {
                return false;
            }

            LoadValidTypes(types, nodeTable);

            return AddTypesToTreeView(nodeTable, assembly);
        }

        /// <summary>
        /// Loads the tree based on an assembly, filtering the types based on the last known filter.
        /// </summary>
        /// <param name="assembly">The assembly to use to load the tree.</param>
        /// <returns>true if the tree contains the assembly; otherwise false.</returns>
        public bool LoadFilteredTreeView(Assembly assembly)
        {
            bool hasValidTypes = LoadTreeView(assembly);

            FilterCurrentNodes(this.rootNode, this.nameFilter);

            return hasValidTypes;
        }

        void LoadTypes(Type baseType)
        {
            TreeNode treeNode = new TreeNode(String.Empty, -1, -1);
            if (baseType.IsInterface)
            {
                if (configurationType != null)
                {
                    treeNode.Text = string.Format(CultureInfo.CurrentUICulture, Resources.TypeSelectorInterfaceRootNodeTextWithConfigurationType, baseType.FullName, configurationType.FullName);
                }
                else
                {
                    treeNode.Text = string.Format(CultureInfo.CurrentUICulture, Resources.TypeSelectorInterfaceRootNodeText, baseType.FullName);
                }
            }
            else if (baseType.IsClass)
            {
                if (configurationType != null)
                {
                    treeNode.Text = string.Format(CultureInfo.CurrentUICulture, Resources.TypeSelectorClassRootNodeTextWithConfigurationType, baseType.FullName, configurationType.FullName);
                }
                else
                {
                    treeNode.Text = string.Format(CultureInfo.CurrentUICulture, Resources.TypeSelectorClassRootNodeText, baseType.FullName);
                }
            }
            treeView.Nodes.Add(treeNode);
            rootNode = new TreeNode(Resources.AssembliesLabelText, 0, 1);
            treeNode.Nodes.Add(rootNode);
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    LoadTreeView(assembly);
                }
                catch (ReflectionTypeLoadException) { }
            }
            treeNode.ExpandAll();
            treeView.SelectedNode = currentTypeTreeNode;
        }

        static Type[] LoadTypesFromAssembly(Assembly assembly)
        {
            Type[] types;
            types = assembly.GetTypes();

            return types;
        }

        void LoadValidTypes(IEnumerable<Type> types, TreeNodeTable nodeTable)
        {
            if (types == null)
            {
                return;
            }

            foreach (Type t in types)
            {
                if (IsTypeValid(t))
                {
                    TreeNode newNode = nodeTable.AddType(t);
                    if (t == currentType)
                    {
                        currentTypeTreeNode = newNode;
                    }
                }
            }
        }

        /// <devdoc>
        /// Represents the table of tree nodes by assembly type.
        /// </devdoc>
        class TreeNodeTable
        {
            readonly TreeNode assemblyNode;
            readonly Hashtable namespaceTable;

            public TreeNodeTable(Assembly assembly)
            {
                namespaceTable = new Hashtable();
                assemblyNode = new TreeNode(assembly.GetName().Name, (int)TypeImages.Assembly, (int)TypeImages.Assembly);
            }

            public TreeNode AssemblyNode
            {
                get { return assemblyNode; }
            }

            public TreeNode AddType(Type t)
            {
                TreeNode namespaceNode = GetNamespaceNode(t);
                TreeNode typeNode = new TreeNode(t.Name, (int)TypeImages.Class, (int)TypeImages.Class);
                typeNode.Tag = t;
                namespaceNode.Nodes.Add(typeNode);
                return typeNode;
            }

            TreeNode GetNamespaceNode(Type t)
            {
                TreeNode namespaceNode;
                if (t.Namespace == null)
                {
                    namespaceNode = assemblyNode;
                }
                else if (namespaceTable.ContainsKey(t.Namespace))
                {
                    namespaceNode = (TreeNode)namespaceTable[t.Namespace];
                }
                else
                {
                    namespaceNode = new TreeNode(t.Namespace, (int)TypeImages.Namespace, (int)TypeImages.Namespace);
                    assemblyNode.Nodes.Add(namespaceNode);
                    namespaceTable.Add(t.Namespace, namespaceNode);
                }
                return namespaceNode;
            }
        }

        internal void UpdateFilter(string nameFilter)
        {
            if (!nameFilter.Contains(this.nameFilter))
            {
                // full blown refresh is required
                treeView.Nodes.Clear();
                this.loadedAssemblies.Clear();
                LoadTypes(this.baseType_);
            }
            else
            {
                // just remove non-matching types from the tree
                // the new filter is more specific than the previous one
            }

            this.nameFilter = nameFilter;
            FilterCurrentNodes(this.rootNode, nameFilter);
        }

        private static void FilterCurrentNodes(TreeNode rootNode, string nameFilter)
        {
            RemoveNonMatchingNodes(                         // assembly nodes
                RemoveNonMatchingNodes(                     // namespace nodes
                    RemoveNonMatchingNodes(                 // type nodes
                        (TreeNode typeNode)
                            => typeNode.Text.IndexOf(nameFilter, StringComparison.InvariantCultureIgnoreCase) < 0)))
                (rootNode);
        }

        private static Func<TreeNode, bool> RemoveNonMatchingNodes(Func<TreeNode, bool> childTestFunc)
        {
            return (TreeNode node) =>
                {
                    TreeNode[] childNodes = new TreeNode[node.Nodes.Count];
                    node.Nodes.CopyTo(childNodes, 0);

                    foreach (TreeNode childNode in childNodes)
                    {
                        if (childTestFunc(childNode))
                        {
                            node.Nodes.Remove(childNode);
                        }
                    }

                    return node.Nodes.Count == 0;
                };
        }

        private enum TypeImages
        {
            FolderClosed = 0,
            FolderOpen = 1,
            Assembly = 2,
            Namespace = 3,
            Class = 4
        }
    }
}
