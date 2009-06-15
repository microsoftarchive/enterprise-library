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

using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// Dependency injection-friendlier subclass of <see cref="Microsoft.Practices.Unity.InterceptionExtension.RuleDrivenPolicy"/>
    /// </summary>
    public class InjectionFriendlyRuleDrivenPolicy : Microsoft.Practices.Unity.InterceptionExtension.RuleDrivenPolicy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InjectionFriendlyRuleDrivenPolicy"/> class.
        /// </summary>
        public InjectionFriendlyRuleDrivenPolicy(string name, IEnumerable<IMatchingRule> matchingRules, string[] callHandlerNames)
            : base(name, matchingRules.ToArray(), callHandlerNames)
        { }
    }
}
