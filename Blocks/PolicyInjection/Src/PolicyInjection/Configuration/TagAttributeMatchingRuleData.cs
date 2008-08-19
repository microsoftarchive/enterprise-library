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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.MatchingRules;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A configuration element that stores configuration information for
    /// an instance of <see cref="TagAttributeMatchingRule"/>.
    /// </summary>
    [Assembler(typeof(TagAttributeMatchingRuleAssembler))]
    public class TagAttributeMatchingRuleData : StringBasedMatchingRuleData
    {
        /// <summary>
        /// Constructs a new <see cref="TagAttributeMatchingRuleData"/> instance.
        /// </summary>
        public TagAttributeMatchingRuleData()
        {
        }

        /// <summary>
        /// Constructs a new <see cref="TagAttributeMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule instance name in configuration.</param>
        /// <param name="tagToMatch">Tag string to match.</param>
        public TagAttributeMatchingRuleData(string matchingRuleName, string tagToMatch)
            : base(matchingRuleName, tagToMatch, typeof(TagAttributeMatchingRule))
        {
        }
    }

    /// <summary>
    /// A class used by ObjectBuilder to construct instance of <see cref="TagAttributeMatchingRule"/>
    /// from instances of <see cref="TagAttributeMatchingRuleData"/>.
    /// </summary>
    public class TagAttributeMatchingRuleAssembler : IAssembler<IMatchingRule, MatchingRuleData>
    {
        /// <summary>
        /// Builds an instance of the subtype of IMatchingRule type the receiver knows how to build, based on 
        /// a configuration object.
        /// </summary>
        /// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
        /// <param name="objectConfiguration">The configuration object that describes the object to build.</param>
        /// <param name="configurationSource">The source for configuration objects.</param>
        /// <param name="reflectionCache">The cache to use for retrieving reflection information.</param>
        /// <returns>A fully initialized instance of the IMatchingRule subtype.</returns>
        public IMatchingRule Assemble(IBuilderContext context, MatchingRuleData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            TagAttributeMatchingRuleData castedRuleData = (TagAttributeMatchingRuleData)objectConfiguration;

            TagAttributeMatchingRule matchingRule = new TagAttributeMatchingRule(castedRuleData.Match, castedRuleData.IgnoreCase);

            return matchingRule;
        }
    }
}
