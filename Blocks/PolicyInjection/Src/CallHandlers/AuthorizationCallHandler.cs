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
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Logging;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Properties;
using Microsoft.Practices.EnterpriseLibrary.Security;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers
{
    /// <summary>
    /// An <see cref="ICallHandler"/> that checks the Security Block for authorization
    /// before permitting the call to proceed to the target.
    /// </summary>
    [ConfigurationElementType(typeof(AuthorizationCallHandlerData))]
    public class AuthorizationCallHandler : ICallHandler
    {
        private string providerName;
        private string operationName;
        private IConfigurationSource configurationSource;
        private int order = 0;

        /// <summary>
        /// Constructs a new <see cref="AuthorizationCallHandler"/> that checks using the given
        /// information.
        /// </summary>
        /// <param name="providerName">Name of authorization provider.</param>
        /// <param name="operationName">Operation name to use to check authorization rules.</param>
        /// <param name="configurationSource">Configuration source to read authorization configuration from.</param>
        public AuthorizationCallHandler(string providerName, string operationName, IConfigurationSource configurationSource)
        {
            this.providerName = providerName;
            this.operationName = operationName;
            this.configurationSource = configurationSource;
        }

        /// <summary>
        /// Constructs a new <see cref="AuthorizationCallHandler"/> that checks using the given
        /// information.
        /// </summary>
        /// <param name="providerName">Name of authorization provider.</param>
        /// <param name="operationName">Operation name to use to check authorization rules.</param>
        /// <param name="configurationSource">Configuration source to read authorization configuration from.</param>
        /// <param name="order">Order in which the handler will be executed.</param>
        public AuthorizationCallHandler(string providerName, string operationName, IConfigurationSource configurationSource, int order)
        {
            this.providerName = providerName;
            this.operationName = operationName;
            this.configurationSource = configurationSource;
            this.order = order;
        }

        /// <summary>
        /// Gets or sets the order in which the handler will be executed
        /// </summary>
        public int Order
        {
            get
            {
                return order;
            }
            set
            {
                order = value;
            }
        }

        /// <summary>
        /// Performs the operation of the handler.
        /// </summary>
        /// <param name="input">Input to the method call.</param>
        /// <param name="getNext">Delegate used to get the next delegate in the call handler pipeline.</param>
        /// <returns>Returns value from the target method, or an <see cref="UnauthorizedAccessException"/>
        /// if the call fails the authorization check.</returns>
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            IAuthorizationProvider authProvider = GetAuthorizationProvider();
            ReplacementFormatter formatter = new MethodInvocationFormatter(input);
            if (!authProvider.Authorize(Thread.CurrentPrincipal, formatter.Format(operationName)))
            {
                UnauthorizedAccessException unauthorizedExeption = new UnauthorizedAccessException(Resources.AuthorizationFailed);
                return input.CreateExceptionMethodReturn(unauthorizedExeption);
            }

            return getNext().Invoke(input, getNext);
        }

        /// <summary>
        /// Gets or sets the security provider name.
        /// </summary>
        /// <value>security provider name.</value>
        public string ProviderName
        {
            get { return providerName; }
            set { providerName = value; }
        }

        /// <summary>
        /// Gets or sets the security operation name to check with.
        /// </summary>
        /// <remarks>The operation name can include tokens. See the <see cref="MethodInvocationFormatter"/> for the list.
        /// </remarks>
        /// <value>operation name.</value>
        public string OperationName
        {
            get { return operationName; }
            set { operationName = value; }
        }

        private IAuthorizationProvider GetAuthorizationProvider()
        {
            AuthorizationProviderFactory authorizationProviderFactory = new AuthorizationProviderFactory(configurationSource);
            if (string.IsNullOrEmpty(providerName))
            {
                return authorizationProviderFactory.CreateDefault();
            }
            else
            {
                return authorizationProviderFactory.Create(providerName);
            }
        }
    }
}
