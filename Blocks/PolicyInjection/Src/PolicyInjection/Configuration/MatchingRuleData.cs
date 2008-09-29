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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A configuration element base class that stores config information about a matching rule.
    /// </summary>
    public class MatchingRuleData : NameTypeConfigurationElement
    {
        /// <summary>
        /// Creates a new <see cref="MatchingRuleData"/> instance.
        /// </summary>
        public MatchingRuleData()
        {
        }

        /// <summary>
        /// Creates a new <see cref="MatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Name of the rule in config.</param>
        /// <param name="matchingRuleType">The underlying type of matching rule this object configures.</param>
        public MatchingRuleData(string matchingRuleName, Type matchingRuleType)
            : base(matchingRuleName, matchingRuleType)
        {
        }

        /// <summary>
        /// Adds the rule represented by this configuration object to <paramref name="policy"/>.
        /// </summary>
        /// <param name="policy">The policy to which the rule must be added.</param>
        /// <param name="configurationSource">The configuration source from which additional information
        /// can be retrieved, if necessary.</param>
        public virtual void ConfigurePolicy(PolicyDefinition policy, IConfigurationSource configurationSource)
        {
            throw new NotImplementedException();
        }
    }
}
