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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.MatchingRules;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A configuration element that supports the <see cref="MemberNameMatchingRule"/>.
    /// </summary>
    [Assembler(typeof(MemberNameMatchingRuleAssembler))]
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
            : base(matchingRuleName, typeof(MemberNameMatchingRule))
        {
        }

        /// <summary>
        /// Constructs a new <see cref="MemberNameMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule name in config file.</param>
        /// <param name="match">Member name pattern to match.</param>
        public MemberNameMatchingRuleData(string matchingRuleName, string match)
            : this( matchingRuleName, new MatchData[] { new MatchData(match) } )
        {

        }

        /// <summary>
        /// Constructs a new <see cref="MemberNameMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule name in config file.</param>
        /// <param name="matches">Collection of <see cref="MatchData"/> containing the patterns
        /// to match. If any pattern matches, the rule matches.</param>
        public MemberNameMatchingRuleData(string matchingRuleName, IEnumerable<MatchData> matches)
            : base(matchingRuleName, typeof(MemberNameMatchingRule))
        {
            foreach(MatchData match in matches)
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
            get { return (MatchDataCollection<MatchData>)base[MatchesPropertyName];  }
            set { base[MatchesPropertyName] = value; }
        }
    }

    /// <summary>
    /// A class used by ObjectBuilder to construct <see cref="MemberNameMatchingRule"/>
    /// instances from a <see cref="MemberNameMatchingRuleData"/> instance.
    /// </summary>
    public class MemberNameMatchingRuleAssembler : IAssembler<IMatchingRule, MatchingRuleData>
    {
        /// <summary>
        /// Builds an instance of the subtype of the IMatching type the receiver knows how to build, based on 
        /// a configuration object.
        /// </summary>
        /// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
        /// <param name="objectConfiguration">The configuration object that describes the object to build.</param>
        /// <param name="configurationSource">The source for configuration objects.</param>
        /// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
        /// <returns>A fully initialized instance of the IMatchingRule subtype.</returns>
        public IMatchingRule Assemble(IBuilderContext context, MatchingRuleData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            MemberNameMatchingRuleData castedRuleData = (MemberNameMatchingRuleData)objectConfiguration;
            List<MatchingInfo> namesToMatch = new List<MatchingInfo>();
            foreach(MatchData matchData in castedRuleData.Matches)
            {
                namesToMatch.Add(new MatchingInfo(matchData.Match, matchData.IgnoreCase));
            }

            MemberNameMatchingRule matchingRule = new MemberNameMatchingRule(namesToMatch);

            return matchingRule;
        }
    }
}
