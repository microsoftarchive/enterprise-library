//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
    /// <summary>
    /// Represents a command that adds a <see cref="OracleConnectionElementNode"/> as a child of the  <see cref="ConfigurationNode"/> that this command is executing upon.    
    /// </summary>
    public sealed class AddOracleConnectionElementNodeCommand : AddChildNodeCommand
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="AddOracleConnectionElementNodeCommand"/> class with an <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="serviceProvider">
        /// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
        /// </param>
        public AddOracleConnectionElementNodeCommand(IServiceProvider serviceProvider)
            : base(serviceProvider, typeof(OracleConnectionElementNode)) {}

        /// <summary>
        /// Raises the <see cref="ConfigurationNodeCommand.Executed"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> containing the event data.</param>
        protected override void OnExecuted(EventArgs e)
        {
            base.OnExecuted(e);
            OracleConnectionElementNode node = ChildNode as OracleConnectionElementNode;
            Debug.Assert(null != node, "Expected OracleConnectionElementNode");

            node.AddNode(new OraclePackageElementNode());
        }
    }
}
