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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Service
{
    /// <summary>
    /// Contains extension methods for the <see cref="LogEntry"/> class.
    /// </summary>
    public static partial class LogEntryExtensions
    {
        /// <summary>
        /// Maps a <see cref="LogEntry"/> to a <see cref="LogEntryMessage"/>.
        /// </summary>
        /// <param name="entry">The log entry to map.</param>
        /// <returns>The resulting message.</returns>
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
