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
using System.Diagnostics;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// Provides tracing services through a set of <see cref="TraceListener"/>s.
    /// </summary>
    public class LogSource
    {
        /// <summary>
        /// Default Auto Flush property for the LogSource instance.
        /// </summary>
        public const bool DefaultAutoFlushProperty = true;
        private SourceLevels level;
        private string name;
        private IList<TraceListener> traceListeners;
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
        {
            this.name = name;
            this.traceListeners = new List<TraceListener>(traceListeners);
            this.level = level;
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
        public IEnumerable<TraceListener> Listeners
        {
            get { return traceListeners; }
        }

        /// <summary>
        /// Gets or sets the <see cref="SourceLevels"/> values at which to trace for the <see cref="LogSource"/> instance.
        /// </summary>
        public SourceLevels Level
        {
            get { return level; }
            set { this.level = value; }
        }

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
            this.TraceData(eventType, id, logEntry, traceListenerFilter, new TraceEventCache(), null);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2002:DoNotLockOnObjectsWithWeakIdentity", Justification = "Copied behavior from TraceSource")]
        internal void TraceData(
            TraceEventType eventType,
            int id,
            LogEntry logEntry,
            TraceListenerFilter traceListenerFilter,
            TraceEventCache traceEventCache,
            ReportTracingError reportError)
        {
            if (!ShouldTrace(eventType)) return;

            bool isTransfer = logEntry.Severity == TraceEventType.Transfer && logEntry.RelatedActivityId != null;

            try
            {
                foreach (TraceListener listener in traceListenerFilter.GetAvailableTraceListeners(traceListeners))
                {
                    var asynchronousListener = listener as IAsynchronousTraceListener;
                    if (asynchronousListener == null)
                    {
                        try
                        {
                            if (!listener.IsThreadSafe) Monitor.Enter(listener);

                            if (!isTransfer)
                            {
                                listener.TraceData(traceEventCache, this.Name, eventType, id, logEntry);
                            }
                            else
                            {
                                listener.TraceTransfer(traceEventCache, this.Name, id, logEntry.Message, logEntry.RelatedActivityId.Value);
                            }

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
                    else
                    {
                        if (!isTransfer)
                        {
                            asynchronousListener.TraceData(traceEventCache, this.Name, eventType, id, logEntry, reportError);
                        }
                        else
                        {
                            asynchronousListener.TraceTransfer(traceEventCache, this.Name, id, logEntry.Message, logEntry.RelatedActivityId.Value, reportError);
                        }

                        if (this.AutoFlush)
                        {
                            asynchronousListener.Flush(reportError);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (reportError == null)
                {
                    throw;
                }

                reportError(e, logEntry, this.name);
            }
        }

        private bool ShouldTrace(TraceEventType eventType)
        {
            return ((((TraceEventType)level) & eventType) != (TraceEventType)0);
        }
    }
}
