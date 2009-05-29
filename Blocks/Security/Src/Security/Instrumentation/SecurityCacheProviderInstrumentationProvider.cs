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
    /// Defines the logical events that can be instrumented for <see cref="SecurityCacheProvider"/> instances.
    /// </summary>
    [HasInstallableResourcesAttribute]
    [PerformanceCountersDefinition(PerfomanceCountersCategoryName, "SecurityHelpResourceName")]
    [EventLogDefinition("Application", "Enterprise Library Security")]
	public class SecurityCacheProviderInstrumentationProvider : InstrumentationListener, ISecurityCacheProviderInstrumentationProvider
	{
        
		static EnterpriseLibraryPerformanceCounterFactory factory = new EnterpriseLibraryPerformanceCounterFactory();

		/// <summary>
        /// Made public for testing purposes.
		/// </summary>
		public const string SecurityCacheReadPerformedCounterName = "Security Cache Reads/sec";

        /// <summary>
        /// Made public for testing purposes.
        /// </summary>
        public const string TotalSecurityCacheReadPerformedCounterName = "Total Security Cache Reads/sec";

		/// <summary>
        /// Made public for testing purposes.
		/// </summary>
		public const string PerfomanceCountersCategoryName = "Enterprise Library Security Counters";

		[PerformanceCounter(SecurityCacheReadPerformedCounterName, "SecurityCacheReadPerformedHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
		EnterpriseLibraryPerformanceCounter securityCacheReadPerformedCounter;

        [PerformanceCounter(TotalSecurityCacheReadPerformedCounterName, "TotalSecurityCacheReadPerformedHelpResource", PerformanceCounterType.NumberOfItems32)]
        EnterpriseLibraryPerformanceCounter totalSecurityCacheReadPerformedCounter;

		private string instanceName;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityCacheProviderInstrumentationProvider"/> class.
        /// </summary>
        /// <param name="instanceName">The name of the <see cref="SecurityCacheProvider"/> instance the events apply on.</param>
        /// <param name="performanceCountersEnabled"><code>true</code> if performance counters should be updated.</param>
        /// <param name="eventLoggingEnabled"><code>true</code> if event log entries should be written.</param>
        /// <param name="wmiEnabled"><code>true</code> if WMI events should be fired.</param>
        /// <param name="applicationInstanceName">The application instance name.</param>
        public SecurityCacheProviderInstrumentationProvider(string instanceName,
										   bool performanceCountersEnabled,
										   bool eventLoggingEnabled,
										   bool wmiEnabled,
                                           string applicationInstanceName)
            : this(instanceName, performanceCountersEnabled, eventLoggingEnabled, wmiEnabled, new AppDomainNameFormatter(applicationInstanceName))
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityCacheProviderInstrumentationProvider"/> class.
        /// </summary>
        /// <param name="instanceName">The name of the <see cref="SecurityCacheProvider"/> instance the events apply on.</param>
        /// <param name="performanceCountersEnabled"><code>true</code> if performance counters should be updated.</param>
        /// <param name="eventLoggingEnabled"><code>true</code> if event log entries should be written.</param>
        /// <param name="wmiEnabled"><code>true</code> if WMI events should be fired.</param>
        /// <param name="nameFormatter">The <see cref="IPerformanceCounterNameFormatter"/> that is used to creates unique name for each <see cref="PerformanceCounter"/> instance.</param>
        public SecurityCacheProviderInstrumentationProvider(string instanceName,
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
        /// <param name="itemType">The type of item that is read from the <see cref="SecurityCacheProvider"/>.</param>
        /// <param name="token">The token that was is used to read an item from the <see cref="SecurityCacheProvider"/>.</param>
        public void FireSecurityCacheReadPerformed(SecurityEntityType itemType, IToken token)
		{
            if (PerformanceCountersEnabled)
            {
                securityCacheReadPerformedCounter.Increment();
                totalSecurityCacheReadPerformedCounter.Increment();
            }

            if (WmiEnabled) FireManagementInstrumentation(new SecurityCacheReadPerformedEvent(instanceName, itemType.ToString(), ( token == null) ? string.Empty : token.Value));
        }

        /// <summary>
        /// Creates the performance counters to instrument a <see cref="SecurityCacheProvider"/>'s events.
        /// </summary>
        /// <param name="instanceNames">The instance names for the performance counters.</param>
        protected override void CreatePerformanceCounters(string[] instanceNames)
        {
            securityCacheReadPerformedCounter
                = factory.CreateCounter(PerfomanceCountersCategoryName, SecurityCacheReadPerformedCounterName, instanceNames);

            totalSecurityCacheReadPerformedCounter
                = factory.CreateCounter(PerfomanceCountersCategoryName, TotalSecurityCacheReadPerformedCounterName, instanceNames);
        }
	}
}
