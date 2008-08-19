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
	/// Represents a <see cref="TagAttributeMatchingRuleData"/> instance.
	/// </summary>
	public class TagAttributeMatchingRuleNode : MatchingRuleNode
	{
		string tagToMatch;
		bool ignoreCase;

		/// <summary>
		/// Initializes a new instance of the <see cref="TagAttributeMatchingRuleNode"/> class with default values.
		/// </summary>
		public TagAttributeMatchingRuleNode()
			: this(new TagAttributeMatchingRuleData(Resources.TagAttributeMatchingRuleNodeName, Resources.DefaultTagValue))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TagAttributeMatchingRuleNode"/> class for representing a <see cref="TagAttributeMatchingRuleData"/> instance.
		/// </summary>
		/// <param name="ruleData">The <see cref="TagAttributeMatchingRuleData"/> to represent.</param>
		public TagAttributeMatchingRuleNode(TagAttributeMatchingRuleData ruleData)
			: base(ruleData)
		{
			tagToMatch = ruleData.Match;
			ignoreCase = ruleData.IgnoreCase;
		}

		/// <summary>
		/// Gets or sets the indication of whether case should be ignored when matching for the represented configuration object.
		/// </summary>
		[SRDescription("TagAttributeIgnoreCaseDescription", typeof(Resources))]
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
		[SRDescription("TagAttributeMatchDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public string Match
		{
			get { return tagToMatch; }
			set { tagToMatch = value; }
		}

		/// <summary>
		/// Returs the represented <see cref="TagAttributeMatchingRuleData"/> instance.
		/// </summary>
		/// <returns>A newly created <see cref="TagAttributeMatchingRuleData"/> instance.</returns>
		public override MatchingRuleData GetConfigurationData()
		{
			TagAttributeMatchingRuleData ruleData = new TagAttributeMatchingRuleData(Name, Match);
			ruleData.IgnoreCase = IgnoreCase;

			return ruleData;
		}
	}
}
