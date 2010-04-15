//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Instrumentation
{
    /// <summary>
    /// Provides the concrete instrumentation for the logical events raised through <see cref="IValidationInstrumentationProvider"/>
    /// </summary>
    [HasInstallableResources]
    [PerformanceCountersDefinition(counterCategoryName, "validationCountersHelpResource")]
    [EventLogDefinition("Application", "Enterprise Library Validation")]
    public class ValidationInstrumentationProvider : InstrumentationListener, IValidationInstrumentationProvider
    {
        static EnterpriseLibraryPerformanceCounterFactory factory = new EnterpriseLibraryPerformanceCounterFactory();

        ///<summary>
        ///</summary>
        ///<param name="configurationSource"></param>
        ///<returns></returns>
        public static IValidationInstrumentationProvider FromConfigurationSource(IConfigurationSource configurationSource)
        {
            var instrumentationSection = InstrumentationConfigurationSection.GetSection(configurationSource);

            return new ValidationInstrumentationProvider(
                        instrumentationSection.PerformanceCountersEnabled,
                        instrumentationSection.EventLoggingEnabled,
                        instrumentationSection.ApplicationInstanceName
                        );
        }

        private const string counterCategoryName = "Enterprise Library Validation Counters";

        [PerformanceCounter("Number of Validation Calls", "validationCallHelpResource", PerformanceCounterType.NumberOfItems32)]
        private EnterpriseLibraryPerformanceCounter validationCall;

        [PerformanceCounter("Number of Validation Successes", "validationSuccessesHelpResource", PerformanceCounterType.NumberOfItems32)]
        private EnterpriseLibraryPerformanceCounter validationSucceeded;

        [PerformanceCounter("Number of Validation Failures", "validationFailuresHelpResource", PerformanceCounterType.NumberOfItems32)]
        private EnterpriseLibraryPerformanceCounter validationFailures;

        [PerformanceCounter("Validation Calls/sec", "validationCallPerSecondHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
        private EnterpriseLibraryPerformanceCounter validationCallPerSecond;

        [PerformanceCounter("Validation Successes/sec", "validationSucceededPerSecondHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
        private EnterpriseLibraryPerformanceCounter validationSucceededPerSecond;

        [PerformanceCounter("Validation Failures/sec", "validationFailuresPerSecondHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
        private EnterpriseLibraryPerformanceCounter validationFailuresPerSecond;

        [PerformanceCounter("% Validation Successes", "percentageValidationSuccessesHelpResource", PerformanceCounterType.RawFraction,
          BaseCounterName = "% Validation Successes Base",
          BaseCounterType = PerformanceCounterType.RawBase,
          BaseCounterHelp = "percentageValidationSuccessesBaseHelpResource")]
        private EnterpriseLibraryPerformanceCounter percentageValidationSuccesses;
        private EnterpriseLibraryPerformanceCounter percentageValidationSuccessesBase;

        private IEventLogEntryFormatter eventLogEntryFormatter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationInstrumentationProvider"/> class.
        /// </summary>
        /// <param name="performanceCountersEnabled"><code>true</code> if performance counters should be updated.</param>
        /// <param name="eventLoggingEnabled"><code>true</code> if event log entries should be written.</param>
        /// <param name="applicationInstanceName">The application instance name.</param>
        public ValidationInstrumentationProvider(bool performanceCountersEnabled,
                                              bool eventLoggingEnabled,
                                              string applicationInstanceName)
            : this(performanceCountersEnabled, eventLoggingEnabled, new AppDomainNameFormatter(applicationInstanceName))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationInstrumentationProvider"/> class.
        /// </summary>
        /// <param name="performanceCountersEnabled"><code>true</code> if performance counters should be updated.</param>
        /// <param name="eventLoggingEnabled"><code>true</code> if event log entries should be written.</param>
        /// <param name="nameFormatter">Creates unique name for each <see cref="PerformanceCounter"/> instance.</param>
        public ValidationInstrumentationProvider(bool performanceCountersEnabled,
                                              bool eventLoggingEnabled,
                                              IPerformanceCounterNameFormatter nameFormatter)
            : base(performanceCountersEnabled, eventLoggingEnabled, nameFormatter)
        {
            this.eventLogEntryFormatter = new EventLogEntryFormatter(Resources.BlockName);
        }

        /// <summary>
        /// Creates the performance counters to instrument the validation events to the instance names.
        /// </summary>
        /// <param name="instanceNames">The instance names for the performance counters.</param>
        protected override void CreatePerformanceCounters(string[] instanceNames)
        {
            validationCall = new EnterpriseLibraryPerformanceCounter(counterCategoryName, "Number of Validation Calls");
            validationSucceeded = new EnterpriseLibraryPerformanceCounter(counterCategoryName, "Number of Validation Successes");
            validationFailures = new EnterpriseLibraryPerformanceCounter(counterCategoryName, "Number of Validation Failures");
            validationCallPerSecond = new EnterpriseLibraryPerformanceCounter(counterCategoryName, "Validation Calls/sec");
            validationSucceededPerSecond = new EnterpriseLibraryPerformanceCounter(counterCategoryName, "Validation Successes/sec");
            validationFailuresPerSecond = new EnterpriseLibraryPerformanceCounter(counterCategoryName, "Validation Failures/sec");
            percentageValidationSuccesses = new EnterpriseLibraryPerformanceCounter(counterCategoryName, "% Validation Successes");
            percentageValidationSuccessesBase = new EnterpriseLibraryPerformanceCounter(counterCategoryName, "% Validation Successes Base");
        }

        ///<summary>
        ///</summary>
        ///<param name="typeBeingValidated"></param>
        public void NotifyValidationSucceeded(Type typeBeingValidated)
        {
            if (typeBeingValidated == null) throw new ArgumentNullException("typeBeingValidated");

            if (PerformanceCountersEnabled)
            {
                //increment counter specific to this type/ruleSet
                string instanceName = CreateInstanceName(typeBeingValidated.Name);
                validationSucceeded.Increment(instanceName);
                validationSucceededPerSecond.Increment(instanceName);
                percentageValidationSuccesses.Increment(instanceName);

                //increment totals
                validationSucceeded.Increment();
                validationSucceededPerSecond.Increment();
                percentageValidationSuccesses.Increment();
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="typeBeingValidated"></param>
        ///<param name="validationResult"></param>
        public void NotifyValidationFailed(Type typeBeingValidated, ValidationResults validationResult)
        {
            if (typeBeingValidated == null) throw new ArgumentNullException("typeBeingValidated");

            if (PerformanceCountersEnabled)
            {
                //increment counter specific to this type/ruleSet
                string instanceName = CreateInstanceName(typeBeingValidated.Name);
                validationFailures.Increment(instanceName);
                validationFailuresPerSecond.Increment(instanceName);

                //increment totals
                validationFailures.Increment();
                validationFailuresPerSecond.Increment();
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="configurationException"></param>
        public void NotifyConfigurationFailure(ConfigurationErrorsException configurationException)
        {
            if (EventLoggingEnabled)
            {
                string entryText = eventLogEntryFormatter.GetEntryText(Resources.ConfigurationErrorMessage, configurationException);
                EventLog.WriteEntry(GetEventSourceName(), entryText, EventLogEntryType.Error);
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="typeBeingValidated"></param>
        public void NotifyConfigurationCalled(Type typeBeingValidated)
        {
            if (typeBeingValidated == null) throw new ArgumentNullException("typeBeingValidated");

            if (PerformanceCountersEnabled)
            {
                //increment counter specific to this type/ruleSet
                string instanceName = CreateInstanceName(typeBeingValidated.Name);
                validationCall.Increment(instanceName);
                validationCallPerSecond.Increment(instanceName);
                percentageValidationSuccessesBase.Increment(instanceName);

                //increment totals
                validationCall.Increment();
                validationCallPerSecond.Increment();
                percentageValidationSuccessesBase.Increment();

            }
        }

        ///<summary>
        ///</summary>
        ///<param name="typeBeingValidated"></param>
        ///<param name="errorMessage"></param>
        ///<param name="exception"></param>
        public void NotifyValidationException(Type typeBeingValidated, string errorMessage, Exception exception)
        {
            NotifyConfigurationCalled(typeBeingValidated);
        }
    }

}
