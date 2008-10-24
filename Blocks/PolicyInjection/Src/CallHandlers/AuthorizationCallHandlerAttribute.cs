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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers
{
    /// <summary>
    /// Attribute that hooks up the <see cref="AuthorizationCallHandler"/> to the
    /// target.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Method)]
    public class AuthorizationCallHandlerAttribute : HandlerAttribute
    {
        private string operationName;
        private string providerName;

        /// <summary>
        /// Creates a new <see cref="AuthorizationCallHandlerAttribute"/> that uses the given
        /// operation name.
        /// </summary>
        /// <remarks>The operation name may include replacement tokens. See <see cref="MethodInvocationFormatter"/>
        /// for the list of tokens.</remarks>
        /// <param name="operationName">Operation name to use for checking.</param>
        public AuthorizationCallHandlerAttribute(string operationName)
        {
            this.operationName = operationName;
            this.providerName = string.Empty;
        }

        /// <summary>
        /// Operation name.
        /// </summary>
        /// <value>operation name.</value>
        public string OperationName
        {
            get { return operationName; }
            set { operationName = value; }
        }

        /// <summary>
        /// Security provider name.
        /// </summary>
        /// <value>Security Provider name. Defaults to the default security provider.</value>
        public string ProviderName
        {
            get { return providerName; }
            set { providerName = value; }
        }

        /// <summary>
        /// Derived classes implement this method. When called, it
        /// creates a new call handler as specified in the attribute
        /// configuration.
        /// </summary>
        /// <returns>A new call handler object.</returns>
        public override ICallHandler CreateHandler(IUnityContainer ignored)
        {
            return new AuthorizationCallHandler(providerName,
                operationName,
                ConfigurationSourceFactory.Create(), Order);
        }
    }
}
