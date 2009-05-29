//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation
{
    /// <summary>
    /// Defines the logical events that can be instrumented for <see cref="AuthorizationProvider"/> instances.
    /// </summary>
    [HasInstallableResourcesAttribute]
    [PerformanceCountersDefinition(PerformanceCountersCategoryName, "SecurityHelpResourceName")]
    [EventLogDefinition("Application", "Enterprise Library Security")]
    public class AuthorizationProviderInstrumentationProvider : InstrumentationListener, IAuthorizationProviderInstrumentationProvider
	{
		static EnterpriseLibraryPerformanceCounterFactory factory = new EnterpriseLibraryPerformanceCounterFactory();
		
		/// <summary>
        /// Made public for testing purposes.
		/// </summary>
		public const string AuthorizationCheckPerformedCounterName = "Authorization Requests/sec";

        /// <summary>
        /// Made public for testing purposes.
        /// </summary>
        public const string TotalAuthorizationCheckPerformedCounterName = "Total Authorization Requests/sec";

		/// <summary>
        /// Made public for testing purposes.
		/// </summary>
		public const string AuthorizationCheckFailedCounterName = "Authorization Requests Denied/sec";

        /// <summary>
        /// Made public for testing purposes.
        /// </summary>
        public const string TotalAuthorizationCheckFailedCounterName = "Total Authorization Requests Denied/sec";

		/// <summary>
        /// Made public for testing purposes.
		/// </summary>
		public const string PerformanceCountersCategoryName = "Enterprise Library Security Counters";

		[PerformanceCounter(AuthorizationCheckPerformedCounterName, "AuthorizationCheckPerformedHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
		EnterpriseLibraryPerformanceCounter authorizationCheckPerformedCounter;

		[PerformanceCounter(AuthorizationCheckFailedCounterName, "AuthorizationCheckFailedHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
		EnterpriseLibraryPerformanceCounter authorizationCheckFailedCounter;

        [PerformanceCounter(TotalAuthorizationCheckPerformedCounterName, "TotalAuthorizationCheckPerformedHelpResource", PerformanceCounterType.NumberOfItems32)]
        EnterpriseLibraryPerformanceCounter totalAuthorizationCheckPerformedCounter;

        [PerformanceCounter(TotalAuthorizationCheckFailedCounterName, "TotalAuthorizationCheckFailedHelpResource", PerformanceCounterType.NumberOfItems32)]
        EnterpriseLibraryPerformanceCounter totalAuthorizationCheckFailedCounter;
		
		private string instanceName;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationProviderInstrumentationProvider"/> class.
        /// </summary>
        /// <param name="instanceName">The name of the <see cref="AuthorizationProvider"/> instance the events apply on.</param>
        /// <param name="performanceCountersEnabled"><code>true</code> if performance counters should be updated.</param>
        /// <param name="eventLoggingEnabled"><code>true</code> if event log entries should be written.</param>
        /// <param name="wmiEnabled"><code>true</code> if WMI events should be fired.</param>
        /// <param name="applicationInstanceName">The application instance name</param>
        public AuthorizationProviderInstrumentationProvider(string instanceName,
										   bool performanceCountersEnabled,
										   bool eventLoggingEnabled,
										   bool wmiEnabled,
                                           string applicationInstanceName)
			: this(instanceName, performanceCountersEnabled, eventLoggingEnabled, wmiEnabled, new AppDomainNameFormatter(applicationInstanceName))
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationProviderInstrumentationProvider"/> class.
        /// </summary>
        /// <param name="instanceName">The name of the <see cref="AuthorizationProvider"/> instance the events apply on.</param>
        /// <param name="performanceCountersEnabled"><code>true</code> if performance counters should be updated.</param>
        /// <param name="eventLoggingEnabled"><code>true</code> if event log entries should be written.</param>
        /// <param name="wmiEnabled"><code>true</code> if WMI events should be fired.</param>
        /// <param name="nameFormatter">The <see cref="IPerformanceCounterNameFormatter"/> that is used to creates unique name for each <see cref="PerformanceCounter"/> instance.</param>
        public AuthorizationProviderInstrumentationProvider(string instanceName,
										   bool performanceCountersEnabled,
										   bool eventLoggingEnabled,
										   bool wmiEnabled,
										   IPerformanceCounterNameFormatter nameFormatter)
			: base(instanceName, performanceCountersEnabled, eventLoggingEnabled, wmiEnabled, nameFormatter)
        {
            this.instanceName = instanceName;
		}

        /// <summary>
        /// </summary>
        /// <param name="identity">The name of the identify which authorization has been checked for.</param>
        /// <param name="ruleName">The name of the authorization rule that has been evaluated.</param>
		public void FireAuthorizationCheckPerformed(string identity, string ruleName)
        {
            if (PerformanceCountersEnabled)
            {
                authorizationCheckPerformedCounter.Increment();
                totalAuthorizationCheckPerformedCounter.Increment();
            }

            if (WmiEnabled) FireManagementInstrumentation(new AuthorizationCheckPerformedEvent(instanceName, identity, ruleName));
        }

        /// <summary>
        /// </summary>
        /// <param name="identity">The name of the identify which authorization has been checked for.</param>
        /// <param name="ruleName">The name of the authorization rule that has been evaluated.</param>
        public void FireAuthorizationCheckFailed(string identity, string ruleName)
        {
            if (PerformanceCountersEnabled)
            {
                authorizationCheckFailedCounter.Increment();
                totalAuthorizationCheckFailedCounter.Increment();
            }

            if (WmiEnabled) FireManagementInstrumentation(new AuthorizationCheckFailedEvent(instanceName, identity, ruleName));
        }

        /// <summary>
        /// Creates the performance counters to instrument an <see cref="AuthorizationProvider"/>'s events.
        /// </summary>
        /// <param name="instanceNames">The instance names for the performance counters.</param>
        protected override void CreatePerformanceCounters(string[] instanceNames)
        {
            authorizationCheckPerformedCounter
                = factory.CreateCounter(PerformanceCountersCategoryName, AuthorizationCheckPerformedCounterName, instanceNames);
            authorizationCheckFailedCounter
                = factory.CreateCounter(PerformanceCountersCategoryName, AuthorizationCheckFailedCounterName, instanceNames);
            totalAuthorizationCheckPerformedCounter
                = factory.CreateCounter(PerformanceCountersCategoryName, TotalAuthorizationCheckPerformedCounterName, instanceNames);
            totalAuthorizationCheckFailedCounter
                = factory.CreateCounter(PerformanceCountersCategoryName, TotalAuthorizationCheckFailedCounterName, instanceNames);
        }

	}
}
