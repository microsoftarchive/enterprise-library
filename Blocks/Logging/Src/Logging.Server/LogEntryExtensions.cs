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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Microsoft.Practices.EnterpriseLibrary.Logging.Service.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Service
{
    /// <summary>
    /// Contains useful extension methods to handle the <see cref="LogEntry"/> class.
    /// </summary>
    public static partial class LogEntryExtensions
    {
        /// <summary>
        /// Maps a <see cref="LogEntryMessage"/> to a <see cref="LogEntry"/>.
        /// </summary>
        /// <param name="entry">The log entry message to map.</param>
        /// <returns>The resulting entry.</returns>
        public static LogEntry ToLogEntry(this LogEntryMessage entry)
        {
            DateTime timeStamp;
            if (!DateTime.TryParse(entry.TimeStamp, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out timeStamp))
            {
                throw new ArgumentException(Resources.CannotDeserializeTimeStamp, "entry");
            }

            if ((entry.ExtendedPropertiesKeys != null ? entry.ExtendedPropertiesKeys.Length : 0) !=
                (entry.ExtendedPropertiesValues != null ? entry.ExtendedPropertiesValues.Length : 0))
            {
                throw new ArgumentException(Resources.CannotDeserializeExtendedProperties, "entry");
            }

            var translated = new LogEntry
            {
                Message = entry.Message,
                Categories = entry.Categories != null ? entry.Categories.ToList() : new List<string>(),
                Priority = entry.Priority,
                EventId = entry.EventId,
                Severity = entry.Severity,
                Title = entry.Title,
                TimeStamp = timeStamp,
                AppDomainName = entry.AppDomainName,
                ManagedThreadName = entry.ManagedThreadName,
                ActivityId = entry.ActivityId,
                RelatedActivityId = entry.RelatedActivityId,
                MachineName = entry.MachineName,
                ProcessId = entry.ProcessId,
                ProcessName = entry.ProcessName,
                Win32ThreadId = entry.Win32ThreadId
            };

            if (entry.ExtendedPropertiesKeys != null && entry.ExtendedPropertiesValues != null)
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
