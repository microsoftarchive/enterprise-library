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
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
	/// <summary>
	/// Base class for <see cref="TraceListener"/>s that deal with formatters.
	/// </summary>
	public abstract class FormattedTraceListenerBase : TraceListener, IInstrumentationEventProvider
	{
		private ILogFormatter formatter;
		private LoggingInstrumentationProvider instrumentationProvider;

		/// <summary>
		/// Gets the object that provides instrumentation services for the trace listener.
		/// </summary>
		protected LoggingInstrumentationProvider InstrumentationProvider
		{
			get { return instrumentationProvider; }
		}

		/// <summary>
		/// Initalizes a new instance of <see cref="FormattedTraceListenerBase"/>.
		/// </summary>
		public FormattedTraceListenerBase()
		{
			instrumentationProvider = new LoggingInstrumentationProvider();
		}

		/// <summary>
		/// Initalizes a new instance of <see cref="FormattedTraceListenerBase"/> with a <see cref="ILogFormatter"/>.
		/// </summary>
		/// <param name="formatter">The <see cref="ILogFormatter"/> to use when tracing a <see cref="LogEntry"/>.</param>
		public FormattedTraceListenerBase(ILogFormatter formatter)
		{
			this.Formatter = formatter;
			instrumentationProvider = new LoggingInstrumentationProvider();
		}

        /// <summary>
        /// Specifies whether this TraceListener is threadsafe
        /// </summary>
        public override bool IsThreadSafe
        {
            get
            {
                return true;
            }
        }
	    
	    /// <summary>
		/// The <see cref="ILogFormatter"/> used to format the trace messages.
		/// </summary>
		public ILogFormatter Formatter
		{
			get
			{
				return this.formatter;
			}

			set
			{
				this.formatter = value;
			}
		}

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Returns the object that provides instrumentation services for the trace listener.
		/// </summary>
		/// <see cref="IInstrumentationEventProvider.GetInstrumentationEventProvider()"/>
		/// <returns>The object that providers intrumentation services.</returns>
		public object GetInstrumentationEventProvider()
		{
			return instrumentationProvider;
		}

        /// <summary>
        /// Overriding TraceData method for the base TraceListener class because it calls the 
        /// private WriteHeader and WriteFooter methods which actually call the Write method again
        /// and this amounts to multiple log messages 
        /// </summary>
        /// <param name="eventCache">The context information provided by <see cref="System.Diagnostics"/>.</param>
        /// <param name="source">The name of the trace source that delivered the trace data.</param>
        /// <param name="eventType">The type of event.</param>
        /// <param name="id">The id of the event.</param>
        /// <param name="data">The data to trace.</param>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if ((this.Filter == null) || this.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data, null))
            {
                string text1 = string.Empty;
                if (data != null)
                {
                    text1 = data.ToString();

                    this.WriteLine(text1);
                }
            }
        }
	}
}
