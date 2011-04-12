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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.Unity.InterceptionExtension;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A configuration element base class that stores configuration information about a matching rule.
    /// </summary>
    public partial class MatchingRuleData : NamedConfigurationElement
    {
        /// <summary>
        /// Creates a new <see cref="MatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Name of the rule in config.</param>
        public MatchingRuleData(string matchingRuleName)
            : base(matchingRuleName)
        {
            Name = matchingRuleName;
        }
    }
}
