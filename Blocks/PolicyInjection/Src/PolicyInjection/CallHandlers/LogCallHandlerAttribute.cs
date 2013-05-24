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
using System.Diagnostics;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.PolicyInjection
{
    /// <summary>
    /// Applies the <see cref="LogCallHandler"/> to the target type, property, or method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
    public class LogCallHandlerAttribute : HandlerAttribute
    {
        private int eventId;
        private bool logBeforeCall = LogCallHandlerDefaults.LogBeforeCall;
        private bool logAfterCall = LogCallHandlerDefaults.LogAfterCall;
        private string beforeMessage = LogCallHandlerDefaults.BeforeMessage;
        private string afterMessage = LogCallHandlerDefaults.AfterMessage;
        private bool includeParameters = LogCallHandlerDefaults.IncludeParameters;
        private bool includeCallStack = LogCallHandlerDefaults.IncludeCallStack;
        private bool includeCallTime = LogCallHandlerDefaults.IncludeCallTime;
        private int priority = LogCallHandlerDefaults.Priority;
        private TraceEventType severity = LogCallHandlerDefaults.Severity;

        private string[] categories = new string[0];


        /// <summary>
        /// Gets or sets the collection of categories to place the log entries into.
        /// </summary>
        /// <remarks>The category strings can include replacement tokens. See
        /// the <see cref="CategoryFormatter"/> class for the list of tokens.</remarks>
        /// <value>The list of category strings.</value>
        public string[] Categories
        {
            get { return categories; }
            set { categories = value; }
        }

        /// <summary>
        /// Event ID to include in log entry.
        /// </summary>
        /// <value>event id</value>
        public int EventId
        {
            get { return eventId; }
            set { eventId = value; }
        }

        /// <summary>
        /// Should there be a log entry before calling the target?
        /// </summary>
        /// <value>true = yes, false = no</value>
        public bool LogBeforeCall
        {
            get { return logBeforeCall; }
            set { logBeforeCall = value; }
        }

        /// <summary>
        /// Should there be a log entry after calling the target?
        /// </summary>
        /// <value>true = yes, false = no</value>
        public bool LogAfterCall
        {
            get { return logAfterCall; }
            set { logAfterCall = value; }
        }

        /// <summary>
        /// Message to include in a pre-call log entry.
        /// </summary>
        /// <value>The message</value>
        public string BeforeMessage
        {
            get { return beforeMessage; }
            set { beforeMessage = value; }
        }

        /// <summary>
        /// Message to include in a post-call log entry.
        /// </summary>
        /// <value>the message.</value>
        public string AfterMessage
        {
            get { return afterMessage; }
            set { afterMessage = value; }
        }

        /// <summary>
        /// Should the log entry include the parameters to the call?
        /// </summary>
        /// <value>true = yes, false = no</value>
        public bool IncludeParameters
        {
            get { return includeParameters; }
            set { includeParameters = value; }
        }

        /// <summary>
        /// Should the log entry include the call stack?
        /// </summary>
        /// <remarks>Logging the call stack requires full trust code access security permissions.</remarks>
        /// <value>true = yes, false = no</value>
        public bool IncludeCallStack
        {
            get { return includeCallStack; }
            set { includeCallStack = value; }
        }

        /// <summary>
        /// Should the log entry include the time to execute the target?
        /// </summary>
        /// <value>true = yes, false = no</value>
        public bool IncludeCallTime
        {
            get { return includeCallTime; }
            set { includeCallTime = value; }
        }

        /// <summary>
        /// Priority for the log entry.
        /// </summary>
        /// <value>priority</value>
        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        /// <summary>
        /// Severity to log at.
        /// </summary>
        /// <value><see cref="TraceEventType"/> giving the severity.</value>
        public TraceEventType Severity
        {
            get { return severity; }
            set { severity = value; }
        }

        /// <summary>
        /// Creates the log handler for the target using the configured values.
        /// </summary>
        /// <returns>the created <see cref="LogCallHandler"/>.</returns>
        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            LogCallHandler handler = new LogCallHandler();

            SetCategories(handler);
            handler.EventId = eventId;
            handler.LogAfterCall = logAfterCall;
            handler.LogBeforeCall = logBeforeCall;
            handler.BeforeMessage = beforeMessage;
            handler.AfterMessage = afterMessage;
            handler.IncludeParameters = includeParameters;
            handler.IncludeCallStack = includeCallStack;
            handler.IncludeCallTime = includeCallTime;
            handler.Priority = priority;
            handler.Severity = severity;
            handler.Order = Order;

            return handler;
        }

        private void SetCategories(LogCallHandler handler)
        {
            handler.Categories.Clear();
            handler.Categories.AddRange(categories);
        }
    }
}
