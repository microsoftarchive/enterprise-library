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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Utilities;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection
{
    /// <summary>
    /// A <cref see="T:MatchingRuleSet"/> is a matching rule that
    /// is a collection of other matching rules. All the contained
    /// rules much match for the set to match.
    /// </summary>
    public class MatchingRuleSet : CollectionEx<IMatchingRule>, IMatchingRule
    {
        /// <summary>
        /// Tests the given member against the ruleset. The member matches
        /// if all contained rules in the ruleset match against it.
        /// </summary>
        /// <remarks>If the ruleset is empty, then Matches passes since no rules failed.</remarks>
        /// <param name="member">MemberInfo to test.</param>
        /// <returns>true if all contained rules match, false if any fail.</returns>
        public bool Matches(MethodBase member)
        {
            if(Count == 0 )
            {
                return false;
            }
            return !Exists(delegate(IMatchingRule rule) { return !rule.Matches(member); });
        }
    }
}