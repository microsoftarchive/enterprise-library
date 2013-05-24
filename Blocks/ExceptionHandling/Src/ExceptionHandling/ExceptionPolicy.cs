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
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
    /// <summary>
    /// Represents a policy with exception types and
    /// exception handlers. 
    /// </summary>
    public static class ExceptionPolicy
    {
        private static volatile ExceptionManager exceptionManager;

        /// <summary>
        /// The main entry point into the Exception Handling Application Block.
        /// Handles the specified <see cref="Exception"/>
        /// object according to the given <paramref name="policyName"></paramref>.
        /// </summary>
        /// <param name="exceptionToHandle">An <see cref="Exception"/> object.</param>
        /// <param name="policyName">The name of the policy to handle.</param>        
        /// <returns><see langword="true"/> if  rethrowing an exception is recommended; otherwise, <see langword="false"/>.</returns>
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
        ///		if (ExceptionPolicy.HandleException(e, name)) throw;
        ///	}
        /// </code>
        /// </example>
        public static bool HandleException(Exception exceptionToHandle, string policyName)
        {
            if (exceptionToHandle == null) throw new ArgumentNullException("exceptionToHandle");
            if (string.IsNullOrEmpty(policyName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty);

            return EnsureManager().HandleException(exceptionToHandle, policyName);
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
        /// <returns><see langword="true"/> if  rethrowing an exception is recommended; otherwise, <see langword="false"/>.</returns>
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
        ///		if (ExceptionPolicy.HandleException(e, name, out exceptionToThrow))
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "By Design. Core feature of block.")]
        public static bool HandleException(Exception exceptionToHandle, string policyName, out Exception exceptionToThrow)
        {
            if (exceptionToHandle == null) throw new ArgumentNullException("exceptionToHandle");
            if (string.IsNullOrEmpty(policyName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty);

            try
            {
                bool retrowAdviced = HandleException(exceptionToHandle, policyName);
                exceptionToThrow = null;

                return retrowAdviced;
            }
            catch (Exception exception)
            {
                exceptionToThrow = exception;
                return true;
            }
        }

        /// <summary>
        /// Sets the global exception manager.
        /// </summary>
        /// <param name="exceptionManager">The exception manager.</param>
        /// <param name="throwIfSet"><see langword="true"/> to throw an exception if the manager is already set; otherwise, <see langword="false"/>. Defaults to <see langword="true"/>.</param>
        /// <exception cref="InvalidOperationException">The manager is already set and <paramref name="throwIfSet"/> is <see langword="true"/>.</exception>
        public static void SetExceptionManager(ExceptionManager exceptionManager, bool throwIfSet = true)
        {
            var currentExceptionManager = ExceptionPolicy.exceptionManager;
            if (currentExceptionManager != null && throwIfSet)
            {
                throw new InvalidOperationException(Resources.ExceptionManagerAlreadySet);
            }

            ExceptionPolicy.exceptionManager = exceptionManager;
        }

        /// <summary>
        /// Resets the global exception manager.
        /// </summary>
        public static void Reset()
        {
            exceptionManager = null;
        }

        private static ExceptionManager EnsureManager()
        {
            var manager = exceptionManager;

            if (manager == null)
            {
                throw new InvalidOperationException(Resources.ExceptionManagerNotSet);
            }

            return manager;
        }
    }
}
