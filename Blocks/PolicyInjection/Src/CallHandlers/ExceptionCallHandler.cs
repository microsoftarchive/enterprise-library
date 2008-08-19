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
        private string exceptionPolicyName;
        private ExceptionPolicyImpl exceptionPolicy;
        private int order = 0;

        /// <summary>
        /// Creates a new <see cref="ExceptionCallHandler"/> that processes exceptions
        /// using the given exception policy name and default configuration.
        /// </summary>
        /// <param name="exceptionPolicyName">Exception policy name.</param>
        public ExceptionCallHandler(string exceptionPolicyName)
            :this(exceptionPolicyName, ConfigurationSourceFactory.Create())
        {
        }

        /// <summary>
        /// Creates a new <see cref="ExceptionCallHandler"/> that processes exceptions
        /// using the given exception policy name and default configuration.
        /// </summary>
        /// <param name="exceptionPolicyName">Exception policy name.</param>
        /// <param name="handlerOrder">The Order for the handler.</param>
        public ExceptionCallHandler(string exceptionPolicyName, int handlerOrder)
            : this(exceptionPolicyName, ConfigurationSourceFactory.Create())
        {
            this.order = handlerOrder;
        }

        /// <summary>
        /// Creates a new <see cref="ExceptionCallHandler"/> that processses exceptions
        /// using the given exception policy name, as defined in <paramref name="configurationSource"/>.
        /// </summary>
        /// <param name="exceptionPolicyName">Exception policy name.</param>
        /// <param name="configurationSource">Configuration source defining the exception handling policy.</param>
        public ExceptionCallHandler(string exceptionPolicyName, IConfigurationSource configurationSource)
        {
            this.exceptionPolicyName = exceptionPolicyName;
            ExceptionPolicyFactory policyFactory = new ExceptionPolicyFactory(configurationSource);
            exceptionPolicy = policyFactory.Create(exceptionPolicyName);
        }

        /// <summary>
        /// Gets the exception policy name used by this handler.
        /// </summary>
        /// <value>Exception policy name.</value>
        public string ExceptionPolicyName
        {
            get { return exceptionPolicyName; }
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
            if( result.Exception != null )
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
                catch( Exception ex)
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
