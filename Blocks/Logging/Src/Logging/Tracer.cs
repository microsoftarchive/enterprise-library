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
using System.Diagnostics;
using System.Reflection;
using System.Security;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
using System.Collections.Generic;
using System.Security.Permissions;
using Microsoft.Practices.ServiceLocation;


namespace Microsoft.Practices.EnterpriseLibrary.Logging
{


    /// <summary>
    /// Represents a performance tracing class to log method entry/exit and duration.
    /// </summary>
    /// <remarks>
    /// <para>Lifetime of the Tracer object will determine the beginning and the end of
    /// the trace.  The trace message will include, method being traced, start time, end time 
    /// and duration.</para>
    /// <para>Since Tracer uses the logging block to log the trace message, you can include application
    /// data as part of your trace message. Configured items from call context will be logged as
    /// part of the message.</para>
    /// <para>Trace message will be logged to the log category with the same name as the tracer operation name.
    /// You must configure the operation categories, or the catch-all categories, with desired log sinks to log 
    /// the trace messages.</para>
    /// </remarks>
    public class Tracer : IDisposable
    {
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

        private readonly ITracerInstrumentationProvider instrumentationProvider;
        private Stopwatch stopwatch;
        private long tracingStartTicks;
        private bool tracerDisposed;
        private bool tracingAvailable;

