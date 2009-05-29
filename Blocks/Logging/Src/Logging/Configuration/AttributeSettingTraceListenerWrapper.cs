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
using System.Collections.Specialized;
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Wraps a <see cref="TraceListener"/> to allow the attribute properties to
    /// be injected.  This is primarily used with custom trace listeners that
    /// provide attributes in their configuration.
    /// </summary>
    public class AttributeSettingTraceListenerWrapper : TraceListener
    {
        ///<summary>
        /// Initializes an instance of <see cref="AttributeSettingTraceListenerWrapper"/>.
        ///</summary>
        ///<param name="listener">The <see cref="TraceListener"/> to wrap.</param>
        ///<param name="attributes">The attributes to set on the trace listener.</param>
        public AttributeSettingTraceListenerWrapper(
            TraceListener listener,
            NameValueCollection attributes)
        {
            this.InnerTraceListener = listener;
            foreach(string key in attributes)
            {
                InnerTraceListener.Attributes.Add(key, attributes[key]);
            }

            this.TraceOutputOptions = InnerTraceListener.TraceOutputOptions;
            this.IndentLevel = InnerTraceListener.IndentLevel;
            this.IndentSize = InnerTraceListener.IndentSize;
            this.Filter = InnerTraceListener.Filter;
        }


        ///<summary>
        /// The wrapped <see cref="TraceListener"/>.
        ///</summary>
        public TraceListener InnerTraceListener { get; private set; }

        #region InnerTraceListener overrides

        /// <summary>
        /// When overridden in a derived class, closes the output stream so it no longer receives tracing or debugging output.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public override void Close()
        {
            this.InnerTraceListener.Close();
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.Diagnostics.TraceListener"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. 
        ///                 </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.InnerTraceListener.Dispose();
            }
        }

        /// <summary>
        /// Emits an error message to the listener you create when you implement the <see cref="T:System.Diagnostics.TraceListener"/> class.
        /// </summary>
        /// <param name="message">A message to emit. 
        ///                 </param><filterpriority>2</filterpriority>
        public override void Fail(string message)
        {
            this.InnerTraceListener.Fail(message);
        }

        /// <summary>
        /// Emits an error message and a detailed error message to the listener you create when you implement the <see cref="T:System.Diagnostics.TraceListener"/> class.
        /// </summary>
        /// <param name="message">A message to emit. 
        ///                 </param><param name="detailMessage">A detailed message to emit. 
        ///                 </param><filterpriority>2</filterpriority>
        public override void Fail(string message, string detailMessage)
        {
            this.InnerTraceListener.Fail(message, detailMessage);
        }

        /// <summary>
        /// When overridden in a derived class, flushes the output buffer.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public override void Flush()
        {
            this.InnerTraceListener.Flush();
        }

        /// <summary>
        /// Writes trace information, a data object and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">A <see cref="T:System.Diagnostics.TraceEventCache"/> object that contains the current process ID, thread ID, and stack trace information.
        ///                 </param><param name="source">A name used to identify the output, typically the name of the application that generated the trace event.
        ///                 </param><param name="eventType">One of the <see cref="T:System.Diagnostics.TraceEventType"/> values specifying the type of event that has caused the trace.
        ///                 </param><param name="id">A numeric identifier for the event.
        ///                 </param><param name="data">The trace data to emit.
        ///                 </param><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode"/></PermissionSet>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            this.InnerTraceListener.TraceData(eventCache, source, eventType, id, data);
        }

        /// <summary>
        /// Writes trace information, an array of data objects and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">A <see cref="T:System.Diagnostics.TraceEventCache"/> object that contains the current process ID, thread ID, and stack trace information.
        ///                 </param><param name="source">A name used to identify the output, typically the name of the application that generated the trace event.
        ///                 </param><param name="eventType">One of the <see cref="T:System.Diagnostics.TraceEventType"/> values specifying the type of event that has caused the trace.
        ///                 </param><param name="id">A numeric identifier for the event.
        ///                 </param><param name="data">An array of objects to emit as data.
        ///                 </param><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode"/></PermissionSet>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            this.InnerTraceListener.TraceData(eventCache, source, eventType, id, data);
        }

        /// <summary>
        /// Writes trace and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">A <see cref="T:System.Diagnostics.TraceEventCache"/> object that contains the current process ID, thread ID, and stack trace information.
        ///                 </param><param name="source">A name used to identify the output, typically the name of the application that generated the trace event.
        ///                 </param><param name="eventType">One of the <see cref="T:System.Diagnostics.TraceEventType"/> values specifying the type of event that has caused the trace.
        ///                 </param><param name="id">A numeric identifier for the event.
        ///                 </param><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode"/></PermissionSet>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            this.InnerTraceListener.TraceEvent(eventCache, source, eventType, id);
        }

        /// <summary>
        /// Writes trace information, a formatted array of objects and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">A <see cref="T:System.Diagnostics.TraceEventCache"/> object that contains the current process ID, thread ID, and stack trace information.
        ///                 </param><param name="source">A name used to identify the output, typically the name of the application that generated the trace event.
        ///                 </param><param name="eventType">One of the <see cref="T:System.Diagnostics.TraceEventType"/> values specifying the type of event that has caused the trace.
        ///                 </param><param name="id">A numeric identifier for the event.
        ///                 </param><param name="format">A format string that contains zero or more format items, which correspond to objects in the <paramref name="args"/> array.
        ///                 </param><param name="args">An object array containing zero or more objects to format.
        ///                 </param><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode"/></PermissionSet>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            this.InnerTraceListener.TraceEvent(eventCache, source, eventType, id, format, args);
        }

        /// <summary>
        /// Writes trace information, a message, and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">A <see cref="T:System.Diagnostics.TraceEventCache"/> object that contains the current process ID, thread ID, and stack trace information.
        ///                 </param><param name="source">A name used to identify the output, typically the name of the application that generated the trace event.
        ///                 </param><param name="eventType">One of the <see cref="T:System.Diagnostics.TraceEventType"/> values specifying the type of event that has caused the trace.
        ///                 </param><param name="id">A numeric identifier for the event.
        ///                 </param><param name="message">A message to write.
        ///                 </param><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode"/></PermissionSet>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            this.InnerTraceListener.TraceEvent(eventCache, source, eventType, id, message);
        }

        /// <summary>
        /// Writes trace information, a message, a related activity identity and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">A <see cref="T:System.Diagnostics.TraceEventCache"/> object that contains the current process ID, thread ID, and stack trace information.
        ///                 </param><param name="source">A name used to identify the output, typically the name of the application that generated the trace event.
        ///                 </param><param name="id">A numeric identifier for the event.
        ///                 </param><param name="message">A message to write.
        ///                 </param><param name="relatedActivityId">A <see cref="T:System.Guid"/>  object identifying a related activity.
        ///                 </param><filterpriority>1</filterpriority>
        public override void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId)
        {
            this.InnerTraceListener.TraceTransfer(eventCache, source, id, message, relatedActivityId);
        }

        /// <summary>
        /// When overridden in a derived class, writes the specified message to the listener you create in the derived class.
        /// </summary>
        /// <param name="message">A message to write. 
        ///                 </param><filterpriority>2</filterpriority>
        public override void Write(string message)
        {
            this.InnerTraceListener.Write(message);
        }

        /// <summary>
        /// When overridden in a derived class, writes a message to the listener you create in the derived class, followed by a line terminator.
        /// </summary>
        /// <param name="message">A message to write. 
        ///                 </param><filterpriority>2</filterpriority>
        public override void WriteLine(string message)
        {
            this.InnerTraceListener.WriteLine(message);
        }

        /// <summary>
        /// Writes the value of the object's <see cref="M:System.Object.ToString"/> method to the listener you create when you implement the <see cref="T:System.Diagnostics.TraceListener"/> class.
        /// </summary>
        /// <param name="o">An <see cref="T:System.Object"/> whose fully qualified class name you want to write. 
        ///                 </param><filterpriority>2</filterpriority>
        public override void Write(object o)
        {
            this.InnerTraceListener.Write(o);
        }

        /// <summary>
        /// Writes a category name and the value of the object's <see cref="M:System.Object.ToString"/> method to the listener you create when you implement the <see cref="T:System.Diagnostics.TraceListener"/> class.
        /// </summary>
        /// <param name="o">An <see cref="T:System.Object"/> whose fully qualified class name you want to write. 
        ///                 </param><param name="category">A category name used to organize the output. 
        ///                 </param><filterpriority>2</filterpriority>
        public override void Write(object o, string category)
        {
            this.InnerTraceListener.Write(o, category);
        }

        /// <summary>
        /// Writes a category name and a message to the listener you create when you implement the <see cref="T:System.Diagnostics.TraceListener"/> class.
        /// </summary>
        /// <param name="message">A message to write. 
        ///                 </param><param name="category">A category name used to organize the output. 
        ///                 </param><filterpriority>2</filterpriority>
        public override void Write(string message, string category)
        {
            this.InnerTraceListener.Write(message, category);
        }

        /// <summary>
        /// Writes the value of the object's <see cref="M:System.Object.ToString"/> method to the listener you create when you implement the <see cref="T:System.Diagnostics.TraceListener"/> class, followed by a line terminator.
        /// </summary>
        /// <param name="o">An <see cref="T:System.Object"/> whose fully qualified class name you want to write. 
        ///                 </param><filterpriority>2</filterpriority>
        public override void WriteLine(object o)
        {
            this.InnerTraceListener.WriteLine(o);
        }

        /// <summary>
        /// Writes a category name and the value of the object's <see cref="M:System.Object.ToString"/> method to the listener you create when you implement the <see cref="T:System.Diagnostics.TraceListener"/> class, followed by a line terminator.
        /// </summary>
        /// <param name="o">An <see cref="T:System.Object"/> whose fully qualified class name you want to write. 
        ///                 </param><param name="category">A category name used to organize the output. 
        ///                 </param><filterpriority>2</filterpriority>
        public override void WriteLine(object o, string category)
        {
            this.InnerTraceListener.WriteLine(o, category);
        }

        /// <summary>
        /// Writes a category name and a message to the listener you create when you implement the <see cref="T:System.Diagnostics.TraceListener"/> class, followed by a line terminator.
        /// </summary>
        /// <param name="message">A message to write. 
        ///                 </param><param name="category">A category name used to organize the output. 
        ///                 </param><filterpriority>2</filterpriority>
        public override void WriteLine(string message, string category)
        {
            this.InnerTraceListener.WriteLine(message, category);
        }

        /// <summary>
        /// Gets a value indicating whether the trace listener is thread safe. 
        /// </summary>
        /// <returns>
        /// true if the trace listener is thread safe; otherwise, false. The default is false.
        /// </returns>
        public override bool IsThreadSafe
        {
            get
            {
                return this.InnerTraceListener.IsThreadSafe;
            }
        }

        /// <summary>
        /// Gets or sets a name for this <see cref="T:System.Diagnostics.TraceListener"/>.
        /// </summary>
        /// <returns>
        /// A name for this <see cref="T:System.Diagnostics.TraceListener"/>. The default is an empty string ("").
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string Name
        {
            get
            {
                return this.InnerTraceListener.Name;
            }
            set
            {
                this.InnerTraceListener.Name = value;
            }
        }

        #endregion
    }
}
