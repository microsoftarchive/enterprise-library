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
    /// Configuration element that stores the config information for an instance
    /// of <see cref="NamespaceMatchingRule"/>.
    /// </summary>
    [Assembler(typeof(NamespaceMatchingRuleAssembler))]
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
            :this(matchingRuleName, new List<MatchData>())
        {
        }

        /// <summary>
        /// Constructs a new <see cref="NamespaceMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule name in config file.</param>
        /// <param name="namespaceName">Namespace pattern to match.</param>
        public NamespaceMatchingRuleData(string matchingRuleName, string namespaceName)
            : this(matchingRuleName, new MatchData[] { new MatchData( namespaceName )})
        {
        }

        /// <summary>
        /// Constructs a new <see cref="NamespaceMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule name in config file.</param>
        /// <param name="matches">Collection of namespace patterns to match. If any
        /// of the patterns match then the rule matches.</param>
        public NamespaceMatchingRuleData(string matchingRuleName, IEnumerable<MatchData> matches)
            : base( matchingRuleName, typeof(NamespaceMatchingRule))
        {
            foreach(MatchData match in matches )
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

    }

    /// <summary>
    /// Class used by ObjectBuilder to create instances of <see cref="NamespaceMatchingRule"/>
    /// from a <see cref="NamespaceMatchingRuleData"/> element.
    /// </summary>
    public class NamespaceMatchingRuleAssembler : IAssembler<IMatchingRule, MatchingRuleData>
    {
        /// <summary>
        /// Builds an instance of the subtype of IMatchingRule type the receiver knows how to build, based on 
        /// a configuration object.
        /// </summary>
        /// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
        /// <param name="objectConfiguration">The configuration object that describes the object to build.</param>
        /// <param name="configurationSource">The source for configuration objects.</param>
        /// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
        /// <returns>A fully initialized instance of the IMatchingRule subtype.</returns>
        public IMatchingRule Assemble(IBuilderContext context, MatchingRuleData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            NamespaceMatchingRuleData castedRuleData = (NamespaceMatchingRuleData)objectConfiguration;

            List<MatchingInfo> matches = new List<MatchingInfo>();
            foreach(MatchData matchData in castedRuleData.Matches)
            {
                matches.Add(new MatchingInfo(matchData.Match, matchData.IgnoreCase));
            }

            NamespaceMatchingRule matchingRule = new NamespaceMatchingRule(matches);

            return matchingRule;
        }
    }
}
