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
    /// Configuration element that stores configuration information for
    /// an instance of <see cref="TypeMatchingRule"/>.
    /// </summary>
    public class TypeMatchingRuleData : MatchingRuleData
    {
        private const string MatchesPropertyName = "matches";

        /// <summary>
        /// Constructs a new <see cref="TypeMatchingRuleData"/> instance.
        /// </summary>
        public TypeMatchingRuleData()
        {
        }

        /// <summary>
        /// Constructs a new <see cref="TypeMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule instance name in configuration.</param>
        public TypeMatchingRuleData(string matchingRuleName)
            : this(matchingRuleName, new MatchData[0])
        {
        }

        /// <summary>
        /// Constructs a new <see cref="TypeMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule instance name in configuration.</param>
        /// <param name="typeName">Type name to match.</param>
        public TypeMatchingRuleData(string matchingRuleName, string typeName)
            : this(matchingRuleName, new MatchData[] { new MatchData(typeName) })
        {
        }

        /// <summary>
        /// Constructs a new <see cref="TypeMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule instance name in configuration.</param>
        /// <param name="matches">Collection of <see cref="MatchData"/> containing
        /// types to match. If any one matches, the rule matches.</param>
        public TypeMatchingRuleData(string matchingRuleName, IEnumerable<MatchData> matches)
            : base(matchingRuleName, typeof(FakeRules.TypeMatchingRule))
        {
            foreach (MatchData match in matches)
            {
                Matches.Add(match);
            }
        }

        /// <summary>
        /// The collection of <see cref="MatchData"/> giving the types to match.
        /// </summary>
        /// <value>The "matches" configuration subelement.</value>
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
            foreach (MatchData match in this.Matches)
            {
                matches.Add(new MatchingInfo(match.Match, match.IgnoreCase));
            }

            policy.AddMatchingRule<TypeMatchingRule>(
                new InjectionConstructor(new InjectionParameter<IEnumerable<MatchingInfo>>(matches)));
        }
    }
}
