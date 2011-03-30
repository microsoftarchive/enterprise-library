using System;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// Represent a dispatcher for trace events.
    /// </summary>
    public interface ITraceDispatcher
    {
        /// <summary>
        /// The event raised when a trace event is received.
        /// </summary>
        event EventHandler<TraceReceivedEventArgs> TraceReceived;

        /// <summary>
        /// Raise the TraceReceived event handler with the trace event information.
        /// </summary>
        /// <param name="traceEventCache">The context information provided by <see cref="System.Diagnostics"/>.</param>
        /// <param name="source">The name of the trace source that delivered the trace data.</param>
        /// <param name="eventType">The type of event.</param>
        /// <param name="id">The id of the event.</param>
        /// <param name="data">The data to trace.</param>
        /// <param name="tag">A tag containing additionnal information about the trace event.</param>
        void ReceiveTrace(TraceEventCache traceEventCache, string source, TraceEventType eventType, int id, object data,
                      string tag);
    }
}
