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
    /// A configuration element class that stores config information for instances
    /// of <see cref="PropertyMatchingRule"/>.
    /// </summary>
    [Assembler(typeof(PropertyMatchingRuleAssembler))]
    public class PropertyMatchingRuleData : MatchingRuleData
    {
        private const string MatchesPropertyName = "matches";

        /// <summary>
        /// Constructs a new <see cref="PropertyMatchingRuleData"/> instance.
        /// </summary>
        public PropertyMatchingRuleData()
        {
        }

        /// <summary>
        /// Constructs a new <see cref="PropertyMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule instance name in configuration.</param>
        public PropertyMatchingRuleData(string matchingRuleName)
            :this(matchingRuleName, new List<PropertyMatchData>())
        {
        }

        /// <summary>
        /// Constructs a new <see cref="PropertyMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule instance name in configuration.</param>
        /// <param name="matches">Collection of <see cref="PropertyMatchData"/> containing
        /// property patterns to match.</param>
        public PropertyMatchingRuleData(string matchingRuleName, IEnumerable<PropertyMatchData> matches) 
            : base(matchingRuleName, typeof(PropertyMatchingRule))
        {
            foreach(PropertyMatchData match in matches)
            {
                Matches.Add(match);
            }
        }

        /// <summary>
        /// The collection of <see cref="PropertyMatchData"/> containing property names to match.
        /// </summary>
        /// <value>The "matches" config subelement.</value>
        [ConfigurationProperty(MatchesPropertyName)]
        [ConfigurationCollection(typeof(PropertyMatchData))]
        public MatchDataCollection<PropertyMatchData> Matches
        {
            get { return (MatchDataCollection<PropertyMatchData>)base[MatchesPropertyName]; }
            set { base[MatchesPropertyName] = value; }
        }
    }

    /// <summary>
    /// A derived <see cref="MatchData"/> which adds storage for which methods
    /// on the property to match.
    /// </summary>
    public class PropertyMatchData : MatchData
    {
        private const string OptionPropertyName = "matchOption";

        /// <summary>
        /// Constructs a new <see cref="PropertyMatchData"/> instance.
        /// </summary>
        public PropertyMatchData()
        {
        }

        /// <summary>
        /// Constructs a new <see cref="PropertyMatchData"/> instance.
        /// </summary>
        /// <param name="match">Property name pattern to match. The rule will match both getter and setter methods of a property.</param>
        public PropertyMatchData(string match) : base(match)
        {
        }

        /// <summary>
        /// Construct a new <see cref="PropertyMatchData"/> instance.
        /// </summary>
        /// <param name="match">Property name pattern to match.</param>
        /// <param name="option">Which of the property methods to match. See <see cref="PropertyMatchingOption"/>
        /// for the valid options.</param>
        public PropertyMatchData( string match, PropertyMatchingOption option ) : base(match)
        {
            MatchOption = option;

        }

        /// <summary>
        /// Construct a new <see cref="PropertyMatchData"/> instance.
        /// </summary>
        /// <param name="match">Property name pattern to match.</param>
        /// <param name="option">Which of the property methods to match. See <see cref="PropertyMatchingOption"/>
        /// for the valid options.</param>
        /// <param name="ignoreCase">If false, type name comparisons are case sensitive. If true, 
        /// comparisons are case insensitive.</param>
        public PropertyMatchData(string match, PropertyMatchingOption option, bool ignoreCase)
            : base(match, ignoreCase)
        {
            MatchOption = option;
        }

        /// <summary>
        /// Which methods of the property to match. Default is to match both getters and setters.
        /// </summary>
        /// <value>The "matchOption" config attribute.</value>
        [ConfigurationProperty(OptionPropertyName, DefaultValue = PropertyMatchingOption.GetOrSet, IsRequired = false)]
        public PropertyMatchingOption MatchOption
        {
            get { return (PropertyMatchingOption)base[OptionPropertyName]; }
            set { base[OptionPropertyName] = value; }
        }
    }

    /// <summary>
    /// A class used by ObjectBuilder to construct instances of <see cref="PropertyMatchingRule"/>
    /// from instances of <see cref="PropertyMatchingRuleData"/>.
    /// </summary>
    public class PropertyMatchingRuleAssembler : IAssembler<IMatchingRule, MatchingRuleData>
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
        public IMatchingRule Assemble(
            IBuilderContext context,
            MatchingRuleData objectConfiguration,
            IConfigurationSource configurationSource,
            ConfigurationReflectionCache reflectionCache)
        {
            PropertyMatchingRuleData propertyData = (PropertyMatchingRuleData)objectConfiguration;
            List<PropertyMatchingInfo> info = new List<PropertyMatchingInfo>();
            foreach(PropertyMatchData data in propertyData.Matches)
            {
                info.Add(new PropertyMatchingInfo(data.Match, data.MatchOption, data.IgnoreCase));
            }

            PropertyMatchingRule rule = new PropertyMatchingRule(info);
            return rule;
        }
    }
}
