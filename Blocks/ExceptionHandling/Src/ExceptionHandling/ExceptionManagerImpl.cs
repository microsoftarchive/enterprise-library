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
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
    /// <summary>
    /// Non-static entry point to the exception handling functionality.
    /// </summary>
    /// <remarks>
    /// Instances of <see cref="ExceptionManagerImpl"/> can be used to replace references to the static <see cref="ExceptionPolicy"/>
    /// facade.
    /// </remarks>
    /// <seealso cref="ExceptionPolicy"/>
    public class ExceptionManagerImpl : ExceptionManager
    {
        private readonly IDictionary<string, ExceptionPolicyImpl> exceptionPolicies;
        private readonly IDefaultExceptionHandlingInstrumentationProvider instrumentationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionManagerImpl"/> class with a set
        /// of exception policies.
        /// </summary>
        /// <param name="exceptionPolicies">The complete set of exception policies.</param>
        /// <param name="instrumentationProvider">Instrumentation provider used to report errors.</param>
        public ExceptionManagerImpl(IEnumerable<ExceptionPolicyImpl> exceptionPolicies,
            IDefaultExceptionHandlingInstrumentationProvider instrumentationProvider)
            : this(exceptionPolicies.ToDictionary(e => e.PolicyName), instrumentationProvider)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class <see cref="ExceptionManagerImpl"/> with a set of policies
        /// and no instrumentation.
        /// </summary>
        /// <param name="exceptionPolicies">The complete set of exception policies.</param>
        public ExceptionManagerImpl(IDictionary<string, ExceptionPolicyImpl> exceptionPolicies)
            : this(exceptionPolicies, new NullDefaultExceptionHandlingInstrumentationProvider())
        {
        }

        /// <summary>
        /// Initializes a new instance of the class <see cref="ExceptionManagerImpl"/> with a set of policies.
        /// </summary>
        /// <param name="exceptionPolicies">The complete set of exception policies.</param>
        /// <param name="instrumentationProvider">Instrumentation provider used to report errors.</param>
        public ExceptionManagerImpl(IDictionary<string, ExceptionPolicyImpl> exceptionPolicies, IDefaultExceptionHandlingInstrumentationProvider instrumentationProvider)
        {
            if (exceptionPolicies == null)
                throw new ArgumentNullException("exceptionPolicies");
            if (instrumentationProvider == null) throw new ArgumentNullException("instrumentationProvider");

            this.exceptionPolicies = exceptionPolicies;
            this.instrumentationProvider = instrumentationProvider;
        }

        /// <summary>
        /// Handles the specified <see cref="Exception"/>
        /// object according to the rules configured for <paramref name="policyName"/>.
        /// </summary>
        /// <param name="exceptionToHandle">An <see cref="Exception"/> object.</param>
        /// <param name="policyName">The name of the policy to handle.</param>        
        /// <returns>
        /// Whether or not a rethrow is recommended.
        /// </returns>
        /// <example>
        /// The following code shows the usage of the 
        /// exception handling framework.
        /// <code>
        /// try
        ///	{
        ///		DoWork();
        ///	}
        ///	catch (Exception e)
        ///	{
        ///		if (exceptionManager.HandleException(e, name)) throw;
        ///	}
        /// </code>
        /// </example>
        /// <seealso cref="ExceptionManagerImpl.Process"/>
        public override bool HandleException(Exception exceptionToHandle, string policyName)
        {
            if (policyName == null)
                throw new ArgumentNullException("policyName");
            if (exceptionToHandle == null)
                throw new ArgumentNullException("exceptionToHandle");

            ExceptionPolicyImpl exceptionPolicy;
            if (!exceptionPolicies.TryGetValue(policyName, out exceptionPolicy))
            {
                string message = string.Format(Resources.ExceptionPolicyNotFound, policyName);
                instrumentationProvider.FireExceptionHandlingErrorOccurred(policyName, message);
                throw new ExceptionHandlingException(message);
            }

            return exceptionPolicy.HandleException(exceptionToHandle);
        }

        /// <summary>
        /// Handles the specified <see cref="Exception"/>
        /// object according to the rules configured for <paramref name="policyName"/>.
        /// </summary>
        /// <param name="exceptionToHandle">An <see cref="Exception"/> object.</param>
        /// <param name="policyName">The name of the policy to handle.</param>
        /// <param name="exceptionToThrow">The new <see cref="Exception"/> to throw, if any.</param>
        /// <remarks>
        /// If a rethrow is recommended and <paramref name="exceptionToThrow"/> is <see langword="null"/>,
        /// then the original exception <paramref name="exceptionToHandle"/> should be rethrown; otherwise,
        /// the exception returned in <paramref name="exceptionToThrow"/> should be thrown.
        /// </remarks>
        /// <returns>
        /// Whether or not a rethrow is recommended. 
        /// </returns>
        /// <example>
        /// The following code shows the usage of the 
        /// exception handling framework.
        /// <code>
        /// try
        ///	{
        ///		DoWork();
        ///	}
        ///	catch (Exception e)
        ///	{
        ///	    Exception exceptionToThrow;
        ///		if (exceptionManager.HandleException(e, name, out exceptionToThrow))
        ///		{
        ///		  if(exceptionToThrow == null)
        ///		    throw;
        ///		  else
        ///		    throw exceptionToThrow;
        ///		}
        ///	}
        /// </code>
        /// </example>
        /// <seealso cref="ExceptionManagerImpl.HandleException(Exception, string)"/>
        public override bool HandleException(Exception exceptionToHandle, string policyName, out Exception exceptionToThrow)
        {
            try
            {
                bool shouldRethrow = HandleException(exceptionToHandle, policyName);
                exceptionToThrow = null;

                return shouldRethrow;
            }
            catch (Exception exception)
            {
                exceptionToThrow = exception;
                return true;
            }
        }

        /// <summary>
        /// Excecutes the supplied delegate <paramref name="action"/> and handles 
        /// any thrown exception according to the rules configured for <paramref name="policyName"/>.
        /// </summary>
        /// <param name="action">The delegate to execute.</param>
        /// <param name="policyName">The name of the policy to handle.</param>        
        /// <example>
        /// The following code shows the usage of this method.
        /// <code>
        ///		exceptionManager.Process(() => { DoWork(); }, "policy");
        /// </code>
        /// </example>
        /// <seealso cref="ExceptionManagerImpl.HandleException(Exception, string)"/>
        public override void Process(Action action, string policyName)
        {
            if (action == null) throw new ArgumentNullException("action");
            if (policyName == null) throw new ArgumentNullException("policyName");

            try
            {
                action();
            }
            catch (Exception e)
            {
                if (HandleException(e, policyName))
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Executes the supplied delegate <paramref name="action"/>, and handles
        /// any thrown exception according to the rules configured for <paramref name="policyName"/>.
        /// </summary>
        /// <typeparam name="TResult">Type of return value from <paramref name="action"/>.</typeparam>
        /// <param name="action">The delegate to execute.</param>
        /// <param name="defaultResult">The value to return if an exception is thrown and the
        /// exception policy swallows it instead of rethrowing.</param>
        /// <param name="policyName">The name of the policy to handle.</param>
        /// <returns>If no exception occurs, returns the result from executing <paramref name="action"/>. If
        /// an exception occurs and the policy does not re-throw, returns <paramref name="defaultResult"/>.</returns>
        public override TResult Process<TResult>(Func<TResult> action, TResult defaultResult, string policyName)
        {
            if(action == null) throw new ArgumentNullException("action");
            if(policyName == null) throw new ArgumentNullException("policyName");

            try
            {
                return action();
            }
            catch (Exception e)
            {
                if(HandleException(e, policyName))
                {
                    throw;
                }
            }
            return defaultResult;
        }
    }
}
