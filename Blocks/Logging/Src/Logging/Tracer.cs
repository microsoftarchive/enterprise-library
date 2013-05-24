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
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// Represents a performance tracing class to log method entry/exit and duration.
    /// </summary>
    /// <remarks>
    /// <para>Lifetime of the Tracer object will determine the beginning and the end of
    /// the trace.  The trace message will include, method being traced, start time, end time 
    /// and duration.</para>
    /// <para>Since Tracer uses the Logging Application Block to log the trace message, you can include application
    /// data as part of your trace message. Configured items from call context will be logged as
    /// part of the message.</para>
    /// <para>Trace message will be logged to the log category with the same name as the tracer operation name.
    /// You must configure the operation categories, or the catch-all categories, with desired log sinks to log 
    /// the trace messages.</para>
    /// </remarks>
    public class Tracer : IDisposable
    {
        private readonly static bool isFullyTrusted;

        /// <summary>
        /// Priority value for Trace messages
        /// </summary>
        public const int priority = 5;

        /// <summary>
        /// Event id for Trace messages
        /// </summary>
        public const int eventId = 1;

        /// <summary>
        /// Title for operation start Trace messages
        /// </summary>
        public const string startTitle = "TracerEnter";

        /// <summary>
        /// Title for operation end Trace messages
        /// </summary>
        public const string endTitle = "TracerExit";

        /// <summary>
        /// Name of the entry in the ExtendedProperties having the activity id
        /// </summary>
        public const string ActivityIdPropertyKey = "TracerActivityId";

        private Stopwatch stopwatch;
        private long tracingStartTicks;
        private bool tracerDisposed;
        private readonly Guid? previousActivityId;

        private readonly bool tracingAvailable;
        private readonly LogWriter writer;

        static Tracer()
        {
            Tracer.isFullyTrusted = typeof(Tracer).Assembly.IsFullyTrusted;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tracer"/> class with the given logical operation name.
        /// </summary>
        /// <remarks>
        /// If an existing activity id is already set, it will be kept. Otherwise, a new activity id will be created.
        /// </remarks>
        /// <param name="operation">The operation for the <see cref="Tracer"/>.</param>
        public Tracer(string operation)
            : this(operation, Logger.Writer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tracer"/> class with the given logical operation name and activity id.
        /// </summary>
        /// <remarks>
        /// The activity id will override a previous activity id.
        /// </remarks>
        /// <param name="operation">The operation for the <see cref="Tracer"/>.</param>
        /// <param name="activityId">The activity id.</param>
        public Tracer(string operation, Guid activityId)
            : this(operation, activityId, Logger.Writer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tracer"/> class with the given logical operation name and activity id.
        /// </summary>
        /// <remarks>
        /// The activity id will override a previous activity id.
        /// </remarks>
        /// <param name="operation">The operation for the <see cref="Tracer"/>.</param>
        /// <param name="activityId">The activity id.</param>
        /// <param name="writer">The <see cref="LogWriter"/> that is used to write trace messages.</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is <see langword="null"/>.</exception>
        public Tracer(string operation, Guid activityId, LogWriter writer)
        {
            Guard.ArgumentNotNull(writer, "writer");
            this.writer = writer;

            try
            {
                this.tracingAvailable = true;
                CheckPermissionsIfFullyTrusted();
                this.previousActivityId = GetActivityId();
            }
            catch (SecurityException)
            {
                this.tracingAvailable = false;
                return;
            }

            SetActivityId(activityId);

            Initialize(operation);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tracer"/> class with the given logical operation name and activity id.
        /// </summary>
        /// <remarks>
        /// If an existing activity id is already set, it will be kept. Otherwise, a new activity id will be created.
        /// </remarks>
        /// <param name="operation">The operation for the <see cref="Tracer"/>.</param>
        /// <param name="writer">The <see cref="LogWriter"/> that is used to write trace messages.</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is <see langword="null"/>.</exception>
        public Tracer(string operation, LogWriter writer)
        {
            Guard.ArgumentNotNull(writer, "writer");
            this.writer = writer;

            try
            {
                this.tracingAvailable = true;
                CheckPermissionsIfFullyTrusted();

                if (GetActivityId().Equals(Guid.Empty))
                {
                    this.previousActivityId = Guid.Empty;
                    SetActivityId(Guid.NewGuid());
                }
                else
                {
                    this.previousActivityId = null;
                }
            }
            catch (SecurityException)
            {
                this.tracingAvailable = false;
                return;
            }

            Initialize(operation);
        }

        /// <summary>
        /// Causes the <see cref="Tracer"/> to output its closing message.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Indicates whether tracing is enabled
        /// </summary>
        /// <returns><see langword="true"/> if tracing is enabled; otherwise, <see langword="false"/>.</returns>
        public bool IsTracingEnabled()
        {
            return GetWriter().IsTracingEnabled();
        }

        [SecuritySafeCritical]
        private static void CheckPermissionsIfFullyTrusted()
        {
            if (Tracer.isFullyTrusted)
            {
                new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!tracerDisposed)
                {
                    if (tracingAvailable)
                    {
                        try
                        {
                            if (this.IsTracingEnabled())
                            {
                                WriteTraceEndMessage(endTitle);
                            }
                        }
                        finally
                        {
                            try
                            {
                                StopLogicalOperation();
                                if (this.previousActivityId != null)
                                {
                                    SetActivityId(this.previousActivityId.Value);
                                }
                            }
                            catch (SecurityException)
                            {
                            }
                        }
                    }

                    tracerDisposed = true;
                }
            }
        }

        private void Initialize(string operation)
        {
            StartLogicalOperation(operation);
            if (this.IsTracingEnabled())
            {
                stopwatch = Stopwatch.StartNew();
                tracingStartTicks = Stopwatch.GetTimestamp();

                WriteTraceStartMessage(startTitle);
            }
        }

        private void WriteTraceStartMessage(string entryTitle)
        {
            string methodName = GetExecutingMethodName();
            string message = string.Format(CultureInfo.CurrentCulture, Resources.Tracer_StartMessageFormat, GetActivityId(), methodName, tracingStartTicks);

            WriteTraceMessage(message, entryTitle, TraceEventType.Start);
        }

        private void WriteTraceEndMessage(string entryTitle)
        {
            long tracingEndTicks = Stopwatch.GetTimestamp();
            decimal secondsElapsed = GetSecondsElapsed(stopwatch.ElapsedMilliseconds);

            string methodName = GetExecutingMethodName();
            string message = string.Format(CultureInfo.CurrentCulture, Resources.Tracer_EndMessageFormat, GetActivityId(), methodName, tracingEndTicks, secondsElapsed);
            WriteTraceMessage(message, entryTitle, TraceEventType.Stop);
        }

        private void WriteTraceMessage(string message, string entryTitle, TraceEventType eventType)
        {
            var extendedProperties = new Dictionary<string, object>();
            var entry = new LogEntry(message, PeekLogicalOperationStack() as string, priority, eventId, eventType, entryTitle, extendedProperties);

            GetWriter().Write(entry);
        }

        private string GetExecutingMethodName()
        {
            string result = "Unknown";
            StackTrace trace = new StackTrace(false);

            for (int index = 0; index < trace.FrameCount; ++index)
            {
                StackFrame frame = trace.GetFrame(index);
                MethodBase method = frame.GetMethod();
                Type declaringType = method.DeclaringType;
                if (declaringType != GetType() && declaringType != typeof(TraceManager))
                {
                    result = string.Concat(method.DeclaringType.FullName, ".", method.Name);
                    break;
                }
            }

            return result;
        }

        private decimal GetSecondsElapsed(long milliseconds)
        {
            decimal result = Convert.ToDecimal(milliseconds) / 1000m;
            return Math.Round(result, 6);
        }

        private LogWriter GetWriter()
        {
            return this.writer ?? Logger.Writer;
        }

        [SecuritySafeCritical]
        private static Guid GetOrCreateActivityId()
        {
            return (Trace.CorrelationManager.ActivityId == Guid.Empty) ? Guid.NewGuid() : Trace.CorrelationManager.ActivityId;
        }

        [SecuritySafeCritical]
        private static Guid GetActivityId()
        {
            return Trace.CorrelationManager.ActivityId;
        }

        [SecuritySafeCritical]
        private static Guid SetActivityId(Guid activityId)
        {
            return Trace.CorrelationManager.ActivityId = activityId;
        }

        [SecuritySafeCritical]
        private static void StartLogicalOperation(string operation)
        {
            Trace.CorrelationManager.StartLogicalOperation(operation);
        }

        [SecuritySafeCritical]
        private static void StopLogicalOperation()
        {
            Trace.CorrelationManager.StopLogicalOperation();
        }

        [SecuritySafeCritical]
        private static object PeekLogicalOperationStack()
        {
            return Trace.CorrelationManager.LogicalOperationStack.Peek();
        }
    }
}
