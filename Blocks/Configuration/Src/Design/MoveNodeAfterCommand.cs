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
    /// Represents a command that will move a <see cref="ConfigurationNode"/> after a given <see cref="ConfigurationNode"/>.
    /// </summary>
    public class MoveNodeAfterCommand : ConfigurationNodeCommand
    {


        /// <summary>
        /// Initialize a new instance of the <see cref="MoveNodeAfterCommand"/> class with an <see cref="IServiceProvider"/>, the node to move, and the sibling node to move it after.
        /// </summary>
        /// <param name="serviceProvider">
        /// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
        /// </param>
        public MoveNodeAfterCommand(IServiceProvider serviceProvider) : base(serviceProvider, false)
        {

        }

        /// <summary>
        /// Executes the moving the node after it's sibling.
        /// </summary>
        /// <param name="node">
        /// The node to execute the command upon.
        /// </param>
        protected override void ExecuteCore(ConfigurationNode node)
        {
            if (node.NextSibling != null)
            {
                node.Parent.MoveAfter(node, node.NextSibling);
            }
        }
    }
}
