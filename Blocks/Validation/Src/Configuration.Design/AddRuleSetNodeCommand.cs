//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design
{
	/// <summary>
    /// Represents a command for adding <see cref="RuleSetNode"/> to a <see cref="TypeNode"/>.
	/// </summary>
	public class AddRuleSetNodeCommand : AddChildNodeCommand
	{
		private IServiceProvider serviceProvider;

        /// <summary>
        /// Initialize a new instance of the <see cref="AddRuleSetNodeCommand"/> class with an <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
        public AddRuleSetNodeCommand(IServiceProvider serviceProvider)
            : base(serviceProvider, typeof(RuleSetNode))
		{
			this.serviceProvider = serviceProvider;
		}

        /// <summary>
        /// <para>Creates an instance of the child node class and adds it as a child of the parent node. The node will be a <see cref="RuleSetNode"/>.</para>
        /// </summary>
        /// <param name="node">
        /// <para>The parent node to add the newly created <see cref="RuleSetNode"/>.</para>
        /// </param>
        protected override void ExecuteCore(ConfigurationNode node)
		{
			base.ExecuteCore(node);

            RuleSetNode ruleNode = ChildNode as RuleSetNode;
            if (ruleNode == null) return;

            ruleNode.AddNode(new SelfNode());

		}
	}
}
