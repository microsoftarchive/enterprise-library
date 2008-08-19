//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block QuickStart
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Runtime.Serialization;

namespace ExceptionHandlingQuickStart.DataLayer
{
	/// <summary>
	/// 
	/// </summary>
	public class RecoverableUpdateException : ApplicationException 
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public RecoverableUpdateException() : base() 
		{
		}

		/// <summary>
		/// Initializes with a specified error message.
		/// </summary>
		/// <param name="message">A message that describes the error.</param>
		public RecoverableUpdateException(string message) : base(message) 
		{
		}

		/// <summary>
		/// Initializes with a specified error 
		/// message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.
		/// </param>
		/// <param name="exception">The exception that is the cause of the current exception. 
		/// If the innerException parameter is not a null reference, the current exception 
		/// is raised in a catch block that handles the inner exception.
		/// </param>
		public RecoverableUpdateException(string message, Exception exception) : 
			base(message, exception) 
		{
		}

		/// <summary>
		/// Initializes with serialized data.
		/// </summary>
		/// <param name="info">The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.
		/// </param>
		protected RecoverableUpdateException(SerializationInfo info, StreamingContext context) :
			base(info, context) 
		{
		}
	}
}
