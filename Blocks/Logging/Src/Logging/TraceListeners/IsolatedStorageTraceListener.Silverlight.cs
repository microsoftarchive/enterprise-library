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
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
    /// <summary>
    /// A trace listener that logs entries to a log entry repository in isolated storage.
    /// </summary>
    /// <remarks>
    /// Older entries will be automatically discarded by new ones if the specified maximum size is reached.
    /// </remarks> 
    public class IsolatedStorageTraceListener : TraceListener
    {
        private readonly ILogEntryRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatedStorageTraceListener"/> class with a repository.
        /// </summary>
        /// <param name="repository">The repository to store log entries.</param>
        public IsolatedStorageTraceListener(ILogEntryRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// The repository used to store log entries.
        /// </summary>
        public ILogEntryRepository Repository
        {
            get { return this.repository; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="traceEventCache"></param>
        /// <param name="name"></param>
        /// <param name="eventType"></param>
        /// <param name="id"></param>
        /// <param name="data"></param>
        public override void TraceData(TraceEventCache traceEventCache, string name, TraceEventType eventType, int id, object data)
        {
            var logEntry = data as LogEntry;

            if (logEntry == null)
            {
                logEntry =
                    new LogEntry
                    {
                        Message = (data ?? string.Empty).ToString(),
                        Categories = new[] { name },
                        Severity = eventType,
                        EventId = id
                    };
            }

            this.Repository.Add(logEntry);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public override void Write(string message)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public override void WriteLine(string message)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Flushes the output buffer.
        /// </summary>
        public override void Flush()
        {
            this.Repository.Flush();
        }

        /// <summary>
        /// Provides an update context to batch change requests to the <see cref="IsolatedStorageTraceListener"/> configuration.
        /// </summary>
        /// <returns>Returns an <see cref="IIsolatedStorageTraceListenerUpdateContext"/> instance that can be used to apply the configuration changes.</returns>
        protected internal override ITraceListenerUpdateContext GetUpdateContext()
        {
            return new IsolatedStorageTraceListenerUpdateContext(this);
        }

        /// <summary>
        /// Provides an update context for changing the <see cref="IsolatedStorageTraceListener"/> settings.
        /// </summary>
        protected class IsolatedStorageTraceListenerUpdateContext : TraceListenerUpdateContext, IIsolatedStorageTraceListenerUpdateContext
        {
            private int maxSizeInKilobytes;

            /// <summary>
            /// Initializes a new instance of <see cref="IsolatedStorageTraceListenerUpdateContext"/>.
            /// </summary>
            /// <param name="traceListener">The <see cref="TraceListener"/> being configured.</param>
            public IsolatedStorageTraceListenerUpdateContext(IsolatedStorageTraceListener traceListener)
                : base(traceListener)
            {
                this.IsRepositoryAvailable = traceListener.repository.IsAvailable;
                if (this.IsRepositoryAvailable)
                {
                    this.maxSizeInKilobytes = traceListener.Repository.ActualMaxSizeInKilobytes;
                }
            }

            /// <summary>
            /// Gets a value indicating if the underlying repository is available for the running instance.
            /// </summary>
            public bool IsRepositoryAvailable { get; private set; }

            /// <summary>
            /// Gets or sets the maximum size in kilobytes to be used in the isolated storage by the log entry repository.
            /// </summary>
            /// <remarks>When the repository is resized, it will try to allocate the specified space, but a smaller size than the 
            /// specified might be allocated if not as much space is available.</remarks>
            /// <returns>The maximum size in kilobytes as available when the storage was initialized.</returns>
            public int MaxSizeInKilobytes
            {
                get { return this.maxSizeInKilobytes; }
                set
                {
                    if (!this.IsRepositoryAvailable)
                    {
                        throw new InvalidOperationException(Resources.MaxSizeInKilobytesCannotBeChanged);
                    }

                    this.maxSizeInKilobytes = value;
                }
            }

            /// <summary>
            /// Applies the changes.
            /// </summary>
            protected internal override void ApplyChanges()
            {
                base.ApplyChanges();

                var repository = ((IsolatedStorageTraceListener)this.TraceListener).Repository;
                if (repository.ActualMaxSizeInKilobytes != this.MaxSizeInKilobytes)
                {
                    repository.Resize(this.MaxSizeInKilobytes);
                }
            }
        }
    }
}
