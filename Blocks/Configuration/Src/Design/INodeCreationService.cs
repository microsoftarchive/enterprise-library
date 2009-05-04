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
using System.Collections.Specialized;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Provides a service for creating nodes based on data types.
    /// </summary>
    public interface INodeCreationService
    {

        /// <summary>
        /// When implemented by a class, creates a <see cref="ConfigurationNode"/> based on the data type.
        /// </summary>
        /// <param name="dataType">
        /// The data type to base the creation upon.
        /// </param>
        /// <returns>
        /// A <see cref="ConfigurationNode"/> based on the data type or <see langword="null"/> if one does not exists.
        /// </returns>
        ConfigurationNode CreateNodeByDataType(Type dataType);

        /// <summary>
        /// When implemented by a class, creates a <see cref="ConfigurationNode"/> based on the data type.
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
        ConfigurationNode CreateNodeByDataType(Type dataType, object[] constructorArguments);
        
        /// <summary>
        /// When implemented by a class, creates a map between the node and the data that it represents.
        /// </summary>
        /// <param name="nodeCreationEntry">
        /// A <see cref="NodeCreationEntry"/> object.
        /// </param>
        void AddNodeCreationEntry(NodeCreationEntry nodeCreationEntry);        
    }
}
