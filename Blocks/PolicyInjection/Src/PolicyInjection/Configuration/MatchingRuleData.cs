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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

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
            :base(matchingRuleName, matchingRuleType)
        {
        }
    }
}
