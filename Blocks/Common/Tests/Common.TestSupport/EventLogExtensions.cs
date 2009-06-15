//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Microsoft.Practices.EnterpriseLibrary.Common.TestSupport
{
    public static class EventLogExtensions
    {
        public static IEnumerable<EventLogEntry> GetNewEntries(this EventLog log, int oldEntriesCount)
        {
            for (int index = oldEntriesCount; index < log.Entries.Count; ++index)
            {
                yield return log.Entries[index];
            }
        }

        public static IEnumerable<EventLogEntry> GetEntriesSince(this EventLog log, DateTime startTime)
        {
            var entries = log.Entries;
            int i = entries.Count;

            do
            {
                var entry = entries[--i];
                if(entry.TimeGenerated < startTime)
                {
                    break;
                }
                yield return entry;
            } while (i >= 0);
        }
    }
}
