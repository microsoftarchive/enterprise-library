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
using System.Globalization;
using System.Diagnostics;
using System.ComponentModel.Design.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	/// <summary>
	/// Provides a service that can generate unique names for <see cref="ConfigurationNode"/> objects.
	/// </summary>
    public class NodeNameCreationService : INodeNameCreationService
    {
		/// <summary>
		/// Initialize a new instance of the <see cref="NodeNameCreationService"/> class.
		/// </summary>
        public NodeNameCreationService()
        {
        }

		/// <summary>
		/// Gets a unique name for the node.
		/// </summary>
		/// <param name="nodeName">The current name of the node.</param>
		/// <param name="node">The node to rename.</param>
		/// <param name="parentNode">The parent of the node.</param>
		/// <returns>A unique name.</returns>
		public string GetUniqueName(string nodeName, ConfigurationNode node, ConfigurationNode parentNode)
		{
            if (null == parentNode || node == parentNode.Nodes[nodeName]) return nodeName;

			string baseName = nodeName;
            string newName = baseName;
			int uniqueID = 0;

            while (true)
            {
                //if (null == parentNode.Nodes[newName]) break;
				ConfigurationNode nodeToFind = parentNode.Hierarchy.FindNodeByName(parentNode, newName);
				if (null == nodeToFind) break;

                uniqueID++;
                newName = baseName + uniqueID.ToString(CultureInfo.CurrentUICulture);
            }

            return newName;
		}
	}
}