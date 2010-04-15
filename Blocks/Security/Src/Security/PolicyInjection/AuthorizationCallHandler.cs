//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Properties;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.Security.PolicyInjection
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
            this.AuthorizationProvider = provider;
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
            if (input == null) throw new ArgumentNullException("input");
            if (getNext == null) throw new ArgumentNullException("getNext");

            ReplacementFormatter formatter = new MethodInvocationFormatter(input);
            if (!this.AuthorizationProvider.Authorize(Thread.CurrentPrincipal, formatter.Format(OperationName)))
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
        public IAuthorizationProvider AuthorizationProvider
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
