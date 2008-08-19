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
	/// 
	/// </summary>
	public class AddRuleNodeCommand : AddChildNodeCommand
	{
		private IServiceProvider serviceProvider;

		/// <summary>
        /// 
		/// </summary>
		/// <param name="serviceProvider"></param>
        public AddRuleNodeCommand(IServiceProvider serviceProvider)
            : base(serviceProvider, typeof(RuleNode))
		{
			this.serviceProvider = serviceProvider;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="node"></param>
		protected override void ExecuteCore(ConfigurationNode node)
		{
			base.ExecuteCore(node);

            RuleNode ruleNode = ChildNode as RuleNode;
            if (ruleNode == null) return;

		}
	}
}
