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
    /// Configuration element that stores the config information for an instance
    /// of <see cref="NamespaceMatchingRule"/>.
    /// </summary>
    public class NamespaceMatchingRuleData : MatchingRuleData
    {
        private const string MatchesPropertyName = "matches";

        /// <summary>
        /// Constructs a new <see cref="NamespaceMatchingRuleData"/> instance.
        /// </summary>
        public NamespaceMatchingRuleData()
        {
        }

        /// <summary>
        /// Constructs a new <see cref="NamespaceMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule name in config file.</param>
        public NamespaceMatchingRuleData(string matchingRuleName)
            : this(matchingRuleName, new List<MatchData>())
        {
        }

        /// <summary>
        /// Constructs a new <see cref="NamespaceMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule name in config file.</param>
        /// <param name="namespaceName">Namespace pattern to match.</param>
        public NamespaceMatchingRuleData(string matchingRuleName, string namespaceName)
            : this(matchingRuleName, new MatchData[] { new MatchData(namespaceName) })
        {
        }

        /// <summary>
        /// Constructs a new <see cref="NamespaceMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule name in config file.</param>
        /// <param name="matches">Collection of namespace patterns to match. If any
        /// of the patterns match then the rule matches.</param>
        public NamespaceMatchingRuleData(string matchingRuleName, IEnumerable<MatchData> matches)
            : base(matchingRuleName, typeof(FakeRules.NamespaceMatchingRule))
        {
            foreach (MatchData match in matches)
            {
                Matches.Add(match);
            }
        }

        /// <summary>
        /// The collection of match data containing patterns to match.
        /// </summary>
        /// <value>The "matches" child element.</value>
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
            List<MatchingInfo> matches = new List<MatchingInfo>();
            foreach (MatchData matchData in this.Matches)
            {
                matches.Add(new MatchingInfo(matchData.Match, matchData.IgnoreCase));
            }

            policy.AddMatchingRule<NamespaceMatchingRule>(
                new InjectionConstructor(new InjectionParameter<IEnumerable<MatchingInfo>>(matches)));
        }
    }
}
