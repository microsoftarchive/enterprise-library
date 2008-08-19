//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
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
	public class ExceptionManagerImpl : ExceptionManager, IInstrumentationEventProvider
	{
		private readonly IDictionary<string, ExceptionPolicyImpl> exceptionPolicies;
		private readonly DefaultExceptionHandlingInstrumentationProvider instrumentationProvider;

		/// <summary>
		/// Initializes a new instance of the class <see cref="ExceptionManagerImpl"/> with a set of policies.
		/// </summary>
		/// <param name="exceptionPolicies"></param>
		public ExceptionManagerImpl(IDictionary<string, ExceptionPolicyImpl> exceptionPolicies)
		{
			if (exceptionPolicies == null)
				throw new ArgumentNullException("exceptionPolicies");

			this.exceptionPolicies = exceptionPolicies;
			this.instrumentationProvider = new DefaultExceptionHandlingInstrumentationProvider();
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
		///		Foo();
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
			if (!this.exceptionPolicies.TryGetValue(policyName, out exceptionPolicy))
			{
				string message = string.Format(Resources.ExceptionPolicyNotFound, policyName);
				this.instrumentationProvider.FireExceptionHandlingErrorOccurred(policyName, message);
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
		/// <seealso cref="ExceptionManagerImpl.HandleException(Exception, string)"/>
		public override bool HandleException(Exception exceptionToHandle, string policyName, out Exception exceptionToThrow)
		{
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
		/// Excecutes the supplied delegate <paramref name="action"/> and handles 
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
		/// <seealso cref="ExceptionManagerImpl.HandleException(Exception, string)"/>
		public override void Process(Action action, string policyName)
		{
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

		object IInstrumentationEventProvider.GetInstrumentationEventProvider()
		{
			return this.instrumentationProvider;
		}
	}
}
