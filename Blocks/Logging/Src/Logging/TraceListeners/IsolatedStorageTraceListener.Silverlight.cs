using System;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
    /// <summary>
    /// A trace listener that logs entries to a log entry repository in isolated storage.
    /// </summary>
    public class IsolatedStorageTraceListener : TraceListener
    {
        private ILogEntryRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatedStorageTraceListener"/> class with a repository.
        /// </summary>
        /// <param name="repository">The repository to store log entries.</param>
        public IsolatedStorageTraceListener(ILogEntryRepository repository)
        {
            this.repository = repository;
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

            this.repository.Add(logEntry);
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
        /// 
        /// </summary>
        public override void Flush()
        {
            this.repository.Flush();
        }
    }
}
