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

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.MatchingRules
{
	/// <summary>
	/// Represents a <see cref="NamespaceMatchingRuleData"/> instance.
	/// </summary>
	public class NamespaceMatchingRuleNode : MatchingRuleNode
	{
		List<Match> matches;

		/// <summary>
		/// Initializes a new instance of the <see cref="NamespaceMatchingRuleNode"/> class with default values.
		/// </summary>
		public NamespaceMatchingRuleNode()
			: this(new NamespaceMatchingRuleData(Resources.NamespaceMatchingRuleNodeName, new List<MatchData>()))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NamespaceMatchingRuleNode"/> class for representing a <see cref="NamespaceMatchingRuleData"/> instance.
		/// </summary>
		/// <param name="ruleData">The <see cref="NamespaceMatchingRuleData"/> to represent.</param>
		public NamespaceMatchingRuleNode(NamespaceMatchingRuleData ruleData)
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
		[SRDescription("NamespaceMatchesDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[MatchDataAttribute]
		public List<Match> Matches
		{
			get { return matches; }
		}

		/// <summary>
		/// Returs the represented <see cref="NamespaceMatchingRuleData"/> instance.
		/// </summary>
		/// <returns>A newly created <see cref="NamespaceMatchingRuleData"/> instance.</returns>
		public override MatchingRuleData GetConfigurationData()
		{
			NamespaceMatchingRuleData ruleData = new NamespaceMatchingRuleData(Name);

			foreach (Match match in matches)
			{
				ruleData.Matches.Add(new MatchData(match.Value, match.IgnoreCase));
			}

			return ruleData;
		}
	}
}
