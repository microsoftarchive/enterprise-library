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
using System.Configuration;
using System.Globalization;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Properties;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A data class that maps the information about
    /// a policy from the configuration source.
    /// </summary>
    public partial class PolicyData 
    {
        /// <summary>
        /// Creates a new <see cref="PolicyData"/> with no name.
        /// </summary>
        public PolicyData()
            : base()
        {
        }

        /// <summary>
        /// Get the set of <see cref="TypeRegistration"/> objects needed to
        /// register the policy represented by this config element and its associated objects.
        /// </summary>
        /// <returns>The set of <see cref="TypeRegistration"/> objects.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public IEnumerable<TypeRegistration> GetRegistrations()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                throw new InvalidOperationException(Resources.ErrorPolicyNameNotSet);
            }

            List<TypeRegistration> registrations = new List<TypeRegistration>();
            List<string> matchingRuleNames = new List<string>();
            List<string> callHandlerNames = new List<string>();

            var nameSuffix = "-" + this.Name;

            foreach (var matchingRuleData in this.MatchingRules)
            {
                var matchingRuleRegistrations = matchingRuleData.GetRegistrations(nameSuffix);

                registrations.AddRange(matchingRuleRegistrations);
                matchingRuleNames.AddRange(
                    matchingRuleRegistrations.Where(tr => tr.ServiceType == typeof(IMatchingRule)).Select(tr => tr.Name));
            }

            foreach (var callHandlerData in this.Handlers)
            {
                var callHandlerRegistrations = callHandlerData.GetRegistrations(nameSuffix);

                registrations.AddRange(callHandlerRegistrations);
                callHandlerNames.AddRange(
                    callHandlerRegistrations.Where(tr => tr.ServiceType == typeof(ICallHandler)).Select(tr => tr.Name));
            }
            
            registrations.Add(
                new TypeRegistration<InjectionPolicy>(
                    () =>
                        new InjectionFriendlyRuleDrivenPolicy(
                            this.Name,
                            Container.ResolvedEnumerable<IMatchingRule>(matchingRuleNames),
                            callHandlerNames.ToArray()))
                {
                    Name = this.Name,
                    Lifetime = TypeRegistrationLifetime.Transient
                });

            return registrations;
        }
    }
}
