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

using System.ComponentModel.Design.Serialization;
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
	/// Provides a service that can generate unique names for <see cref="ConfigurationNode"/> objects.
    /// </summary>
    public interface INodeNameCreationService 
    {
        /// <summary>
        /// Gets a unique name for the node.
        /// </summary>
        /// <param name="nodeName">The current name of the node.</param>
        /// <param name="node">The node to rename.</param>
        /// <param name="parentNode">The parent of the node.</param>
        /// <returns>A unique name.</returns>
        string GetUniqueName(string nodeName, ConfigurationNode node, ConfigurationNode parentNode);
    }

}