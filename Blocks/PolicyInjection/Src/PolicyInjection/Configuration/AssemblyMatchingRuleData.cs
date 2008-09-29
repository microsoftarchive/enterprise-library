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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using FakeRules = Microsoft.Practices.EnterpriseLibrary.PolicyInjection.MatchingRules;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// Configuration element for the <see cref="AssemblyMatchingRule"/>.
    /// </summary>
    public class AssemblyMatchingRuleData : MatchingRuleData
    {
        private const string MatchPropertyName = "match";

        /// <summary>
        /// Constructs an <see cref="AssemblyMatchingRuleData"/> with default settings.
        /// </summary>
        public AssemblyMatchingRuleData()
            : base()
        {
        }

        /// <summary>
        /// Constructs an <see cref="AssemblyMatchingRuleData"/> instance with the given
        /// rule name and assembly name pattern to match.
        /// </summary>
        /// <param name="matchingRuleName">Name of rule from the config file.</param>
        /// <param name="assemblyName">Assembly name to match.</param>
        public AssemblyMatchingRuleData(string matchingRuleName, string assemblyName)
            : base(matchingRuleName, typeof(FakeRules.AssemblyMatchingRule))
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

        /// <summary>
        /// Adds the rule represented by this configuration object to <paramref name="policy"/>.
        /// </summary>
        /// <param name="policy">The policy to which the rule must be added.</param>
        /// <param name="configurationSource">The configuration source from which additional information
        /// can be retrieved, if necessary.</param>
        public override void ConfigurePolicy(PolicyDefinition policy, IConfigurationSource configurationSource)
        {
            policy.AddMatchingRule<AssemblyMatchingRule>(
                new InjectionConstructor(new InjectionParameter<string>(this.Match)));
        }
    }
}
