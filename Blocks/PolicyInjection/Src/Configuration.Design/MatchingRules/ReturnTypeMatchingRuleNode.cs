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
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.MatchingRules
{
	/// <summary>
	/// Represents a <see cref="ReturnTypeMatchingRuleData"/> instance.
	/// </summary>
	public class ReturnTypeMatchingRuleNode : MatchingRuleNode
	{
		bool ignoreCase;
		string match;

		/// <summary>
		/// Initializes a new instance of the <see cref="ReturnTypeMatchingRuleNode"/> class with default values.
		/// </summary>
		public ReturnTypeMatchingRuleNode()
			: this(new ReturnTypeMatchingRuleData(Resources.ReturnTypeMatchingRuleNodeName, Resources.DefaultReturnTypeMatchValue))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ReturnTypeMatchingRuleNode"/> class for representing a <see cref="ReturnTypeMatchingRuleData"/> instance.
		/// </summary>
		/// <param name="ruleData">The <see cref="ReturnTypeMatchingRuleData"/> to represent.</param>
		public ReturnTypeMatchingRuleNode(ReturnTypeMatchingRuleData ruleData)
			: base(ruleData)
		{
			ignoreCase = ruleData.IgnoreCase;
			match = ruleData.Match;
		}

		/// <summary>
		/// Gets or sets the indication of whether case should be ignored when matching for the represented configuration object.
		/// </summary>
		[SRDescription("ReturnTypeIgnoreCaseDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public bool IgnoreCase
		{
			get { return ignoreCase; }
			set { ignoreCase = value; }
		}

		/// <summary>
		/// Gets or sets the match for the represented configuration object.
		/// </summary>
		[Required]
		[SRDescription("ReturnTypeMatchDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public string Match
		{
			get { return match; }
			set { match = value; }
		}

		/// <summary>
		/// Returs the represented <see cref="ReturnTypeMatchingRuleData"/> instance.
		/// </summary>
		/// <returns>A newly created <see cref="ReturnTypeMatchingRuleData"/> instance.</returns>
		public override MatchingRuleData GetConfigurationData()
		{
			ReturnTypeMatchingRuleData ruleData = new ReturnTypeMatchingRuleData(Name, Match);
			ruleData.IgnoreCase = IgnoreCase;

			return ruleData;
		}
	}
}
