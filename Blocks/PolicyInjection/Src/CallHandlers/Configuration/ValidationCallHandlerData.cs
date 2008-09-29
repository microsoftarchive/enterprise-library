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
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration
{
    /// <summary>
    /// A configuration element class that stores the config data for
    /// the <see cref="ValidationCallHandler"/>.
    /// </summary>
    public class ValidationCallHandlerData : CallHandlerData
    {
        private const string RuleSetPropertyName = "ruleSet";
        private const string SpecificationSourcePropertyName = "specificationSource";

        /// <summary>
        /// Create a new <see cref="ValidationCallHandlerData"/> instance.
        /// </summary>
        public ValidationCallHandlerData()
            : base()
        {
        }

        /// <summary>
        /// Create a new <see cref="ValidationCallHandlerData"/> instance.
        /// </summary>
        /// <param name="handlerName">Name of handler in configuration.</param>
        public ValidationCallHandlerData(string handlerName)
            : base(handlerName, typeof(ValidationCallHandler))
        {
        }

        /// <summary>
        /// Create a new <see cref="ValidationCallHandlerData"/> instance.
        /// </summary>
        /// <param name="handlerName">Name of handler in configuration.</param>
        /// <param name="handlerOrder">Order of handler in configuration.</param>
        public ValidationCallHandlerData(string handlerName, int handlerOrder)
            : base(handlerName, typeof(ValidationCallHandler))
        {
            this.Order = handlerOrder;
        }

        /// <summary>
        /// The ruleset name to use for all types. Empty string means default ruleset 
        /// </summary>
        /// <value>The "ruleSet" configuration property.</value>
        [ConfigurationProperty(RuleSetPropertyName)]
        public string RuleSet
        {
            get { return (string)base[RuleSetPropertyName]; }
            set { base[RuleSetPropertyName] = value; }
        }

        /// <summary>
        /// SpecificationSource (Both | Attributes | Configuration) : Where to look for validation rules. Default is Both.
        /// </summary>
        /// <value>The "specificationSource" configuration attribute.</value>
        [ConfigurationProperty(SpecificationSourcePropertyName, IsRequired = true, DefaultValue = SpecificationSource.Both)]
        public SpecificationSource SpecificationSource
        {
            get { return (SpecificationSource)base[SpecificationSourcePropertyName]; }
            set { base[SpecificationSourcePropertyName] = value; }
        }

        /// <summary>
        /// Adds the call handler represented by this configuration object to <paramref name="policy"/>.
        /// </summary>
        /// <param name="policy">The policy to which the rule must be added.</param>
        /// <param name="configurationSource">The configuration source from which additional information
        /// can be retrieved, if necessary.</param>
        public override void ConfigurePolicy(PolicyDefinition policy, IConfigurationSource configurationSource)
        {
            policy.AddCallHandler<ValidationCallHandler>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new InjectionParameter<string>(this.RuleSet),
                    new InjectionParameter<SpecificationSource>(this.SpecificationSource)),
                new InjectionProperty("Order", new InjectionParameter<int>(this.Order)));
        }
    }
}
