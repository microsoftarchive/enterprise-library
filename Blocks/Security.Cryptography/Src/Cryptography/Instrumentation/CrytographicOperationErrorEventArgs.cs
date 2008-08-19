//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation
{
	/// <summary>
	/// Provides data for the cryptographic operation failure events.
	/// </summary>
	public class CrytographicOperationErrorEventArgs : EventArgs
	{
		string message;
		Exception exception;

		/// <summary>
		/// Initializes a new instance of the <see cref="CrytographicOperationErrorEventArgs"/> class.
		/// </summary>
		/// <param name="message">The message that describes the kind of failure.</param>
		/// <param name="exception">The exception causing the failure.</param>
		public CrytographicOperationErrorEventArgs(string message, Exception exception)
		{
			this.message = message;
			this.exception = exception;
		}

		/// <summary>
		/// Gets the message that describes the kind of failure.
		/// </summary>
		public string Message { get { return message; } }

		/// <summary>
		/// Gets the exception causing the failure.
		/// </summary>
		public Exception Exception { get { return exception; } }
	}
}
