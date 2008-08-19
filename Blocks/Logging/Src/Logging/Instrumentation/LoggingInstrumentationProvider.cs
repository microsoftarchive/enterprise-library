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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation
{
	/// <summary>
	/// Defines the logical events that can be instrumented for the logging block.
	/// </summary>
	/// <remarks>
	/// The concrete instrumentation is provided by an object bound to the events of the provider. 
	/// The default listener, automatically bound during construction, is <see cref="LoggingInstrumentationListener"/>.
	/// </remarks>
	[InstrumentationListener(typeof(LoggingInstrumentationListener))]
	public class LoggingInstrumentationProvider
	{
		/// <summary>
		/// Occurs when a log entry is traced by a trace listener.
		/// </summary>
		[InstrumentationProvider("TraceListenerEntryWritten")]
		public event EventHandler<EventArgs> traceListenerEntryWritten;

		/// <summary>
		/// Occurs when a failure is detected and it cannot be logged though the errors special log source.
		/// </summary>
		[InstrumentationProvider("FailureLoggingError")]
		public event EventHandler<FailureLoggingErrorEventArgs> failureLoggingError;

		/// <summary>
		/// Occurs when a failure in the configuration is detected while building the logging objects.
		/// </summary>
		[InstrumentationProvider("ConfigurationFailure")]
		public event EventHandler<LoggingConfigurationFailureEventArgs> configurationFailure;

		/// <summary>
		/// Occurs when a log entry is logged.
		/// </summary>
		[InstrumentationProvider("LoggingEventRaised")]
		public event EventHandler<EventArgs> logEventRaised;

		/// <summary>
		/// Occurs when a timeout is detected while trying to acquire a lock in the log writer.
		/// </summary>
		[InstrumentationProvider("LockAcquisitionError")]
		public event EventHandler<LockAcquisitionErrorEventArgs> lockAcquisitionError;

		/// <summary>
		/// Fires the <see cref="LoggingInstrumentationProvider.traceListenerEntryWritten"/> event.
		/// </summary>
		public void FireTraceListenerEntryWrittenEvent()
		{
			if (traceListenerEntryWritten != null) traceListenerEntryWritten(this, new EventArgs());
		}

		/// <summary>
		/// Fires the <see cref="LoggingInstrumentationProvider.failureLoggingError"/> event.
		/// </summary>
		/// <param name="message">A message describing the failure.</param>
		/// <param name="exception">The exception that caused the failure, or <see langword="null"/>.</param>
		public void FireFailureLoggingErrorEvent(string message, Exception exception)
		{
			if (failureLoggingError != null) failureLoggingError(this, new FailureLoggingErrorEventArgs(message, exception));
		}

		/// <summary>
		/// Fires the <see cref="LoggingInstrumentationProvider.configurationFailure"/> event.
		/// </summary>
		/// <param name="configurationException">The exception that describes the configuration error.</param>
		public void FireConfigurationFailureEvent(System.Configuration.ConfigurationErrorsException configurationException)
		{
			if (configurationFailure != null) configurationFailure(this, new LoggingConfigurationFailureEventArgs(configurationException));
		}

		/// <summary>
		/// Fires the <see cref="LoggingInstrumentationProvider.logEventRaised"/> event.
		/// </summary>
		public void FireLogEventRaised()
		{
			if (logEventRaised != null) logEventRaised(this, new EventArgs());
		}

		internal void FireLockAcquisitionError(string message)
		{
			if (lockAcquisitionError != null) lockAcquisitionError(this, new LockAcquisitionErrorEventArgs(message));
		}
	}

	/// <summary>
	/// Provides data for the <see cref="LoggingInstrumentationProvider.failureLoggingError"/> event.
	/// </summary>
	public class FailureLoggingErrorEventArgs : EventArgs
	{
		private string errorMessage;
		private Exception exception;

		/// <summary>
		/// Initializes a new instance of the <see cref="FailureLoggingErrorEventArgs"/> class.
		/// </summary>
		/// <param name="errorMessage">The message that describes the failure.</param>
		/// <param name="exception">The exception that caused the failure.</param>
		public FailureLoggingErrorEventArgs(string errorMessage, Exception exception)
		{
			this.errorMessage = errorMessage;
			this.exception = exception;
		}

		/// <summary>
		/// Gets the message that describes the failure.
		/// </summary>
		public string ErrorMessage
		{
			get { return this.errorMessage; }
		}

		/// <summary>
		/// Gets the exception that caused the failure.
		/// </summary>
		public Exception Exception
		{
			get { return exception; }
		}
	}

	/// <summary>
	/// Provides data for the <see cref="LoggingInstrumentationProvider.configurationFailure"/> event.
	/// </summary>
	public class LoggingConfigurationFailureEventArgs : EventArgs
	{
		private Exception exception;

		/// <summary>
		/// Initializes a new instance of the <see cref="LoggingConfigurationFailureEventArgs"/> class.
		/// </summary>
		/// <param name="exception">The exception that describes the configuration error.</param>
		public LoggingConfigurationFailureEventArgs(Exception exception)
		{
			this.exception = exception;
		}

		/// <summary>
		/// Gets the exception that describes the configuration error.
		/// </summary>
		public Exception Exception
		{
			get { return exception; }
		}
	}

	/// <summary>
	/// </summary>
	public class LockAcquisitionErrorEventArgs : EventArgs
	{
		private string errorMessage;

		/// <summary>
		/// Initializes a new instance of the <see cref="LockAcquisitionErrorEventArgs"/> class.
		/// </summary>
		/// <param name="errorMessage">Error message to be included in the event.</param>
		public LockAcquisitionErrorEventArgs(string errorMessage)
		{
			this.errorMessage = errorMessage;
		}

		/// <summary>
		/// Error message displayed for this event.
		/// </summary>
		public string ErrorMessage
		{
			get
			{
				return errorMessage;
			}
		}
	}
}
