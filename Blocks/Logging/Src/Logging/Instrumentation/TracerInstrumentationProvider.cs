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

using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation
{
    ///<summary>
    ///</summary>
    [HasInstallableResourcesAttribute]
    [PerformanceCountersDefinition(counterCategoryName, "LoggingCountersHelpResource")]
    public class TracerInstrumentationProvider : InstrumentationListener, ITracerInstrumentationProvider
    {
        static EnterpriseLibraryPerformanceCounterFactory factory = new EnterpriseLibraryPerformanceCounterFactory();

        /// <summary>
        /// Made public for test
        /// </summary>
        public const string TotalTraceOperationsStartedCounterName = "Total Trace Operations Started";

        [PerformanceCounter("Trace Operations Started/sec", "TraceOperationStartedHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
        private TracerPerformanceCounter traceOperationStarted;

        [PerformanceCounter(TotalTraceOperationsStartedCounterName, "TotalTraceOperationsStartedHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
        private TracerPerformanceCounter totalTraceOperationsStartedCounter;

        [PerformanceCounter("Avg. Trace Execution Time", "AverageTraceExecutionTimeHelpResource", PerformanceCounterType.AverageCount64,
            BaseCounterName = "Avg. Trace Execution Time Base", BaseCounterHelp = "AverageTraceExecutionTimeBaseHelpResource", BaseCounterType = PerformanceCounterType.AverageBase)]
        private TracerPerformanceCounter averageTraceExecutionTime;
        private TracerPerformanceCounter averageTraceExecutionTimeBase;

        /// <summary>
        /// Made public for test
        /// </summary>
        public const string counterCategoryName = "Enterprise Library Logging Counters";

        ///<summary>
        /// Initializes a new instance of the <see cref="TracerInstrumentationProvider"/> class.
        ///</summary>
        ///<param name="performanceCountersEnabled"></param>
        ///<param name="eventLoggingEnabled"></param>
        ///<param name="applicationName"></param>
        public TracerInstrumentationProvider(bool performanceCountersEnabled, bool eventLoggingEnabled, string applicationName)
            : base(performanceCountersEnabled, eventLoggingEnabled, new AppDomainNameFormatter(applicationName))
        {
        }

        ///<summary>
        ///</summary>
        public void FireTraceOperationStarted(string operationName)
        {
            if (PerformanceCountersEnabled)
            {
                string instanceName = CreateInstanceName(operationName);
                traceOperationStarted.Increment(instanceName);
                totalTraceOperationsStartedCounter.Increment(instanceName);
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="operationName"></param>
        ///<param name="elapsedTimeInMilleseconds"></param>
        public void FireTraceOperationEnded(string operationName, long elapsedTimeInMilleseconds)
        {
            if (PerformanceCountersEnabled)
            {
                string instanceName = CreateInstanceName(operationName);
                averageTraceExecutionTime.IncrementBy(instanceName, elapsedTimeInMilleseconds);
                averageTraceExecutionTimeBase.Increment(instanceName);
            }
        }

        /// <summary>
        /// Creates the performance counters to instrument the <see cref="Tracer"/> operations associated to the instance names.
        /// </summary>
        /// <param name="instanceNames">The instance names for the performance counters.</param>
        protected override void CreatePerformanceCounters(string[] instanceNames)
        {
            traceOperationStarted = new TracerPerformanceCounter(counterCategoryName, "Trace Operations Started/sec");
            averageTraceExecutionTime = new TracerPerformanceCounter(counterCategoryName, "Avg. Trace Execution Time");
            averageTraceExecutionTimeBase = new TracerPerformanceCounter(counterCategoryName, "Avg. Trace Execution Time Base");
            totalTraceOperationsStartedCounter = new TracerPerformanceCounter(counterCategoryName, TotalTraceOperationsStartedCounterName);
        }
    }
}
