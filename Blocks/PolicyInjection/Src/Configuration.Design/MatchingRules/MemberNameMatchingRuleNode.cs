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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.MatchingRules
{
	/// <summary>
	/// Represents a <see cref="MemberNameMatchingRuleData"/> instance.
	/// </summary>
	public class MemberNameMatchingRuleNode : MatchingRuleNode
	{
		List<Match> matches;

		/// <summary>
		/// Initializes a new instance of the <see cref="MemberNameMatchingRuleNode"/> class with default values.
		/// </summary>
		public MemberNameMatchingRuleNode()
			: this(new MemberNameMatchingRuleData(Resources.MemberNameMatchingRuleNodeName))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MemberNameMatchingRuleNode"/> class for representing a <see cref="MemberNameMatchingRuleData"/> instance.
		/// </summary>
		/// <param name="ruleData">The <see cref="MemberNameMatchingRuleData"/> to represent.</param>
		public MemberNameMatchingRuleNode(MemberNameMatchingRuleData ruleData)
			: base(ruleData)
		{
			matches = new List<Match>();
			foreach (MatchData match in ruleData.Matches)
			{
				matches.Add(new Match(match.Match, match.IgnoreCase));
			}
		}

		/// <summary>
		/// Gets the list of matches for the represented configuration object.
		/// </summary>
		[SRDescription("MemberNameMatchesDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[MatchDataAttribute]
		public List<Match> Matches
		{
			get { return matches; }
		}

		/// <summary>
		/// Returs the represented <see cref="MemberNameMatchingRuleData"/> instance.
		/// </summary>
		/// <returns>A newly created <see cref="MemberNameMatchingRuleData"/> instance.</returns>
		public override MatchingRuleData GetConfigurationData()
		{
			MemberNameMatchingRuleData ruleData = new MemberNameMatchingRuleData(Name);
			foreach (Match match in matches)
			{
				ruleData.Matches.Add(new MatchData(match.Value, match.IgnoreCase));
			}

			return ruleData;
		}
	}
}
