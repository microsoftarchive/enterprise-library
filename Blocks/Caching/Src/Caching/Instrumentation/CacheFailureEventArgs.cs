//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation
{
    /// <summary>
    /// Provides data for the <see cref="CachingInstrumentationProvider.cacheFailed"/> event.
	/// </summary>
	public class CacheFailureEventArgs : EventArgs
	{
		private string errorMessage;
		private Exception exception;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheFailureEventArgs"/> class.
        /// </summary>
        /// <param name="errorMessage">The message that describes the failure.</param>
        /// <param name="exception">The exception causing the failure.</param>
        public CacheFailureEventArgs(string errorMessage, Exception exception)
		{
			this.errorMessage = errorMessage;
			this.exception = exception;
		}

		/// <summary>
        /// Gets the message that describes the failure.
		/// </summary>
		public string ErrorMessage
		{
			get { return errorMessage; }
		}

		/// <summary>
        /// Gets the message that represents the exception causing the failure.
		/// </summary>
		public Exception Exception
		{
			get { return exception; }
		}
	}
}