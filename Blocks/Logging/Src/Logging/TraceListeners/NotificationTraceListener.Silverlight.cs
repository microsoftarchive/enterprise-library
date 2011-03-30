using System;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
    /// <summary>
    /// Trace listener that raises an event while tracing.
    /// </summary>
    public class NotificationTraceListener : TraceListener
    {
        private readonly ITraceDispatcher traceDispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationTraceListener"/> class.
        /// </summary>
        public NotificationTraceListener(ITraceDispatcher traceDispatcher)
        {
            this.traceDispatcher = traceDispatcher;
        }

        /// <summary>
        /// Send the tracing information to the provided ITraceDispatcher with the trace event.
        /// </summary>
        /// <param name="traceEventCache">The context information provided by <see cref="System.Diagnostics"/>.</param>
        /// <param name="source">The name of the trace source that delivered the trace data.</param>
        /// <param name="eventType">The type of event.</param>
        /// <param name="id">The id of the event.</param>
        /// <param name="data">The data to trace.</param>
        public override void TraceData(TraceEventCache traceEventCache, string source, TraceEventType eventType, int id, object data)
        {
            if (this.traceDispatcher != null)
            {
                this.traceDispatcher.ReceiveTrace(traceEventCache, source, eventType, id, data, Name);
            }
        }

        public override void Write(string message)
        {
            throw new NotSupportedException();
        }

        public override void WriteLine(string message)
        {
            throw new NotSupportedException();
        }

        public override bool IsThreadSafe
        {
            get
            {
                return true;
            }
        }
    }
}
