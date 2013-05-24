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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A <see cref="ConfigurationElement"/> that maps the information about
    /// a policy from the configuration source.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "PolicyDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "PolicyDataDisplayName")]
    [ViewModel(PolicyInjectionDesignTime.ViewModelTypeNames.PolicyDataViewModel)]
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
        [ConfigurationCollection(typeof(MatchingRuleData))]
        [ResourceDescription(typeof(DesignResources), "PolicyDataMatchingRulesDescription")]
        [ResourceDisplayName(typeof(DesignResources), "PolicyDataMatchingRulesDisplayName")]
        [PromoteCommands]
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
        [ConfigurationCollection(typeof(CallHandlerData))]
        [ResourceDescription(typeof(DesignResources), "PolicyDataHandlersDescription")]
        [ResourceDisplayName(typeof(DesignResources), "PolicyDataHandlersDisplayName")]
        [PromoteCommands]
        public NameTypeConfigurationElementCollection<CallHandlerData, CustomCallHandlerData> Handlers
        {
            get { return (NameTypeConfigurationElementCollection<CallHandlerData, CustomCallHandlerData>)base[HandlersPropertyName]; }
            set { base[HandlersPropertyName] = value; }
        }

        /// <summary>
        /// Adds the policy represented by the configuration object to a container.
        /// </summary>
        /// <param name="container">The container to add the policy to.</param>
        public void ConfigureContainer(IUnityContainer container)
        {
            var matchingRuleNames = new List<string>();
            var callHandlerNames = new List<string>();

            var nameSuffix = "-" + this.Name;

            foreach (var matchingRuleData in this.MatchingRules)
            {
                matchingRuleNames.Add(matchingRuleData.ConfigureContainer(container, nameSuffix));
            }

            foreach (var callHandlerData in this.Handlers)
            {
                callHandlerNames.Add(callHandlerData.ConfigureContainer(container, nameSuffix));
            }

            container.RegisterType<InjectionPolicy, RuleDrivenPolicy>(
                this.Name,
                new InjectionConstructor(
                    this.Name,
                    new ResolvedArrayParameter<IMatchingRule>(matchingRuleNames.Select(n => (object)new ResolvedParameter<IMatchingRule>(n)).ToArray()),
                    callHandlerNames.ToArray()));
        }
    }
}
