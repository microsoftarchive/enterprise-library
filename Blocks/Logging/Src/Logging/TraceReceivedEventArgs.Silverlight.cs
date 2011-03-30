using System;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// Event argument passed when trace event is received.
    /// </summary>
    public class TraceReceivedEventArgs : EventArgs
    {
        private readonly TraceEventCache traceEventCache;
        private readonly string source;
        private readonly TraceEventType eventType;
        private readonly int id;
        private readonly object data;
        private readonly string tag;

        /// <summary>
        /// Construct a new instance of <see cref="TraceReceivedEventArgs"/> with details of the trace event.
        /// </summary>
        /// <param name="traceEventCache">The context information provided by <see cref="System.Diagnostics"/>.</param>
        /// <param name="source">The name of the trace source that delivered the trace data.</param>
        /// <param name="eventType">The type of event.</param>
        /// <param name="id">The id of the event.</param>
        /// <param name="data">The data to trace.</param>
        /// <param name="tag">A tag containing additionnal information about the trace event.</param>
        public TraceReceivedEventArgs(TraceEventCache traceEventCache, string source, TraceEventType eventType, int id, object data, string tag)
        {
            this.traceEventCache = traceEventCache;
            this.source = source;
            this.eventType = eventType;
            this.id = id;
            this.data = data;
            this.tag = tag;
        }

        /// <summary>
        /// The context information provided by <see cref="System.Diagnostics"/>.
        /// </summary>
        public TraceEventCache TraceEventCache
        {
            get { return traceEventCache; }
        }
        
        /// <summary>
        /// The name of the trace source that delivered the trace data.
        /// </summary>
        public string Source
        {
            get { return source; }
        }

        /// <summary>
        /// The type of event.
        /// </summary>
        public TraceEventType EventType
        {
            get { return eventType; }
        }

        /// <summary>
        /// The id of the event.
        /// </summary>
        public int Id
        {
            get { return id; }
        }

        /// <summary>
        /// The data to trace.
        /// </summary>
        public object Data
        {
            get { return data; }
        }

        /// <summary>
        /// A tag containing additionnal information about the trace event.
        /// </summary>
        public string Tag
        {
            get { return tag; }
        }
    }
}
