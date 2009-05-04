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
using System.Diagnostics;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design
{
    /// <summary>
    /// Represents a design time representation of a <see cref="LoggingExceptionHandlerData"/> configuration element.
    /// </summary>
    public sealed class LoggingExceptionHandlerNode : ExceptionHandlerNode
    {
        int eventId;
        string formatterTypeName;
        string logCategoryName;
        CategoryTraceSourceNode logCategoryNode;
        EventHandler<ConfigurationNodeChangedEventArgs> logCategoryNodeRemoved;
        EventHandler<ConfigurationNodeChangedEventArgs> logCategoryNodeRenamed;
        int priority;
        TraceEventType severity;
        string title;
        bool useDefaultLogger;

        /// <summary>
        /// Initialize a new instance of the <see cref="LoggingExceptionHandlerNode"/> class.
        /// </summary>
        public LoggingExceptionHandlerNode()
            : this(new LoggingExceptionHandlerData(Resources.LoggingHandlerName, Resources.DefaultCategory, 100, TraceEventType.Error, Resources.DefaultTitle, string.Empty, 0)) {}

        /// <summary>
        /// Initialize a new instance of the <see cref="LoggingExceptionHandlerNode"/> class with a <see cref="LoggingExceptionHandlerData"/> instance.
        /// </summary>
        /// <param name="loggingExceptionHandlerData">A <see cref="LoggingExceptionHandlerData"/> instance.</param>
        public LoggingExceptionHandlerNode(LoggingExceptionHandlerData loggingExceptionHandlerData)
        {
            if (null == loggingExceptionHandlerData) throw new ArgumentNullException("loggingExceptionHandlerData");

            Rename(loggingExceptionHandlerData.Name);
            eventId = loggingExceptionHandlerData.EventId;
            severity = loggingExceptionHandlerData.Severity;
            title = loggingExceptionHandlerData.Title;
            formatterTypeName = loggingExceptionHandlerData.FormatterTypeName;
            logCategoryName = loggingExceptionHandlerData.LogCategory;
            priority = loggingExceptionHandlerData.Priority;
            logCategoryNodeRemoved = new EventHandler<ConfigurationNodeChangedEventArgs>(OnLogCategoryNodeRemoved);
            logCategoryNodeRenamed = new EventHandler<ConfigurationNodeChangedEventArgs>(OnLogCategoryNodeRenamed);
            useDefaultLogger = loggingExceptionHandlerData.UseDefaultLogger;
        }

        /// <summary>
        /// Gets or sets the event id.
        /// </summary>
        /// <value>
        /// The event id.
        /// </value>
        [Required]
        [SRDescription("DefaultEventIdDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public int EventId
        {
            get { return eventId; }
            set { eventId = value; }
        }

        /// <summary>
        /// Gets the <see cref="LoggingExceptionHandlerData"/> this node represents.
        /// </summary>
        /// <value>
        /// The <see cref="LoggingExceptionHandlerData"/> this node represents.
        /// </value>
        public override ExceptionHandlerData ExceptionHandlerData
        {
            get { return new LoggingExceptionHandlerData(Name, logCategoryName, eventId, severity, title, formatterTypeName, priority, useDefaultLogger); }
        }

        /// <summary>
        /// Gets or sets the formatter type to use.
        /// </summary>
        /// <value>
        /// The formatter type.
        /// </value>
        [Required]
        [SRDescription("FormatterTypeNameDescription", typeof(Resources))]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(ExceptionFormatter))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string FormatterType
        {
            get { return formatterTypeName; }
            set { formatterTypeName = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="CategoryTraceSourceNode"/> that represents the log category.
        /// </summary>
        /// <value>
        /// The <see cref="CategoryTraceSourceNode"/> that represents the log category.
        /// </value>
        [Required]
        [SRDescription("DefaultLogCategoryDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(CategoryTraceSourceNode))]
        public CategoryTraceSourceNode LogCategory
        {
            get { return logCategoryNode; }
            set
            {
                logCategoryNode = LinkNodeHelper.CreateReference<CategoryTraceSourceNode>(logCategoryNode,
                                                                                          value,
                                                                                          logCategoryNodeRemoved,
                                                                                          logCategoryNodeRenamed);

                logCategoryName = logCategoryNode == null ? String.Empty : logCategoryNode.Name;
            }
        }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        [Required]
        [SRDescription("MinimumPriorityDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        /// <summary>
        /// Gets or sets the severity.
        /// </summary>
        /// <value>
        /// One of the <see cref="TraceEventType"/> values.
        /// </value>
        [Required]
        [SRDescription("DefaultSeverityDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public TraceEventType Severity
        {
            get { return severity; }
            set { severity = value; }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [Required]
        [SRDescription("DefaultTitleDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// Gets or sets the use default logger.
        /// </summary>
        [SRDescription("UseDefaultLoggerDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public bool UseDefaultLogger
        {
            get { return useDefaultLogger; }
            set { useDefaultLogger = value; }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="LoggingExceptionHandlerNode"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != logCategoryNode)
                {
                    logCategoryNode.Removed -= logCategoryNodeRemoved;
                    logCategoryNode.Renamed -= logCategoryNodeRenamed;
                }
            }
            base.Dispose(disposing);
        }

        void OnLogCategoryNodeRemoved(object sender,
                                      ConfigurationNodeChangedEventArgs e)
        {
            logCategoryNode = null;
        }

        void OnLogCategoryNodeRenamed(object sender,
                                      ConfigurationNodeChangedEventArgs e)
        {
            logCategoryName = e.Node.Name;
        }
    }
}
