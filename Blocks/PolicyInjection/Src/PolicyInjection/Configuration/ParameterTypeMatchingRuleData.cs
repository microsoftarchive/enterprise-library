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
using System.Configuration;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.MatchingRules;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A configuration element storing config information for an instance of
    /// <see cref="ParameterTypeMatchingRule"/>.
    /// </summary>
    [Assembler(typeof(ParameterTypeMatchingRuleAssembler))]
    public class ParameterTypeMatchingRuleData : MatchingRuleData
    {
        private const string matchesPropertyName = "matches";

        /// <summary>
        /// Constructs a new <see cref="ParameterTypeMatchingRuleData"/> instance.
        /// </summary>
        public ParameterTypeMatchingRuleData()
        {
        }

        /// <summary>
        /// Constructs a new <see cref="ParameterTypeMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule instance name in configuration.</param>
        public ParameterTypeMatchingRuleData(string matchingRuleName) 
            : base(matchingRuleName, typeof(ParameterTypeMatchingRule))
        {
        }

        /// <summary>
        /// Constructs a new <see cref="ParameterTypeMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule instance name in configuration.</param>
        /// <param name="matches">Collection of <see cref="ParameterTypeMatchData"/> to match against.
        /// If any of them match, the rule matches.</param>
        public ParameterTypeMatchingRuleData(string matchingRuleName, IEnumerable<ParameterTypeMatchData> matches)
            : base(matchingRuleName, typeof(ParameterTypeMatchingRule))
        {
            foreach(ParameterTypeMatchData matchData in matches )
            {
                Matches.Add(matchData);
            }
        }

        /// <summary>
        /// The collection of parameter types to match against.
        /// </summary>
        /// <value>The "matches" subelement.</value>
        [ConfigurationProperty(matchesPropertyName)]
        [ConfigurationCollection(typeof(ParameterTypeMatchData))]
        public MatchDataCollection<ParameterTypeMatchData> Matches
        {
            get { return (MatchDataCollection < ParameterTypeMatchData > )base[matchesPropertyName]; }
            set { base[matchesPropertyName] = value; }
        }
    }

    /// <summary>
    /// An extended <see cref="MatchData"/> class that also includes the
    /// <see cref="ParameterKind"/> of the parameter to match.
    /// </summary>
    public class ParameterTypeMatchData : MatchData
    {
        private const string kindPropertyName = "parameterKind";

        /// <summary>
        /// Constructs a new <see cref="ParameterTypeMatchData"/> instance.
        /// </summary>
        public ParameterTypeMatchData()
        {
        }

        /// <summary>
        /// Constructs a new <see cref="ParameterTypeMatchData"/> instance.
        /// </summary>
        /// <param name="match">Parameter type to match. Kind is InputOrOutput.</param>
        public ParameterTypeMatchData(string match) : base(match)
        {
        }

        /// <summary>
        /// Constructs a new <see cref="ParameterTypeMatchData"/> instance.
        /// </summary>
        /// <param name="match">Parameter type to match.</param>
        /// <param name="kind"><see cref="ParameterKind"/> to match.</param>
        public ParameterTypeMatchData( string match, ParameterKind kind ) : base( match )
        {
            ParameterKind = kind;
        }

        /// <summary>
        /// Constructs a new <see cref="ParameterTypeMatchData"/> instance.
        /// </summary>
        /// <param name="match">Parameter type to match.</param>
        /// <param name="kind"><see cref="ParameterKind"/> to match.</param>
        /// <param name="ignoreCase">If false, type name comparisons are case sensitive. If true, 
        /// comparisons are case insensitive.</param>
        public ParameterTypeMatchData(string match, ParameterKind kind, bool ignoreCase) : base(match, ignoreCase)
        {
            ParameterKind = kind;
        }

        /// <summary>
        /// What kind of parameter is this? See <see cref="ParameterKind"/> for available values.
        /// </summary>
        /// <value>The "parameterKind" config attribute.</value>
        [ConfigurationProperty(kindPropertyName, IsRequired = false, DefaultValue = ParameterKind.InputOrOutput)]
        public ParameterKind ParameterKind
        {
            get { return (ParameterKind) base[kindPropertyName]; }
            set { base[kindPropertyName] = value; }
        }
    }

    /// <summary>
    /// Helper class used by ObjectBuilder to construct a <see cref="ParameterTypeMatchingRule"/>
    /// instance from a <see cref="ParameterTypeMatchData"/> instance.
    /// </summary>
    public class ParameterTypeMatchingRuleAssembler : IAssembler< IMatchingRule, MatchingRuleData>
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
        public IMatchingRule Assemble(IBuilderContext context, MatchingRuleData objectConfiguration,
            IConfigurationSource configurationSource,
            ConfigurationReflectionCache reflectionCache)
        {
            ParameterTypeMatchingRuleData ruleData = (ParameterTypeMatchingRuleData) objectConfiguration;
            ParameterTypeMatchingRule matchingRule =
                new ParameterTypeMatchingRule(ConvertFromConfigToRuntimeInfo(ruleData));
            return matchingRule;
        }

        private IEnumerable<ParameterTypeMatchingInfo> ConvertFromConfigToRuntimeInfo(ParameterTypeMatchingRuleData ruleData)
        {
            foreach(ParameterTypeMatchData matchData in ruleData.Matches)
            {
                yield return
                    new ParameterTypeMatchingInfo(matchData.Match, matchData.IgnoreCase, matchData.ParameterKind);
            }
        }
    }
}
