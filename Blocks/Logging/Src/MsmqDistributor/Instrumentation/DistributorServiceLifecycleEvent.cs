//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Instrumentation
{
	/// <summary>
	/// Represents the WMI event fired when an lifecycle event occurs.
	/// </summary>
	public class DistributorServiceLifecycleEvent : DistributorServiceEvent
	{
		private string message;
		private bool started;

		/// <summary>
		/// Initializes a new instance of the <see cref="DistributorServiceFailureEvent"/> class.
		/// </summary>
		/// <param name="message">The message that describes the lifecycle event.</param>
		/// <param name="started"><code>true</code> if the event is start or resume, false otherwise.</param>
		public DistributorServiceLifecycleEvent(string message, bool started)
		{
			this.message = message;
			this.started = started;
		}

		/// <summary>
		/// Gets the message that describes the lifecycle event.
		/// </summary>
		public string Message
		{
			get { return message; }
		}

		/// <summary>
		/// Gets the <code>bool</code> value that describes whether the event is start or resume.
		/// </summary>
		public bool Started
		{
			get { return started; }
		}

		/// <summary>
		/// Application base.
		/// </summary>
		public string ApplicationBase
		{
			get { return AppDomain.CurrentDomain.SetupInformation.ApplicationBase; }
		}

		/// <summary>
		/// Application or service name.
		/// </summary>
		public string ApplicationName
		{
			get { return AppDomain.CurrentDomain.SetupInformation.ApplicationName; }
		}

		/// <summary>
		/// Configuration file name.
		/// </summary>
		public string ConfigurationFile
		{
			get { return AppDomain.CurrentDomain.SetupInformation.ConfigurationFile; }
		}
	}
}
