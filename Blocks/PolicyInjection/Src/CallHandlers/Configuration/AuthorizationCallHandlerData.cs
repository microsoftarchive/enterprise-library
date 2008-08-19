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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration
{
    /// <summary>
    /// Call handler data describing the information for the authorization call handler
    /// in configuration.
    /// </summary>
    [Assembler(typeof(AuthorizationCallHandlerAssembler))]
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
            :base(handlerName, typeof(AuthorizationCallHandler))
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
            get { return(string) base[AuthorizationProviderPropertyName]; }
            set { base[AuthorizationProviderPropertyName] = value; }
        }

        /// <summary>
        /// Operation name to use for this call handler.
        /// </summary>
        /// <value>The "operationName" attribute.</value>
        [ConfigurationProperty(OperationNamePropertyName, IsRequired=true)]
        public string OperationName
        {
            get { return (string)base[OperationNamePropertyName]; }
            set { base[OperationNamePropertyName] = value; }
        }
    }

    /// <summary>
    /// A class used by ObjectBuilder to take a <see cref="AuthorizationCallHandlerData"/> object
    /// and build the corresponding <see cref="AuthorizationCallHandler"/>.
    /// </summary>
    public class AuthorizationCallHandlerAssembler : IAssembler<ICallHandler, CallHandlerData>
    {
        /// <summary>
        /// Create the call handler.
        /// </summary>
        /// <param name="context">ObjectBuilder context.</param>
        /// <param name="objectConfiguration">The call handler data.</param>
        /// <param name="configurationSource">Configuration source.</param>
        /// <param name="reflectionCache">ObjectBuild reflection cache.</param>
        /// <returns>The constructed call handler.</returns>
        public ICallHandler Assemble(IBuilderContext context, CallHandlerData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            AuthorizationCallHandlerData castedConfiguration = (AuthorizationCallHandlerData)objectConfiguration;

            AuthorizationCallHandler callHandler = new AuthorizationCallHandler(castedConfiguration.AuthorizationProvider, 
                castedConfiguration.OperationName, 
                configurationSource, 
                objectConfiguration.Order);

            return callHandler;
        }
    }
}
