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
using System.Linq;

#if !SILVERLIGHT
using System.Diagnostics;
#else
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
#endif
using System.Runtime.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Service
{
    /// <summary>
    /// Represents a log message.  Contains the common properties that are required for all log messages.
    /// </summary>
    [DataContract]
    public class LogEntryMessage
    {
        /// <summary>
        /// Message body to log.
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// Category name used to route the log entry to a one or more trace listeners.
        /// </summary>
        [DataMember]
        public string[] Categories { get; set; }

        /// <summary>
        /// Importance of the log message.  Only messages whose priority is between the minimum and maximum priorities (inclusive)
        /// will be processed.
        /// </summary>
        [DataMember]
        public int Priority { get; set; }

        /// <summary>
        /// Event number or identifier.
        /// </summary>
        [DataMember]
        public int EventId { get; set; }

        /// <summary>
        /// Log entry severity as a <see cref="Severity"/> enumeration. (Unspecified, Information, Warning or Error).
        /// </summary>
        [DataMember]
        public TraceEventType Severity { get; set; }

        /// <summary>
        /// Additional description of the log entry message.
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Date and time of the log entry message.
        /// </summary>
        [DataMember]
        public DateTimeOffset TimeStamp { get; set; }

        /// <summary>
        /// The <see cref="AppDomain"/> in which the program is running
        /// </summary>
        [DataMember]
        public string AppDomainName { get; set; }

        /// <summary>
        /// The name of the .NET thread.
        /// </summary>
        ///  <seealso cref="Win32ThreadId"/>
        [DataMember]
        public string ManagedThreadName { get; set; }

        /// <summary>
        /// Dictionary of key/value pairs to record.
        /// </summary>
        [DataMember]
        public IDictionary<string, object> ExtendedProperties { get; set; }

        /// <summary>
        /// Tracing activity id
        /// </summary>
        [DataMember]
        public Guid ActivityId { get; set; }

        /// <summary>
        /// Related activity id
        /// </summary>
        [DataMember]
        public Guid? RelatedActivityId { get; set; }
    }
}
