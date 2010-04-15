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
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Properties;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation
{
    /// <summary>
    /// Defines the logical events that can be instrumented for symmetric cryptography providers.
    /// </summary>
    [HasInstallableResourcesAttribute]
    [PerformanceCountersDefinition(counterCategoryName, "CryptographyHelpResourceName")]
    [EventLogDefinition("Application", "Enterprise Library Cryptography")]
    public class SymmetricAlgorithmInstrumentationProvider : InstrumentationListener, ISymmetricAlgorithmInstrumentationProvider
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
        /// Initializes a new instance of the <see cref="SymmetricAlgorithmInstrumentationProvider"/> class.
        /// </summary>
        /// <param name="instanceName">The name of the <see cref="ISymmetricCryptoProvider"/> instance this instrumentation listener is created for.</param>
        /// <param name="performanceCountersEnabled"><b>true</b> if performance counters should be updated.</param>
        /// <param name="eventLoggingEnabled"><b>true</b> if event log entries should be written.</param>
        /// <param name="applicationInstanceName">The application instance name.</param>
        public SymmetricAlgorithmInstrumentationProvider(string instanceName,
                                           bool performanceCountersEnabled,
                                           bool eventLoggingEnabled,
                                           string applicationInstanceName)
            : this(instanceName, performanceCountersEnabled, eventLoggingEnabled, new AppDomainNameFormatter(applicationInstanceName))
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="instanceName">The name of the <see cref="ISymmetricCryptoProvider"/> instance this instrumentation listener is created for.</param>
        /// <param name="performanceCountersEnabled"><b>true</b> if performance counters should be updated.</param>
        /// <param name="eventLoggingEnabled"><b>true</b> if event log entries should be written.</param>
        /// <param name="nameFormatter">The <see cref="IPerformanceCounterNameFormatter"/> that is used to creates unique name for each <see cref="PerformanceCounter"/> instance.</param>
        public SymmetricAlgorithmInstrumentationProvider(string instanceName,
                                           bool performanceCountersEnabled,
                                           bool eventLoggingEnabled,
                                           IPerformanceCounterNameFormatter nameFormatter)
            : base(instanceName, performanceCountersEnabled, eventLoggingEnabled, nameFormatter)
        {
            this.instanceName = instanceName;
        }

        /// <summary>
        /// </summary>
        /// <param name="message">The message that describes the failure.</param>
        /// <param name="exception">The exception thrown during the failure.</param>
        public void FireCyptographicOperationFailed(string message, Exception exception)
        {
            if (exception == null) throw new ArgumentNullException("exception");

            if (EventLoggingEnabled)
            {
                string errorMessage
                    = string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.ErrorCryptographicOperationFailed,
                        instanceName);
                string entryText = new EventLogEntryFormatter(Resources.BlockName).GetEntryText(errorMessage, exception, message);

                EventLog.WriteEntry(GetEventSourceName(), entryText, EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// </summary>
        public void FireSymmetricEncryptionPerformed()
        {
            if (PerformanceCountersEnabled)
            {
                symmetricEncryptionPerformedCounter.Increment();
                totalSymmetricEncryptionPerformedCounter.Increment();
            }
        }

        /// <summary>
        /// </summary>
        public void FireSymmetricDecryptionPerformed()
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
