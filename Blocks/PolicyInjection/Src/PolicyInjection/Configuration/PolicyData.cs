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
using System.Configuration;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A <see cref="ConfigurationElement"/> that maps the information about
    /// a policy from the configuration source.
    /// </summary>
    public class PolicyData : NamedConfigurationElement
    {
        private const string HandlersPropertyName = "handlers";
        private const string MatchingRulesPropertyName = "matchingRules";

        /// <summary>
        /// Creates a new <see cref="PolicyData"/> with the given name.
        /// </summary>
        /// <param name="policyName">Name of the policy.</param>
        public PolicyData(string policyName)
            : base(policyName)
        {
        }

        /// <summary>
        /// Creates a new <see cref="PolicyData"/> with no name.
        /// </summary>
        public PolicyData()
            : base()
        {
        }

        /// <summary>
        /// Gets or sets the collection of matching rules from configuration.
        /// </summary>
        /// <value>The matching rule data collection.</value>
        [ConfigurationProperty(MatchingRulesPropertyName)]
        public NameTypeConfigurationElementCollection<MatchingRuleData, CustomMatchingRuleData> MatchingRules
        {
            get { return (NameTypeConfigurationElementCollection<MatchingRuleData, CustomMatchingRuleData>)base[MatchingRulesPropertyName]; }
            set { base[MatchingRulesPropertyName] = value; }
        }

        /// <summary>
        /// Get or sets the collection of handlers from configuration.
        /// </summary>
        /// <value>The handler data collection.</value>
        [ConfigurationProperty(HandlersPropertyName)]
        public NameTypeConfigurationElementCollection<CallHandlerData, CustomCallHandlerData> Handlers
        {
            get { return (NameTypeConfigurationElementCollection<CallHandlerData, CustomCallHandlerData>)base[HandlersPropertyName]; }
            set { base[HandlersPropertyName] = value; }
        }

        /// <summary>
        /// Get the set of <see cref="TypeRegistration"/> objects needed to
        /// register the policy represented by this config element and its associated objects.
        /// </summary>
        /// <returns>The set of <see cref="TypeRegistration"/> objects.</returns>
        public IEnumerable<TypeRegistration> GetRegistrations()
        {
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
                        new RuleDrivenPolicy(
                            this.Name,
                            Container.ResolvedEnumerable<IMatchingRule>(matchingRuleNames),
                            callHandlerNames.ToArray()))
                {
                    Name = this.Name
                });

            return registrations;
        }

        /// <summary>
        /// Injection-friendlier subclass of <see cref="Microsoft.Practices.Unity.InterceptionExtension.RuleDrivenPolicy"/>.
        /// </summary>
        public class RuleDrivenPolicy : Microsoft.Practices.Unity.InterceptionExtension.RuleDrivenPolicy
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="RuleDrivenPolicy"/> class.
            /// </summary>
            public RuleDrivenPolicy(string name, IEnumerable<IMatchingRule> matchingRules, string[] callHandlerNames)
                : base(name, matchingRules.ToArray(), callHandlerNames)
            { }
        }
    }
}
