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
using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration
{
    /// <summary>
    /// Represents configuration for a <see cref="LoggingExceptionHandler"/>.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "LoggingExceptionHandlerDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "LoggingExceptionHandlerDataDisplayName")]
    [AddSateliteProviderCommand(LoggingSettings.SectionName, typeof(LoggingSettings), "DefaultCategory", "LogCategory")]
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
            : base(typeof(LoggingExceptionHandler))
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
        [ConfigurationProperty(eventId, IsRequired = true, DefaultValue = 100)]
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
        [ConfigurationProperty(title, IsRequired = true, DefaultValue = "Enterprise Library Exception Handling")]
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
        [ConfigurationProperty(formatterType, IsRequired = true)]
        [ResourceDescription(typeof(DesignResources), "LoggingExceptionHandlerDataFormatterTypeNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "LoggingExceptionHandlerDataFormatterTypeNameDisplayName")]
        [Editor(CommonDesignTime.EditorTypes.TypeSelector, CommonDesignTime.EditorTypes.UITypeEditor)]
        [BaseType(typeof(ExceptionFormatter))]
        [DesigntimeDefaultAttribute("Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.TextExceptionFormatter, Microsoft.Practices.EnterpriseLibrary.ExceptionHandling")]
        [Validation(ExceptionHandlingLoggingDesigntime.ValidatorTypes.LogFormatterValidator)]
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
        /// Builds the exception handler represented by this configuration object.
        /// </summary>
        /// <returns>An <see cref="IExceptionHandler"/>.</returns>
        public override IExceptionHandler BuildExceptionHandler()
        {
            Type exceptionFormatterType;
            try
            {
                exceptionFormatterType = this.FormatterType;
            }
            catch(ArgumentException e)
            {
                throw new ConfigurationErrorsException(Resources.ExceptionFormatterTypeNotSetOrInvalid, e, this.ElementInformation.Source, this.ElementInformation.LineNumber);
            }

            if (exceptionFormatterType == null)
            {
                throw new ConfigurationErrorsException(Resources.ExceptionFormatterTypeNotSetOrInvalid, this.ElementInformation.Source, this.ElementInformation.LineNumber);
            }

            if(!exceptionFormatterType.IsSubclassOf(typeof(ExceptionFormatter)))
            {
                throw new ConfigurationErrorsException(Resources.ExceptionFormatterTypeNotFormatter, this.ElementInformation.Source, this.ElementInformation.LineNumber);
            }

            return new LoggingExceptionHandler(this.LogCategory, this.EventId, this.Severity, this.Title, this.Priority, exceptionFormatterType, Logger.Writer);
        }
    }
}
