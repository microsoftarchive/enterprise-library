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
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation
{
	/// <summary>
	/// Defines the logical events that can be instrumented for the exception handling block's <see cref="ExceptionManagerImpl"/>.
	/// </summary>
	/// <remarks>
	/// The concrete instrumentation is provided by an object bound to the events of the provider. 
	/// The default listener, automatically bound during construction, is <see cref="DefaultExceptionHandlingEventLogger"/>.
	/// </remarks>
	[InstrumentationListener(typeof(DefaultExceptionHandlingEventLogger))]
	public class DefaultExceptionHandlingInstrumentationProvider
	{
		/// <summary>
		/// Occurs when an error is detected while trying to determine the policy to use.
		/// </summary>
		/// <remarks>
		/// Errors detected while processing a policy are notified by the policy itself.
		/// </remarks>
		[InstrumentationProvider("ExceptionHandlingErrorOccurred")]
		public event EventHandler<DefaultExceptionHandlingErrorEventArgs> exceptionHandlingErrorOccurred;

		/// <summary>
		/// Fires the <see cref="DefaultExceptionHandlingInstrumentationProvider.exceptionHandlingErrorOccurred"/> event.
		/// </summary>
		/// <param name="policyName">The name of the policy involved with the errror.</param>
		/// <param name="message">The message that describes the failure.</param>
		public void FireExceptionHandlingErrorOccurred(string policyName, string message)
		{
			if (exceptionHandlingErrorOccurred != null)
				exceptionHandlingErrorOccurred(this, new DefaultExceptionHandlingErrorEventArgs(policyName, message));
		}
	}
}
