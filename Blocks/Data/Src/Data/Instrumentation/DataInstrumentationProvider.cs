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
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation
{

	/// <summary>
	/// Defines the logical events that can be instrumented for <see cref="Database"/> objects.
	/// </summary>
	/// <remarks>
	/// The concrete instrumentation is provided by an object bound to the events of the provider. 
	/// The default listener, automatically bound during construction, is <see cref="DataInstrumentationListener"/>.
	/// </remarks>
	[InstrumentationListener(typeof(DataInstrumentationListener), typeof(DataInstrumentationListenerBinder))]
	public class DataInstrumentationProvider
	{
		/// <summary>
		/// Occurs when a new database connection is opened by a <see cref="Database"/> instance.
		/// </summary>
		[InstrumentationProvider("ConnectionOpened")]
		public event EventHandler<EventArgs> connectionOpened;

		/// <summary>
		/// Occurs when the attempt to open a new database connection by a <see cref="Database"/> instance fails.
		/// </summary>
		[InstrumentationProvider("ConnectionFailed")]
		public event EventHandler<ConnectionFailedEventArgs> connectionFailed;

		/// <summary>
		/// Occurs when a database command is executed by a <see cref="Database"/> instance.
		/// </summary>
		[InstrumentationProvider("CommandExecuted")]
		public event EventHandler<CommandExecutedEventArgs> commandExecuted;

		/// <summary>
		/// Occurs when the attempt to execute a database command by a <see cref="Database"/> instance fails.
		/// </summary>
		[InstrumentationProvider("CommandFailed")]
		public event EventHandler<CommandFailedEventArgs> commandFailed;

		/// <summary>
		/// Fires the <see cref="DataInstrumentationProvider.commandExecuted"/> event.
		/// </summary>
		/// <param name="startTime">The time the command started its execution.</param>
		public void FireCommandExecutedEvent(DateTime startTime)
		{
			if (commandExecuted != null) commandExecuted(this, new CommandExecutedEventArgs(startTime));
		}

		/// <summary>
		/// Fires the <see cref="DataInstrumentationProvider.commandFailed"/> event.
		/// </summary>
		/// <param name="commandText">The text of the command that failed its execution.</param>
		/// <param name="connectionString">The connection string of the <see cref="Database"/> that executed the failed command, with credentials removed.</param>
		/// <param name="exception">The exception thrown when the command failed.</param>
		public void FireCommandFailedEvent(string commandText, string connectionString, Exception exception)
		{
			if (commandFailed != null) commandFailed(this, new CommandFailedEventArgs(commandText, connectionString, exception));
		}

		/// <summary>
		/// Fires the <see cref="DataInstrumentationProvider.connectionOpened"/> event.
		/// </summary>
		public void FireConnectionOpenedEvent()
		{
			if (connectionOpened != null) connectionOpened(this, new EventArgs());
		}

		/// <summary>
		/// Fires the <see cref="DataInstrumentationProvider.connectionFailed"/> event.
		/// </summary>
		/// <param name="connectionString">The connection string that caused the failed connection, with credentials removed.</param>
		/// <param name="exception">The exception thrown when the connection failed.</param>
		public void FireConnectionFailedEvent(string connectionString, Exception exception)
		{
			if (connectionFailed != null) connectionFailed(this, new ConnectionFailedEventArgs(connectionString, exception));
		}
	}
}
