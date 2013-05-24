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
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// Represents a log message.  Contains the common properties that are required for all log messages.
    /// </summary>
    [XmlRoot("logEntry")]
    [Serializable]
    public class LogEntry : ICloneable
    {
        private static readonly bool isFullyTrusted;
        private static readonly TextFormatter toStringFormatter;

        private string message = string.Empty;
        private string title = string.Empty;

        [NonSerialized]
        private ICollection<string> categories = new List<string>(0);
        private string[] categoryStrings;

        private int priority = -1;
        private int eventId = 0;
        private Guid activityId;
        private Guid? relatedActivityId;

        private TraceEventType severity = TraceEventType.Information;

        private string machineName = string.Empty;
        private DateTime timeStamp = DateTime.MaxValue;

        private StringBuilder errorMessages;
        private IDictionary<string, object> extendedProperties;

        private string appDomainName;
        private string processId;
        private string processName;
        private string threadName;
        private string win32ThreadId;

        internal bool timeStampInitialized = false;
        internal bool appDomainNameInitialized = false;
        internal bool machineNameInitialized = false;
        internal bool processIdInitialized = false;
        internal bool processNameInitialized = false;
        internal bool win32ThreadIdInitialized = false;
        internal bool threadNameInitialized = false;
        internal bool activityIdInitialized = false;
        private bool unmanagedCodePermissionAvailable = false;
        private bool unmanagedCodePermissionAvailableInitialized = false;

        static LogEntry()
        {
            isFullyTrusted = typeof(LogEntry).Assembly.IsFullyTrusted;
            toStringFormatter = new TextFormatter();
        }

        /// <summary>
        /// Initialize a new instance of a <see cref="LogEntry"/> class.
        /// </summary>
        public LogEntry()
        {
        }

        /// <summary>
        /// Create a new instance of <see cref="LogEntry"/> with a full set of constructor parameters
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="category">Category name used to route the log entry to a one or more trace listeners.</param>
        /// <param name="priority">Only messages must be above the minimum priority are processed.</param>
        /// <param name="eventId">Event number or identifier.</param>
        /// <param name="severity">Log entry severity as a <see cref="Severity"/> enumeration. (Unspecified, Information, Warning or Error).</param>
        /// <param name="title">Additional description of the log entry message.</param>
        /// <param name="properties">Dictionary of key/value pairs to record.</param>
        public LogEntry(object message, string category, int priority, int eventId,
                        TraceEventType severity, string title, IDictionary<string, object> properties)
            : this(message, BuildCategoriesCollection(category), priority, eventId, severity, title, properties)
        {
        }

        /// <summary>
        /// Create a new instance of <see cref="LogEntry"/> with a full set of constructor parameters
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="categories">Collection of category names used to route the log entry to a one or more sinks.</param>
        /// <param name="priority">Only messages must be above the minimum priority are processed.</param>
        /// <param name="eventId">Event number or identifier.</param>
        /// <param name="severity">Log entry severity as a <see cref="Severity"/> enumeration. (Unspecified, Information, Warning or Error).</param>
        /// <param name="title">Additional description of the log entry message.</param>
        /// <param name="properties">Dictionary of key/value pairs to record.</param>
        public LogEntry(object message, ICollection<string> categories, int priority, int eventId,
                        TraceEventType severity, string title, IDictionary<string, object> properties)
        {
            if (message == null)
                throw new ArgumentNullException("message");
            if (categories == null)
                throw new ArgumentNullException("categories");

            this.Message = message.ToString();
            this.Priority = priority;
            this.Categories = categories;
            this.EventId = eventId;
            this.Severity = severity;
            this.Title = title;
            this.ExtendedProperties = properties;
        }

        /// <summary>
        /// Message body to log.  Value from ToString() method from message object.
        /// </summary>
        public string Message
        {
            get { return this.message; }
            set { this.message = value; }
        }

        /// <summary>
        /// Category name used to route the log entry to a one or more trace listeners.
        /// </summary>
        public ICollection<string> Categories
        {
            get { return categories; }
            set { this.categories = value; }
        }

        /// <summary>
        /// Importance of the log message.  Only messages whose priority is between the minimum and maximum priorities (inclusive)
        /// will be processed.
        /// </summary>
        public int Priority
        {
            get { return this.priority; }
            set { this.priority = value; }
        }

        /// <summary>
        /// Event number or identifier.
        /// </summary>
        public int EventId
        {
            get { return this.eventId; }
            set { this.eventId = value; }
        }

        /// <summary>
        /// Log entry severity as a <see cref="Severity"/> enumeration. (Unspecified, Information, Warning or Error).
        /// </summary>
        public TraceEventType Severity
        {
            get { return this.severity; }
            set { this.severity = value; }
        }

        /// <summary>
        /// <para>Gets the string representation of the <see cref="Severity"/> enumeration.</para>
        /// </summary>
        /// <value>
        /// <para>The string value of the <see cref="Severity"/> enumeration.</para>
        /// </value>
        public string LoggedSeverity
        {
            get { return severity.ToString(); }
        }

        /// <summary>
        /// Additional description of the log entry message.
        /// </summary>
        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        /// <summary>
        /// Date and time of the log entry message.
        /// </summary>
        public DateTime TimeStamp
        {
            get
            {
                if (!timeStampInitialized)
                {
                    InitializeTimeStamp();
                }

                return this.timeStamp;
            }
            set
            {
                this.timeStamp = value;
                timeStampInitialized = true;
            }
        }

        /// <summary>
        /// Name of the computer.
        /// </summary>
        public string MachineName
        {
            get
            {
                if (!machineNameInitialized)
                {
                    InitializeMachineName();
                }

                return this.machineName;
            }
            set
            {
                this.machineName = value;
                machineNameInitialized = true;
            }
        }


        /// <summary>
        /// The <see cref="AppDomain"/> in which the program is running
        /// </summary>
        public string AppDomainName
        {
            get
            {
                if (!appDomainNameInitialized)
                {
                    InitializeAppDomainName();
                }

                return this.appDomainName;
            }
            set
            {
                this.appDomainName = value;
                appDomainNameInitialized = true;
            }
        }

        /// <summary>
        /// The Win32 process ID for the current running process.
        /// </summary>
        public string ProcessId
        {
            get
            {
                if (!processIdInitialized)
                {
                    InitializeProcessId();
                }

                return this.processId;
            }
            set
            {
                processId = value;
                processIdInitialized = true;
            }
        }

        /// <summary>
        /// The name of the current running process.
        /// </summary>
        public string ProcessName
        {
            get
            {
                if (!processNameInitialized)
                {
                    InitializeProcessName();
                }

                return this.processName;
            }
            set
            {
                this.processName = value;
                processNameInitialized = true;
            }
        }

        /// <summary>
        /// The name of the .NET thread.
        /// </summary>
        ///  <seealso cref="Win32ThreadId"/>
        public string ManagedThreadName
        {
            get
            {
                if (!threadNameInitialized)
                {
                    InitializeThreadName();
                }

                return this.threadName;
            }
            set
            {
                this.threadName = value;
                threadNameInitialized = true;
            }
        }

        /// <summary>
        /// The Win32 Thread ID for the current thread.
        /// </summary>
        public string Win32ThreadId
        {
            get
            {
                if (!win32ThreadIdInitialized)
                {
                    InitializeWin32ThreadId();
                }

                return this.win32ThreadId;
            }
            set
            {
                this.win32ThreadId = value;
                win32ThreadIdInitialized = true;
            }
        }

        /// <summary>
        /// Dictionary of key/value pairs to record.
        /// </summary>
        public IDictionary<string, object> ExtendedProperties
        {
            get
            {
                if (extendedProperties == null)
                {
                    extendedProperties = new Dictionary<string, object>();
                }
                return this.extendedProperties;
            }
            set { this.extendedProperties = value; }
        }

        /// <summary>
        /// Read-only property that returns the timeStamp formatted using the current culture.
        /// </summary>
        public string TimeStampString
        {
            get { return TimeStamp.ToString(CultureInfo.CurrentCulture); }
        }

        #region Intrinsic Property Initialization

        private void InitializeTimeStamp()
        {
            this.TimeStamp = DateTime.UtcNow;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Queries environment")]
        private void InitializeActivityId()
        {
            try
            {
                this.ActivityId = LogEntryContext.GetActivityId();
            }
            catch (Exception)
            {
                this.ActivityId = Guid.Empty;
            }
        }

        private void InitializeMachineName()
        {
            this.MachineName = LogEntryContext.GetMachineNameSafe();
        }

        private void InitializeAppDomainName()
        {
            this.AppDomainName = LogEntryContext.GetAppDomainNameSafe();
        }

        private void InitializeProcessId()
        {
            if (!LogEntry.isFullyTrusted)
            {
                this.ProcessId = string.Format(CultureInfo.CurrentCulture,
                    Properties.Resources.IntrinsicPropertyError,
                    Properties.Resources.LogEntryIntrinsicPropertyNotFullyTrusted);
            }
            else if (!this.UnmanagedCodePermissionAvailable)
            {
                this.ProcessId = string.Format(CultureInfo.CurrentCulture,
                    Properties.Resources.IntrinsicPropertyError,
                    Properties.Resources.LogEntryIntrinsicPropertyNoUnmanagedCodePermissionError);
            }
            else
            {
                this.ProcessId = LogEntryContext.GetProcessIdSafe();
            }
        }

        private void InitializeProcessName()
        {
            if (!LogEntry.isFullyTrusted)
            {
                this.ProcessName = string.Format(CultureInfo.CurrentCulture,
                    Properties.Resources.IntrinsicPropertyError,
                    Properties.Resources.LogEntryIntrinsicPropertyNotFullyTrusted);
            }
            else if (!this.UnmanagedCodePermissionAvailable)
            {
                this.ProcessName = string.Format(CultureInfo.CurrentCulture,
                    Properties.Resources.IntrinsicPropertyError,
                    Properties.Resources.LogEntryIntrinsicPropertyNoUnmanagedCodePermissionError);
            }
            else
            {
                this.ProcessName = LogEntryContext.GetProcessNameSafe();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Queries environment")]
        private void InitializeThreadName()
        {
            try
            {
                this.ManagedThreadName = Thread.CurrentThread.Name;
            }
            catch (Exception e)
            {
                this.ManagedThreadName = string.Format(CultureInfo.CurrentCulture, Properties.Resources.IntrinsicPropertyError, e.Message);
            }
        }

        private void InitializeWin32ThreadId()
        {
            if (!LogEntry.isFullyTrusted)
            {
                this.Win32ThreadId = string.Format(CultureInfo.CurrentCulture,
                    Properties.Resources.IntrinsicPropertyError,
                    Properties.Resources.LogEntryIntrinsicPropertyNotFullyTrusted);
            }
            else if (!this.UnmanagedCodePermissionAvailable)
            {
                this.Win32ThreadId = string.Format(CultureInfo.CurrentCulture,
                    Properties.Resources.IntrinsicPropertyError,
                    Properties.Resources.LogEntryIntrinsicPropertyNoUnmanagedCodePermissionError);
            }
            else
            {
                this.Win32ThreadId = LogEntryContext.GetCurrentThreadIdSafe();
            }
        }

        #endregion


        /// <summary>
        /// Tracing activity id
        /// </summary>
        public Guid ActivityId
        {
            get
            {
                if (!activityIdInitialized)
                {
                    InitializeActivityId();
                }

                return this.activityId;
            }
            set
            {
                this.activityId = value;
                activityIdInitialized = true;
            }
        }

        /// <summary>
        /// Related activity id
        /// </summary>
        public Guid? RelatedActivityId
        {
            get { return this.relatedActivityId; }
            set { this.relatedActivityId = value; }
        }

        /// <summary>
        /// Creates a new <see cref="LogEntry"/> that is a copy of the current instance.
        /// </summary>
        /// <remarks>
        /// If the dictionary contained in <see cref="ExtendedProperties"/> implements <see cref="ICloneable"/>, the resulting
        /// <see cref="LogEntry"/> will have its ExtendedProperties set by calling <c>Clone()</c>. Otherwise the resulting
        /// <see cref="LogEntry"/> will have its ExtendedProperties set to <see langword="null"/>.
        /// </remarks>
        /// <implements>ICloneable.Clone</implements>
        /// <returns>A new <c>LogEntry</c> that is a copy of the current instance.</returns>
        public object Clone()
        {
            LogEntry result = new LogEntry();

            result.Message = this.Message;
            result.EventId = this.EventId;
            result.Title = this.Title;
            result.Severity = this.Severity;
            result.Priority = this.Priority;

            result.TimeStamp = this.TimeStamp;
            result.MachineName = this.MachineName;
            result.AppDomainName = this.AppDomainName;
            result.ProcessId = this.ProcessId;
            result.ProcessName = this.ProcessName;
            result.ManagedThreadName = this.ManagedThreadName;
            result.ActivityId = this.ActivityId;

            // clone categories
            result.Categories = new List<string>(this.Categories);

            // clone extended properties
            if (this.extendedProperties != null)
                result.ExtendedProperties = new Dictionary<string, object>(this.extendedProperties);

            // clone error messages
            if (this.errorMessages != null)
            {
                result.errorMessages = new StringBuilder(this.errorMessages.ToString());
            }

            return result;
        }

        /// <summary>
        /// Add an error or warning message to the start of the messages string builder.
        /// </summary>
        /// <param name="message">Message to be added to this instance</param>
        public virtual void AddErrorMessage(string message)
        {
            if (errorMessages == null)
            {
                errorMessages = new StringBuilder();
            }
            errorMessages.Insert(0, Environment.NewLine);
            errorMessages.Insert(0, Environment.NewLine);
            errorMessages.Insert(0, message);
        }

        /// <summary>
        /// Gets the error message with the <see cref="LogEntry"></see>
        /// </summary>
        public string ErrorMessages
        {
            get
            {
                if (errorMessages == null)
                    return null;
                else
                    return errorMessages.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents the current <see cref="LogEntry"/>, 
        /// using a default formatting template.
        /// </summary>
        /// <returns>A <see cref="String"/> that represents the current <see cref="LogEntry"/>.</returns>
        public override string ToString()
        {
            return toStringFormatter.Format(this);
        }

        private bool UnmanagedCodePermissionAvailable
        {
            [SecuritySafeCritical]
            get
            {
                if (!unmanagedCodePermissionAvailableInitialized)
                {
                    // check whether the unmanaged code permission is available to avoid three potential stack walks
                    // this optimization might prevent code with the necessary permissions to get the values after this property has been initialized
                    // but that's a very unlikely situation which will not occur 
                    bool internalUnmanagedCodePermissionAvailable = false;
                    // avoid a stack walk by checking for the permission on the current assembly
                    if (LogEntry.isFullyTrusted)
                    {
                        try
                        {
                            SecurityPermission unmanagedCodePermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
                            unmanagedCodePermission.Demand();
                            internalUnmanagedCodePermissionAvailable = true;
                        }
                        catch (SecurityException)
                        { }
                    }

                    this.UnmanagedCodePermissionAvailable = internalUnmanagedCodePermissionAvailable;
                }

                return this.unmanagedCodePermissionAvailable;
            }
            set
            {
                this.unmanagedCodePermissionAvailable = value;
                unmanagedCodePermissionAvailableInitialized = true;
            }
        }

        /// <summary>
        /// Set the intrinsic properties such as MachineName and UserIdentity.
        /// </summary>
        internal void CollectIntrinsicProperties()
        {
            CollectThreadSpecificIntrinsicProperties();
            InitializeMachineName();
            InitializeAppDomainName();
            InitializeProcessId();
            InitializeProcessName();
        }

        internal void CollectThreadSpecificIntrinsicProperties()
        {
            InitializeTimeStamp();
            InitializeActivityId();
            InitializeThreadName();
            InitializeWin32ThreadId();
        }

        /// <summary>
        /// Gets the current process name.
        /// </summary>
        /// <returns>The process name.</returns>
        public static string GetProcessName()
        {
            return LogEntryContext.GetProcessNameSafe();
        }

        private static ICollection<string> BuildCategoriesCollection(string category)
        {
            if (string.IsNullOrEmpty(category))
                throw new ArgumentNullException("category");

            return new string[] { category };
        }

        /// <summary>
        /// Tracing activity id as a string to support WMI Queries
        /// </summary>
        public string ActivityIdString
        {
            get { return this.ActivityId.ToString(); }
        }

        /// <summary>
        /// Category names used to route the log entry to a one or more trace listeners.
        /// This readonly property is available to support WMI queries
        /// </summary>
        public string[] CategoriesStrings
        {
            get
            {
                string[] categoriesStrings = new string[Categories.Count];
                this.Categories.CopyTo(categoriesStrings, 0);
                return categoriesStrings;
            }
        }

        // Serialization customization methods

        // The Categories collection may be non-serializable. We copy it to an array so that
        // the categories are guaranteed to be serialized.
        [OnSerializing]
        private void OnSerializing(StreamingContext context)
        {
            if (categories != null)
            {
                categoryStrings = categories.ToArray();
            }
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            // We've just be deserialized, stick in our categories collection.
            if (categoryStrings != null)
                categories = categoryStrings;
        }

        /// <summary>
        /// Gets the default title for an entry.
        /// </summary>
        public static string DefaultTitle
        {
            get { return Resources.DefaultLogEntryTitle; }
        }
    }

    internal static class LogEntryContext
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Queries environment")]
        internal static string GetAppDomainNameSafe()
        {
            try
            {
                return AppDomain.CurrentDomain.FriendlyName;
            }
            catch (Exception e)
            {
                return string.Format(CultureInfo.CurrentCulture, Properties.Resources.IntrinsicPropertyError, e.Message);
            }
        }

        internal static string GetMachineNameSafe()
        {
            try
            {
                return Environment.MachineName;
            }
            catch (InvalidOperationException e)
            {
                return string.Format(CultureInfo.CurrentCulture, Properties.Resources.IntrinsicPropertyError, e.Message);
            }
            catch (SecurityException e)
            {
                return string.Format(CultureInfo.CurrentCulture, Properties.Resources.IntrinsicPropertyError, e.Message);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Queries environment")]
        [SecuritySafeCritical]
        internal static string GetProcessIdSafe()
        {
            try
            {
                return GetCurrentProcessId();
            }
            catch (MethodAccessException)
            {
                return string.Format(CultureInfo.CurrentCulture, Properties.Resources.IntrinsicPropertyError, Properties.Resources.LogEntryIntrinsicPropertyNotFullyTrusted);
            }
            catch (Exception e)
            {
                return string.Format(CultureInfo.CurrentCulture, Properties.Resources.IntrinsicPropertyError, e.Message);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Queries environment")]
        [SecuritySafeCritical]
        internal static string GetProcessNameSafe()
        {
            try
            {
                return GetProcessName();
            }
            catch (MethodAccessException)
            {
                return string.Format(CultureInfo.CurrentCulture, Properties.Resources.IntrinsicPropertyError, Properties.Resources.LogEntryIntrinsicPropertyNotFullyTrusted);
            }
            catch (Exception e)
            {
                return string.Format(CultureInfo.CurrentCulture, Properties.Resources.IntrinsicPropertyError, e.Message);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Queries environment")]
        [SecuritySafeCritical]
        internal static string GetCurrentThreadIdSafe()
        {
            try
            {
                return GetCurrentThreadId();
            }
            catch (MethodAccessException)
            {
                return string.Format(CultureInfo.CurrentCulture, Properties.Resources.IntrinsicPropertyError, Properties.Resources.LogEntryIntrinsicPropertyNotFullyTrusted);
            }
            catch (Exception e)
            {
                return string.Format(CultureInfo.CurrentCulture, Properties.Resources.IntrinsicPropertyError, e.Message);
            }
        }

        [SecuritySafeCritical]
        internal static Guid GetActivityId()
        {
            return Trace.CorrelationManager.ActivityId;
        }

        [SecurityCritical]
        private static string GetProcessName()
        {
            StringBuilder buffer = new StringBuilder(1024);
            int length = NativeMethods.GetModuleFileName(NativeMethods.GetModuleHandle(null), buffer, buffer.Capacity);
            return buffer.ToString();
        }

        [SecurityCritical]
        private static string GetCurrentProcessId()
        {
            return NativeMethods.GetCurrentProcessId().ToString(NumberFormatInfo.InvariantInfo);
        }

        [SecurityCritical]
        private static string GetCurrentThreadId()
        {
            return NativeMethods.GetCurrentThreadId().ToString(NumberFormatInfo.InvariantInfo);
        }
    }
}
