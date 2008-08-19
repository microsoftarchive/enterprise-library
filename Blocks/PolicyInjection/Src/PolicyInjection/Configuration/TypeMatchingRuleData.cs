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
    /// Configuration element that stores configuration information for
    /// an instance of <see cref="TypeMatchingRule"/>.
    /// </summary>
    [Assembler(typeof(TypeMatchingRuleAssembler))]
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
            : this(matchingRuleName, new MatchData[] { new MatchData(typeName)})
        {
        }

        /// <summary>
        /// Constructs a new <see cref="TypeMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule instance name in configuration.</param>
        /// <param name="matches">Collection of <see cref="MatchData"/> containing
        /// types to match. If any one matches, the rule matches.</param>
        public TypeMatchingRuleData(string matchingRuleName, IEnumerable<MatchData> matches)
            : base( matchingRuleName, typeof(TypeMatchingRule))
        {
            foreach(MatchData match in matches)
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
    }

    /// <summary>
    /// A class used by ObjectBuilder to construct instance of <see cref="TypeMatchingRule"/>
    /// from instances of <see cref="TypeMatchingRuleData"/>.
    /// </summary>
    public class TypeMatchingRuleAssembler : IAssembler<IMatchingRule, MatchingRuleData>
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
            TypeMatchingRuleData castedRuleData = (TypeMatchingRuleData)objectConfiguration;
            List<MatchingInfo> matches = new List<MatchingInfo>();
            foreach(MatchData match in castedRuleData.Matches)
            {
                matches.Add(new MatchingInfo(match.Match, match.IgnoreCase));
            }
            TypeMatchingRule matchingRule = new TypeMatchingRule(matches);

            return matchingRule;
        }
    }
}
