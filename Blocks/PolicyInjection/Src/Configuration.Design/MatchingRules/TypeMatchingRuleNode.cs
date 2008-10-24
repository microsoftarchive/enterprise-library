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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.MatchingRules
{
	/// <summary>
	/// Represents a <see cref="TypeMatchingRuleData"/> instance.
	/// </summary>
	public class TypeMatchingRuleNode : MatchingRuleNode
	{
		List<Match> matches;

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeMatchingRuleNode"/> class with default values.
		/// </summary>
		public TypeMatchingRuleNode()
			: this(new TypeMatchingRuleData(Resources.TypeMatchingRuleNodeName))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeMatchingRuleNode"/> class for representing a <see cref="TypeMatchingRuleData"/> instance.
		/// </summary>
		/// <param name="ruleData">The <see cref="TypeMatchingRuleData"/> to represent.</param>
		public TypeMatchingRuleNode(TypeMatchingRuleData ruleData)
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
		[SRDescription("TypeMatchesDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[MatchDataAttribute]
		public List<Match> Matches
		{
			get { return matches; }
		}

		/// <summary>
		/// Returs the represented <see cref="TypeMatchingRuleData"/> instance.
		/// </summary>
		/// <returns>A newly created <see cref="TypeMatchingRuleData"/> instance.</returns>
		public override MatchingRuleData GetConfigurationData()
		{
			TypeMatchingRuleData ruleData = new TypeMatchingRuleData(Name);
			foreach (Match match in matches)
			{
				ruleData.Matches.Add(new MatchData(match.Value, match.IgnoreCase));
			}

			return ruleData;
		}
	}
}
