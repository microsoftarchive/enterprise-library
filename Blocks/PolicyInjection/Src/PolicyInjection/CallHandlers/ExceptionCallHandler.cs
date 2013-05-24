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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.PolicyInjection
{
    /// <summary>
    /// Represents an <see cref="ICallHandler"/> that runs any exceptions returned from the
    /// target through the Exception Handling Application Block.
    /// </summary>
    /// <remarks>If the exception policy is configured to swallow exceptions,
    /// do not configure this call handler on a method that has a non-void return value,
    /// because the handler doesn't know which value to return if the exception is swallowed.
    /// </remarks>
    [ConfigurationElementType(typeof(ExceptionCallHandlerData))]
    public class ExceptionCallHandler : ICallHandler
    {
        private string policyName;
        private int order = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionCallHandler"/> class that processses exceptions
        /// by using the exception policy with the given name.
        /// </summary>
        /// <param name="policyName">The name of the exception policy to use.</param>
        public ExceptionCallHandler(string policyName)
        {
            this.policyName = policyName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionCallHandler"/> class that processses exceptions
        /// by using the exception policy with the given name.
        /// </summary>
        /// <param name="policyName">The name of the exception policy to use.</param>
        /// <param name="order">The order in which the handler will be executed.</param>
        public ExceptionCallHandler(string policyName, int order)
            : this(policyName)
        {
            this.order = order;
        }

        /// <summary>
        /// Gets the name of the exception policy used by this handler.
        /// </summary>
        /// <value>The name of the exception policy.</value>
        public string ExceptionPolicyName
        {
            get { return policyName; }
        }

        #region ICallHandler Members
        /// <summary>
        /// Gets or sets the order in which the handler will be executed.
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
        /// Processes the method call.
        /// </summary>
        /// <remarks>This handler does nothing before the call. If an exception is
        /// returned, it runs the exception through the Exception Handling Application Block.</remarks>
        /// <param name="input">The <see cref="IMethodInvocation"/> that has information about the call.</param>
        /// <param name="getNext">The delegate to call to get the next handler in the pipeline.</param>
        /// <returns>Return value from the target, or the (possibly changed) exceptions.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "As designed. Main feature of the handler.")]
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            if (input == null) throw new ArgumentNullException("input");
            if (getNext == null) throw new ArgumentNullException("getNext");

            IMethodReturn result = getNext()(input, getNext);
            if (result.Exception != null)
            {
                try
                {
                    bool rethrow = ExceptionPolicy.HandleException(result.Exception, this.policyName);
                    if (!rethrow)
                    {
                        // Exception is being swallowed
                        result.ReturnValue = null;
                        result.Exception = null;

                        if (input.MethodBase.MemberType == MemberTypes.Method)
                        {
                            MethodInfo method = (MethodInfo)input.MethodBase;
                            if (method.ReturnType != typeof(void))
                            {
                                result.Exception =
                                    new InvalidOperationException(
                                        Resources.CantSwallowNonVoidReturnMessage);
                            }
                        }
                    }
                    // Otherwise the original exception will be returned to the previous handler
                }
                catch (Exception ex)
                {
                    // New exception was returned
                    result.Exception = ex;
                }
            }
            return result;
        }

        #endregion
    }
}
