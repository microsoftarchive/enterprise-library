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
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
#if !SILVERLIGHT
using System.Diagnostics;
#else
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// Provides tracing services through a set of <see cref="TraceListener"/>s.
    /// </summary>
    public partial class LogSource : IDisposable
    {
        /// <summary>
        /// Default Auto Flush property for the LogSource instance.
        /// </summary>
        public const bool DefaultAutoFlushProperty = true;
        private readonly ILoggingInstrumentationProvider instrumentationProvider;
        private readonly string name;
        private bool autoFlush = DefaultAutoFlushProperty;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogSource"/> class with a name.
        /// </summary>
        /// <param name="name">The name for the instance.</param>
        public LogSource(string name)
            : this(name, new TraceListener[] { new DefaultTraceListener() }, SourceLevels.All)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogSource"/> class with a name and a level.
        /// </summary>
        /// <param name="name">The name for the instance.</param>
        /// <param name="level">The <see cref="SourceLevels"/> value.</param>
        public LogSource(string name, SourceLevels level)
            : this(name, new TraceListener[] { new DefaultTraceListener() }, level)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogSource"/> class with a name, a collection of <see cref="TraceListener"/>s and a level.
        /// </summary>
        /// <param name="name">The name for the instance.</param>
        /// <param name="traceListeners">The collection of <see cref="TraceListener"/>s.</param>
        /// <param name="level">The <see cref="SourceLevels"/> value.</param>
        public LogSource(string name, IEnumerable<TraceListener> traceListeners, SourceLevels level)
            : this(name, traceListeners, level, DefaultAutoFlushProperty)
        { }


        /// <summary>
        /// Initializes a new instance of the <see cref="LogSource"/> class with a name, a collection of <see cref="TraceListener"/>s, a level and the auto flush.
        /// </summary>
        /// <param name="name">The name for the instance.</param>
        /// <param name="traceListeners">The collection of <see cref="TraceListener"/>s.</param>
        /// <param name="level">The <see cref="SourceLevels"/> value.</param>
        /// <param name="autoFlush">If Flush should be called on the Listeners after every write.</param>
        public LogSource(string name, IEnumerable<TraceListener> traceListeners, SourceLevels level, bool autoFlush)
            : this(name, traceListeners, level, autoFlush, new NullLoggingInstrumentationProvider())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogSource"/> class with a name, a collection of <see cref="TraceListener"/>s, a level and the auto flush.
        /// </summary>
        /// <param name="name">The name for the instance.</param>
        /// <param name="traceListeners">The collection of <see cref="TraceListener"/>s.</param>
        /// <param name="level">The <see cref="SourceLevels"/> value.</param>
        /// <param name="autoFlush">If Flush should be called on the Listeners after every write.</param>
        /// <param name="instrumentationProvider">The instrumentation provider to use.</param>
        public LogSource(
            string name,
            IEnumerable<TraceListener> traceListeners,
            SourceLevels level,
            bool autoFlush,
            ILoggingInstrumentationProvider instrumentationProvider)
        {
            this.name = name;
            this.Listeners = new List<TraceListener>(traceListeners);
            this.Level = level;
            this.instrumentationProvider = instrumentationProvider;
            this.autoFlush = autoFlush;
        }

        /// <summary>
        /// Gets the name for the <see cref="LogSource"/> instance.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets the collection of trace listeners for the <see cref="LogSource"/> instance.
        /// </summary>
        public IList<TraceListener> Listeners { get; private set; }

        /// <summary>
        /// Gets the <see cref="SourceLevels"/> values at which to trace for the <see cref="LogSource"/> instance.
        /// </summary>
        public SourceLevels Level { get; protected set; }

        /// <summary>
        /// Gets or sets the <see cref="AutoFlush"/> values for the <see cref="LogSource"/> instance.
        /// </summary>
        public bool AutoFlush
        {
            get { return autoFlush; }
            set { this.autoFlush = value; }
        }

        /// <summary>
        /// Writes trace data to the trace listeners in the <see cref="LogSource.Listeners"/> collection using the specified 
        /// event type, event identifier, and trace data. 
        /// </summary>
        /// <param name="eventType">The value that specifies the type of event that caused the trace.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="logEntry">The <see cref="LogEntry"/> to trace.</param>
        public void TraceData(TraceEventType eventType, int id, LogEntry logEntry)
        {
            TraceData(eventType, id, logEntry, new TraceListenerFilter());
        }

        /// <summary>
        /// Writes trace data to the trace listeners in the <see cref="LogSource.Listeners"/> collection that have not already been
        /// written to for tracing using the specified event type, event identifier, and trace data.
        /// </summary>
        /// <remarks>
        /// The <paramref name="traceListenerFilter"/> will be updated to reflect the trace listeners that were written to by the 
        /// <see cref="LogSource"/>.
        /// </remarks>
        /// <param name="eventType">The value that specifies the type of event that caused the trace.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="logEntry">The <see cref="LogEntry"/> to trace.</param>
        /// <param name="traceListenerFilter">The filter for already written to trace listeners.</param>
        public void TraceData(TraceEventType eventType, int id, LogEntry logEntry, TraceListenerFilter traceListenerFilter)
        {
            this.TraceData(eventType, id, logEntry, traceListenerFilter, new TraceEventCache());
        }

        internal void TraceData(
            TraceEventType eventType,
            int id,
            LogEntry logEntry,
            TraceListenerFilter traceListenerFilter,
            TraceEventCache traceEventCache)
        {
            if (!ShouldTrace(eventType)) return;

#if !SILVERLIGHT
            bool isTransfer = logEntry.Severity == TraceEventType.Transfer && logEntry.RelatedActivityId != null;
#endif

            foreach (TraceListener listener in traceListenerFilter.GetAvailableTraceListeners(this.Listeners))
            {
                try
                {
                    if (!listener.IsThreadSafe) Monitor.Enter(listener);

#if !SILVERLIGHT
                    if (!isTransfer)
#endif
                    {
                        listener.TraceData(traceEventCache, Name, eventType, id, logEntry);
                    }
#if !SILVERLIGHT
                    else
                    {
                        listener.TraceTransfer(traceEventCache, Name, id, logEntry.Message, logEntry.RelatedActivityId.Value);
                    }
#endif
                    instrumentationProvider.FireTraceListenerEntryWrittenEvent();

                    if (this.AutoFlush)
                    {
                        listener.Flush();
                    }
                }
                finally
                {
                    if (!listener.IsThreadSafe) Monitor.Exit(listener);
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> if the method is being called from the <see cref="Dispose()"/> method. <see langword="false"/> if it is being called from within the object finalizer.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (TraceListener listener in this.Listeners)
                {
                    listener.Dispose();
                }
            }
        }

        /// <summary>
        /// Releases resources for the <see cref="LogSource"/> instance before garbage collection.
        /// </summary>
        ~LogSource()
        {
            this.Dispose(false);
        }

        private bool ShouldTrace(TraceEventType eventType)
        {
            return ((((TraceEventType)this.Level) & eventType) != (TraceEventType)0);
        }
    }
}
