//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration
{
    /// <summary>
    /// Represents configuration for a <see cref="LoggingExceptionHandler"/>.
    /// </summary>
    partial class LoggingExceptionHandlerData
    {
        /// <summary>
        /// Initializes with default values.
        /// </summary>
        public LoggingExceptionHandlerData()
        {
            EventId = 100;
            Severity = TraceEventType.Error;
            Title = "Enterprise Library Exception Handling";
            FormatterTypeName =
                "Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.TextExceptionFormatter, Microsoft.Practices.EnterpriseLibrary.ExceptionHandling";
        }

        /// <summary>
        /// Gets or sets the default log category.
        /// </summary>
        public string LogCategory
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the default event ID.
        /// </summary>
        public int EventId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the default severity.
        /// </summary>
        public TraceEventType Severity
        {
            get;
            set;
        }

        /// <summary>
        ///  Gets or sets the default title.
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the formatter fully qualified assembly type name.
        /// </summary>
        /// <value>
        /// The formatter fully qualified assembly type name
        /// </value>
        public string FormatterTypeName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the minimum value for messages to be processed.  Messages with a priority
        /// below the minimum are dropped immediately on the client.
        /// </summary>
        public int Priority
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the default logger to be used.
        /// </summary>
        [Obsolete("Behavior is limited to UseDefaultLogger = true")]
        [Browsable(false)]
        public bool UseDefaultLogger
        {
            get;
            set;
        }
    }
}
