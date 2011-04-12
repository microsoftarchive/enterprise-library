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


#if !SILVERLIGHT
using System.Diagnostics;
#else
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
#endif
using System.Runtime.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Service
{
    /// <summary>
    /// Represents a log message contract.  Contains the common properties that are required for all log messages.
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
        public string TimeStamp { get; set; }

        /// <summary>
        /// The <see cref="AppDomain"/> in which the program is running
        /// </summary>
        [DataMember]
        public string AppDomainName { get; set; }

        /// <summary>
        /// The name of the .NET thread.
        /// </summary>
        [DataMember]
        public string ManagedThreadName { get; set; }

        /// <summary>
        /// Together with <see cref="ExtendedPropertiesValues"/>, this property represent the keys for a Dictionary of extended properties.
        /// </summary>
        [DataMember]
        public string[] ExtendedPropertiesKeys { get; set; }

        /// <summary>
        /// Together with <see cref="ExtendedPropertiesKeys"/>, this property represent the values for a Dictionary of extended properties.
        /// </summary>
        [DataMember]
        public string[] ExtendedPropertiesValues { get; set; }

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

        /// <summary>
        /// Name of the computer.
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// The Win32 process ID for the current running process.
        /// </summary>
        public string ProcessId { get; set; }

        /// <summary>
        /// The name of the current running process.
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// The Win32 Thread ID for the current thread.
        /// </summary>
        public string Win32ThreadId { get; set; }
    }
}
