using System;
using System.Linq;
using System.ServiceModel.Activation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Service
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [SilverlightFaultBehavior]
    public class LoggingService : ILoggingService
    {
        private LogWriter logWriter;

        public LoggingService()
            : this(Logger.Writer)
        {
        }

        public LoggingService(LogWriter logWriter)
        {
            this.logWriter = logWriter;
        }

        public void SendLogEntries(LogEntryMessage[] entries)
        {
            if (entries != null)
            {
                foreach (var message in entries)
                {
                    var entry = Translate(message);
                    this.logWriter.Write(entry);
                }
            }
        }

        private static LogEntry Translate(LogEntryMessage entry)
        {
            var translated = new LogEntry
            {
                Message = entry.Message,
                Categories = entry.Categories.ToArray(),
                Priority = entry.Priority,
                EventId = entry.EventId,
                Severity = entry.Severity,
                Title = entry.Title,
                TimeStamp = entry.TimeStamp.DateTime,   // TODO is this conversion OK?
                AppDomainName = entry.AppDomainName,
                ManagedThreadName = entry.ManagedThreadName,
                ExtendedProperties = entry.ExtendedProperties,
                ActivityId = entry.ActivityId,
                RelatedActivityId = entry.RelatedActivityId
            };

            return translated;
        }
    }
}
