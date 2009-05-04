//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Diagnostics;
using ManagementInstrumentation = System.Management.Instrumentation.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation
{
	/// <summary>
	/// Provides the concrete instrumentation for the logical events raised by a <see cref="HashAlgorithmInstrumentationProvider"/> object.
	/// </summary>
	[HasInstallableResourcesAttribute]
	[PerformanceCountersDefinition(counterCategoryName, "CryptographyHelpResourceName")]
	[EventLogDefinition("Application", "Enterprise Library Cryptography")]
	public class HashAlgorithmInstrumentationListener : InstrumentationListener
	{
		static EnterpriseLibraryPerformanceCounterFactory factory = new EnterpriseLibraryPerformanceCounterFactory();

        /// <summary>
        /// Made public for testing
        /// </summary>
        public const string TotalHashOperationPerformedPerformanceCounterName = "Total Hash Operations";

        /// <summary>
        /// Made public for testing
        /// </summary>
        public const string TotalHashComparisonPerformedPerformanceCounterName = "Total Hash Comparisons";

        /// <summary>
        /// Made public for testing
        /// </summary>
        public const string TotalHashMismatchesPerformedPerformanceCounterName = "Total Hash Mismatches";

		[PerformanceCounter("Hash Operations/sec", "HashOperationPerformedHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
		EnterpriseLibraryPerformanceCounter hashOperationPerformedCounter;

        [PerformanceCounter(TotalHashOperationPerformedPerformanceCounterName, "TotalHashOperationPerformedHelpResource", PerformanceCounterType.NumberOfItems32)]
        EnterpriseLibraryPerformanceCounter totalHashOperationPerformedPerformanceCounter;

		[PerformanceCounter("Hash Comparisons/sec", "HashComparisonPerformedHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
		EnterpriseLibraryPerformanceCounter hashComparisonPerformedCounter;

        [PerformanceCounter(TotalHashComparisonPerformedPerformanceCounterName, "TotalHashComparisonPerformedHelpResource", PerformanceCounterType.NumberOfItems32)]
        EnterpriseLibraryPerformanceCounter totalHashComparisonPerformedPerformanceCounter;

		[PerformanceCounter("Hash Mismatches/sec", "HashMismatchDetectedHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
		EnterpriseLibraryPerformanceCounter hashMismatchDetectedCounter;

        [PerformanceCounter(TotalHashMismatchesPerformedPerformanceCounterName, "TotalHashMismatchDetectedHelpResource", PerformanceCounterType.NumberOfItems32)]
        EnterpriseLibraryPerformanceCounter totalHashMismatchesPerformedPerformanceCounter;

		private string instanceName;

        /// <summary>
        /// Made public for testing
        /// </summary>
		public const string counterCategoryName = "Enterprise Library Cryptography Counters";

		/// <summary>
		/// Initializes a new instance of the <see cref="HashAlgorithmInstrumentationListener"/> class.
		/// </summary>
		/// <param name="instanceName">The name of the <see cref="IHashProvider"/> instance this instrumentation listener is created for.</param>
		/// <param name="performanceCountersEnabled"><b>true</b> if performance counters should be updated.</param>
		/// <param name="eventLoggingEnabled"><b>true</b> if event log entries should be written.</param>
		/// <param name="wmiEnabled"><b>true</b> if WMI events should be fired.</param>
        /// <param name="applicationInstanceName">The application instance name.</param>
		public HashAlgorithmInstrumentationListener(string instanceName,
										   bool performanceCountersEnabled,
										   bool eventLoggingEnabled,
										   bool wmiEnabled,
                                           string applicationInstanceName)
            : this(instanceName, performanceCountersEnabled, eventLoggingEnabled, wmiEnabled, new AppDomainNameFormatter(applicationInstanceName))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="HashAlgorithmInstrumentationListener"/> class.
		/// </summary>
		/// <param name="instanceName">The name of the <see cref="IHashProvider"/> instance this instrumentation listener is created for.</param>
		/// <param name="performanceCountersEnabled"><b>true</b> if performance counters should be updated.</param>
		/// <param name="eventLoggingEnabled"><b>true</b> if event log entries should be written.</param>
		/// <param name="wmiEnabled"><b>true</b> if WMI events should be fired.</param>
		/// <param name="nameFormatter">The <see cref="IPerformanceCounterNameFormatter"/> that is used to creates unique name for each <see cref="PerformanceCounter"/> instance.</param>
		public HashAlgorithmInstrumentationListener(string instanceName,
										   bool performanceCountersEnabled,
										   bool eventLoggingEnabled,
										   bool wmiEnabled,
										   IPerformanceCounterNameFormatter nameFormatter)
			: base(instanceName, performanceCountersEnabled, eventLoggingEnabled, wmiEnabled, nameFormatter)
		{
			this.instanceName = instanceName;
		}

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Handler for the <see cref="HashAlgorithmInstrumentationProvider.cyptographicOperationFailed"/> event.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e">Data for the event.</param>
		[InstrumentationConsumer("CyptographicOperationFailed")]
		public void CyptographicOperationFailed(object sender, CrytographicOperationErrorEventArgs e)
		{
			if (EventLoggingEnabled)
			{
				string errorMessage
					= string.Format(
						Resources.Culture,
						Resources.ErrorCryptographicOperationFailed,
						instanceName);
				string entryText = new EventLogEntryFormatter(Resources.BlockName).GetEntryText(errorMessage, e.Exception, e.Message);

				EventLog.WriteEntry(GetEventSourceName(), entryText, EventLogEntryType.Error);
			}
			if (WmiEnabled) FireManagementInstrumentation(new HashOperationFailedEvent(instanceName, e.Message, e.Exception.ToString()));
		}

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Handler for the <see cref="HashAlgorithmInstrumentationProvider.hashOperationPerformed"/> event.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e">Data for the event.</param>
		[InstrumentationConsumer("HashOperationPerformed")]
		public void HashOperationPerformed(object sender, EventArgs e)
		{
            if (PerformanceCountersEnabled)
            {
                hashOperationPerformedCounter.Increment();
                totalHashOperationPerformedPerformanceCounter.Increment();
            }
		}

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Handler for the <see cref="HashAlgorithmInstrumentationProvider.hashComparisonPerformed"/> event.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e">Data for the event.</param>
		[InstrumentationConsumer("HashComparisonPerformed")]
		public void HashComparisonPerformed(object sender, EventArgs e)
		{
            if (PerformanceCountersEnabled)
            {
                hashComparisonPerformedCounter.Increment();
                totalHashComparisonPerformedPerformanceCounter.Increment();
            }
		}

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Handler for the <see cref="HashAlgorithmInstrumentationProvider.hashMismatchDetected"/> event.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e">Data for the event.</param>
		[InstrumentationConsumer("HashMismatchDetected")]
		public void HashMismatchDetected(object sender, EventArgs e)
		{
            if (PerformanceCountersEnabled)
            {
                hashMismatchDetectedCounter.Increment();
                totalHashMismatchesPerformedPerformanceCounter.Increment();
            }

			if (WmiEnabled) FireManagementInstrumentation(new HashMismatchDetectedEvent(instanceName));
		}

		/// <summary>
		/// Creates the performance counters to instrument the hash provider events for the specified instance names.
		/// </summary>
		/// <param name="instanceNames">The instance names for the performance counters.</param>
		protected override void CreatePerformanceCounters(string[] instanceNames)
		{
			hashOperationPerformedCounter
				= factory.CreateCounter(counterCategoryName, "Hash Operations/sec", instanceNames);
			hashComparisonPerformedCounter
				= factory.CreateCounter(counterCategoryName, "Hash Comparisons/sec", instanceNames);
			hashMismatchDetectedCounter
				= factory.CreateCounter(counterCategoryName, "Hash Mismatches/sec", instanceNames);
            totalHashOperationPerformedPerformanceCounter
                = factory.CreateCounter(counterCategoryName, TotalHashOperationPerformedPerformanceCounterName, instanceNames);
            totalHashComparisonPerformedPerformanceCounter
                = factory.CreateCounter(counterCategoryName, TotalHashComparisonPerformedPerformanceCounterName, instanceNames);
            totalHashMismatchesPerformedPerformanceCounter
                = factory.CreateCounter(counterCategoryName, TotalHashMismatchesPerformedPerformanceCounterName, instanceNames);
		}
	}
}
