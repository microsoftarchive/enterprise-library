using System;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Service;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
    public class NetworkTraceListener : TraceListener
    {
        private Func<ILoggingService> loggingServiceFactory;

        public NetworkTraceListener(Func<ILoggingService> loggingServiceFactory)
        {
            this.loggingServiceFactory = loggingServiceFactory;
        }

        public override bool IsThreadSafe
        {
            get { return true; }
        }

        public override void TraceData(TraceEventCache traceEventCache, string name, TraceEventType eventType, int id, object data)
        {
            var logEntry = data as LogEntry;

            if (logEntry == null)
            {
                logEntry = new LogEntry
                                {
                                    Message = (data ?? string.Empty).ToString(),
                                    Categories = new[] { name },
                                    Severity = eventType,
                                    EventId = id
                                };
            }

            logEntry.CollectIntrinsicProperties();

            var entries = new[] { Translate(logEntry) };
            var channel = this.loggingServiceFactory.Invoke();
            try
            {
                channel.BeginSendLogEntries(entries, r =>
                {
                    try
                    {
                        ((ILoggingService)r.AsyncState).EndSendLogEntries(r);
                    }
                    finally
                    {
                        using (r.AsyncState as IDisposable) { };
                    }
                }, channel);
            }
            catch
            {
                using (channel as IDisposable) { };
                throw;
            }
        }

        private LogEntryMessage Translate(LogEntry entry)
        {
            var translated = new LogEntryMessage
            {
                Message = entry.Message,
                Categories = entry.Categories.ToArray(),
                Priority = entry.Priority,
                EventId = entry.EventId,
                Severity = entry.Severity,
                Title = entry.Title,
                TimeStamp = entry.TimeStamp,
                AppDomainName = entry.AppDomainName,
                ManagedThreadName = entry.ManagedThreadName,
                ExtendedProperties = entry.ExtendedProperties,
                ActivityId = entry.ActivityId,
                RelatedActivityId = entry.RelatedActivityId
            };

            return translated;
        }

        public override void Write(string message)
        {
            throw new NotImplementedException();
        }

        public override void WriteLine(string message)
        {
            throw new NotImplementedException();
        }
    }
}
