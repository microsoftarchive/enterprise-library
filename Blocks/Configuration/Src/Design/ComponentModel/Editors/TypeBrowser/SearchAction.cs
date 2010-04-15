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
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors
{
    /// <summary>
    /// Finds and updates nodes containing classes that match the supplied string.
    /// </summary>
    public class SearchAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchAction"/> class.
        /// </summary>
        /// <param name="searchText">The text to search.</param>
        /// <param name="nodes">The tree of nodes representing the assemblies, namespaces and classes to match.</param>
        public SearchAction(string searchText, IEnumerable<AssemblyGroupNode> nodes)
        {
            this.searchText = searchText;
            this.nodes = nodes;
        }

        /// <summary>
        /// Event raised whent the search has completed.
        /// </summary>
        public event EventHandler<SearchActionEventArgs> Completed;

        private readonly string searchText;
        private readonly IEnumerable<AssemblyGroupNode> nodes;
        private DispatcherOperation dispatcherOperation;

        private static readonly Func<TypeNode, string, bool> MatchTypeName = (t, s) =>
            t.DisplayName.StartsWith(s, StringComparison.OrdinalIgnoreCase);

        private static readonly Func<TypeNode, string, bool> MatchFullTypeName = (t, s) =>
            t.FullName.StartsWith(s, StringComparison.OrdinalIgnoreCase)
            || t.FullName.IndexOf(s, StringComparison.OrdinalIgnoreCase) != -1;

        /// <summary>
        /// Aborts the execution of the search.
        /// </summary>
        /// <returns><see langword="true"/>.</returns>
        public bool Abort()
        {
            if (this.dispatcherOperation != null)
            {
                return this.dispatcherOperation.Abort();
            }
            return true;
        }

        /// <summary>
        /// Schedules the execution of the search action.
        /// </summary>
        public void Run()
        {
            this.dispatcherOperation = Dispatcher.CurrentDispatcher.BeginInvoke(new Func<TypeNode>(this.OnRun));
            this.dispatcherOperation.Completed += new EventHandler(this.OnCompleted);
        }

        private TypeNode OnRun()
        {
            bool noText = false;
            Func<TypeNode, string, bool> match = null;
            if (!string.IsNullOrEmpty(this.searchText))
            {
                match = this.searchText.Contains('.') ? MatchFullTypeName : MatchTypeName;
            }
            else
            {
                noText = true;
            }
            TypeNode bestMatchingNode = null;
            TypeNode firstMatchingNode = null;
            bool multipleMatchingNodes = false;

            foreach (var assemblyGroupNode in this.nodes)
            {
                foreach (var assemblyNode in assemblyGroupNode.Assemblies)
                {
                    Visibility assemblyNodeVisibility = Visibility.Collapsed;
                    bool matchingTypesInAssembly = false;
                    if (noText && assemblyNode.Namespaces.Any())
                    {
                        assemblyNodeVisibility = Visibility.Visible;
                    }
                    foreach (var namespaceNode in assemblyNode.Namespaces)
                    {
                        Visibility namespaceNodeVisibility = Visibility.Collapsed;
                        bool matchingTypesInNamespace = false;

                        bool namespaceMatches = false;

                        if (noText)
                        {
                            namespaceNodeVisibility = Visibility.Visible;
                        }
                        else
                        {
                            // check whether the namespace itself is a match
                            namespaceMatches =
                                match == MatchFullTypeName
                                && namespaceNode.DisplayName.StartsWith(this.searchText, StringComparison.OrdinalIgnoreCase);
                        }
                        foreach (var typeNode in namespaceNode.Types)
                        {
                            if (noText)
                            {
                                // all nodes are visible if there is no text
                                typeNode.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                if (namespaceMatches || match(typeNode, this.searchText))
                                {
                                    typeNode.Visibility = Visibility.Visible;
                                    assemblyNodeVisibility = Visibility.Visible;
                                    matchingTypesInAssembly = true;
                                    namespaceNodeVisibility = Visibility.Visible;
                                    matchingTypesInNamespace = true;
                                    if (string.Equals(this.searchText, typeNode.FullName, StringComparison.OrdinalIgnoreCase))
                                    {
                                        bestMatchingNode = typeNode;
                                    }
                                    if (firstMatchingNode == null)
                                    {
                                        firstMatchingNode = typeNode;
                                    }
                                    else if (!multipleMatchingNodes)
                                    {
                                        multipleMatchingNodes = true;
                                    }
                                    continue;
                                }

                                typeNode.Visibility = Visibility.Collapsed;
                            }
                        }
                        if (namespaceMatches)
                        {
                            // override collapse status to favor namespaces over individual types
                            matchingTypesInNamespace = false;
                        }

                        namespaceNode.Visibility = namespaceNodeVisibility;
                        namespaceNode.IsExpanded = matchingTypesInNamespace;
                    }
                    assemblyNode.Visibility = assemblyNodeVisibility;
                    assemblyNode.IsExpanded = matchingTypesInAssembly;
                }
            }
            if ((bestMatchingNode == null) && !multipleMatchingNodes)
            {
                bestMatchingNode = firstMatchingNode;
            }
            return bestMatchingNode;
        }

        private void OnCompleted(object sender, EventArgs args)
        {
            this.dispatcherOperation.Completed -= new EventHandler(this.OnCompleted);
            if (this.Completed != null)
            {
                this.Completed(this, new SearchActionEventArgs((TypeNode)this.dispatcherOperation.Result));
            }
        }
    }

    /// <summary>
    /// Event args indicating the completion of a search action.
    /// </summary>
    public class SearchActionEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchActionEventArgs"/> with a selected node.
        /// </summary>
        /// <param name="result">The node that should be selected by default as the result of the search 
        /// action, or <see langword="null"/> if there is no such node.</param>
        public SearchActionEventArgs(TypeNode result)
        {
            this.Result = result;
        }

        /// <summary>
        /// Gets the node that should be selected by default as the result of the search action.
        /// </summary>
        public TypeNode Result { get; private set; }
    }
}
