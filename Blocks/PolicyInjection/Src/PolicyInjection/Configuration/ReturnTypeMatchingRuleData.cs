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
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.MatchingRules;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A configuration element that stores config information about an
    /// instance of <see cref="ReturnTypeMatchingRule"/>.
    /// </summary>
    [Assembler(typeof(ReturnTypeMatchingRuleAssembler))]
    public class ReturnTypeMatchingRuleData : StringBasedMatchingRuleData
    {
        /// <summary>
        /// Constructs a new <see cref="ReturnTypeMatchingRuleData"/> instance.
        /// </summary>
        public ReturnTypeMatchingRuleData()
            :base()
        {
        }

        /// <summary>
        /// Constructs a new <see cref="ReturnTypeMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule instance name in configuration.</param>
        /// <param name="returnTypeName">Return type to match.</param>
        public ReturnTypeMatchingRuleData(string matchingRuleName, string returnTypeName)
            : base(matchingRuleName, returnTypeName, typeof(ReturnTypeMatchingRule))
        {
        }
    }

    /// <summary>
    /// A class used by ObjectBuilder to construct instance of <see cref="ReturnTypeMatchingRule"/>
    /// from instances of <see cref="ReturnTypeMatchingRuleData"/>.
    /// </summary>
    public class ReturnTypeMatchingRuleAssembler : IAssembler<IMatchingRule, MatchingRuleData>
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
            ReturnTypeMatchingRuleData castedRuleData = (ReturnTypeMatchingRuleData)objectConfiguration;

            ReturnTypeMatchingRule matchingRule = new ReturnTypeMatchingRule(castedRuleData.Match, castedRuleData.IgnoreCase);

            return matchingRule;
        }
    }
}
