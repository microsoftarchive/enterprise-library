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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{    
    /// <summary>
    /// Provides data for the 
	/// <seealso cref="ConfigurationNode.ChildAdded"/>,
	/// <seealso cref="ConfigurationNode.ChildRemoved"/>,	
	/// <seealso cref="ConfigurationNode.Removed"/>, and
	/// <seealso cref="ConfigurationNode.Renamed"/> events.
    /// </summary>    
    public class ConfigurationNodeChangedEventArgs : EventArgs
    {
		private readonly ConfigurationNode node;
		private readonly ConfigurationNode parent;

        /// <summary>
		/// Initializes a new instance of the <see cref="ConfigurationNodeChangedEventArgs"/> class the node it was performed upon, and the parent node.
        /// </summary>        
        /// <param name="node">
		/// The <see cref="ConfigurationNode"/> that the action occurred upon.
        /// </param>
        /// <param name="parent">The parent node of the <paramref name="node"/>.</param>
		public ConfigurationNodeChangedEventArgs(ConfigurationNode node,
												 ConfigurationNode parent)
        {            
            this.node = node;
            this.parent = parent;
        }        

        /// <summary>
		/// Gets the <see cref="ConfigurationNode"/> for the action.
        /// </summary>
        /// <value>
		/// The <see cref="ConfigurationNode"/> for the action.
        /// </value>
		public ConfigurationNode Node
        {
            get { return node; }
        }

        /// <summary>
        /// Gets the parent node of the <seealso cref="Node"/> object.
        /// </summary>
        /// <value>
        /// The parent node of the <seealso cref="Node"/> object.
        /// </value>
		public ConfigurationNode ParentNode
        {
            get { return parent; }
        }
    }
}
