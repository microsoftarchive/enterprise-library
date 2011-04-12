using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Service
{
    public static partial class LogEntryExtensions
    {
        public static LogEntryMessage ToLogEntryMessage(this LogEntry entry)
        {
            var extendedProperties = (entry.ExtendedProperties ?? Enumerable.Empty<KeyValuePair<String, object>>()).ToList();
            var translated = new LogEntryMessage
            {
                Message = entry.Message,
                Categories = entry.Categories.ToArray(),
                Priority = entry.Priority,
                EventId = entry.EventId,
                Severity = entry.Severity,
                Title = entry.Title,
                TimeStamp = entry.TimeStamp.ToString("o", CultureInfo.InvariantCulture),
                AppDomainName = entry.AppDomainName,
                ManagedThreadName = entry.ManagedThreadName,
                ExtendedPropertiesKeys = extendedProperties.Select(x => x.Key).ToArray(),
                ExtendedPropertiesValues = extendedProperties.Select(x => (x.Value ?? "(null)").ToString()).ToArray(),
                ActivityId = entry.ActivityId,
                RelatedActivityId = entry.RelatedActivityId
            };

            return translated;
        }
    }
}
