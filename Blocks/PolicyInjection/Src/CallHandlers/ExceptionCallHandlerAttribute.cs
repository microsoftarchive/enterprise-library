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
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers
{
    /// <summary>
    /// An attribute used to apply the <see cref="ExceptionCallHandler"/> to the target.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Method)]
    public class ExceptionCallHandlerAttribute : HandlerAttribute
    {
        private string policyName;

        /// <summary>
        /// Creates a new <see cref="ExceptionCallHandlerAttribute"/> using the given
        /// exception policy name.
        /// </summary>
        /// <remarks>When using this attribute, the exception policy is always read from
        /// the default configuration.</remarks>
        /// <param name="policyName">Exception policy name from configuration.</param>
        public ExceptionCallHandlerAttribute(string policyName)
        {
            this.policyName = policyName;
        }

        /// <summary>
        /// Get or sets the exception policy used by the handler.
        /// </summary>
        /// <value>exception policy name.</value>
        public string PolicyName
        {
            get { return policyName; }
            set { policyName = value; }
        }


        /// <summary>
        /// Derived classes implement this method. When called, it
        /// creates a new call handler as specified in the attribute
        /// configuration.
        /// </summary>
        /// <returns>A new call handler object.</returns>
        public override ICallHandler CreateHandler(IUnityContainer ignored)
        {
            return new ExceptionCallHandler(policyName, Order);
        }
    }
}
