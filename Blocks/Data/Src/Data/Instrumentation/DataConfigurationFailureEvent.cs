//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation
{
	/// <summary>
	/// Represents the WMI event fired when an error in the configuration for the data access block is detected.
	/// </summary>
	public class DataConfigurationFailureEvent : DataEvent
	{
		private string exceptionMessage;

		/// <summary>
		/// Initializes a new instance of the <see cref="DataConfigurationFailureEvent"/> class.
		/// </summary>
		/// <param name="instanceName">Name of the <see cref="Database"/> instance the failure ocurred in.</param>
		/// <param name="exceptionMessage">The message that represents the exception thrown when the configuration error was detected.</param>
		public DataConfigurationFailureEvent(string instanceName, string exceptionMessage)
			: base(instanceName)
		{
			this.exceptionMessage = exceptionMessage;
		}

		/// <summary>
		/// Gets the message that represents the exception thrown when the configuration error was detected.
		/// </summary>
		public string ExceptionMessage
		{
			get { return exceptionMessage; }
		}
	}
}
