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
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation
{
    /// <summary>
    /// Defines the logical events that can be instrumented for the exception handling block.
    /// </summary>
    /// <remarks>
    /// The concrete instrumentation is provided by an object bound to the events of the provider. 
    /// The default listener, automatically bound during construction, is <see cref="ExceptionHandlingInstrumentationListener"/>.
    /// </remarks>
    [InstrumentationListener(typeof(ExceptionHandlingInstrumentationListener))]
	public class ExceptionHandlingInstrumentationProvider
	{
        /// <summary>
        /// Occurs when an exception handler is executed.
        /// </summary>
        [InstrumentationProvider("ExceptionHandlerExecuted")]
		public event EventHandler<EventArgs> exceptionHandlerExecuted;

		/// <summary>
        /// Occurs when an Exception is handled.
		/// </summary>
		[InstrumentationProvider("ExceptionHandled")]
		public event EventHandler<EventArgs> exceptionHandled;

		/// <summary>
        /// Occurs when an error occurs handling an Exception is handled.
		/// </summary>
		[InstrumentationProvider("ExceptionHandlingErrorOccurred")]
		public event EventHandler<ExceptionHandlingErrorEventArgs> exceptionHandlingErrorOccurred;

		/// <summary>
        /// Fires the <see cref="ExceptionHandlingInstrumentationProvider.exceptionHandled"/> event.
        /// </summary>
		public void FireExceptionHandledEvent()
		{
			if (exceptionHandled != null) exceptionHandled(this, new EventArgs());
		}

        /// <summary>
        /// Fires the <see cref="ExceptionHandlingInstrumentationProvider.exceptionHandlerExecuted"/> event.
        /// </summary>
		public void FireExceptionHandlerExecutedEvent()
		{
			if (exceptionHandlerExecuted != null) exceptionHandlerExecuted(this, new EventArgs());
		}

		/// <summary>
        /// Fires the <see cref="ExceptionHandlingInstrumentationProvider.exceptionHandlingErrorOccurred"/> event.
        /// </summary>
        /// <param name="errorMessage">The message that describes the failure.</param>
		public void FireExceptionHandlingErrorOccurred(string errorMessage)
		{
			if (exceptionHandlingErrorOccurred != null) exceptionHandlingErrorOccurred(this, new ExceptionHandlingErrorEventArgs(errorMessage));
		}
	}
}
