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
	/// Provides data for the <see cref="DataInstrumentationProvider.commandFailed"/> event.
	/// </summary>
	public class CommandFailedEventArgs : EventArgs
	{
		string commandText;
		string connectionString;
		Exception exception;

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
		/// Gets the exception thrown when the command failed.
		/// </summary>
		public Exception Exception
		{
			get { return exception; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CommandFailedEventArgs"/> class.
		/// </summary>
		/// <param name="commandText">The text of the command that failed its execution.</param>
		/// <param name="connectionString">The connection string of the <see cref="Database"/> that executed the failed command, with credentials removed.</param>
		/// <param name="exception">The exception thrown when the command failed.</param>
		public CommandFailedEventArgs(string commandText, string connectionString, Exception exception)
		{
			this.commandText = commandText;
			this.connectionString = connectionString;
			this.exception = exception;
		}
	}
}
