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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.MatchingRules
{
	/// <summary>
	/// Represents a <see cref="AssemblyMatchingRuleData"/>.
	/// </summary>
	public class AssemblyMatchingRuleNode : MatchingRuleNode
	{
		private string assemblyName;

		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyMatchingRuleNode"/> class with default values.
		/// </summary>
		public AssemblyMatchingRuleNode()
			: this(new AssemblyMatchingRuleData(Resources.AssemblyMatchingRule, Resources.DefaultMatchingRuleAssemblyName))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyMatchingRuleNode"/> class for representing a <see cref="AssemblyMatchingRuleData"/> instance.
		/// </summary>
		/// <param name="assemblyMatchingRuleData">The <see cref="AssemblyMatchingRuleData"/> to represent.</param>
		public AssemblyMatchingRuleNode(AssemblyMatchingRuleData assemblyMatchingRuleData)
			: base(assemblyMatchingRuleData)
		{
			assemblyName = assemblyMatchingRuleData.Match;
		}

		/// <summary>
		/// Gets or sets the name of the assembly for the represented configuration object.
		/// </summary>
		[Required]
		[SRDescription("AssemblyNameDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public string AssemblyName
		{
			get { return assemblyName; }
			set { assemblyName = value; }
		}

		/// <summary>
		/// Returs the represented <see cref="AssemblyMatchingRuleData"/> instance.
		/// </summary>
		/// <returns>A newly created <see cref="AssemblyMatchingRuleData"/> instance.</returns>
		public override MatchingRuleData GetConfigurationData()
		{
			return new AssemblyMatchingRuleData(this.Name, assemblyName);
		}
	}
}
