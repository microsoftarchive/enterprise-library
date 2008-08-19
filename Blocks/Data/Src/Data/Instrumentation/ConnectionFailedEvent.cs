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
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation
{
	/// <summary>
	/// Represents the WMI event fired when a connection failed to be established.
	/// </summary>
	public class ConnectionFailedEvent : DataEvent
	{
		private string connectionString;
		private string exceptionMessage;

		/// <summary>
		/// Initializes a new instance of the <see cref="ConnectionFailedEvent"/> class.
		/// </summary>
		/// <param name="instanceName">The name of the <see cref="Database"/> instance that tried to establish the failed the connection.</param>
		/// <param name="connectionString">The connection string that caused the failed connection, with credentials removed.</param>
		/// <param name="exceptionMessage">The message that describes the exception thrown when the connection failed.</param>
		public ConnectionFailedEvent(string instanceName, string connectionString, string exceptionMessage)
			: base(instanceName)
		{
			this.connectionString = connectionString;
			this.exceptionMessage = exceptionMessage;
		}

		/// <summary>
		/// Gets the connection string that caused the failed connection, with credentials removed.
		/// </summary>
		public string ConnectionString
		{
			get { return connectionString; }
		}

		/// <summary>
		/// Gets the message that describes the exception thrown when the connection failed.
		/// </summary>
		public string ExceptionMessage
		{
			get { return exceptionMessage; }
		}
	}
}
