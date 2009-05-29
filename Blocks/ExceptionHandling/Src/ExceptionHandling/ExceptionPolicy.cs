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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
    /// <summary>
    /// Represents a policy with exception types and
    /// exception handlers. 
    /// </summary>
    public static class ExceptionPolicy
    {
        private static readonly ExceptionPolicyFactory defaultFactory = new ExceptionPolicyFactory();

        /// <summary>
        /// The main entry point into the Exception Handling Application Block.
        /// Handles the specified <see cref="Exception"/>
        /// object according to the given <paramref name="policyName"></paramref>.
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
        ///		if (ExceptionPolicy.HandleException(e, name)) throw;
        ///	}
        /// </code>
        /// </example>
        public static bool HandleException(Exception exceptionToHandle, string policyName)
        {
            if (exceptionToHandle == null) throw new ArgumentNullException("exceptionToHandle");
            if (string.IsNullOrEmpty(policyName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty);


            return HandleException(exceptionToHandle, policyName, defaultFactory);
        }

        private static bool HandleException(Exception exceptionToHandle, string policyName, ExceptionPolicyFactory policyFactory)
        {
            ExceptionPolicyImpl policy = GetExceptionPolicy(exceptionToHandle, policyName, policyFactory);
            return policy.HandleException(exceptionToHandle);
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
		///		Foo();
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
		/// <seealso cref="ExceptionManagerImpl.HandleException(Exception, string)"/>
		public static bool HandleException(Exception exceptionToHandle, string policyName, out Exception exceptionToThrow)
        {
            if (exceptionToHandle == null) throw new ArgumentNullException("exceptionToHandle");
            if (string.IsNullOrEmpty(policyName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty);

            return HandleException(exceptionToHandle, policyName, defaultFactory, out exceptionToThrow);
        }

        private static bool HandleException(Exception exceptionToHandle, string policyName, ExceptionPolicyFactory policyFactory, out Exception exceptionToThrow)
        {
            try
            {
                bool retrowAdviced = HandleException(exceptionToHandle, policyName, policyFactory);
                exceptionToThrow = null;

                return retrowAdviced;
            }
            catch (Exception exception)
            {
                exceptionToThrow = exception;
                return true;
            }
        }


        private static ExceptionPolicyImpl GetExceptionPolicy(Exception exception, string policyName, ExceptionPolicyFactory factory)
        {
            try
            {
                return factory.Create(policyName);
            }
            catch (ActivationException configurationException)
            {
                try
                {
                    DefaultExceptionHandlingEventLogger logger = EnterpriseLibraryContainer.Current.GetInstance<DefaultExceptionHandlingEventLogger>();
                    logger.LogConfigurationError(configurationException, policyName);
                }
                catch { }

                throw;
            }
            catch (Exception ex)
            {
                try
                {
                    string exceptionMessage = ExceptionUtility.FormatExceptionHandlingExceptionMessage(policyName, ex, null, exception);

                    DefaultExceptionHandlingEventLogger logger = EnterpriseLibraryContainer.Current.GetInstance<DefaultExceptionHandlingEventLogger>();
                    logger.LogInternalError(policyName, exceptionMessage);
                }
                catch { }

                throw new ExceptionHandlingException(ex.Message, ex);
            }
        }
    }
}
