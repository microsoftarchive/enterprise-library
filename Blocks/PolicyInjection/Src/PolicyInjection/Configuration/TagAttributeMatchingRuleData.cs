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
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using FakeRules = Microsoft.Practices.EnterpriseLibrary.PolicyInjection.MatchingRules;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A configuration element that stores configuration information for
    /// an instance of <see cref="TagAttributeMatchingRule"/>.
    /// </summary>
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
            : base(matchingRuleName, tagToMatch, typeof(FakeRules.TagAttributeMatchingRule))
        {
        }

        /// <summary>
        /// Adds the rule represented by this configuration object to <paramref name="policy"/>.
        /// </summary>
        /// <param name="policy">The policy to which the rule must be added.</param>
        /// <param name="configurationSource">The configuration source from which additional information
        /// can be retrieved, if necessary.</param>
        public override void ConfigurePolicy(PolicyDefinition policy, IConfigurationSource configurationSource)
        {
            policy.AddMatchingRule<TagAttributeMatchingRule>(
                new InjectionConstructor(
                    new InjectionParameter<string>(this.Match),
                    new InjectionParameter<bool>(this.IgnoreCase)));
        }
    }
}
