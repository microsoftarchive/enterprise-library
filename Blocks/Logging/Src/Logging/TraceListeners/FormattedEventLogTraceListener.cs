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

using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
	/// <summary>
	/// FormattedEventLogTraceListener is a <see cref="TraceListener"/> that wraps an <see cref="FormattedEventLogTraceListener"/> 
	/// and uses a <see cref="ILogFormatter"/> to transform the data to trace.
	/// </summary>
	public class FormattedEventLogTraceListener : FormattedTraceListenerWrapperBase
	{
		/// <summary>
		/// Default to use when no log name is provided.
		/// </summary>
		public const string DefaultLogName = "";

		/// <summary>
		/// Default to use when no machine name is provided.
		/// </summary>
		public const string DefaultMachineName = ".";

		/// <summary>
		/// Initializes a new instance of <see cref="FormattedEventLogTraceListener"/> with a default <see cref="EventLogTraceListener"/>.
		/// </summary>
		public FormattedEventLogTraceListener()
			: base(new EventLogTraceListener())
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="FormattedEventLogTraceListener"/> with an 
		/// <see cref="ILogFormatter"/> and a default <see cref="EventLogTraceListener"/>.
		/// </summary>
		/// <param name="formater">The formatter for the wrapper.</param>
		public FormattedEventLogTraceListener(ILogFormatter formater)
			: base(new EventLogTraceListener(), formater)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="FormattedEventLogTraceListener"/> with a 
		/// <see cref="EventLogTraceListener"/> initialized with <see cref="EventLog"/>.
		/// </summary>
		/// <param name="eventLog">The event log for the wrapped listener.</param>
		public FormattedEventLogTraceListener(EventLog eventLog)
			: base(new EventLogTraceListener(eventLog))
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="FormattedEventLogTraceListener"/> with an 
		/// <see cref="ILogFormatter"/> and a <see cref="EventLogTraceListener"/> 
		/// initialized with <see cref="EventLog"/>.
		/// </summary>
		/// <param name="eventLog">The event log for the wrapped listener.</param>
		/// <param name="formatter">The formatter for the wrapper.</param>
		public FormattedEventLogTraceListener(EventLog eventLog, ILogFormatter formatter)
			: base(new EventLogTraceListener(eventLog), formatter)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="FormattedEventLogTraceListener"/> with a 
		/// <see cref="EventLogTraceListener"/> initialized with a source name.
		/// </summary>
		/// <param name="source">The source name for the wrapped listener.</param>
		public FormattedEventLogTraceListener(string source)
			: base(new EventLogTraceListener(source))
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="FormattedEventLogTraceListener"/> with an 
		/// <see cref="ILogFormatter"/> and a <see cref="EventLogTraceListener"/> 
		/// initialized with source name and default log and machine names.
		/// </summary>
		/// <param name="source">The source name for the wrapped listener.</param>
		/// <param name="formatter">The formatter for the wrapper.</param>
		public FormattedEventLogTraceListener(string source, ILogFormatter formatter)
			: base(new EventLogTraceListener(source), formatter)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="FormattedEventLogTraceListener"/> with an 
		/// <see cref="ILogFormatter"/> and a <see cref="EventLogTraceListener"/> 
		/// initialized with source name.
		/// </summary>
		/// <param name="source">The source name for the wrapped listener.</param>
		/// <param name="log">The name of the event log.</param>
		/// <param name="formatter">The formatter for the wrapper.</param>
		public FormattedEventLogTraceListener(string source, string log, ILogFormatter formatter)
			: base(new EventLogTraceListener(new EventLog(log, DefaultMachineName, source)), formatter)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="FormattedEventLogTraceListener"/> with an 
		/// <see cref="ILogFormatter"/> and a <see cref="EventLogTraceListener"/> 
		/// </summary>
		/// <param name="source">The source name for the wrapped listener.</param>
		/// <param name="log">The name of the event log.</param>
		/// <param name="machineName">The machine name for the event log.</param>
		/// <param name="formatter">The formatter for the wrapper.</param>
		public FormattedEventLogTraceListener(string source, string log, string machineName, ILogFormatter formatter)
			: base(new EventLogTraceListener(new EventLog(log, NormalizeMachineName(machineName), source)), formatter)
		{
		}

		private static string NormalizeMachineName(string machineName)
		{
			return string.IsNullOrEmpty(machineName) ? DefaultMachineName : machineName;
		}
	}
}
