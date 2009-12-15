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

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
	/// <summary>
	/// Non-static entry point to the exception handling functionality.
	/// </summary>
	/// <remarks>
	/// Instances of <see cref="ExceptionManager"/> can be used to replace references to the static <see cref="ExceptionPolicy"/>
	/// facade.
	/// </remarks>
	/// <seealso cref="ExceptionPolicy"/>
    public abstract class ExceptionManager
    {
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
        ///		Foo();
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
        /// <seealso cref="ExceptionManager.HandleException(Exception, string)"/>
        public abstract bool HandleException(Exception exceptionToHandle, string policyName, out Exception exceptionToThrow);

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
        ///		Foo();
        ///	}
        ///	catch (Exception e)
        ///	{
        ///		if (exceptionManager.HandleException(e, name)) throw;
        ///	}
        /// </code>
        /// </example>
        /// <seealso cref="ExceptionManager.Process"/>
        public abstract bool HandleException(Exception exceptionToHandle, string policyName);

        /// <summary>
        /// Executes the supplied delegate <paramref name="action"/> and handles 
        /// any thrown exception according to the rules configured for <paramref name="policyName"/>.
        /// </summary>
        /// <param name="action">The delegate to execute.</param>
        /// <param name="policyName">The name of the policy to handle.</param>        
        /// <example>
        /// The following code shows the usage of this method.
        /// <code>
        ///		exceptionManager.Process(() => { Foo(); }, "policy");
        /// </code>
        /// </example>
        /// <seealso cref="ExceptionManager.HandleException(Exception, string)"/>
        public abstract void Process(Action action, string policyName);

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
	    public abstract TResult Process<TResult>(Func<TResult> action, TResult defaultResult, string policyName);

        /// <summary>
        /// Executes the supplied delegate <paramref name="action"/>, and handles
        /// any thrown exception according to the rules configured for <paramref name="policyName"/>.
        /// </summary>
        /// <typeparam name="TResult">Type of return value from <paramref name="action"/>.</typeparam>
        /// <param name="action">The delegate to execute.</param>
        /// <param name="policyName">The name of the policy to handle.</param>
        /// <returns>If no exception occurs, returns the result from executing <paramref name="action"/>. If
        /// an exception occurs and the policy does not re-throw, returns the default value for <typeparamref name="TResult"/>.</returns>
        public virtual TResult Process<TResult>(Func<TResult> action, string policyName)
        {
            return Process(action, default(TResult), policyName);
        }
    }
}
