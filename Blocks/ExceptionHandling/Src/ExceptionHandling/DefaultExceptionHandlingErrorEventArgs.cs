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

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
	/// <summary>
	/// Provides data for the DefaultExceptionHandlingInstrumentationProvider.exceptionHandlingErrorOccurred event.
	/// </summary>
	public class DefaultExceptionHandlingErrorEventArgs : EventArgs
	{
		private readonly string policyName;
		private readonly string message;

		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultExceptionHandlingErrorEventArgs"/> class.
		/// </summary>
		/// <param name="policyName">The name of the associated policy.</param>
		/// <param name="message">The message that describes the error.</param>
		public DefaultExceptionHandlingErrorEventArgs(string policyName, string message)
		{
			this.policyName = policyName;
			this.message = message;
		}

		/// <summary>
		/// Gets the name of the policy associated to the error.
		/// </summary>
		public string PolicyName { get { return policyName; } }

		/// <summary>
		/// Gets the message that describes the error.
		/// </summary>
		public string Message { get { return message; } }
	}
}
