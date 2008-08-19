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
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
    /// <summary>
    /// Provides data for the <see cref="ExceptionHandlingInstrumentationProvider.exceptionHandlingErrorOccurred"/> event.
    /// </summary>
	public class ExceptionHandlingErrorEventArgs : EventArgs
	{
		string message;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlingErrorEventArgs"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
		public ExceptionHandlingErrorEventArgs(string message)
		{
			this.message = message;
		}

        /// <summary>
        /// Gets the message that describes the error.
        /// </summary>
		public string Message { get { return message; } }
	}
}
