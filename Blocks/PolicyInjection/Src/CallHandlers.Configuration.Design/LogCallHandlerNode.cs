//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration.Design
{
    /// <summary>
    /// A <see cref="ConfigurationNode"/> that represents the configuration
    /// of an <see cref="LogCallHandler" /> in the Configuration
    /// Console.
    /// </summary>
    public class LogCallHandlerNode : CallHandlerNode
    {
        string afterMessage;
        string beforeMessage;
        List<LogCategory> categories;
        int eventId;
        bool includeCallStack;
        bool includeCallTime;
        bool includeParameterValues;
        HandlerLogBehavior logBehavior;
        int priority;
        TraceEventType severity;

        /// <summary>
        /// Create a new <see cref="LogCallHandlerNode"/> with default settings.
        /// </summary>
        public LogCallHandlerNode()
            : this(new LogCallHandlerData(Resources.LogCallHandlerNodeName)) {}

        /// <summary>
        /// Create a new <see cref="LogCallHandlerNode"/> with the given settings.
        /// </summary>
        /// <param name="callHandlerData">Configuration information to initialize the node with.</param>
        public LogCallHandlerNode(LogCallHandlerData callHandlerData)
            : base(callHandlerData)
        {
            categories = new List<LogCategory>();
            foreach (LogCallHandlerCategoryEntry categoryEntry in callHandlerData.Categories)
            {
                categories.Add(new LogCategory(categoryEntry.Name));
            }
            LogBehavior = callHandlerData.LogBehavior;
            beforeMessage = callHandlerData.BeforeMessage;
            afterMessage = callHandlerData.AfterMessage;
            eventId = callHandlerData.EventId;
            includeCallStack = callHandlerData.IncludeCallStack;
            includeParameterValues = callHandlerData.IncludeParameterValues;
            includeCallTime = callHandlerData.IncludeCallTime;
            priority = callHandlerData.Priority;
            severity = callHandlerData.Severity;
        }

        /// <summary>
        /// Message to include when logging after the call has returned.
        /// </summary>
        /// <value>message string.</value>
        [SRDescription("AfterMessageDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        public string AfterMessage
        {
            get { return afterMessage; }
            set { afterMessage = value; }
        }

        /// <summary>
        /// Message to include when logging before the call occurs.
        /// </summary>
        /// <value>message string.</value>
        [SRDescription("BeforeMessageDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        public string BeforeMessage
        {
            get { return beforeMessage; }
            set { beforeMessage = value; }
        }

        /// <summary>
        /// Categories to use for the log message.
        /// </summary>
        /// <value>List of <see cref="LogCategory"/>.</value>
        [SRDescription("CategoriesDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [LogCategoriesAttribute]
        public List<LogCategory> Categories
        {
            get { return categories; }
            set { categories = value; }
        }

        /// <summary>
        /// EventId to log with.
        /// </summary>
        /// <value>integer event id.</value>
        [SRDescription("EventIdDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public int EventId
        {
            get { return eventId; }
            set { eventId = value; }
        }

        /// <summary>
        /// Should the call stack be included in the log message?
        /// </summary>
        /// <value>true then yes, false then no.</value>
        [SRDescription("IncludeCallStackDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public bool IncludeCallStack
        {
            get { return includeCallStack; }
            set { includeCallStack = value; }
        }

        /// <summary>
        /// Should the call duration be included in the log message?
        /// </summary>
        /// <value>true then yes, false then no.</value>
        [SRDescription("IncludeCallTimeDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public bool IncludeCallTime
        {
            get { return includeCallTime; }
            set { includeCallTime = value; }
        }

        /// <summary>
        /// Should the parameter values be included in the log message?
        /// </summary>
        /// <value>true then yes, false then no.</value>
        [SRDescription("IncludeParameterValuesDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public bool IncludeParameterValues
        {
            get { return includeParameterValues; }
            set { includeParameterValues = value; }
        }

        /// <summary>
        /// When should logging occur.
        /// </summary>
        /// <value>Before call, after call or both.</value>
        [SRDescription("LogBehaviorDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public HandlerLogBehavior LogBehavior
        {
            get { return logBehavior; }
            set { logBehavior = value; }
        }

        /// <summary>
        /// Priority of log entry.
        /// </summary>
        /// <value>integer priority.</value>
        [SRDescription("PriorityDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        /// <summary>
        /// Severity of log entry.
        /// </summary>
        /// <value>severity as indicated by the <see cref="TraceEventType"/>.</value>
        [SRDescription("SeverityDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public TraceEventType Severity
        {
            get { return severity; }
            set { severity = value; }
        }

        /// <summary>
        /// Converts the information stored in the node and generate
        /// the corresponding configuration element to store in
        /// an <see cref="Microsoft.Practices.EnterpriseLibrary.Common.Configuration.IConfigurationSource" />.
        /// </summary>
        /// <returns>Newly created <see cref="LogCallHandlerData"/> containing
        /// the configuration data from this node.</returns>
        public override CallHandlerData CreateCallHandlerData()
        {
            LogCallHandlerData callHandlerData = new LogCallHandlerData(Name, Order);
            callHandlerData.EventId = eventId;
            callHandlerData.Severity = severity;
            callHandlerData.Priority = priority;
            callHandlerData.IncludeCallTime = includeCallTime;
            callHandlerData.IncludeCallStack = includeCallStack;
            callHandlerData.IncludeParameterValues = includeParameterValues;
            callHandlerData.BeforeMessage = beforeMessage;
            callHandlerData.AfterMessage = afterMessage;
            callHandlerData.LogBehavior = logBehavior;
            foreach (LogCategory category in categories)
            {
                callHandlerData.Categories.Add(new LogCallHandlerCategoryEntry(category.CategoryName));
            }

            return callHandlerData;
        }
    }
}
