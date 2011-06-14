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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.Unity.Utility;

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
        /// Provides an update context to batch change requests to the <see cref="TraceListener"/> configuration.
        /// </summary>
        /// <returns>Returns an <see cref="ITraceListenerUpdateContext"/> instance that can be used to apply the configuration changes.</returns>
        protected internal virtual ITraceListenerUpdateContext GetUpdateContext()
        {
            return new TraceListenerUpdateContext(this);
        }

        /// <summary>
        /// Provides an update context for changing the <see cref="TraceListener"/> settings.
        /// </summary>
        protected class TraceListenerUpdateContext : ITraceListenerUpdateContext, ICommitable
        {
            private readonly TraceListener traceListener;

            /// <summary>
            /// Initializes a new instance of <see cref="TraceListenerUpdateContext"/>.
            /// </summary>
            /// <param name="traceListener">The <see cref="TraceListener"/> being configured.</param>
            public TraceListenerUpdateContext(TraceListener traceListener)
            {
                Guard.ArgumentNotNull(traceListener, "traceListener");

                this.traceListener = traceListener;
                this.Name = this.TraceListener.Name;
            }

            /// <summary>
            /// Gets the name for the <see cref="TraceListener"/> instance being configured.
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            /// Gets the <see cref="TraceListener"/> being configured.
            /// </summary>
            protected TraceListener TraceListener
            {
                get { return this.traceListener; }
            }

            /// <summary>
            /// Applies the changes.
            /// </summary>
            protected internal virtual void ApplyChanges()
            {
            }

            void ICommitable.Commit()
            {
                this.ApplyChanges();
            }
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
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> if the method is being called from the <see cref="Dispose()"/> method. <see langword="false"/> if it is being called from within the object finalizer.</param>
        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
