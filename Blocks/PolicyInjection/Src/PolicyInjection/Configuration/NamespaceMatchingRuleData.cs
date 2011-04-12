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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// Configuration element that stores the configuration information for an instance
    /// of <see cref="NamespaceMatchingRule"/>.
    /// </summary>
    public partial class NamespaceMatchingRuleData : MatchingRuleData
    {
        /// <summary>
        /// Constructs a new <see cref="NamespaceMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule name in configuration file.</param>
        public NamespaceMatchingRuleData(string matchingRuleName)
            : this(matchingRuleName, new List<MatchData>())
        {
        }

        /// <summary>
        /// Constructs a new <see cref="NamespaceMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="matchingRuleName">Matching rule name in configuration file.</param>
        /// <param name="namespaceName">Namespace pattern to match.</param>
        public NamespaceMatchingRuleData(string matchingRuleName, string namespaceName)
            : this(matchingRuleName, new MatchData[] { new MatchData(namespaceName) })
        {
        }

        /// <summary>
        /// Get the set of <see cref="TypeRegistration"/> objects needed to
        /// register the matching rule represented by this config element and its associated objects.
        /// </summary>
        /// <param name="nameSuffix">A suffix for the names in the generated type registration objects.</param>
        /// <returns>The set of <see cref="TypeRegistration"/> objects.</returns>
        public override IEnumerable<TypeRegistration> GetRegistrations(string nameSuffix)
        {
            yield return
                new TypeRegistration<IMatchingRule>(() =>
                    new NamespaceMatchingRule(
                        this.Matches.Select(match => new MatchingInfo(match.Match, match.IgnoreCase)).ToArray()))
                {
                    Name = this.Name + nameSuffix,
                    Lifetime = TypeRegistrationLifetime.Transient
                };
        }
    }
}
