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
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging
{
    /// <summary>
    /// Represents an <see cref="IExceptionHandler"/> that formats
    /// the exception into a log message and sends it to
    /// the Enterprise Library Logging Application Block.
    /// </summary>
    [ConfigurationElementType(typeof(LoggingExceptionHandlerData))]
    public class LoggingExceptionHandler : IExceptionHandler
    {
        private readonly string logCategory;
        private readonly int eventId;
        private readonly TraceEventType severity;
        private readonly string defaultTitle;
        private readonly Type formatterType;
        private readonly int minimumPriority;
        private readonly LogWriter logWriter;


        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingExceptionHandler"/> class with the log category, the event ID, the <see cref="TraceEventType"/>,
        /// the title, minimum priority, the formatter type, and the <see cref="LogWriter"/>.
        /// </summary>
        /// <param name="logCategory">The default category</param>
        /// <param name="eventId">An event id.</param>
        /// <param name="severity">The severity.</param>
        /// <param name="title">The log title.</param>
        /// <param name="priority">The minimum priority.</param>
        /// <param name="formatterType">The type <see cref="ExceptionFormatter"/> type.</param>
        /// <param name="writer">The <see cref="LogWriter"/> to use.</param>
        /// <remarks>
        /// The type specified for the <paramref name="formatterType"/> attribute must have a public constructor with
        /// parameters of type <see name="TextWriter"/>, <see cref="Exception"/> and <see cref="Guid"/>.
        /// </remarks>
        public LoggingExceptionHandler(
            string logCategory,
            int eventId,
            TraceEventType severity,
            string title,
            int priority,
            Type formatterType,
            LogWriter writer)
        {
            this.logCategory = logCategory;
            this.eventId = eventId;
            this.severity = severity;
            this.defaultTitle = title;
            this.minimumPriority = priority;
            this.formatterType = formatterType;
            this.logWriter = writer;
        }

        /// <summary>
        /// <para>Handles the specified <see cref="Exception"/> object by formatting it and writing to the configured log.</para>
        /// </summary>
        /// <param name="exception"><para>The exception to handle.</para></param>        
        /// <param name="handlingInstanceId">
        /// <para>The unique ID attached to the handling chain for this handling instance.</para>
        /// </param>
        /// <returns><para>Modified exception to pass to the next handler in the chain.</para></returns>
        public Exception HandleException(Exception exception, Guid handlingInstanceId)
        {
            WriteToLog(CreateMessage(exception, handlingInstanceId), exception.Data);
            return exception;
        }

        /// <summary>
        /// Writes the specified log message using 
        /// the Logging Application Block's <see cref="Logger.Write(LogEntry)"/>
        /// method.
        /// </summary>
        /// <param name="logMessage">The message to write to the log.</param>
        /// <param name="exceptionData">The exception's data.</param>
        protected virtual void WriteToLog(string logMessage, IDictionary exceptionData)
        {
            LogEntry entry = new LogEntry(
                logMessage,
                logCategory,
                minimumPriority,
                eventId,
                severity,
                defaultTitle,
                null);

            foreach (DictionaryEntry dataEntry in exceptionData)
            {
                if (dataEntry.Key is string)
                {
                    entry.ExtendedProperties.Add(dataEntry.Key as string, dataEntry.Value);
                }
            }

            this.logWriter.Write(entry);
        }

        /// <summary>
        /// Creates an instance of the <see cref="StringWriter"/>
        /// class using its default constructor.
        /// </summary>
        /// <returns>A newly created <see cref="StringWriter"/></returns>
        protected virtual StringWriter CreateStringWriter()
        {
            return new StringWriter(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Creates an <see cref="ExceptionFormatter"/>
        /// object based on the configured ExceptionFormatter
        /// type name.
        /// </summary>
        /// <param name="writer">The stream to write to.</param>
        /// <param name="exception">The <see cref="Exception"/> to pass into the formatter.</param>
        /// <param name="handlingInstanceID">The id of the handling chain.</param>
        /// <returns>A newly created <see cref="ExceptionFormatter"/></returns>
        protected virtual ExceptionFormatter CreateFormatter(
            StringWriter writer,
            Exception exception,
            Guid handlingInstanceID)
        {
            ConstructorInfo constructor = GetFormatterConstructor();
            return (ExceptionFormatter)constructor.Invoke(
                new object[] { writer, exception, handlingInstanceID }
                );
        }

        private ConstructorInfo GetFormatterConstructor()
        {
            Type[] types = new Type[] { typeof(TextWriter), typeof(Exception), typeof(Guid) };
            ConstructorInfo constructor = formatterType.GetConstructor(types);
            if (constructor == null)
            {
                throw new ExceptionHandlingException(
                    string.Format(CultureInfo.CurrentCulture, Resources.MissingConstructor, formatterType.AssemblyQualifiedName));
            }
            return constructor;
        }

        private string CreateMessage(Exception exception, Guid handlingInstanceID)
        {
            StringWriter writer = null;
            StringBuilder stringBuilder = null;
            try
            {
                writer = CreateStringWriter();
                ExceptionFormatter formatter = CreateFormatter(writer, exception, handlingInstanceID);
                formatter.Format();
                stringBuilder = writer.GetStringBuilder();

            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }

            return stringBuilder.ToString();
        }
    }
}
