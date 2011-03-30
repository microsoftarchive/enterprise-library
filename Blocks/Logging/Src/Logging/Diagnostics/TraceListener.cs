using System;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics
{
    public abstract class TraceListener : IDisposable
    {
        public virtual bool IsThreadSafe
        {
            get
            {
                return false;
            }
        }

        private string listenerName;

        public string Name
        {
            get { return listenerName ?? string.Empty; }
            set { listenerName = value; }
        }


        public virtual void Flush()
        {
        }

        public virtual void TraceData(TraceEventCache traceEventCache, string name, TraceEventType eventType, int id, object data)
        {
            this.WriteLine(data == null ? string.Empty : data.ToString());
        }

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

        public abstract void Write(string message);

        public abstract void WriteLine(string message);
    }
}
