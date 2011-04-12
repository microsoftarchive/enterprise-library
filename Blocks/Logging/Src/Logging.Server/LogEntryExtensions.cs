using System;
using System.Globalization;
using System.Linq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Service
{
    public static partial class LogEntryExtensions
    {
        public static LogEntry ToLogEntry(this LogEntryMessage entry)
        {
            var translated = new LogEntry
            {
                Message = entry.Message,
                Categories = entry.Categories.ToList(),
                Priority = entry.Priority,
                EventId = entry.EventId,
                Severity = entry.Severity,
                Title = entry.Title,
                TimeStamp = DateTime.Parse(entry.TimeStamp, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
                AppDomainName = entry.AppDomainName,
                ManagedThreadName = entry.ManagedThreadName,
                ActivityId = entry.ActivityId,
                RelatedActivityId = entry.RelatedActivityId,
                MachineName = entry.MachineName,
                ProcessId = entry.ProcessId,
                ProcessName = entry.ProcessName,
                Win32ThreadId = entry.Win32ThreadId
            };

            if (entry.ExtendedPropertiesKeys != null
                && entry.ExtendedPropertiesValues != null
                && entry.ExtendedPropertiesKeys.Length == entry.ExtendedPropertiesValues.Length)
            {
                for (int i = 0; i < entry.ExtendedPropertiesKeys.Length; i++)
                {
                    translated.ExtendedProperties[entry.ExtendedPropertiesKeys[i]] = entry.ExtendedPropertiesValues[i];
                }
            }

            return translated;
        }
    }
}
