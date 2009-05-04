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
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation
{
	/// <summary>
	/// Provides the concrete instrumentation for the logical events raised by a <see cref="SymmetricAlgorithmInstrumentationProvider"/> object.
	/// </summary>
	[HasInstallableResourcesAttribute]
	[PerformanceCountersDefinition(counterCategoryName, "CryptographyHelpResourceName")]
	[EventLogDefinition("Application", "Enterprise Library Cryptography")]
	public class SymmetricAlgorithmInstrumentationListener : InstrumentationListener
	{
		static EnterpriseLibraryPerformanceCounterFactory factory = new EnterpriseLibraryPerformanceCounterFactory();

        /// <summary>
        /// Made public for testing
        /// </summary>
        public const string TotalSymmetricEncryptionPerformedCounterName = "Total Symmetric Encryptions";

        /// <summary>
        /// Made public for testing
        /// </summary>
        public const string TotalSymmetricDecryptionPerformedCounterName = "Total Symmetric Decryptions";

		[PerformanceCounter("Symmetric Encryptions/sec", "SymmetricEncryptionPerformedHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
		EnterpriseLibraryPerformanceCounter symmetricEncryptionPerformedCounter;

        [PerformanceCounter(TotalSymmetricEncryptionPerformedCounterName, "TotalSymmetricEncryptionPerformedHelpResource", PerformanceCounterType.NumberOfItems32)]
        EnterpriseLibraryPerformanceCounter totalSymmetricEncryptionPerformedCounter;

		[PerformanceCounter("Symmetric Decryptions/sec", "SymmetricDecryptionPerformedHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
		EnterpriseLibraryPerformanceCounter symmetricDecryptionPerformedCounter;

        [PerformanceCounter(TotalSymmetricDecryptionPerformedCounterName, "TotalSymmetricDecryptionPerformedHelpResource", PerformanceCounterType.NumberOfItems32)]
        EnterpriseLibraryPerformanceCounter totalSymmetricDecryptionPerformedCounter;

		/// <summary>
		/// Made public for testing
		/// </summary>
        public const string counterCategoryName = "Enterprise Library Cryptography Counters";

		private string instanceName;

		/// <summary>
		/// Initializes a new instance of the <see cref="SymmetricAlgorithmInstrumentationListener"/> class.
		/// </summary>
		/// <param name="instanceName">The name of the <see cref="ISymmetricCryptoProvider"/> instance this instrumentation listener is created for.</param>
		/// <param name="performanceCountersEnabled"><b>true</b> if performance counters should be updated.</param>
		/// <param name="eventLoggingEnabled"><b>true</b> if event log entries should be written.</param>
		/// <param name="wmiEnabled"><b>true</b> if WMI events should be fired.</param>
        /// <param name="applicationInstanceName">The application instance name.</param>
		public SymmetricAlgorithmInstrumentationListener(string instanceName,
										   bool performanceCountersEnabled,
										   bool eventLoggingEnabled,
										   bool wmiEnabled,
                                           string applicationInstanceName)
            : this(instanceName, performanceCountersEnabled, eventLoggingEnabled, wmiEnabled, new AppDomainNameFormatter(applicationInstanceName))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SymmetricAlgorithmInstrumentationListener"/> class.
		/// </summary>
		/// <param name="instanceName">The name of the <see cref="ISymmetricCryptoProvider"/> instance this instrumentation listener is created for.</param>
		/// <param name="performanceCountersEnabled"><b>true</b> if performance counters should be updated.</param>
		/// <param name="eventLoggingEnabled"><b>true</b> if event log entries should be written.</param>
		/// <param name="wmiEnabled"><b>true</b> if WMI events should be fired.</param>
		/// <param name="nameFormatter">The <see cref="IPerformanceCounterNameFormatter"/> that is used to creates unique name for each <see cref="PerformanceCounter"/> instance.</param>
		public SymmetricAlgorithmInstrumentationListener(string instanceName,
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
		/// Handler for the <see cref="SymmetricAlgorithmInstrumentationProvider.cyptographicOperationFailed"/> event.
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
			if (WmiEnabled) FireManagementInstrumentation(new SymmetricOperationFailedEvent(instanceName, e.Message, e.Exception.ToString()));
		}

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Handler for the <see cref="SymmetricAlgorithmInstrumentationProvider.symmetricEncryptionPerformed"/> event.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e">Data for the event.</param>
		[InstrumentationConsumer("SymmetricEncryptionPerformed")]
		public void SymmetricEncryptionPerformed(object sender, EventArgs e)
		{
            if (PerformanceCountersEnabled)
            {
                symmetricEncryptionPerformedCounter.Increment();
                totalSymmetricEncryptionPerformedCounter.Increment();
            }
		}

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Handler for the <see cref="SymmetricAlgorithmInstrumentationProvider.symmetricDecryptionPerformed"/> event.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e">Data for the event.</param>
		[InstrumentationConsumer("SymmetricDecryptionPerformed")]
		public void SymmetricDecryptionPerformed(object sender, EventArgs e)
		{
            if (PerformanceCountersEnabled)
            {
                symmetricDecryptionPerformedCounter.Increment();
                totalSymmetricDecryptionPerformedCounter.Increment();
            }
		}

		/// <summary>
		/// Creates the performance counters to instrument the symmetric crypto events for the specified instance names.
		/// </summary>
		/// <param name="instanceNames">The instance names for the performance counters.</param>
		protected override void CreatePerformanceCounters(string[] instanceNames)
		{
			symmetricEncryptionPerformedCounter
				= factory.CreateCounter(counterCategoryName, "Symmetric Encryptions/sec", instanceNames);
			symmetricDecryptionPerformedCounter
				= factory.CreateCounter(counterCategoryName, "Symmetric Decryptions/sec", instanceNames);
            totalSymmetricEncryptionPerformedCounter
                = factory.CreateCounter(counterCategoryName, TotalSymmetricEncryptionPerformedCounterName, instanceNames);
            totalSymmetricDecryptionPerformedCounter
                = factory.CreateCounter(counterCategoryName, TotalSymmetricDecryptionPerformedCounterName, instanceNames);
		}
	}
}
