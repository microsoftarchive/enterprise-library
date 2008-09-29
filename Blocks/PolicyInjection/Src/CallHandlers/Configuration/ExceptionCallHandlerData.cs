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
    /// Configuration element storing configuration information for the
    /// <see cref="ExceptionCallHandler"/> class.
    /// </summary>
    public class ExceptionCallHandlerData : CallHandlerData
    {
        private const string ExceptionPolicyNamePropertyName = "exceptionPolicyName";

        /// <summary>
        /// Construct a new <see cref="ExceptionCallHandlerData"/>.
        /// </summary>
        public ExceptionCallHandlerData()
        {
        }

        /// <summary>
        /// Construct a new <see cref="ExceptionCallHandlerData"/>.
        /// </summary>
        /// <param name="handlerName">Name of the handler.</param>
        public ExceptionCallHandlerData(string handlerName)
            : base(handlerName, typeof(ExceptionCallHandler))
        {
        }

        /// <summary>
        /// Construct a new <see cref="ExceptionCallHandlerData"/>.
        /// </summary>
        /// <param name="handlerName">Name of the handler.</param>
        /// <param name="exceptionPolicyName">Exception policy name to use in handler.</param>
        public ExceptionCallHandlerData(string handlerName, string exceptionPolicyName)
            : base(handlerName, typeof(ExceptionCallHandler))
        {
            ExceptionPolicyName = exceptionPolicyName;
        }

        /// <summary>
        /// Construct a new <see cref="ExceptionCallHandlerData"/>.
        /// </summary>
        /// <param name="handlerName">Name of the handler.</param>
        /// <param name="handlerOrder">Order to use in handler.</param>
        public ExceptionCallHandlerData(string handlerName, int handlerOrder)
            : base(handlerName, typeof(ExceptionCallHandler))
        {
            Order = handlerOrder;
        }

        /// <summary>
        /// The exception policy name as defined in configuration for the Exception Handling block.
        /// </summary>
        /// <value>The "exceptionPolicyName" attribute in configuration</value>
        [ConfigurationProperty(ExceptionPolicyNamePropertyName, IsRequired = true)]
        public string ExceptionPolicyName
        {
            get { return (string)base[ExceptionPolicyNamePropertyName]; }
            set { base[ExceptionPolicyNamePropertyName] = value; }
        }

        /// <summary>
        /// Adds the call handler represented by this configuration object to <paramref name="policy"/>.
        /// </summary>
        /// <param name="policy">The policy to which the rule must be added.</param>
        /// <param name="configurationSource">The configuration source from which additional information
        /// can be retrieved, if necessary.</param>
        public override void ConfigurePolicy(PolicyDefinition policy, IConfigurationSource configurationSource)
        {
            policy.AddCallHandler<ExceptionCallHandler>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new InjectionParameter<string>(this.ExceptionPolicyName),
                    new InjectionParameter<int>(this.Order)));
        }
    }
}
