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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// Configuration element for the <see cref="AssemblyMatchingRule"/>.
    /// </summary>
    [Assembler(typeof(AssemblyMatchingRuleAssembler))]
    public class AssemblyMatchingRuleData : MatchingRuleData
    {
        private const string MatchPropertyName = "match";

        /// <summary>
        /// Constructs an <see cref="AssemblyMatchingRuleData"/> with default settings.
        /// </summary>
        public AssemblyMatchingRuleData()
            :base()
        {
        }

        /// <summary>
        /// Constructs an <see cref="AssemblyMatchingRuleData"/> instance with the given
        /// rule name and assembly name pattern to match.
        /// </summary>
        /// <param name="matchingRuleName">Name of rule from the config file.</param>
        /// <param name="assemblyName">Assembly name to match.</param>
        public AssemblyMatchingRuleData(string matchingRuleName, string assemblyName)
            : base(matchingRuleName, typeof(AssemblyMatchingRule))
        {
            Match = assemblyName;
        }

        /// <summary>
        /// The assembly name to match.
        /// </summary>
        /// <value>Assembly name to match.</value>
        [ConfigurationProperty(MatchPropertyName)]
        public string Match
        {
            get { return (string)base[MatchPropertyName]; }
            set { base[MatchPropertyName] = value; }
        }
    }

    /// <summary>
    /// Assembler class used to create the <see cref="AssemblyMatchingRule"/> from configuration data.
    /// </summary>
    public class AssemblyMatchingRuleAssembler : IAssembler<IMatchingRule, MatchingRuleData>
    {
        /// <summary>
        /// Create the matching rule from the configuration data.
        /// </summary>
        /// <param name="context">Build context.</param>
        /// <param name="objectConfiguration">Configuration element object from config file.</param>
        /// <param name="configurationSource">Source of the configuration information.</param>
        /// <param name="reflectionCache">Unused.</param>
        /// <returns>Constructed <see cref="AssemblyMatchingRule"/>.</returns>
        public IMatchingRule Assemble(IBuilderContext context, MatchingRuleData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            AssemblyMatchingRuleData castedRuleData = (AssemblyMatchingRuleData)objectConfiguration;

            AssemblyMatchingRule matchingRule = new AssemblyMatchingRule(castedRuleData.Match);

            return matchingRule;
        }
    }
}
