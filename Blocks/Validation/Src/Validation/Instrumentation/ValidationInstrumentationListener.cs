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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Instrumentation
{
	/// <summary>
	/// Provides the concrete instrumentation for the logical events raised by a <see cref="ValidationInstrumentationProvider"/> object.
	/// </summary>
	[HasInstallableResources]
	[PerformanceCountersDefinition(counterCategoryName, "validationCountersHelpResource")]
	[CustomFactory(typeof(ValidationInstrumentationListenerCustomFactory))]
	[EventLogDefinition("Application", "Enterprise Library Validation")]
	public class ValidationInstrumentationListener : InstrumentationListener
	{
		static EnterpriseLibraryPerformanceCounterFactory factory = new EnterpriseLibraryPerformanceCounterFactory();

		private const string counterCategoryName = "Enterprise Library Validation Counters";

		[PerformanceCounter("Number of Validation Calls", "validationCallHelpResource", PerformanceCounterType.NumberOfItems32)]
		private ValidationPerformanceCounter validationCall;

		[PerformanceCounter("Number of Validation Successes", "validationSuccessesHelpResource", PerformanceCounterType.NumberOfItems32)]
		private ValidationPerformanceCounter validationSucceeded;

		[PerformanceCounter("Number of Validation Failures", "validationFailuresHelpResource", PerformanceCounterType.NumberOfItems32)]
		private ValidationPerformanceCounter validationFailures;

		[PerformanceCounter("Validation Calls/sec", "validationCallPerSecondHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
		private ValidationPerformanceCounter validationCallPerSecond;

		[PerformanceCounter("Validation Successes/sec", "validationSucceededPerSecondHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
		private ValidationPerformanceCounter validationSucceededPerSecond;

		[PerformanceCounter("Validation Failures/sec", "validationFailuresPerSecondHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
		private ValidationPerformanceCounter validationFailuresPerSecond;

		[PerformanceCounter("% Validation Successes", "percentageValidationSuccessesHelpResource", PerformanceCounterType.RawFraction,
		  BaseCounterName = "% Validation Successes Base",
		  BaseCounterType = PerformanceCounterType.RawBase,
		  BaseCounterHelp = "percentageValidationSuccessesBaseHelpResource")]
		private ValidationPerformanceCounter percentageValidationSuccesses;
		private ValidationPerformanceCounter percentageValidationSuccessesBase;

		private IEventLogEntryFormatter eventLogEntryFormatter;

		/// <summary>
		/// Initializes a new instance of the <see cref="ValidationInstrumentationListener"/> class.
		/// </summary>
		/// <param name="performanceCountersEnabled"><code>true</code> if performance counters should be updated.</param>
		/// <param name="eventLoggingEnabled"><code>true</code> if event log entries should be written.</param>
		/// <param name="wmiEnabled"><code>true</code> if WMI events should be fired.</param>
        /// <param name="applicationInstanceName">The application instance name.</param>
		public ValidationInstrumentationListener(bool performanceCountersEnabled,
											  bool eventLoggingEnabled,
											  bool wmiEnabled,
                                              string applicationInstanceName)
            : this(performanceCountersEnabled, eventLoggingEnabled, wmiEnabled, new AppDomainNameFormatter(applicationInstanceName))
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="ValidationInstrumentationListener"/> class.
		/// </summary>
		/// <param name="performanceCountersEnabled"><code>true</code> if performance counters should be updated.</param>
		/// <param name="eventLoggingEnabled"><code>true</code> if event log entries should be written.</param>
		/// <param name="wmiEnabled"><code>true</code> if WMI events should be fired.</param>
		/// <param name="nameFormatter">Creates unique name for each <see cref="PerformanceCounter"/> instance.</param>
		public ValidationInstrumentationListener(bool performanceCountersEnabled,
											  bool eventLoggingEnabled,
											  bool wmiEnabled,
											  IPerformanceCounterNameFormatter nameFormatter)
			: base(performanceCountersEnabled, eventLoggingEnabled, wmiEnabled, nameFormatter)
		{
			this.eventLogEntryFormatter = new EventLogEntryFormatter(Resources.BlockName);
		}


		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Handler for the <see cref="ValidationInstrumentationProvider.validationCalled"/> event.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e">Data for the event.</param>
		[InstrumentationConsumer("ValidateCalled")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2109",
			Justification = "Must be public for instrumentation binding to work in partial trust")]
		public void ValidationCalled(object sender, ValidationEventArgs e)
		{
			if (PerformanceCountersEnabled)
			{
				//increment counter specific to this type/ruleSet
				string instanceName = CreateInstanceName(e.TypeBeingValidated.Name);
				validationCall.Increment(instanceName);
				validationCallPerSecond.Increment(instanceName);
				percentageValidationSuccessesBase.Increment(instanceName);

				//increment totals
				validationCall.Increment();
				validationCallPerSecond.Increment();
				percentageValidationSuccessesBase.Increment();

			}
		}

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Handler for the <see cref="ValidationInstrumentationProvider.configurationFailure"/> event.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e">Data for the event.</param>
		[InstrumentationConsumer("ConfigurationFailure")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2109",
			Justification = "Must be public for instrumentation binding to work in partial trust")]
		public void ConfigurationFailure(object sender, ValidationConfigurationFailureEventArgs e)
		{
			if (WmiEnabled) FireManagementInstrumentation(new ValidationConfigurationFailureEvent(e.Exception.Message));
			if (EventLoggingEnabled)
			{
				string entryText = eventLogEntryFormatter.GetEntryText(Resources.ConfigurationErrorMessage, e.Exception);
				EventLog.WriteEntry(GetEventSourceName(), entryText, EventLogEntryType.Error);
			}
		}

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Handler for the <see cref="ValidationInstrumentationProvider.validationException"/> event.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e">Data for the event.</param>
		[InstrumentationConsumer("ValidateException")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2109",
			Justification = "Must be public for instrumentation binding to work in partial trust")]
		public void ValidationException(object sender, ValidationExceptionEventArgs e)
		{
			if (WmiEnabled) FireManagementInstrumentation(new ValidationExceptionEvent(e.TypeBeingValidated.FullName, e.Exception.ToString()));
		}

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Handler for the <see cref="ValidationInstrumentationProvider.validationFailed"/> event.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e">Data for the event.</param>
		[InstrumentationConsumer("ValidateFailure")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2109",
			Justification = "Must be public for instrumentation binding to work in partial trust")]
		public void ValidationFailed(object sender, ValidationFailedEventArgs e)
		{
			if (PerformanceCountersEnabled)
			{
				//increment counter specific to this type/ruleSet
				string instanceName = CreateInstanceName(e.TypeBeingValidated.Name);
				validationFailures.Increment(instanceName);
				validationFailuresPerSecond.Increment(instanceName);

				//increment totals
				validationFailures.Increment();
				validationFailuresPerSecond.Increment();
			}

			if (WmiEnabled)
			{
				FireManagementInstrumentation(new ValidationFailedEvent(e.TypeBeingValidated.FullName));
			}
		}

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Handler for the <see cref="ValidationInstrumentationProvider.validationSucceeded"/> event.
		/// </summary>
		/// <param name="sender">Sender of the event.</param>
		/// <param name="e">Data for the event.</param>
		[InstrumentationConsumer("ValidateSuccess")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2109",
			Justification = "Must be public for instrumentation binding to work in partial trust")]
		public void ValidationSucceeded(object sender, ValidationEventArgs e)
		{
			if (PerformanceCountersEnabled)
			{
				//increment counter specific to this type/ruleSet
				string instanceName = CreateInstanceName(e.TypeBeingValidated.Name);
				validationSucceeded.Increment(instanceName);
				validationSucceededPerSecond.Increment(instanceName);
				percentageValidationSuccesses.Increment(instanceName);

				//increment totals
				validationSucceeded.Increment();
				validationSucceededPerSecond.Increment();
				percentageValidationSuccesses.Increment();
			}
			if (WmiEnabled)
			{
				FireManagementInstrumentation(new ValidationSucceededEvent(e.TypeBeingValidated.FullName));
			}
		}

		/// <summary>
		/// Creates the performance counters to instrument the validation events to the instance names.
		/// </summary>
		/// <param name="instanceNames">The instance names for the performance counters.</param>
		protected override void CreatePerformanceCounters(string[] instanceNames)
		{
			validationCall = new ValidationPerformanceCounter(counterCategoryName, "Number of Validation Calls");
			validationSucceeded = new ValidationPerformanceCounter(counterCategoryName, "Number of Validation Successes");
			validationFailures = new ValidationPerformanceCounter(counterCategoryName, "Number of Validation Failures");
			validationCallPerSecond = new ValidationPerformanceCounter(counterCategoryName, "Validation Calls/sec");
			validationSucceededPerSecond = new ValidationPerformanceCounter(counterCategoryName, "Validation Successes/sec");
			validationFailuresPerSecond = new ValidationPerformanceCounter(counterCategoryName, "Validation Failures/sec");
			percentageValidationSuccesses = new ValidationPerformanceCounter(counterCategoryName, "% Validation Successes");
			percentageValidationSuccessesBase = new ValidationPerformanceCounter(counterCategoryName, "% Validation Successes Base");
		}
	}
}
