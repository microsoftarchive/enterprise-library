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
using System.Security;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// Facade for writing a log entry to one or more <see cref="TraceListener"/>s.  This class is sealed.
    /// </summary>
    public static class Logger
    {
        private static object sync = new object();
        private static volatile LogWriter writer;

        /// <summary>
        /// Add a key/value pair to the <see cref="System.Runtime.Remoting.Messaging.CallContext"/> dictionary.  
        /// Context items will be recorded with every log entry.
        /// </summary>
        /// <param name="key">Hashtable key</param>
        /// <param name="value">Value.  Objects will be serialized.</param>
        /// <example>The following example demonstrates use of the AddContextItem method.
        /// <code>Logger.SetContextItem("SessionID", myComponent.SessionId);</code></example>
        [SecurityCritical]
        public static void SetContextItem(object key, object value)
        {
            Writer.SetContextItem(key, value);
        }

        /// <summary>
        /// Empty the context items dictionary.
        /// </summary>
        [SecurityCritical]
        public static void FlushContextItems()
        {
            Writer.FlushContextItems();
        }

        /// <overloads>
        /// Write a new log entry to the default category.
        /// </overloads>
        /// <summary>
        /// Write a new log entry to the default category.
        /// </summary>
        /// <example>The following example demonstrates use of the Write method with
        /// one required parameter, message.
        /// <code>Logger.Write("My message body");</code></example>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        public static void Write(object message)
        {
            Writer.Write(message);
        }

        /// <summary>
        /// Write a new log entry to a specific category.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="category">Category name used to route the log entry to a one or more trace listeners.</param>
        public static void Write(object message, string category)
        {
            Writer.Write(message, category);
        }

        /// <summary>
        /// Write a new log entry with a specific category and priority.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="category">Category name used to route the log entry to a one or more trace listeners.</param>
        /// <param name="priority">Only messages must be above the minimum priority are processed.</param>
        public static void Write(object message, string category, int priority)
        {
            Writer.Write(message, category, priority);
        }

        /// <summary>
        /// Write a new log entry with a specific category, priority and event id.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="category">Category name used to route the log entry to a one or more trace listeners.</param>
        /// <param name="priority">Only messages must be above the minimum priority are processed.</param>
        /// <param name="eventId">Event number or identifier.</param>
        public static void Write(object message, string category, int priority, int eventId)
        {
            Writer.Write(message, category, priority, eventId);
        }

        /// <summary>
        /// Write a new log entry with a specific category, priority, event id and severity.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="category">Category name used to route the log entry to a one or more trace listeners.</param>
        /// <param name="priority">Only messages must be above the minimum priority are processed.</param>
        /// <param name="eventId">Event number or identifier.</param>
        /// <param name="severity">Log entry severity as a <see cref="TraceEventType"/> enumeration. (Unspecified, Information, Warning or Error).</param>
        public static void Write(object message, string category, int priority, int eventId, TraceEventType severity)
        {
            Writer.Write(message, category, priority, eventId, severity);
        }

        /// <summary>
        /// Write a new log entry with a specific category, priority, event id, severity
        /// and title.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="category">Category name used to route the log entry to a one or more trace listeners.</param>
        /// <param name="priority">Only messages must be above the minimum priority are processed.</param>
        /// <param name="eventId">Event number or identifier.</param>
        /// <param name="severity">Log message severity as a <see cref="TraceEventType"/> enumeration. (Unspecified, Information, Warning or Error).</param>
        /// <param name="title">Additional description of the log entry message</param>
        public static void Write(object message, string category, int priority, int eventId,
                                 TraceEventType severity, string title)
        {
            Writer.Write(message, category, priority, eventId, severity, title);
        }

        /// <summary>
        /// Write a new log entry and a dictionary of extended properties.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="properties">Dictionary of key/value pairs to log.</param>
        public static void Write(object message, IDictionary<string, object> properties)
        {
            Writer.Write(message, properties);
        }

        /// <summary>
        /// Write a new log entry to a specific category with a dictionary 
        /// of extended properties.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="category">Category name used to route the log entry to a one or more trace listeners.</param>
        /// <param name="properties">Dictionary of key/value pairs to log.</param>
        public static void Write(object message, string category, IDictionary<string, object> properties)
        {
            Writer.Write(message, category, properties);
        }

        /// <summary>
        /// Write a new log entry to with a specific category, priority and a dictionary 
        /// of extended properties.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="category">Category name used to route the log entry to a one or more trace listeners.</param>
        /// <param name="priority">Only messages must be above the minimum priority are processed.</param>
        /// <param name="properties">Dictionary of key/value pairs to log.</param>
        public static void Write(object message, string category, int priority, IDictionary<string, object> properties)
        {
            Writer.Write(message, category, priority, properties);
        }

        /// <summary>
        /// Write a new log entry with a specific category, priority, event Id, severity
        /// title and dictionary of extended properties.
        /// </summary>
        /// <example>The following example demonstrates use of the Write method with
        /// a full set of parameters.
        /// <code></code></example>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="category">Category name used to route the log entry to a one or more trace listeners.</param>
        /// <param name="priority">Only messages must be above the minimum priority are processed.</param>
        /// <param name="eventId">Event number or identifier.</param>
        /// <param name="severity">Log message severity as a <see cref="TraceEventType"/> enumeration. (Unspecified, Information, Warning or Error).</param>
        /// <param name="title">Additional description of the log entry message.</param>
        /// <param name="properties">Dictionary of key/value pairs to log.</param>
        public static void Write(object message, string category, int priority, int eventId,
                                 TraceEventType severity, string title, IDictionary<string, object> properties)
        {
            Writer.Write(message, category, priority, eventId, severity, title, properties);
        }

        /// <summary>
        /// Write a new log entry to a specific collection of categories.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="categories">Category names used to route the log entry to a one or more trace listeners.</param>
        public static void Write(object message, ICollection<string> categories)
        {
            Writer.Write(message, categories);
        }

        /// <summary>
        /// Write a new log entry with a specific collection of categories and priority.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="categories">Category names used to route the log entry to a one or more trace listeners.</param>
        /// <param name="priority">Only messages must be above the minimum priority are processed.</param>
        public static void Write(object message, ICollection<string> categories, int priority)
        {
            Writer.Write(message, categories, priority);
        }

        /// <summary>
        /// Write a new log entry with a specific collection of categories, priority and event id.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="categories">Category names used to route the log entry to a one or more trace listeners.</param>
        /// <param name="priority">Only messages must be above the minimum priority are processed.</param>
        /// <param name="eventId">Event number or identifier.</param>
        public static void Write(object message, ICollection<string> categories, int priority, int eventId)
        {
            Writer.Write(message, categories, priority, eventId);
        }

        /// <summary>
        /// Write a new log entry with a specific collection of categories, priority, event id and severity.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="categories">Category names used to route the log entry to a one or more trace listeners.</param>
        /// <param name="priority">Only messages must be above the minimum priority are processed.</param>
        /// <param name="eventId">Event number or identifier.</param>
        /// <param name="severity">Log entry severity as a <see cref="TraceEventType"/> enumeration. (Unspecified, Information, Warning or Error).</param>
        public static void Write(object message, ICollection<string> categories, int priority, int eventId, TraceEventType severity)
        {
            Writer.Write(message, categories, priority, eventId, severity);
        }

        /// <summary>
        /// Write a new log entry with a specific collection of categories, priority, event id, severity
        /// and title.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="categories">Category names used to route the log entry to a one or more trace listeners.</param>
        /// <param name="priority">Only messages must be above the minimum priority are processed.</param>
        /// <param name="eventId">Event number or identifier.</param>
        /// <param name="severity">Log message severity as a <see cref="TraceEventType"/> enumeration. (Unspecified, Information, Warning or Error).</param>
        /// <param name="title">Additional description of the log entry message</param>
        public static void Write(object message, ICollection<string> categories, int priority, int eventId,
                                 TraceEventType severity, string title)
        {
            Writer.Write(message, categories, priority, eventId, severity, title);
        }

        /// <summary>
        /// Write a new log entry to a specific collection of categories with a dictionary of extended properties.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="categories">Category names used to route the log entry to a one or more trace listeners.</param>
        /// <param name="properties">Dictionary of key/value pairs to log.</param>
        public static void Write(object message, ICollection<string> categories, IDictionary<string, object> properties)
        {
            Writer.Write(message, categories, properties);
        }

        /// <summary>
        /// Write a new log entry to with a specific collection of categories, priority and a dictionary 
        /// of extended properties.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="categories">Category names used to route the log entry to a one or more trace listeners.</param>
        /// <param name="priority">Only messages must be above the minimum priority are processed.</param>
        /// <param name="properties">Dictionary of key/value pairs to log.</param>
        public static void Write(object message, ICollection<string> categories, int priority, IDictionary<string, object> properties)
        {
            Writer.Write(message, categories, priority, properties);
        }

        /// <summary>
        /// Write a new log entry with a specific category, priority, event Id, severity
        /// title and dictionary of extended properties.
        /// </summary>
        /// <example>The following example demonstrates use of the Write method with
        /// a full set of parameters.
        /// <code></code></example>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="categories">Category names used to route the log entry to a one or more trace listeners.</param>
        /// <param name="priority">Only messages must be above the minimum priority are processed.</param>
        /// <param name="eventId">Event number or identifier.</param>
        /// <param name="severity">Log message severity as a <see cref="TraceEventType"/> enumeration. (Unspecified, Information, Warning or Error).</param>
        /// <param name="title">Additional description of the log entry message.</param>
        /// <param name="properties">Dictionary of key/value pairs to log.</param>
        public static void Write(object message, ICollection<string> categories, int priority, int eventId,
                                 TraceEventType severity, string title, IDictionary<string, object> properties)
        {
            Writer.Write(message, categories, priority, eventId, severity, title, properties);
        }

        /// <summary>
        /// Write a new log entry as defined in the <see cref="LogEntry"/> parameter.
        /// </summary>
        /// <example>The following examples demonstrates use of the Write method using
        /// a <see cref="LogEntry"/> type.
        /// <code>
        /// LogEntry log = new LogEntry();
        /// log.Category = "MyCategory1";
        /// log.Message = "My message body";
        /// log.Severity = TraceEventType.Error;
        /// log.Priority = 100;
        /// Logger.Write(log);</code></example>
        /// <param name="log">Log entry object to write.</param>
        public static void Write(LogEntry log)
        {
            Writer.Write(log);
        }

        /// <summary>
        /// Returns the filter of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of filter requiered.</typeparam>
        /// <returns>The instance of <typeparamref name="T"/> in the filters collection, or <see langword="null"/> 
        /// if there is no such instance.</returns>
        public static T GetFilter<T>()
            where T : class, ILogFilter
        {
            return Writer.GetFilter<T>();
        }

        /// <summary>
        /// Returns the filter of type <typeparamref name="T"/> named <paramref name="name"/>.
        /// </summary>
        /// <typeparam name="T">The type of filter required.</typeparam>
        /// <param name="name">The name of the filter required.</param>
        /// <returns>The instance of <typeparamref name="T"/> named <paramref name="name"/> in 
        /// the filters collection, or <see langword="null"/> if there is no such instance.</returns>
        public static T GetFilter<T>(string name)
            where T : class, ILogFilter
        {
            return Writer.GetFilter<T>(name);
        }

        /// <summary>
        /// Returns the filter named <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the filter required.</param>
        /// <returns>The filter named <paramref name="name"/> in 
        /// the filters collection, or <see langword="null"/> if there is no such filter.</returns>
        public static ILogFilter GetFilter(string name)
        {
            return Writer.GetFilter(name);
        }

        /// <summary>
        /// Query whether logging is enabled.
        /// </summary>
        /// <returns><code>true</code> if logging is enabled.</returns>
        public static bool IsLoggingEnabled()
        {
            return Writer.IsLoggingEnabled();
        }

        /// <summary>
        /// Query whether a <see cref="LogEntry"/> shold be logged.
        /// </summary>
        /// <param name="log">The log entry to check</param>
        /// <returns>Returns <code>true</code> if the entry should be logged.</returns>
        public static bool ShouldLog(LogEntry log)
        {
            return Writer.ShouldLog(log);
        }

        /// <summary>
        /// Reset the writer used by the <see cref="Logger"/> facade.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Resetting the writer disposes the current writer and sets the reference to <see langword="null"/> so further attempts to use the facade
        /// will fail until it is re-initialized.
        /// </para>
        /// <para>
        /// This method should be invoked only when no other operations are being performed through the facade.
        /// </para>
        /// <para>
        /// Threads that already acquired the reference to the old writer will fail when it gets disposed.
        /// </para>
        /// </remarks>
        public static void Reset()
        {
            lock (sync)
            {
                LogWriter oldWriter = writer;

                // this will be seen by threads requesting the writer (because of the double check locking pattern the query is outside the lock).
                // these threads should be stopped when trying to lock to create the writer.
                writer = null;

                // the old writer is disposed inside the lock to avoid having two instances with the same configuration.
                if (oldWriter != null)
                    oldWriter.Dispose();
            }
        }

        /// <summary>
        /// Gets the instance of <see cref="LogWriter"/> used by the facade.
        /// </summary>
        public static LogWriter Writer
        {
            get
            {
                var currentWriter = writer;
                if (currentWriter == null)
                {
                    throw new InvalidOperationException(Resources.ExceptionLogWriterNotSet);
                }

                return currentWriter;
            }
        }

        /// <summary>
        /// Sets the log writer.
        /// </summary>
        /// <param name="logWriter">The log writer.</param>
        /// <param name="throwIfSet"><see langword="true"/> to throw an exception if the writer is already set; otherwise <see langword="false"/>. Defaults to <see langword="true"/>.</param>
        /// <exception cref="InvalidOperationException">The factory is already set, and <paramref name="throwIfSet"/> is <see langword="true"/>.</exception>
        public static void SetLogWriter(LogWriter logWriter, bool throwIfSet = true)
        {
            Guard.ArgumentNotNull(logWriter, "logWriter");

            var currentWriter = writer;
            if (currentWriter != null && throwIfSet)
            {
                throw new InvalidOperationException(Resources.ExceptionLogWriterAlreadySet);
            }

            writer = logWriter;

            if (currentWriter != null)
            {
                currentWriter.Dispose();
            }
        }
    }
}
