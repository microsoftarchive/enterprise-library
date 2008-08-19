//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Instrumentation
{
	/// <summary>
	/// Represents the WMI event fired when an failure occurs.
	/// </summary>
	public class DistributorServiceFailureEvent : DistributorServiceEvent
	{
		private string failureMessage;
		private Exception failureException;

		/// <summary>
		/// Initializes a new instance of the <see cref="DistributorServiceFailureEvent"/> class.
		/// </summary>
		/// <param name="failureMessage">The message that describes the failure.</param>
		/// <param name="failureException">The exception thrown during the failure, or null.</param>
		public DistributorServiceFailureEvent(string failureMessage, Exception failureException)
		{
			this.failureMessage = failureMessage;
			this.failureException = failureException;
		}

		/// <summary>
		/// Gets the message that describes the failure.
		/// </summary>
		public string FailureMessage
		{
			get { return failureMessage; }
		}

		/// <summary>
		/// Gets the exception thrown during the failure, or null.
		/// </summary>
		public string FailureException
		{
			get { return failureException == null ? string.Empty : failureException.ToString(); }
		}
	}
}
