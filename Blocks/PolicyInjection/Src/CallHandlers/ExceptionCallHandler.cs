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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Properties;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers
{
    /// <summary>
    /// An <see cref="ICallHandler"/> that runs any exceptions returned from the
    /// target through the Exception Handling block.
    /// </summary>
    /// <remarks>If the exception policy is configured to swallow exceptions,
    /// do not configure this call handler on a method with a non-void return value,
    /// as the handler doesn't know what value to return if the exception is swallowed.
    /// </remarks>
    [ConfigurationElementType(typeof(ExceptionCallHandlerData))]
    public class ExceptionCallHandler : ICallHandler
    {
        private ExceptionPolicyImpl exceptionPolicy;
        private int order = 0;

        /// <summary>
        /// Creates a new <see cref="ExceptionCallHandler"/> that processses exceptions
        /// using the given exception policy.
        /// </summary>
        /// <param name="exceptionPolicy">Exception policy.</param>
        public ExceptionCallHandler(ExceptionPolicyImpl exceptionPolicy)
        {
            this.exceptionPolicy = exceptionPolicy;
        }

        /// <summary>
        /// Gets the exception policy used by this handler.
        /// </summary>
        /// <value>Exception policy.</value>
        public ExceptionPolicyImpl ExceptionPolicy
        {
            get { return exceptionPolicy; }
        }

        #region ICallHandler Members
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
        /// Processes the method call.
        /// </summary>
        /// <remarks>This handler does nothing before the call. If there is an exception
        /// returned, it runs the exception through the Exception Handling Application Block.</remarks>
        /// <param name="input"><see cref="IMethodInvocation"/> with information about the call.</param>
        /// <param name="getNext">delegate to call to get the next handler in the pipeline.</param>
        /// <returns>Return value from the target, or the (possibly changed) exceptions.</returns>
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            IMethodReturn result = getNext()(input, getNext);
            if (result.Exception != null)
            {
                try
                {
                    bool rethrow = exceptionPolicy.HandleException(result.Exception);
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
