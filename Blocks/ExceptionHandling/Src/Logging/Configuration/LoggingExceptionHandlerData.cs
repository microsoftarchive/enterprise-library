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
using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration
{
    /// <summary>
    /// Represents configuration for a <see cref="LoggingExceptionHandler"/>.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "LoggingExceptionHandlerDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "LoggingExceptionHandlerDataDisplayName")]
    [AddSateliteProviderCommand(LoggingSettings.SectionName)]
    public class LoggingExceptionHandlerData : ExceptionHandlerData
    {
        private static readonly AssemblyQualifiedTypeNameConverter typeConverter
            = new AssemblyQualifiedTypeNameConverter();

        private const string logCategory = "logCategory";
        private const string eventId = "eventId";
        private const string severity = "severity";
        private const string title = "title";
        private const string formatterType = "formatterType";
        private const string priority = "priority";
        private const string useDefaultLogger = "useDefaultLogger";

        /// <summary>
        /// Initializes with default values.
        /// </summary>
        public LoggingExceptionHandlerData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingExceptionHandlerData"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the handler.
        /// </param>
        /// <param name="logCategory">
        /// The default log category.
        /// </param>
        /// <param name="eventId">
        /// The default eventID.
        /// </param>
        /// <param name="severity">
        /// The default severity.
        /// </param>
        /// <param name="title">
        /// The default title.
        /// </param>
        /// <param name="formatterType">
        /// The formatter type.
        /// </param>
        /// <param name="priority">
        /// The minimum value for messages to be processed.  Messages with a priority below the minimum are dropped immediately on the client.
        /// </param>
        public LoggingExceptionHandlerData(string name,
                                           string logCategory,
                                           int eventId,
                                           TraceEventType severity,
                                           string title,
                                           Type formatterType,
                                           int priority)
            : this(name, logCategory, eventId, severity, title, typeConverter.ConvertToString(formatterType), priority)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingExceptionHandlerData"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the handler.
        /// </param>
        /// <param name="logCategory">
        /// The default log category.
        /// </param>
        /// <param name="eventId">
        /// The default eventID.
        /// </param>
        /// <param name="severity">
        /// The default severity.
        /// </param>
        /// <param name="title">
        /// The default title.
        /// </param>
        /// <param name="formatterTypeName">
        /// The formatter fully qualified assembly type name.
        /// </param>
        /// <param name="priority">
        /// The minimum value for messages to be processed.  Messages with a priority below the minimum are dropped immediately on the client.
        /// </param>
        public LoggingExceptionHandlerData(string name,
                                           string logCategory,
                                           int eventId,
                                           TraceEventType severity,
                                           string title,
                                           string formatterTypeName,
                                           int priority)
            : base(name, typeof(LoggingExceptionHandler))
        {
            LogCategory = logCategory;
            EventId = eventId;
            Severity = severity;
            Title = title;
            FormatterTypeName = formatterTypeName;
            Priority = priority;
        }

        /// <summary>
        /// Gets or sets the default log category.
        /// </summary>
        [ConfigurationProperty(logCategory, IsRequired = true)]
        [Reference(typeof(NamedElementCollection<TraceSourceData>), typeof(TraceSourceData))]
        [ResourceDescription(typeof(DesignResources), "LoggingExceptionHandlerDataLogCategoryDescription")]
        [ResourceDisplayName(typeof(DesignResources), "LoggingExceptionHandlerDataLogCategoryDisplayName")]
        public string LogCategory
        {
            get { return (string)this[logCategory]; }
            set { this[logCategory] = value; }
        }

        /// <summary>
        /// Gets or sets the default event ID.
        /// </summary>
        [ConfigurationProperty(eventId, IsRequired = true, DefaultValue=100)]
        [ResourceDescription(typeof(DesignResources), "LoggingExceptionHandlerDataEventIdDescription")]
        [ResourceDisplayName(typeof(DesignResources), "LoggingExceptionHandlerDataEventIdDisplayName")]
        public int EventId
        {
            get { return (int)this[eventId]; }
            set { this[eventId] = value; }
        }

        /// <summary>
        /// Gets or sets the default severity.
        /// </summary>
        [ConfigurationProperty(severity, IsRequired = true, DefaultValue = TraceEventType.Error)]
        [ResourceDescription(typeof(DesignResources), "LoggingExceptionHandlerDataSeverityDescription")]
        [ResourceDisplayName(typeof(DesignResources), "LoggingExceptionHandlerDataSeverityDisplayName")]
        public TraceEventType Severity
        {
            get { return (TraceEventType)this[severity]; }
            set { this[severity] = value; }
        }

        /// <summary>
        ///  Gets or sets the default title.
        /// </summary>
        [ConfigurationProperty(title, IsRequired = true, DefaultValue="Enterprise Library Exception Handling")]
        [ResourceDescription(typeof(DesignResources), "LoggingExceptionHandlerDataTitleDescription")]
        [ResourceDisplayName(typeof(DesignResources), "LoggingExceptionHandlerDataTitleDisplayName")]
        public string Title
        {
            get { return (string)this[title]; }
            set { this[title] = value; }
        }

        /// <summary>
        /// Gets or sets the formatter type.
        /// </summary>
        public Type FormatterType
        {
            get { return (Type)typeConverter.ConvertFrom(FormatterTypeName); }
            set { FormatterTypeName = typeConverter.ConvertToString(value); }
        }

        /// <summary>
        /// Gets or sets the formatter fully qualified assembly type name.
        /// </summary>
        /// <value>
        /// The formatter fully qualified assembly type name
        /// </value>
        [ConfigurationProperty(formatterType, IsRequired = true, DefaultValue="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.TextExceptionFormatter, Microsoft.Practices.EnterpriseLibrary.ExceptionHandling")]
        //TODO: review default value correct + test.
        [ResourceDescription(typeof(DesignResources), "LoggingExceptionHandlerDataFormatterTypeNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "LoggingExceptionHandlerDataFormatterTypeNameDisplayName")]
        [Editor(CommonDesignTime.EditorTypes.TypeSelector, CommonDesignTime.EditorTypes.UITypeEditor)]
        [BaseType(typeof(TextExceptionFormatter))]
        public string FormatterTypeName
        {
            get { return (string)this[formatterType]; }
            set { this[formatterType] = value; }
        }

        /// <summary>
        /// Gets or sets the minimum value for messages to be processed.  Messages with a priority
        /// below the minimum are dropped immediately on the client.
        /// </summary>
        [ResourceDescription(typeof(DesignResources), "LoggingExceptionHandlerDataPriorityDescription")]
        [ResourceDisplayName(typeof(DesignResources), "LoggingExceptionHandlerDataPriorityDisplayName")]
        [ConfigurationProperty(priority, IsRequired = true)]
        public int Priority
        {
            get { return (int)this[priority]; }
            set { this[priority] = value; }
        }

        /// <summary>
        /// Gets or sets the default logger to be used.
        /// </summary>
        [ConfigurationProperty(useDefaultLogger, IsRequired = false, DefaultValue = false)]
        [Obsolete("Behavior is limited to UseDefaultLogger = true")]
        [Browsable(false)]
        public bool UseDefaultLogger
        {
            get { return (bool)this[useDefaultLogger]; }
            set { this[useDefaultLogger] = value; }
        }

        /// <summary>
        /// TODOC : review
        /// </summary>
        /// <param name="namePrefix"></param>
        /// <returns></returns>
        public override IEnumerable<TypeRegistration> GetRegistrations(string namePrefix)
        {
            yield return new TypeRegistration<IExceptionHandler>(
                () =>
                new LoggingExceptionHandler(LogCategory, EventId, Severity, Title, Priority, FormatterType,
                                            Common.Configuration.ContainerModel.Container.Resolved<LogWriter>()))
                       {
                           Name = BuildName(namePrefix),
                           Lifetime = TypeRegistrationLifetime.Transient
                       };
        }
    }
}
