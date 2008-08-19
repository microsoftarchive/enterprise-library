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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation
{
	/// <summary>
	/// Provides the concrete instrumentation for the logical events raised by a <see cref="Tracer"/> object.
	/// </summary>
	[HasInstallableResourcesAttribute]
	[PerformanceCountersDefinition(counterCategoryName, "LoggingCountersHelpResource")]
	[CustomFactory(typeof(TracerInstrumentationListenerCustomFactory))]
	public class TracerInstrumentationListener : InstrumentationListener
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

		/// <summary>
		/// Initializes a new instance of the <see cref="TracerInstrumentationListener"/> class.
		/// </summary>
		/// <param name="performanceCountersEnabled"><code>true</code> if performance counters should be updated.</param>
		public TracerInstrumentationListener(bool performanceCountersEnabled)
			: base("", performanceCountersEnabled, false, false, new AppDomainNameFormatter())
		{
		}

		/// <summary>
		/// Instruments the start of a trace operation.
		/// </summary>
		/// <param name="operationName">The name of the started operation.</param>
		public void TracerOperationStarted(string operationName)
		{
			if (PerformanceCountersEnabled)
			{
				string instanceName = CreateInstanceName(operationName);
				traceOperationStarted.Increment(instanceName);
                totalTraceOperationsStartedCounter.Increment(instanceName);
			}
		}

		/// <summary>
		/// Instruments the end of a trace operation.
		/// </summary>
		/// <param name="operationName">The name of the ended operation.</param>
		/// <param name="traceDurationInMilliSeconds">The duration of the traced operation in milliseconds.</param>
		public void TracerOperationEnded(string operationName, decimal traceDurationInMilliSeconds)
		{
			if (PerformanceCountersEnabled)
			{
				string instanceName = CreateInstanceName(operationName);
				averageTraceExecutionTime.IncrementBy(instanceName, (long)traceDurationInMilliSeconds);
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
