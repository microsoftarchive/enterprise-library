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

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation
{
    /// <summary>
    /// An implementation of <see cref="IExceptionHandlingInstrumentationProvider"/> that
    /// does nothing; an implementation of the Null Object pattern for this interface.
    /// </summary>
    public class NullExceptionHandlingInstrumentationProvider : IExceptionHandlingInstrumentationProvider
    {
        /// <summary>
        /// Fires the ExceptionHandled event - reported when an exception is handled.
        /// </summary>
        public void FireExceptionHandledEvent()
        {
        }

        /// <summary>
        /// Fires the ExceptionHandlerExecuted - reported when an exception handler executes.
        /// </summary>
        public void FireExceptionHandlerExecutedEvent()
        {
        }

        /// <summary>
        /// Fires the ExceptionHandlingErrorOccurred - reported when there's a problem handling an exception.
        /// </summary>
        /// <param name="errorMessage">The message that describes the failure.</param>
        public void FireExceptionHandlingErrorOccurred(string errorMessage)
        {
        }
    }
}
