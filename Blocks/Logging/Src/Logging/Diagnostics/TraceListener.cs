using System;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics
{
    /// <summary>
    /// Provides the abstract base class for the listeners who monitor trace and debug output.
    /// </summary>
    public abstract class TraceListener : IDisposable
    {
        private string listenerName;

        /// <summary>
        /// Gets a value indicating whether this instance is thread safe.
        /// </summary>
        public virtual bool IsThreadSafe
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets or sets the name for the trace listener.
        /// </summary>
        public string Name
        {
            get { return listenerName ?? string.Empty; }
            set { listenerName = value; }
        }

        /// <summary>
        /// When overridden in a derived class, flushes the output buffer.
        /// </summary>
        public virtual void Flush()
        {
        }

        /// <summary>
        /// Writes trace information, a data object and event information to the listener specific output.
        /// </summary>
        /// <param name="traceEventCache">A <see cref="TraceEventCache"/> object that contains context information.</param>
        /// <param name="source">A name used to identify the output.</param>
        /// <param name="eventType">One of the <see cref="TraceEventType"/> values specifying the type of event that has caused the trace.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="data">The data.</param>
        public virtual void TraceData(TraceEventCache traceEventCache, string source, TraceEventType eventType, int id, object data)
        {
            this.WriteLine(data == null ? string.Empty : data.ToString());
        }

        /// <summary>
        /// Writes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public abstract void Write(string message);

        /// <summary>
        /// Writes the specified message, followed by a line terminator.
        /// </summary>
        /// <param name="message">The message.</param>
        public abstract void WriteLine(string message);

        /// <summary>
        /// Finalizer for TraceListener
        /// </summary>
        ~TraceListener()
        {
            Dispose(false);
        }

        /// <summary>
        /// Dispose method for all backing stores. This implementation is sufficient for any class that does not need any finalizer behavior
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposing method as used in the Dispose pattern
        /// </summary>
        /// <param name="disposing">True if called during Dispose. False if called from finalizer</param>
        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
