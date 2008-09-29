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
    /// Call handler data describing the information for the authorization call handler
    /// in configuration.
    /// </summary>
    public class AuthorizationCallHandlerData : CallHandlerData
    {
        private const string AuthorizationProviderPropertyName = "authorizationProvider";
        private const string OperationNamePropertyName = "operationName";

        /// <summary>
        /// Create a new <see cref="AuthorizationCallHandlerData"/>.
        /// </summary>
        public AuthorizationCallHandlerData()
            : base()
        {
        }

        /// <summary>
        /// Create a new <see cref="AuthorizationCallHandlerData"/>.
        /// </summary>
        /// <param name="handlerName">Name of the call handler.</param>
        public AuthorizationCallHandlerData(string handlerName)
            : base(handlerName, typeof(AuthorizationCallHandler))
        {
        }

        /// <summary>
        /// Create a new <see cref="AuthorizationCallHandlerData"/>.
        /// </summary>
        /// <param name="handlerName">Name of the call handler.</param>
        /// <param name="handlerOrder">Order of the call handler.</param>
        public AuthorizationCallHandlerData(string handlerName, int handlerOrder)
            : base(handlerName, typeof(AuthorizationCallHandler))
        {
            this.Order = handlerOrder;
        }

        /// <summary>
        /// Authorization provider to use for this call handler.
        /// </summary>
        /// <value>The "authorizationProvider" attribute.</value>
        [ConfigurationProperty(AuthorizationProviderPropertyName)]
        public string AuthorizationProvider
        {
            get { return (string)base[AuthorizationProviderPropertyName]; }
            set { base[AuthorizationProviderPropertyName] = value; }
        }

        /// <summary>
        /// Operation name to use for this call handler.
        /// </summary>
        /// <value>The "operationName" attribute.</value>
        [ConfigurationProperty(OperationNamePropertyName, IsRequired = true)]
        public string OperationName
        {
            get { return (string)base[OperationNamePropertyName]; }
            set { base[OperationNamePropertyName] = value; }
        }

        /// <summary>
        /// Adds the call handler represented by this configuration object to <paramref name="policy"/>.
        /// </summary>
        /// <param name="policy">The policy to which the rule must be added.</param>
        /// <param name="configurationSource">The configuration source from which additional information
        /// can be retrieved, if necessary.</param>
        public override void ConfigurePolicy(PolicyDefinition policy, IConfigurationSource configurationSource)
        {
            policy.AddCallHandler<AuthorizationCallHandler>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new InjectionParameter<string>(this.AuthorizationProvider),
                    new InjectionParameter<string>(this.OperationName),
                    new InjectionParameter<IConfigurationSource>(configurationSource),
                    new InjectionParameter<int>(this.Order)));
        }
    }
}
