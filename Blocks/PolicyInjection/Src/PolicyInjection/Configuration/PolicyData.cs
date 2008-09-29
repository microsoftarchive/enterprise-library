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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.Unity;
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
        /// Adds to the <paramref name="container"/> the policy definition represented by the configuration element.
        /// </summary>
        /// <param name="container">The container on which the injection policies must be configured.</param>
        /// <param name="configurationSource">The configuration source from which additional information
        /// can be retrieved, if necessary.</param>
        internal void ConfigureContainer(IUnityContainer container, IConfigurationSource configurationSource)
        {
            Interception interception = container.Configure<Interception>();
            if (interception == null)
            {
                interception = new Interception();
                container.AddExtension(interception);
            }

            var policy = interception.AddPolicy(this.Name);

            foreach (var ruleData in this.MatchingRules)
            {
                ruleData.ConfigurePolicy(policy, configurationSource);
            }

            foreach (var handlerData in this.Handlers)
            {
                handlerData.ConfigurePolicy(policy, configurationSource);
            }

            // make it a singleton - no API support for this.
            container.RegisterType<RuleDrivenPolicy>(this.Name, new ContainerControlledLifetimeManager());
        }
    }
}
