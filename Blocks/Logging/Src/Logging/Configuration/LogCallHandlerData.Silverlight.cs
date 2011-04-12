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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;


namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// A class with the data for the LogCallHandler.
    /// </summary>
    public partial class LogCallHandlerData 
    {
        /// <summary>
        /// Construct a new <see cref="LogCallHandlerData"/> instance.
        /// </summary>
        public LogCallHandlerData()
        {
        }

        /// <summary>
        /// Construct a new <see cref="LogCallHandlerData"/> instance.
        /// </summary>
        /// <param name="handlerName">Handler name</param>
        public LogCallHandlerData(string handlerName)
        {
            Name = handlerName;
        }

        /// <summary>
        /// Construct a new <see cref="LogCallHandlerData"/> instance.
        /// </summary>
        /// <param name="handlerName">Handler name</param>
        /// <param name="handlerOrder">Handler order</param>
        public LogCallHandlerData(string handlerName, int handlerOrder)
            : this(handlerName)
        {
            this.Order = handlerOrder;
        }

        /// <summary>
        /// Should the handler log before the call, after the call, or both?
        /// </summary>
        /// <value>The "logBehavior" config attribute.</value>
        public HandlerLogBehavior LogBehavior
        {
            get;
            set;
        }

        NamedElementCollection<LogCallHandlerCategoryEntry> categories = new NamedElementCollection<LogCallHandlerCategoryEntry>();

        /// <summary>
        /// Collection of log categories to use in the log message.
        /// </summary>
        public NamedElementCollection<LogCallHandlerCategoryEntry> Categories
        {
            get { return categories; }
        }

        /// <summary>
        /// Message for the log entry in before-call logging.
        /// </summary>
        public string BeforeMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Message for the log entry in after-call logging.
        /// </summary>
        public string AfterMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Event Id to put into log entries.
        /// </summary>
        public int EventId
        {
            get;
            set;
        }

        /// <summary>
        /// Include parameter values and return values in the log entry
        /// </summary>
        public bool IncludeParameterValues
        {
            get;
            set;
        }

        /// <summary>
        /// Include the call stack in the log entries.
        /// </summary>
        /// <remarks>Setting this to true requires UnmanagedCode permissions.</remarks>
        public bool IncludeCallStack
        {
            get;
            set;
        }

        /// <summary>
        /// Include the time to execute the call in the log entries.
        /// </summary>
        public bool IncludeCallTime
        {
            get;
            set;
        }

        /// <summary>
        /// The priority of the log entries.
        /// </summary>
        public int Priority
        {
            get;
            set;
        }

        /// <summary>
        /// The severity of the log entry.
        /// </summary>
        public TraceEventType Severity
        {
            get;
            set;
        }
    }
}
