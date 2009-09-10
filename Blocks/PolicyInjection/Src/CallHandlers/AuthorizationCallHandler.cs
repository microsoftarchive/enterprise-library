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
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers
{
    /// <summary>
    /// An <see cref="ICallHandler"/> that checks the Security Application Block for authorization
    /// before permitting the call to proceed to the target.
    /// </summary>
    [ConfigurationElementType(typeof(AuthorizationCallHandlerData))]
    public class AuthorizationCallHandler : ICallHandler
    {
        /// <summary>
        /// Constructs a new <see cref="AuthorizationCallHandler"/> that checks using the given
        /// information.
        /// </summary>
        /// <param name="provider">Authorization provider.</param>
        /// <param name="operationName">Operation name to use to check authorization rules.</param>
        /// <param name="order">Order in which the handler will be executed.</param>
        public AuthorizationCallHandler(IAuthorizationProvider provider, string operationName, int order)
        {
            this.AutorizationProvider = provider;
            this.OperationName = operationName;
            this.Order = order;
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
            ReplacementFormatter formatter = new MethodInvocationFormatter(input);
            if (!this.AutorizationProvider.Authorize(Thread.CurrentPrincipal, formatter.Format(OperationName)))
            {
                UnauthorizedAccessException unauthorizedExeption =
                    new UnauthorizedAccessException(Resources.AuthorizationFailed);
                return input.CreateExceptionMethodReturn(unauthorizedExeption);
            }

            return getNext().Invoke(input, getNext);
        }

        /// <summary>
        /// Gets the authorization provider to use when performing authorizations.
        /// </summary>
        public IAuthorizationProvider AutorizationProvider
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the security operation name to check with.
        /// </summary>
        /// <remarks>The operation name can include tokens. See the <see cref="MethodInvocationFormatter"/> for the list.
        /// </remarks>
        /// <value>operation name.</value>
        public string OperationName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the order in which the handler will be executed
        /// </summary>
        public int Order
        {
            get;
            set;
        }
    }
}
