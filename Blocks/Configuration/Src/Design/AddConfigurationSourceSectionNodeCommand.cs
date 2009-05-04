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
using System.Text;
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Represents a command that adds a <see cref="ConfigurationSourceSectionNode"/> as a child of the  <see cref="ConfigurationNode"/> that this command is executing upon.    
    /// </summary>
	public class AddConfigurationSourceSectionNodeCommand : AddChildNodeCommand
	{
        /// <summary>
        /// Initialize a new instance of the <see cref="AddConfigurationSourceSectionNodeCommand"/> class with an <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="serviceProvider">
        /// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
        /// </param>
		public AddConfigurationSourceSectionNodeCommand(IServiceProvider serviceProvider) : base(serviceProvider, typeof(ConfigurationSourceSectionNode))
		{

		}

        /// <summary>
        /// Raises the <see cref="ConfigurationNodeCommand.Executed"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> containing the event data.</param>
        protected override void OnExecuted(EventArgs e)
		{
			base.OnExecuted(e);
			ConfigurationSourceSectionNode node = ChildNode as ConfigurationSourceSectionNode;
			Debug.Assert(node != null);

			SystemConfigurationSourceElementNode sourceNode = new SystemConfigurationSourceElementNode();
			node.AddNode(sourceNode);
			node.SelectedSource = sourceNode;
		}
	}
}
