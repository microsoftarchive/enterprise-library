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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection
{

    /// <summary>
    /// A factory class used by ObjectBuilder and the config system to construct
    /// <see cref="PolicySet"/> objects based on configuration.
    /// </summary>
    class PolicySetCustomFactory : ICustomFactory
    {
        /// <summary>
        /// Create the <see cref="PolicySet"/> based on the configuration settings.
        /// </summary>
        /// <param name="context">Builder context.</param>
        /// <param name="name">Name of object to create.</param>
        /// <param name="configurationSource">Configuration source.</param>
        /// <param name="reflectionCache">reflection cache, unused in this method.</param>
        /// <returns>The constructed <see cref="PolicySet"/> object.</returns>
        public object CreateObject(IBuilderContext context, string name, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            PolicyInjectionSettings injectionSettings = configurationSource.GetSection(PolicyInjectionSettings.SectionName) as PolicyInjectionSettings;
            if (injectionSettings == null)
            {
                return new PolicySet();
            }

            PolicySet policySet = new PolicySet();
            foreach (PolicyData policyData in injectionSettings.Policies)
            {
                RuleDrivenPolicy policy = new RuleDrivenPolicy(policyData.Name);

                foreach (CallHandlerData handlerData in policyData.Handlers)
                {
                    ICallHandler callHandler = CallHandlerCustomFactory.Instance.Create(context, handlerData, configurationSource, reflectionCache);
                    policy.Handlers.Add(callHandler);
                }

                foreach (MatchingRuleData matchingRuleData in policyData.MatchingRules)
                {
                    IMatchingRule matchingRule = MatchingRuleCustomFactory.Instance.Create(context, matchingRuleData, configurationSource, reflectionCache);
                    policy.RuleSet.Add(matchingRule);
                }
                policySet.Add(policy);
            }
            return policySet;
        }
    }

    /// <summary>
    /// A factory that creates <see cref="IMatchingRule"/> instances based on configuration.
    /// </summary>
    public class MatchingRuleCustomFactory : AssemblerBasedObjectFactory<IMatchingRule, MatchingRuleData>
    {
        /// <summary>
        /// Singleton instance of this factory.
        /// </summary>
        public static MatchingRuleCustomFactory Instance = new MatchingRuleCustomFactory();
    }

    /// <summary>
    /// A factory that creates <see cref="ICallHandler"/> instances based on configuration.
    /// </summary>
    public class CallHandlerCustomFactory : AssemblerBasedObjectFactory<ICallHandler, CallHandlerData>
    {
        /// <summary>
        /// Singleton instance of this factory.
        /// </summary>
        public static CallHandlerCustomFactory Instance = new CallHandlerCustomFactory();

        /// <summary>
        /// Returns a new instance of <typeparamref name="ICallHandler"/>, described by the matching configuration object 
        /// of a concrete subtype of <typeparamref name="CallHandlerData"/> in <paramref name="objectConfiguration"/>.
        /// </summary>
        /// <remarks>
        /// The created ICallHandler is assigned the Order received in the configuration object <typeparamref name="CallHandlerData"/>
        /// </remarks>
        /// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
        /// <param name="objectConfiguration">The configuration object that describes the object to build.</param>
        /// <param name="configurationSource">The source for configuration objects.</param>
        /// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
        /// <returns>A new instance of the appropriate subtype of <typeparamref name="Tobject"/>.</returns>
        public override ICallHandler Create(IBuilderContext context, CallHandlerData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            ICallHandler createdHandler = base.Create(context, objectConfiguration, configurationSource, reflectionCache);
            createdHandler.Order = objectConfiguration.Order;

            return createdHandler;
        }
    }
}
