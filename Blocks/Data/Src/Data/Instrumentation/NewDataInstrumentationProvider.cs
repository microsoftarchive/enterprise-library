//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation
{
    /// <summary>
    /// An implementation of <see cref="IDataInstrumentationProvider"/> that raises
    /// the instrumentation events to the event log and performance counters.
    /// </summary>
    [HasInstallableResourcesAttribute]
    [PerformanceCountersDefinition(CounterCategoryName, "CounterCategoryHelpResourceName")]
    [EventLogDefinition("Application", "Enterprise Library Data")]
    public class NewDataInstrumentationProvider : InstrumentationListener, IDataInstrumentationProvider
    {
        /// <summary>
        /// Name of the performance counter category updated by this provider.
        /// </summary>
        public const string CounterCategoryName = "Enterprise Library Data Counters";

        /// <summary>
        /// Instance name for the "connections opened" performance counter.
        /// </summary>
        public const string TotalConnectionOpenedCounter = "Total Connections Opened";

        /// <summary>
        /// Name for the "connections failed" performance counter.
        /// </summary>
        public const string TotalConnectionFailedCounter = "Total Connections Failed";

        /// <summary>
        /// Name for the "commands executed" performance counter.
        /// </summary>
        public const string TotalCommandsExecutedCounter = "Total Commands Executed";

        /// <summary>
        /// Name for the "commands failed" performance counter.
        /// </summary>
        public const string TotalCommandsFailedCounter = "Total Commands Failed";

        static readonly EnterpriseLibraryPerformanceCounterFactory counterCache = new EnterpriseLibraryPerformanceCounterFactory();

        [PerformanceCounter("Connections Opened/sec", "ConnectionOpenedCounterHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
        EnterpriseLibraryPerformanceCounter connectionOpenedCounter;

        [PerformanceCounter(TotalConnectionOpenedCounter, "TotalConnectionOpenedHelpResource", PerformanceCounterType.NumberOfItems32)]
        EnterpriseLibraryPerformanceCounter totalConnectionOpenedCounter;

        [PerformanceCounter("Commands Executed/sec", "CommandExecutedCounterHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
        EnterpriseLibraryPerformanceCounter commandExecutedCounter;

        [PerformanceCounter(TotalCommandsExecutedCounter, "TotalCommandsExecutedHelpResource", PerformanceCounterType.NumberOfItems32)]
        EnterpriseLibraryPerformanceCounter totalCommandsExecutedCounter;

        [PerformanceCounter("Connections Failed/sec", "ConnectionFailedCounterHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
        EnterpriseLibraryPerformanceCounter connectionFailedCounter;

        [PerformanceCounter(TotalConnectionFailedCounter, "TotalConnectionFailedHelpResource", PerformanceCounterType.NumberOfItems32)]
        EnterpriseLibraryPerformanceCounter totalConnectionFailedCounter;

        [PerformanceCounter("Commands Failed/sec", "CommandFailedCounterHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
        EnterpriseLibraryPerformanceCounter commandFailedCounter;

        [PerformanceCounter(TotalCommandsFailedCounter, "TotalCommandsFailedHelpResource", PerformanceCounterType.NumberOfItems32)]
        EnterpriseLibraryPerformanceCounter totalCommandsFailedCounter;


        private readonly string instanceName;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewDataInstrumentationProvider"/> class.
        /// </summary>
        /// <param name="instanceName">The name of the <see cref="Database"/> instance this instrumentation Provider is created for.</param>
        /// <param name="performanceCountersEnabled"><b>true</b> if performance counters should be updated.</param>
        /// <param name="eventLoggingEnabled"><b>true</b> if event log entries should be written.</param>
        /// <param name="applicationInstanceName">The application instance name.</param>
        public NewDataInstrumentationProvider(
            string instanceName,
            bool performanceCountersEnabled,
            bool eventLoggingEnabled,
            string applicationInstanceName)
            : this(instanceName, performanceCountersEnabled, eventLoggingEnabled, new AppDomainNameFormatter(applicationInstanceName))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewDataInstrumentationProvider"/> class.
        /// </summary>
        /// <param name="instanceName">The name of the <see cref="Database"/> instance this instrumentation Provider is created for.</param>
        /// <param name="performanceCountersEnabled"><b>true</b> if performance counters should be updated.</param>
        /// <param name="eventLoggingEnabled"><b>true</b> if event log entries should be written.</param>
        /// <param name="nameFormatter">The <see cref="IPerformanceCounterNameFormatter"/> that is used to creates unique name for each <see cref="PerformanceCounter"/> instance.</param>
        public NewDataInstrumentationProvider(
            string instanceName,
            bool performanceCountersEnabled,
            bool eventLoggingEnabled,
            IPerformanceCounterNameFormatter nameFormatter)
            : base(instanceName, performanceCountersEnabled, eventLoggingEnabled, nameFormatter)
        {
            this.instanceName = instanceName;
        }

        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Default handler for the <see cref="NewDataInstrumentationProvider.FireConnectionOpenedEvent"/> event.
        /// </summary>
        /// <remarks>
        /// Increments the "Connections Opened/sec" and the "Total Connections Opened" performance counter 
        /// </remarks>
        public void FireConnectionOpenedEvent()
        {
            if (PerformanceCountersEnabled)
            {
                connectionOpenedCounter.Increment();
                totalConnectionOpenedCounter.Increment();
            }
        }

        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Default handler for the <see cref="NewDataInstrumentationProvider.FireCommandExecutedEvent"/> event.
        /// </summary>
        /// <remarks>
        /// Increments the "Commands Executed/sec" performance counter.
        /// </remarks>
        /// <param name="startTime">Time the command was executed</param>
        public void FireCommandExecutedEvent(DateTime startTime)
        {
            if (PerformanceCountersEnabled)
            {
                commandExecutedCounter.Increment();
                totalCommandsExecutedCounter.Increment();
            }
        }

        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Default handler for the <see cref="NewDataInstrumentationProvider.FireConnectionFailedEvent"/> event.
        /// </summary>
        /// <remarks>
        /// Increments the "Connections Failed/sec" performance counter and writes 
        /// an error entry to the event log.
        /// </remarks>
        /// <param name="connectionString">The connection string that caused the failed connection, with credentials removed.</param>
        /// <param name="exception">The exception thrown when the connection failed.</param>
        public void FireConnectionFailedEvent(string connectionString, Exception exception)
        {
            if (exception == null) throw new ArgumentNullException("exception");

            if (PerformanceCountersEnabled)
            {
                connectionFailedCounter.Increment();
                totalConnectionFailedCounter.Increment();
            }
            if (EventLoggingEnabled)
            {
                string errorMessage
                    = string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.ErrorConnectionFailedMessage,
                        instanceName);
                string extraInformation
                    = string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.ErrorConnectionFailedExtraInformation,
                        connectionString);
                string entryText = new EventLogEntryFormatter(Resources.BlockName).GetEntryText(errorMessage, exception, extraInformation);

                EventLog.WriteEntry(GetEventSourceName(), entryText, EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Default handler for the <see cref="NewDataInstrumentationProvider.FireCommandFailedEvent"/> event.
        /// </summary>
        /// <remarks>
        /// Increments the "Commands Failed/sec" performance counter and writes 
        /// an error entry to the event log.
        /// </remarks>
        /// <param name="commandText">The text of the command that failed its execution.</param>
        /// <param name="connectionString">The connection string of the <see cref="Database"/> that executed the failed command, with credentials removed.</param>
        /// <param name="exception">The exception thrown when the command failed.</param>
        public void FireCommandFailedEvent(string commandText, string connectionString, Exception exception)
        {
            if (exception == null) throw new ArgumentNullException("exception");

            if (PerformanceCountersEnabled)
            {
                commandFailedCounter.Increment();
                totalCommandsFailedCounter.Increment();
            }
        }

        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Clears the cached performance counter instances.
        /// </summary>
        public static void ClearCounterCache()
        {
            counterCache.ClearCachedCounters();
        }

        /// <summary>
        /// Creates the performance counters to instrument the caching events for the specified instance names.
        /// </summary>
        /// <param name="instanceNames">The instance names for the performance counters.</param>
        protected override void CreatePerformanceCounters(string[] instanceNames)
        {
            connectionOpenedCounter = counterCache.CreateCounter(CounterCategoryName, "Connections Opened/sec", instanceNames);
            commandExecutedCounter = counterCache.CreateCounter(CounterCategoryName, "Commands Executed/sec", instanceNames);
            connectionFailedCounter = counterCache.CreateCounter(CounterCategoryName, "Connections Failed/sec", instanceNames);
            commandFailedCounter = counterCache.CreateCounter(CounterCategoryName, "Commands Failed/sec", instanceNames);
            totalConnectionOpenedCounter = counterCache.CreateCounter(CounterCategoryName, TotalConnectionOpenedCounter, instanceNames);
            totalConnectionFailedCounter = counterCache.CreateCounter(CounterCategoryName, TotalConnectionFailedCounter, instanceNames);
            totalCommandsExecutedCounter = counterCache.CreateCounter(CounterCategoryName, TotalCommandsExecutedCounter, instanceNames);
            totalCommandsFailedCounter = counterCache.CreateCounter(CounterCategoryName, TotalCommandsFailedCounter, instanceNames);
        }

    }
}
