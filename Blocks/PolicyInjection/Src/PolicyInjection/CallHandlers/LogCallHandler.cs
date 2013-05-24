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
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.PolicyInjection
{
    /// <summary>
    /// An <see cref="ICallHandler"/> that will log information using the
    /// Logging Application Block before and/or after the
    /// call to the target completes.
    /// </summary>
    [ConfigurationElementType(typeof(LogCallHandlerData))]
    public class LogCallHandler : ICallHandler
    {
        private int eventId = LogCallHandlerDefaults.EventId;
        private bool logBeforeCall = LogCallHandlerDefaults.LogBeforeCall;
        private bool logAfterCall = LogCallHandlerDefaults.LogAfterCall;
        private string beforeMessage = LogCallHandlerDefaults.BeforeMessage;
        private string afterMessage = LogCallHandlerDefaults.AfterMessage;
        private List<string> categories = new List<string>();
        private bool includeParameters = LogCallHandlerDefaults.IncludeParameters;
        private bool includeCallStack = LogCallHandlerDefaults.IncludeCallStack;
        private bool includeCallTime = LogCallHandlerDefaults.IncludeCallTime;
        private int priority = LogCallHandlerDefaults.Priority;
        private TraceEventType severity = LogCallHandlerDefaults.Severity;

        private int order = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogCallHandler"/> class with default settings.
        /// </summary>
        /// <remarks>See the <see cref="LogCallHandlerDefaults"/> class for the default values.</remarks>
        public LogCallHandler()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogCallHandler"/> class that writes entries using the given logging settings.
        /// </summary>
        /// <param name="eventId">The event ID to include in log entries.</param>
        /// <param name="logBeforeCall"><see langword="true"/> to log information before calling the target; otherwise, <see langword="false"/>.</param>
        /// <param name="logAfterCall"><see langword="true"/> to log information after calling the target; otherwise, <see langword="false"/>.</param>
        /// <param name="beforeMessage">The message to include in an  entry logged before the call.</param>
        /// <param name="afterMessage">The message to include in an entry logged after the call.</param>
        /// <param name="includeParameters"><see langword="true"/> to include the parameter values in the log entry; otherwise, <see langword="false"/>.</param>
        /// <param name="includeCallStack"><see langword="true"/> to include the current call stack in the log entry; otherwise, <see langword="false"/>.</param>
        /// <param name="includeCallTime"><see langword="true"/> to include the time to execute the target in the log entry; otherwise, <see langword="false"/>.</param>
        /// <param name="priority">The priority of the log entry.</param>
        public LogCallHandler(int eventId,
                              bool logBeforeCall,
                              bool logAfterCall, string beforeMessage, string afterMessage,
                              bool includeParameters, bool includeCallStack,
                              bool includeCallTime, int priority)
        {
            this.eventId = eventId;
            this.logBeforeCall = logBeforeCall;
            this.logAfterCall = logAfterCall;
            this.beforeMessage = beforeMessage;
            this.afterMessage = afterMessage;
            this.includeParameters = includeParameters;
            this.includeCallStack = includeCallStack;
            this.includeCallTime = includeCallTime;
            this.priority = priority;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogCallHandler"/> class that writes entries using the given logging settings.
        /// </summary>
        /// <param name="eventId">The event ID to include in log entries.</param>
        /// <param name="logBeforeCall"><see langword="true"/> to log information before calling the target; otherwise, <see langword="false"/>.</param>
        /// <param name="logAfterCall"><see langword="true"/> to log information after calling the target; otherwise, <see langword="false"/>.</param>
        /// <param name="beforeMessage">The message to include in an  entry logged before the call.</param>
        /// <param name="afterMessage">The message to include in an entry logged after the call.</param>
        /// <param name="includeParameters"><see langword="true"/> to include the parameter values in the log entry; otherwise, <see langword="false"/>.</param>
        /// <param name="includeCallStack"><see langword="true"/> to include the current call stack in the log entry; otherwise, <see langword="false"/>.</param>
        /// <param name="includeCallTime"><see langword="true"/> to include the time to execute the target in the log entry; otherwise, <see langword="false"/>.</param>
        /// <param name="priority">The priority of the log entry.</param>
        /// <param name="order">The order in which the handler will be executed.</param>
        public LogCallHandler(int eventId,
                              bool logBeforeCall,
                              bool logAfterCall, string beforeMessage, string afterMessage,
                              bool includeParameters, bool includeCallStack,
                              bool includeCallTime, int priority,
                              int order)
            : this(eventId, logBeforeCall, logAfterCall, beforeMessage, afterMessage, includeParameters, includeCallStack, includeCallTime, priority)
        {
            this.order = order;
        }

        /// <summary>
        /// Event ID to include in log entry
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
        /// Gets the collection of categories to place the log entries into.
        /// </summary>
        /// <remarks>The category strings can include replacement tokens. See
        /// the <see cref="CategoryFormatter"/> class for the list of tokens.</remarks>
        /// <value>The list of category strings.</value>
        public List<string> Categories
        {
            get { return categories; }
            set
            {
                categories.Clear();
                categories.AddRange(value);
            }
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

        #region ICallHandler Members
        /// <summary>
        /// Gets or sets the order in which the handler will be executed
        /// </summary>
        public int Order
        {
            get
            {
                return order;
            }
            set
            {
                order = value;
            }
        }

        /// <summary>
        /// Executes the call handler.
        /// </summary>
        /// <param name="input"><see cref="IMethodInvocation"/> containing the information about the current call.</param>
        /// <param name="getNext">delegate to get the next handler in the pipeline.</param>
        /// <returns>Return value from the target method.</returns>
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            if (getNext == null) throw new ArgumentNullException("getNext");

            LogPreCall(input);
            Stopwatch sw = new Stopwatch();
            if (includeCallTime)
            {
                sw.Start();
            }
            IMethodReturn result = getNext()(input, getNext);
            if (includeCallTime)
            {
                sw.Stop();
            }
            LogPostCall(input, sw, result);
            return result;
        }

        private void LogPreCall(IMethodInvocation input)
        {
            if (logBeforeCall)
            {
                TraceLogEntry logEntry = GetLogEntry(input);
                logEntry.Message = beforeMessage;
                Logger.Write(logEntry);
            }
        }

        private void LogPostCall(IMethodInvocation input, Stopwatch sw, IMethodReturn result)
        {
            if (logAfterCall)
            {
                TraceLogEntry logEntry = GetLogEntry(input);
                logEntry.Message = afterMessage;
                logEntry.ReturnValue = null;

                if (result.ReturnValue != null && includeParameters)
                {
                    logEntry.ReturnValue = result.ReturnValue.ToString();
                }
                if (result.Exception != null)
                {
                    logEntry.Exception = result.Exception.ToString();
                }
                if (includeCallTime)
                {
                    logEntry.CallTime = sw.Elapsed;
                }
                Logger.Write(logEntry);
            }
        }

        private TraceLogEntry GetLogEntry(IMethodInvocation input)
        {
            TraceLogEntry logEntry = new TraceLogEntry();
            CategoryFormatter formatter = new CategoryFormatter(input.MethodBase);
            foreach (string category in categories)
            {
                logEntry.Categories.Add(formatter.FormatCategory(category));
            }

            logEntry.EventId = eventId;
            logEntry.Priority = priority;
            logEntry.Severity = severity;
            logEntry.Title = LogCallHandlerDefaults.Title;

            if (includeParameters)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                for (int i = 0; i < input.Arguments.Count; ++i)
                {
                    parameters[input.Arguments.GetParameterInfo(i).Name] = input.Arguments[i];
                }

                logEntry.ExtendedProperties = parameters;
            }

            if (includeCallStack)
            {
                logEntry.CallStack = Environment.StackTrace;
            }

            logEntry.TypeName = input.Target.GetType().FullName;
            logEntry.MethodName = input.MethodBase.Name;
            return logEntry;
        }

        #endregion
    }

    /// <summary>
    /// Specifies the default values for the various <see cref="LogCallHandler"/> settings.
    /// </summary>
    /// <remarks>The default values are:
    /// <list type="table">
    /// <item><term>EventId</term><description>0</description></item>
    /// <item><term>LogBeforeCall</term><description>true</description></item>
    /// <item><term>LogAfterCall</term><description>true</description></item>
    /// <item><term>BeforeMessage</term><description></description></item>
    /// <item><term>AfterMessage</term><description></description></item>
    /// <item><term>Title</term><description>Call Logging</description></item>
    /// <item><term>IncludeParameters</term><description>true</description></item>
    /// <item><term>IncludeCallStack</term><description>false</description></item>
    /// <item><term>IncludeCallTime</term><description>true</description></item>
    /// <item><term>Priority</term><description>-1</description></item>
    /// <item><term>Severity</term><description><see cref="TraceEventType"/>.Information</description></item>
    /// </list></remarks>
    public static class LogCallHandlerDefaults
    {
        /// <summary>
        /// Default Order = 0
        /// </summary>
        public const int Order = 0;
        /// <summary>
        /// Default EventId = 0
        /// </summary>
        public const int EventId = 0;
        /// <summary>
        /// Default option to log before the call = true
        /// </summary>
        public const bool LogBeforeCall = true;
        /// <summary>
        /// Default option to log after the call = true
        /// </summary>
        public const bool LogAfterCall = true;
        /// <summary>
        /// Default message in before-call logs = nothing
        /// </summary>
        public static readonly string BeforeMessage = string.Empty;
        /// <summary>
        /// Default message in after-call logs = nothing
        /// </summary>
        public static readonly string AfterMessage = string.Empty;
        /// <summary>
        /// Default log entry title = "Call Logging" (localizable)
        /// </summary>
        public static readonly string Title = LogEntry.DefaultTitle;
        /// <summary>
        /// Default option to include parameter values = true
        /// </summary>
        public const bool IncludeParameters = true;
        /// <summary>
        /// Default option to include the call stack = false
        /// </summary>
        public const bool IncludeCallStack = false;
        /// <summary>
        /// Default option to include total time to call target = true
        /// </summary>
        public const bool IncludeCallTime = true;
        /// <summary>
        /// Default priority = -1
        /// </summary>
        public const int Priority = -1;
        /// <summary>
        /// Default severity = <see cref="TraceEventType"/>.Information
        /// </summary>
        public const TraceEventType Severity = TraceEventType.Information;
    }
}
