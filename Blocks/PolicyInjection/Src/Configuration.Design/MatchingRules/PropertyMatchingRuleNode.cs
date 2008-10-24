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
	/// Represents a <see cref="PropertyMatchingRuleData"/> instance.
	/// </summary>
	public class PropertyMatchingRuleNode : MatchingRuleNode
	{
		List<PropertyMatch> matches;

		/// <summary>
		/// Initializes a new instance of the <see cref="PropertyMatchingRuleNode"/> class with default values.
		/// </summary>
		public PropertyMatchingRuleNode()
			: this(new PropertyMatchingRuleData(Resources.PropertyMatchingRuleNodeName))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PropertyMatchingRuleNode"/> class for representing a <see cref="PropertyMatchingRuleData"/> instance.
		/// </summary>
		/// <param name="ruleData">The <see cref="PropertyMatchingRuleData"/> to represent.</param>
		public PropertyMatchingRuleNode(PropertyMatchingRuleData ruleData)
			: base(ruleData)
		{
			matches = new List<PropertyMatch>();
			foreach (PropertyMatchData match in ruleData.Matches)
			{
				matches.Add(new PropertyMatch(match.Match, match.IgnoreCase, match.MatchOption));
			}
		}

		/// <summary>
		/// Gets the collection of property matches for the represented configuration object.
		/// </summary>
		[SRDescription("PropertyRuleMatchesDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[PropertyMatchDataAttribute]
		public List<PropertyMatch> Matches
		{
			get { return matches; }
		}

		/// <summary>
		/// Returs the represented <see cref="PropertyMatchingRuleData"/> instance.
		/// </summary>
		/// <returns>A newly created <see cref="PropertyMatchingRuleData"/> instance.</returns>
		public override MatchingRuleData GetConfigurationData()
		{
			PropertyMatchingRuleData ruleData = new PropertyMatchingRuleData(Name);
			foreach (PropertyMatch match in matches)
			{
				ruleData.Matches.Add(new PropertyMatchData(match.Value, match.MatchOption, match.IgnoreCase));
			}

			return ruleData;
		}
	}
}
