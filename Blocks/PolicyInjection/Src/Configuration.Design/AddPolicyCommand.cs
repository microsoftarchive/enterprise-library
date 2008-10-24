//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design
{
    /// <summary>
    /// Represents a command that adds a <see cref="PolicyNode"/> as a child of the  <see cref="ConfigurationNode"/> that this command is executing upon.    
    /// </summary>
	public class AddPolicyCommand : AddChildNodeCommand
	{
        /// <summary>
        /// Initialize a new instance of the <see cref="AddPolicyCommand"/> class with an <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="serviceProvider">
        /// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
        /// </param>
		public AddPolicyCommand(IServiceProvider serviceProvider)
			: base(serviceProvider, typeof(PolicyNode))
		{ }

        /// <summary>
        /// Creates an instance of the child node class and adds it as a child of the parent node.
        /// </summary>
        /// <param name="node">
        /// The parent node to add the newly created <see cref="AddChildNodeCommand.ChildNode"/>.
        /// </param>
        protected override void ExecuteCore(ConfigurationNode node)
		{
			base.ExecuteCore(node);

			if (ChildNode != null)
			{
				ChildNode.AddNode(new MatchingRuleCollectionNode());
				ChildNode.AddNode(new CallHandlersCollectionNode());
			}
		}
	}
}
