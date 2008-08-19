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
	/// Represents the WMI event fired when a command failed during its execution.
	/// </summary>
	public class CommandFailedEvent : DataEvent
	{
		private string connectionString;
		private string commandText;
		private string exceptionMessage;

		/// <summary>
		/// Initializes a new instance of the <see cref="CommandFailedEvent"/> class.
		/// </summary>
		/// <param name="instanceName">The name of the <see cref="Database"/> instance that executed the failed command.</param>
		/// <param name="connectionString">The connection string of the <see cref="Database"/> that executed the failed command, with credentials removed.</param>
		/// <param name="commandText">The text of the command that failed its execution.</param>
		/// <param name="exceptionMessage">The message that describes the exception thrown when the command failed.</param>
		public CommandFailedEvent(string instanceName, string connectionString, string commandText, string exceptionMessage)
			: base(instanceName)
		{
			this.connectionString = connectionString;
			this.commandText = commandText;
			this.exceptionMessage = exceptionMessage;
		}
		/// <summary>
		/// Gets the text of the command that failed its execution.
		/// </summary>
		public string CommandText
		{
			get { return commandText; }
		}

		/// <summary>
		/// Gets the connection string of the <see cref="Database"/> that executed the failed command, with credentials removed.
		/// </summary>
		public string ConnectionString
		{
			get { return connectionString; }
		}

		/// <summary>
		/// Gets the message that describes the exception thrown when the command failed.
		/// </summary>
		public string ExceptionMessage
		{
			get { return exceptionMessage; }
		}
	}
}
