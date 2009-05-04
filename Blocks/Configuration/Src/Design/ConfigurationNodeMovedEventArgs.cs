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
	/// Provides data for the <seealso cref="ConfigurationNode.ChildMoved"/> events.
	/// </summary>
    public class ConfigurationNodeMovedEventArgs : ConfigurationNodeChangedEventArgs
    {
        private int newIndex;
        private int oldIndex;

		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigurationNodeChangedEventArgs"/> class the node it was performed upon, the parent node, the old index and new index.
		/// </summary>        
		/// <param name="node">
		/// The <see cref="ConfigurationNode"/> that the action occurred upon.
		/// </param>
		/// <param name="parent">The parent node of the <paramref name="node"/>.</param>
        /// <param name="oldIndex">The old index of the node being moved.</param>
        /// <param name="newIndex">The new index of the node being moved.</param>
        public ConfigurationNodeMovedEventArgs(ConfigurationNode node, ConfigurationNode parent, int oldIndex, int newIndex)
            : base(node, parent)
        {
            this.newIndex = newIndex;
            this.oldIndex = oldIndex;
        }

        /// <summary>
        /// Gets the new index of the node being moved.
        /// </summary>
		/// <value>
		/// The new index of the node being moved.
		/// </value>
        public int NewIndex
        {
            get { return newIndex; }
        }

        /// <summary>
        /// Gets the old index of the node being moved.
        /// </summary>
		/// <value>
		/// The old index of the node being moved.
		/// </value>
        public int OldIndex
        {
            get { return oldIndex; }
        }

    }
}
