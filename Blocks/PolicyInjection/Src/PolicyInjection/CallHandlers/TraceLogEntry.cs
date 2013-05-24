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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.PolicyInjection
{
    /// <summary>
    /// A <see cref="LogEntry"/> class that contains the extra information logged
    /// by the <see cref="LogCallHandler"/>.
    /// </summary>
    [Serializable]
    public class TraceLogEntry : LogEntry
    {
        private string typeName;
        private string methodName;
        private string returnValue;
        private string callStack;
        private string exception;
        private TimeSpan? callTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceLogEntry"/> class.
        /// </summary>
        public TraceLogEntry()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceLogEntry"/> class with the specified values.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="category">The log category.</param>
        /// <param name="priority">The log priority.</param>
        /// <param name="eventId">The log event id.</param>
        /// <param name="severity">The log severity.</param>
        /// <param name="title">The log title.</param>
        /// <param name="properties">Extra properties. This contains the parameters to the call.</param>
        /// <param name="typeName">The name of type that implements the method being called.</param>
        /// <param name="methodName">The name of the method that is called.</param>
        public TraceLogEntry(object message, string category, int priority, int eventId, TraceEventType severity, string title, IDictionary<string, object> properties, string typeName, string methodName)
            : base(message, category, priority, eventId, severity, title, properties)
        {
            this.typeName = typeName;
            this.methodName = methodName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceLogEntry"/> class with the specified values.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="categories">The categories of the log entry.</param>
        /// <param name="priority">The log priority.</param>
        /// <param name="eventId">The log event id.</param>
        /// <param name="severity">The log severity.</param>
        /// <param name="title">The log title.</param>
        /// <param name="properties">Extra properties. This contains the parameters to the call.</param>
        /// <param name="typeName">The name of type that implements the method being called.</param>
        /// <param name="methodName">The name of the method that is called.</param>
        public TraceLogEntry(object message, ICollection<string> categories, int priority, int eventId, TraceEventType severity, string title, IDictionary<string, object> properties, string typeName, string methodName)
            : base(message, categories, priority, eventId, severity, title, properties)
        {
            this.typeName = typeName;
            this.methodName = methodName;
        }

        /// <summary>
        /// Type name
        /// </summary>
        /// <value>type name</value>
        public string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }

        /// <summary>
        /// Method name
        /// </summary>
        /// <value>method name</value>
        public string MethodName
        {
            get { return methodName; }
            set { methodName = value; }
        }

        /// <summary>
        /// Return value from the call.
        /// </summary>
        /// <value>return value</value>
        public string ReturnValue
        {
            get { return returnValue; }
            set { returnValue = value; }
        }

        /// <summary>
        /// The call stack from the current call.
        /// </summary>
        /// <value>call stack string.</value>
        public string CallStack
        {
            get { return callStack; }
            set { callStack = value; }
        }

        /// <summary>
        /// Exception thrown from the target.
        /// </summary>
        /// <value>If exception was thrown, this is the exception object. Null if no exception thrown.</value>
        public string Exception
        {
            get { return exception; }
            set { exception = value; }
        }

        /// <summary>
        /// Total time to call the target.
        /// </summary>
        /// <value>null if not logged, else the elapsed time.</value>
        public TimeSpan? CallTime
        {
            get { return callTime; }
            set { callTime = value; }
        }

        /// <summary>
        /// This is to support WMI instrumentation by returning
        /// the actual <see cref="CallTime"/> 
        /// </summary>
        public TimeSpan ElapsedTime
        {
            get { return callTime.HasValue ? callTime.Value : TimeSpan.Zero; }
        }
    }
}
