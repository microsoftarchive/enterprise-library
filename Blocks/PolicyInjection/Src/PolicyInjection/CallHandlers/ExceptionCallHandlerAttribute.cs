//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
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

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.PolicyInjection
{
    /// <summary>
    /// Applies the <see cref="ExceptionCallHandler"/> to the target.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Method)]
    public class ExceptionCallHandlerAttribute : HandlerAttribute
    {
        private string policyName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionCallHandlerAttribute"/> class using the given
        /// exception policy name.
        /// </summary>
        /// <remarks>When using this attribute, the exception policy is always read from
        /// the default configuration.</remarks>
        /// <param name="policyName">The name of the exception policy name from configuration.</param>
        public ExceptionCallHandlerAttribute(string policyName)
        {
            this.policyName = policyName;
        }

        /// <summary>
        /// Get or sets the name of the exception policy that is used by the handler.
        /// </summary>
        /// <value>The name of the exception policy.</value>
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
        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            var handler = new ExceptionCallHandler(this.PolicyName);
            handler.Order = this.Order;

            return handler;
        }
    }
}
