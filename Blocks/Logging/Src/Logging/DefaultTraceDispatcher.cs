using System;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// A default implementation of <see cref="ITraceDispatcher"/> receiving trace events 
    /// and dispatching them throught its TraceReceived event handler.
    /// </summary>
    public class DefaultTraceDispatcher : ITraceDispatcher
    {
        /// <summary>
        /// The event raised when a trace event is received.
        /// </summary>
        public event EventHandler<TraceReceivedEventArgs> TraceReceived;

        /// <summary>
        /// Raise the TraceReceived event handler with the trace event information.
        /// </summary>
        /// <param name="traceEventCache">The context information provided by <see cref="System.Diagnostics"/>.</param>
        /// <param name="source">The name of the trace source that delivered the trace data.</param>
        /// <param name="eventType">The type of event.</param>
        /// <param name="id">The id of the event.</param>
        /// <param name="data">The data to trace.</param>
        /// <param name="tag">A tag containing additionnal information about the trace event.</param>
        public void ReceiveTrace(TraceEventCache traceEventCache, string source, TraceEventType eventType, int id, object data, string tag)
        {
            this.OnLogEntryReceived(new TraceReceivedEventArgs(traceEventCache, source, eventType, id, data, tag));
        }

        private void OnLogEntryReceived(TraceReceivedEventArgs e)
        {
            var handler = this.TraceReceived;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
