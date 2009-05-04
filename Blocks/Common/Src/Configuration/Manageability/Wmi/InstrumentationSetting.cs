//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using System.Configuration;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
    /// <summary>
    /// Represents the configuration information for the <see cref="Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration.InstrumentationConfigurationSection">Instrumentation</see>
    /// features provided by the Enterprise Library.
    /// </summary>
    [ManagementEntity]
    public partial class InstrumentationSetting : ConfigurationSectionSetting
    {
        bool eventLoggingEnabled;
        bool performanceCountersEnabled;
        bool wmiEnabled;

        /// <summary>
        /// Initialize a new instance of the <see cref="InstrumentationSetting"/> class.
        /// </summary>
        /// <param name="sourceElement">The configuraiton source element</param>
        /// <param name="eventLoggingEnabled">true if event logging is enabled;otherwise, false.</param>
        /// <param name="performanceCountersEnabled">true if performance counters are enabled; otherwise, false.</param>
        /// <param name="wmiEnabled">true if wmi is enabled; otherwise, false.</param>
        public InstrumentationSetting(ConfigurationElement sourceElement,
                                      bool eventLoggingEnabled,
                                      bool performanceCountersEnabled,
                                      bool wmiEnabled)
            : base(sourceElement)
        {
            this.eventLoggingEnabled = eventLoggingEnabled;
            this.performanceCountersEnabled = performanceCountersEnabled;
            this.wmiEnabled = wmiEnabled;
        }

        /// <summary>
        /// Gets the event logging enablement status on the represented instrumentation configuration.
        /// </summary>
        [ManagementConfiguration]
        public bool EventLoggingEnabled
        {
            get { return eventLoggingEnabled; }
            set { eventLoggingEnabled = value; }
        }

        /// <summary>
        /// Gets the performance counter enablement status on the represented instrumentation configuration.
        /// </summary>
        [ManagementConfiguration]
        public bool PerformanceCountersEnabled
        {
            get { return performanceCountersEnabled; }
            set { performanceCountersEnabled = value; }
        }

        /// <summary>
        /// Gets the wmi enablement status on the represented instrumentation configuration.
        /// </summary>
        [ManagementConfiguration]
        public bool WmiEnabled
        {
            get { return wmiEnabled; }
            set { wmiEnabled = value; }
        }

        /// <summary>
        /// Returns the <see cref="InstrumentationSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <returns>The published <see cref="InstrumentationSetting"/> instance specified by the values for the key properties,
        /// or <see langword="null"/> if no such an instance is currently published.</returns>
        [ManagementBind]
        public static InstrumentationSetting BindInstance(string ApplicationName,
                                                          string SectionName)
        {
            return BindInstance<InstrumentationSetting>(ApplicationName, SectionName);
        }

        /// <summary>
        /// Returns an enumeration of the published <see cref="InstrumentationSetting"/> instances.
        /// </summary>
        /// <returns></returns>
        [ManagementEnumerator]
        public static IEnumerable<InstrumentationSetting> GetInstances()
        {
            return GetInstances<InstrumentationSetting>();
        }

        /// <summary>
        /// Saves the changes on the <see cref="InstrumentationSetting"/> to its corresponding configuration object.
        /// </summary>
        /// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
        /// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return InstrumentationConfigurationSectionWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}
