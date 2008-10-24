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

using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using FakeRules = Microsoft.Practices.EnterpriseLibrary.PolicyInjection.MatchingRules;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A configuration element that supports the <see cref="MemberNameMatchingRule"/>.
    /// </summary>
    public class MemberNameMatchingRuleData : MatchingRuleData
    {
        private const string MatchesPropertyName = "matches";

        /// <summary>
        /// Constructs a new <see cref="MemberNameMatchingRuleData"/> instance.
        /// </summary>
        public MemberNameMatchingRuleData()
        {
        }

        /// <summary>
        /// Constructs a new <see cref="MemberNameMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule name in config file.</param>
        public MemberNameMatchingRuleData(string matchingRuleName)
            : base(matchingRuleName, typeof(FakeRules.MemberNameMatchingRule))
        {
        }

        /// <summary>
        /// Constructs a new <see cref="MemberNameMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule name in config file.</param>
        /// <param name="match">Member name pattern to match.</param>
        public MemberNameMatchingRuleData(string matchingRuleName, string match)
            : this(matchingRuleName, new MatchData[] { new MatchData(match) })
        {

        }

        /// <summary>
        /// Constructs a new <see cref="MemberNameMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule name in config file.</param>
        /// <param name="matches">Collection of <see cref="MatchData"/> containing the patterns
        /// to match. If any pattern matches, the rule matches.</param>
        public MemberNameMatchingRuleData(string matchingRuleName, IEnumerable<MatchData> matches)
            : base(matchingRuleName, typeof(FakeRules.MemberNameMatchingRule))
        {
            foreach (MatchData match in matches)
            {
                Matches.Add(match);
            }
        }

        /// <summary>
        /// The collection of patterns to match.
        /// </summary>
        /// <value>The "matches" child element in config.</value>
        [ConfigurationProperty(MatchesPropertyName)]
        [ConfigurationCollection(typeof(MatchData))]
        public MatchDataCollection<MatchData> Matches
        {
            get { return (MatchDataCollection<MatchData>)base[MatchesPropertyName]; }
            set { base[MatchesPropertyName] = value; }
        }

        /// <summary>
        /// Adds the rule represented by this configuration object to <paramref name="policy"/>.
        /// </summary>
        /// <param name="policy">The policy to which the rule must be added.</param>
        /// <param name="configurationSource">The configuration source from which additional information
        /// can be retrieved, if necessary.</param>
        public override void ConfigurePolicy(PolicyDefinition policy, IConfigurationSource configurationSource)
        {
            List<MatchingInfo> namesToMatch = new List<MatchingInfo>();
            foreach (MatchData matchData in this.Matches)
            {
                namesToMatch.Add(new MatchingInfo(matchData.Match, matchData.IgnoreCase));
            }

            policy.AddMatchingRule<MemberNameMatchingRule>
                (new InjectionConstructor(new InjectionParameter<IEnumerable<MatchingInfo>>(namesToMatch)));
        }
    }
}
