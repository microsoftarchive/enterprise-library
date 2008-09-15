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

using System.Collections.Generic;
using System.Configuration;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability
{
    /// <summary>
    /// Represents the configuration information for the <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.LoggingSettings">Logging</see>
    /// features provided by the Enterprise Library.
    /// </summary>
    [ManagementEntity]
    public partial class LoggingBlockSetting : ConfigurationSectionSetting
    {
        string defaultCategory;
        bool logWarningWhenNoCategoriesMatch;
        bool tracingEnabled;
        bool revertImpersonation;

        /// <summary>
        /// Initialize a new instance of the <see cref="LoggingBlockSetting"/> class with the block configuration,
        /// the default category, if a warning is log when no categories match, and if tracing is enabled.
        /// </summary>
        /// <param name="sourceElement">The logging block configuration.</param>
        /// <param name="defaultCategory">The default category.</param>
        /// <param name="logWarningWhenNoCategoriesMatch">true to log warnings if no categories match; otherwise false.</param>
        /// <param name="tracingEnabled">true if tracing is enabled; otherwise false.</param>
        /// <param name="revertImpersonation">true if impersonation should be reverted while logging; otherise false.</param>
        public LoggingBlockSetting(
            LoggingSettings sourceElement,
            string defaultCategory,
            bool logWarningWhenNoCategoriesMatch,
            bool tracingEnabled,
            bool revertImpersonation)
            : base(sourceElement)
        {
            this.defaultCategory = defaultCategory;
            this.logWarningWhenNoCategoriesMatch = logWarningWhenNoCategoriesMatch;
            this.tracingEnabled = tracingEnabled;
            this.revertImpersonation = revertImpersonation;
        }

        /// <summary>
        /// Gets the name of the default log category for the represented configuration section.
        /// </summary>
        [ManagementConfiguration]
        public string DefaultCategory
        {
            get { return defaultCategory; }
            set { defaultCategory = value; }
        }

        /// <summary>
        /// Gets the value for the represented configuration section.
        /// </summary>
        [ManagementConfiguration]
        public bool LogWarningWhenNoCategoriesMatch
        {
            get { return logWarningWhenNoCategoriesMatch; }
            set { logWarningWhenNoCategoriesMatch = value; }
        }

        /// <summary>
        /// Gets the tracing enabled status for the represented configuration section.
        /// </summary>
        [ManagementConfiguration]
        public bool TracingEnabled
        {
            get { return tracingEnabled; }
            set { tracingEnabled = value; }
        }

        /// <summary>
        /// Gets the tracing impersonation-reverting status for the represented configuration section.
        /// </summary>
        [ManagementConfiguration]
        public bool RevertImpersonation
        {
            get { return revertImpersonation; }
            set { revertImpersonation = value; }
        }

        /// <summary>
        /// Returns the <see cref="LoggingBlockSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <returns>The published <see cref="LoggingBlockSetting"/> instance specified by the values for the key properties,
        /// or <see langword="null"/> if no such an instance is currently published.</returns>
        [ManagementBind]
        public static LoggingBlockSetting BindInstance(string ApplicationName,
                                                       string SectionName)
        {
            return BindInstance<LoggingBlockSetting>(ApplicationName, SectionName);
        }

        /// <summary>
        /// Returns an enumeration of the published <see cref="LoggingBlockSetting"/> instances.
        /// </summary>
        [ManagementEnumerator]
        public static IEnumerable<LoggingBlockSetting> GetInstances()
        {
            return GetInstances<LoggingBlockSetting>();
        }

        /// <summary>
        /// Saves the changes on the <see cref="LoggingBlockSetting"/> to its corresponding configuration object.
        /// </summary>
        /// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
        /// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return LoggingSettingsWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}