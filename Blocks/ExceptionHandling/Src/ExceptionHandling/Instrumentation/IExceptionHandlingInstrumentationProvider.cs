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
    /// Interface defining the instrumentation events available from the Exception Handling
    /// Application Block.
    /// </summary>
    public interface IExceptionHandlingInstrumentationProvider
    {
        /// <summary>
        /// Fires the ExceptionHandled event - reported when an exception is handled.
        /// </summary>
        void FireExceptionHandledEvent();

        /// <summary>
        /// Fires the ExceptionHandlerExecuted - reported when an exception handler executes.
        /// </summary>
        void FireExceptionHandlerExecutedEvent();

        /// <summary>
        /// Fires the ExceptionHandlingErrorOccurred - reported when there's a problem handling an exception.
        /// </summary>
        /// <param name="errorMessage">The message that describes the failure.</param>
        void FireExceptionHandlingErrorOccurred(string errorMessage);
    }
}
