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

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation
{
	/// <summary>
	/// Represents the WMI event fired when an error ocurred in the caching block.
	/// </summary>
	public class CacheFailureEvent : CacheEvent
	{
		private string errorMessage;
		private string exceptionMessage;

		/// <summary>
		/// Initializes a new instance of the <see cref="CacheFailureEvent"/> class.
		/// </summary>
		/// <param name="instanceName">The name of the <see cref="CacheManager"/> instance this failure ocurred in.</param>
		/// <param name="errorMessage">The message that describes the failure.</param>
		/// <param name="exceptionMessage">The message that represents the exception causing the failure.</param>
		public CacheFailureEvent(string instanceName, string errorMessage, string exceptionMessage)
			: base(instanceName)
		{
			this.errorMessage = errorMessage;
			this.exceptionMessage = exceptionMessage;
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
		public string ExceptionMessage
		{
			get { return exceptionMessage; }
		}
	}
}
