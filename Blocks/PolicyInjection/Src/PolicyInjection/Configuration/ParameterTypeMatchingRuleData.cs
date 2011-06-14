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
using System.Globalization;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Properties;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A class storing configuration information for an instance of
    /// <see cref="ParameterTypeMatchingRule"/>.
    /// </summary>
    public partial class ParameterTypeMatchingRuleData : MatchingRuleData
    {
        private static IEnumerable<ParameterTypeMatchingInfo> ConvertFromConfigToRuntimeInfo(
            ParameterTypeMatchingRuleData ruleData)
        {
            foreach (ParameterTypeMatchData matchData in ruleData.Matches)
            {
                yield return
                    new ParameterTypeMatchingInfo(matchData.Match, matchData.IgnoreCase, matchData.ParameterKind);
            }
        }

        /// <summary>
        /// Get the set of <see cref="TypeRegistration"/> objects needed to
        /// register the matching rule represented by this config element and its associated objects.
        /// </summary>
        /// <param name="nameSuffix">A suffix for the names in the generated type registration objects.</param>
        /// <returns>The set of <see cref="TypeRegistration"/> objects.</returns>
        public override IEnumerable<TypeRegistration> GetRegistrations(string nameSuffix)
        {
            if (this.Matches.Any(matche => string.IsNullOrEmpty(matche.Match)))
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, Resources.ErrorParameterTypeMatchingRuleMatchNotSet, this.Name));
            }

            yield return
                new TypeRegistration<IMatchingRule>(
                    () => new ParameterTypeMatchingRule(ConvertFromConfigToRuntimeInfo(this).ToArray()))
                {
                    Name = this.Name + nameSuffix,
                    Lifetime = TypeRegistrationLifetime.Transient
                };
        }
    }

    /// <summary>
    /// An extended <see cref="MatchData"/> class that also includes the
    /// <see cref="ParameterKind"/> of the parameter to match.
    /// </summary>
    public partial class ParameterTypeMatchData : MatchData
    {
    }
}