        private readonly LogWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tracer"/> class with the given logical operation name.
        /// </summary>
        /// <remarks>
        /// If an existing activity id is already set, it will be kept. Otherwise, a new activity id will be created.
        /// </remarks>
        /// <param name="operation">The operation for the <see cref="Tracer"/></param>
        public Tracer(string operation)
            : this(operation, Logger.Writer, GetTracerInstrumentationProvider())
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Tracer"/> class with the given logical operation name and activity id.
        /// </summary>
        /// <remarks>
        /// The activity id will override a previous activity id
        /// </remarks>
        /// <param name="operation">The operation for the <see cref="Tracer"/></param>
        /// <param name="activityId">The activity id</param>
        public Tracer(string operation, Guid activityId)
            :this(operation, activityId, Logger.Writer, GetTracerInstrumentationProvider())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tracer"/> class with the given logical operation name.
        /// </summary>
        /// <remarks>
        /// If an existing activity id is already set, it will be kept. Otherwise, a new activity id will be created.
        /// </remarks>
        /// <param name="operation">The operation for the <see cref="Tracer"/></param>
        /// <param name="writer">The <see cref="LogWriter"/> that is used to write trace messages</param>
        /// <param name="instrumentationConfiguration">The configuration source that is used to determine instrumentation should be enabled.</param>
        public Tracer(string operation, LogWriter writer, IConfigurationSource instrumentationConfiguration) :
            this(operation, writer, GetTracerInstrumentationProvider(instrumentationConfiguration))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tracer"/> class with the given logical operation name.
        /// </summary>
        /// <remarks>
        /// If an existing activity id is already set, it will be kept. Otherwise, a new activity id will be created.
        /// </remarks>
        /// <param name="operation">The operation for the <see cref="Tracer"/></param>
        /// <param name="writer">The <see cref="LogWriter"/> that is used to write trace messages</param>
        /// <param name="serviceLocator"><see cref="IServiceLocator"/> used to retrieve the instrumentation provider for this tracer.</param>
        public Tracer(string operation, LogWriter writer, IServiceLocator serviceLocator) :
            this(operation, writer, GetTracerInstrumentationProvider(serviceLocator))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tracer"/> class with the given logical name.
        /// It retrieves require dependent objects from the given <paramref name="container"/>.
        /// </summary>
        /// <remarks>
        /// If an existing activity id is already set, it will be kept. Otherwise, a new activity id will be created.
        /// </remarks>
        /// <param name="operation">The operation for the <see cref="Tracer"/></param>
        /// <param name="container"><see cref="IServiceLocator"/> used to retrieve dependent objects.</param>
        public Tracer(string operation, IServiceLocator container)
            : this(operation, container.GetInstance<LogWriter>(), container.GetInstance<ITracerInstrumentationProvider>())
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tracer"/> class with the given logical operation name and activity id.
        /// </summary>
        /// <remarks>
        /// The activity id will override a previous activity id
        /// </remarks>
        /// <param name="operation">The operation for the <see cref="Tracer"/></param>
        /// <param name="activityId">The activity id</param>
        /// <param name="writer">The <see cref="LogWriter"/> that is used to write trace messages</param>
        /// <param name="instrumentationConfiguration">configuration source that is used to determine instrumentation should be enabled</param>
		public Tracer(string operation, Guid activityId, LogWriter writer, IConfigurationSource instrumentationConfiguration) :
            this(operation, activityId, writer, GetTracerInstrumentationProvider(instrumentationConfiguration))
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="Tracer"/> class with the given logical operation name and activity id.
        /// </summary>
        /// <remarks>
        /// The activity id will override a previous activity id
        /// </remarks>
        /// <param name="operation">The operation for the <see cref="Tracer"/></param>
        /// <param name="activityId">The activity id</param>
        /// <param name="writer">The <see cref="LogWriter"/> that is used to write trace messages</param>
        /// <param name="serviceLocator"><see cref="IServiceLocator"/> used to retrieve the instrumentation provider for this tracer.</param>
        public Tracer(string operation, Guid activityId, LogWriter writer, IServiceLocator serviceLocator) :
            this(operation, activityId, writer, GetTracerInstrumentationProvider(serviceLocator))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tracer"/> class with the given logical operation name and activity id.
        /// </summary>
        /// <remarks>
        /// This is meant to be used internally
        /// </remarks>
        /// <param name="operation">The operation for the <see cref="Tracer"/></param>
        /// <param name="activityId">The activity id</param>
        /// <param name="writer">The <see cref="LogWriter"/> that is used to write trace messages</param>
        /// <param name="instrumentationProvider">Instrumentation provider to use for firing logical instrumentation events from Tracer.</param>
        /// <exception cref="ArgumentNullException"></exception>
        internal Tracer(string operation, Guid activityId, LogWriter writer, ITracerInstrumentationProvider instrumentationProvider)
        {
            this.instrumentationProvider = instrumentationProvider;

            if (CheckTracingAvailable())
            {
                if (writer == null) throw new ArgumentNullException("writer", Resources.ExceptionWriterShouldNotBeNull);

                SetActivityId(activityId);

                this.writer = writer;

                Initialize(operation);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tracer"/> class with the given logical operation name and activity id.
        /// </summary>
        /// <remarks>
        /// This is meant to be used internally
        /// </remarks>
        /// <param name="operation">The operation for the <see cref="Tracer"/></param>
        /// <param name="writer">The <see cref="LogWriter"/> that is used to write trace messages</param>
        /// <param name="instrumentationProvider">Instrumentation provider to use for firing logical instrumentation events from Tracer.</param>
        /// <exception cref="ArgumentNullException"></exception>
        internal Tracer(string operation, LogWriter writer, ITracerInstrumentationProvider instrumentationProvider)
        {
            this.instrumentationProvider = instrumentationProvider;

            if (CheckTracingAvailable())
            {
                if (writer == null) throw new ArgumentNullException("writer", Resources.ExceptionWriterShouldNotBeNull);

                if (GetActivityId().Equals(Guid.Empty))
                {
                    SetActivityId(Guid.NewGuid());
                }

                this.writer = writer;

                Initialize(operation);
            }
        }


        /// <summary>
        /// Causes the <see cref="Tracer"/> to output its closing message.
        /// </summary>
        public void Dispose()
        {
            if (!tracerDisposed)
            {
                if (tracingAvailable)
                {
                    try
                    {
                        if (IsTracingEnabled()) WriteTraceEndMessage(endTitle);
                    }
                    finally
                    {
                        try
                        {
                            StopLogicalOperation();
                        }
                        catch (SecurityException)
                        {
                        }
                    }
                }

                tracerDisposed = true;
            }
        }

        /// <summary>
        /// Answers whether tracing is enabled
        /// </summary>
        /// <returns>true if tracing is enabled</returns>
        public bool IsTracingEnabled()
        {
            return GetWriter().IsTracingEnabled();
        }

        internal static bool IsTracingAvailable()
        {
            bool tracingAvailable = false;

            try
            {
                tracingAvailable = SecurityManager.IsGranted(new SecurityPermission(SecurityPermissionFlag.UnmanagedCode));
            }
            catch (SecurityException)
            { }

            return tracingAvailable;
        }

        private bool CheckTracingAvailable()
        {
            tracingAvailable = IsTracingAvailable();

            return tracingAvailable;
        }

        private static ITracerInstrumentationProvider GetTracerInstrumentationProvider()
        {
            return EnterpriseLibraryContainer.Current.GetInstance<ITracerInstrumentationProvider>();
        }

        private static ITracerInstrumentationProvider GetTracerInstrumentationProvider(IServiceLocator serviceLocator)
        {
            if (serviceLocator == null) throw new ArgumentNullException("serviceLocator");
            return serviceLocator.GetInstance<ITracerInstrumentationProvider>();
        }

        private static ITracerInstrumentationProvider GetTracerInstrumentationProvider(IConfigurationSource configuration)
        {
            if (configuration == null) return new NullTracerInstrumentationProvider();

            var container = EnterpriseLibraryContainer.CreateDefaultContainer(configuration);
            return container.GetInstance<ITracerInstrumentationProvider>();
        }

        private void Initialize(string operation)
        {
            StartLogicalOperation(operation);
            if (IsTracingEnabled())
            {
                instrumentationProvider.FireTraceOperationStarted(PeekLogicalOperationStack() as string);

                stopwatch = Stopwatch.StartNew();
                tracingStartTicks = Stopwatch.GetTimestamp();

                WriteTraceStartMessage(startTitle);
            }
        }

        private void WriteTraceStartMessage(string entryTitle)
        {
            string methodName = GetExecutingMethodName();
            string message = string.Format(Resources.Culture, Resources.Tracer_StartMessageFormat, GetActivityId(), methodName, tracingStartTicks);

            WriteTraceMessage(message, entryTitle, TraceEventType.Start);
        }

        private void WriteTraceEndMessage(string entryTitle)
        {
            long tracingEndTicks = Stopwatch.GetTimestamp();
            decimal secondsElapsed = GetSecondsElapsed(stopwatch.ElapsedMilliseconds);

            string methodName = GetExecutingMethodName();
            string message = string.Format(Resources.Culture, Resources.Tracer_EndMessageFormat, GetActivityId(), methodName, tracingEndTicks, secondsElapsed);
            WriteTraceMessage(message, entryTitle, TraceEventType.Stop);

            instrumentationProvider.FireTraceOperationEnded(PeekLogicalOperationStack() as string, stopwatch.ElapsedMilliseconds);
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
                if (method.DeclaringType != GetType())
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
            return writer ?? Logger.Writer;
        }

        private static Guid GetOrCreateActivityId()
        {
            return (Trace.CorrelationManager.ActivityId == Guid.Empty) ? Guid.NewGuid() : Trace.CorrelationManager.ActivityId;
        }

        private static Guid GetActivityId()
        {
            return Trace.CorrelationManager.ActivityId;
        }

        private static Guid SetActivityId(Guid activityId)
        {
            return Trace.CorrelationManager.ActivityId = activityId;
        }

        private static void StartLogicalOperation(string operation)
        {
            Trace.CorrelationManager.StartLogicalOperation(operation);
        }

        private static void StopLogicalOperation()
        {
            Trace.CorrelationManager.StopLogicalOperation();
        }

        private static object PeekLogicalOperationStack()
        {
            return Trace.CorrelationManager.LogicalOperationStack.Peek();
        }
    }
}
