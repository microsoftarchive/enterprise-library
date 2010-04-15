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

using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration
{
    /// <summary>
    /// Configuration object for Instrumentation. This section defines the instrumentation behavior 
    /// for the entire application
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "InstrumentationConfigurationSectionDescription")]
    [ResourceDisplayName(typeof(DesignResources), "InstrumentationConfigurationSectionDisplayName")]
    public class InstrumentationConfigurationSection : SerializableConfigurationSection
    {
        private const string performanceCountersEnabled = "performanceCountersEnabled";
        private const string eventLoggingEnabled = "eventLoggingEnabled";
        private const string applicationInstanceName = "applicationInstanceName";

        /// <summary>
        /// Section name
        /// </summary>
        public const string SectionName = "instrumentationConfiguration";

        internal bool InstrumentationIsEntirelyDisabled
        {
            get { return (PerformanceCountersEnabled || EventLoggingEnabled) == false; }
        }

        /// <summary>
        /// Initializes enabled state of the three forms of instrumentation
        /// </summary>
        /// <param name="performanceCountersEnabled">True if performance counter instrumentation is to be enabled</param>
        /// <param name="eventLoggingEnabled">True if event logging instrumentation is to be enabled</param>
        public InstrumentationConfigurationSection(bool performanceCountersEnabled, bool eventLoggingEnabled) :
            this(performanceCountersEnabled, eventLoggingEnabled, "")
        { }

        /// <summary>
        /// Initializes enabled state of the three forms of instrumentation and the instance name
        /// </summary>
        /// <param name="performanceCountersEnabled">True if performance counter instrumentation is to be enabled</param>
        /// <param name="eventLoggingEnabled">True if event logging instrumentation is to be enabled</param>
        /// <param name="applicationInstanceName">Value of the InstanceName</param>
        public InstrumentationConfigurationSection(bool performanceCountersEnabled, bool eventLoggingEnabled, string applicationInstanceName)
        {
            this.PerformanceCountersEnabled = performanceCountersEnabled;
            this.EventLoggingEnabled = eventLoggingEnabled;
            this.ApplicationInstanceName = applicationInstanceName;
        }

        /// <summary>
        /// Initializes object to default settings of all instrumentation types disabled and an empty InstanceName
        /// </summary>
        public InstrumentationConfigurationSection()
        {
        }

        /// <summary>
        /// Gets and sets the value of PerformanceCountersEnabled
        /// </summary>
        [ConfigurationProperty(performanceCountersEnabled, IsRequired = false, DefaultValue = false)]
        [ResourceDescription(typeof(DesignResources), "InstrumentationConfigurationSectionPerformanceCountersEnabledDescription")]
        [ResourceDisplayName(typeof(DesignResources), "InstrumentationConfigurationSectionPerformanceCountersEnabledDisplayName")]
        public bool PerformanceCountersEnabled
        {
            get { return (bool)this[performanceCountersEnabled]; }
            set { this[performanceCountersEnabled] = value; }
        }

        /// <summary>
        /// Gets and sets the value of EventLoggingEnabled
        /// </summary>
        [ConfigurationProperty(eventLoggingEnabled, IsRequired = false, DefaultValue = false)]
        [ResourceDescription(typeof(DesignResources), "InstrumentationConfigurationSectionEventLoggingEnabledDescription")]
        [ResourceDisplayName(typeof(DesignResources), "InstrumentationConfigurationSectionEventLoggingEnabledDisplayName")]
        public bool EventLoggingEnabled
        {
            get { return (bool)this[eventLoggingEnabled]; }
            set { this[eventLoggingEnabled] = value; }
        }

        /// <summary>
        /// Gets and sets value of ApplicationInstanceName
        /// </summary>
        [ConfigurationProperty(applicationInstanceName, IsRequired = false, DefaultValue = "")]
        [ResourceDescription(typeof(DesignResources), "InstrumentationConfigurationSectionApplicationInstanceNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "InstrumentationConfigurationSectionApplicationInstanceNameDisplayName")]
        public string ApplicationInstanceName
        {
            get { return (string)this[applicationInstanceName]; }
            set { this[applicationInstanceName] = value; }
        }

        /// <summary>
        /// Retrieve the <see cref="InstrumentationConfigurationSection"/> from the given configuratio source.
        /// If the source is null, or does not contain an instrumentation section, then return a default
        /// section with instrumentation turned off.
        /// </summary>
        /// <param name="configurationSource">Configuration source containing section (or not).</param>
        /// <returns>The configuration section.</returns>
        public static InstrumentationConfigurationSection GetSection(IConfigurationSource configurationSource)
        {
            if (configurationSource != null)
            {
                var section =
                    configurationSource.GetSection(SectionName) as InstrumentationConfigurationSection;
                if (section != null)
                {
                    return section;
                }
            }

            return new InstrumentationConfigurationSection();
        }

        /// <summary>
        /// Gets a value indicating whether an unknown attribute is encountered during deserialization.
        /// </summary>
        protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
        {
            return string.Equals("wmiEnabled", name, StringComparison.Ordinal)
                || base.OnDeserializeUnrecognizedAttribute(name, value);
        }
    }
}
