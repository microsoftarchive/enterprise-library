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
    /// A noop implementation of <see cref="IDefaultExceptionHandlingInstrumentationProvider"/>
    /// that does nothing with the instrumentation events.
    /// </summary>
    public class NullDefaultExceptionHandlingInstrumentationProvider : IDefaultExceptionHandlingInstrumentationProvider
    {
        /// <summary>
        /// Fires the ExceptionHandlingErrorOccurred"/> event.
        /// </summary>
        /// <param name="policyName">The name of the policy involved with the error.</param>
        /// <param name="message">The message that describes the failure.</param>
        public void FireExceptionHandlingErrorOccurred(string policyName, string message)
        {
        }
    }
}
