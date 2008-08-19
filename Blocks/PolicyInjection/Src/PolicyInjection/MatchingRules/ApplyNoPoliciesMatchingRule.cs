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
using System.Reflection;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.MatchingRules
{
    /// <summary>
    /// A <see cref="IMatchingRule"/> implementation that fails to match
    /// if the method in question has the ApplyNoPolicies attribute on it.
    /// </summary>
    internal class ApplyNoPoliciesMatchingRule : IMatchingRule
    {
        /// <summary>
        /// Check if the <paramref name="member"/> matches this rule.
        /// </summary>
        /// <remarks>This rule returns true if the member does NOT have the <see cref="ApplyNoPoliciesAttribute"/>
        /// on it, or a containing type doesn't have the attribute.</remarks>
        /// <param name="member">Member to check.</param>
        /// <returns>True if the rule matches, false if it doesn't.</returns>
        public bool Matches(MethodBase member)
        {
            bool hasNoPoliciesAttribute =
                ( member.GetCustomAttributes(typeof(ApplyNoPoliciesAttribute), false).Length != 0 );

            hasNoPoliciesAttribute |=
                ( member.DeclaringType.GetCustomAttributes(typeof(ApplyNoPoliciesAttribute), false).
                    Length != 0 );
            return !hasNoPoliciesAttribute;
        }
    }
}
