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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Filters
{
    /// <summary>
    /// Represents an instance of <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.LogEnabledFilterData"/>
    /// as an instrumentation class.
    /// </summary>
    [ManagementEntity]
    public class LogEnabledFilterSetting : LogFilterSetting
    {
        bool enabled;

        /// <summary>
        /// Initialize a new instance of the <see cref="LogEnabledFilterSetting"/> class with the filter configuration data,
        /// the name of the filter and if the filter is enabled.
        /// </summary>
        /// <param name="sourceElement">The filter configuration.</param>
        /// <param name="name">The name of the filter.</param>
        /// <param name="enabled">true if the filter is enabled; otherwise false.</param>
        public LogEnabledFilterSetting(LogEnabledFilterData sourceElement,
                                       string name,
                                       bool enabled)
            : base(sourceElement, name)
        {
            this.enabled = enabled;
        }

        /// <summary>
        /// Gets the value of the enabled property for the represented configuration element.
        /// </summary>
        [ManagementConfiguration]
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        /// <summary>
        /// Returns the <see cref="LogEnabledFilterSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <param name="Name">The value for the Name key property.</param>
        /// <returns>The published <see cref="LogEnabledFilterSetting"/> instance specified by the values for the key properties,
        /// or <see langword="null"/> if no such an instance is currently published.</returns>
        [ManagementBind]
        public static LogEnabledFilterSetting BindInstance(string ApplicationName,
                                                           string SectionName,
                                                           string Name)
        {
            return BindInstance<LogEnabledFilterSetting>(ApplicationName, SectionName, Name);
        }

        /// <summary>
        /// Returns an enumeration of the published <see cref="LogEnabledFilterSetting"/> instances.
        /// </summary>
        [ManagementEnumerator]
        public static IEnumerable<LogEnabledFilterSetting> GetInstances()
        {
            return GetInstances<LogEnabledFilterSetting>();
        }

        /// <summary>
        /// Saves the changes on the <see cref="LogEnabledFilterSetting"/> to its corresponding configuration object.
        /// </summary>
        /// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
        /// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return LogEnabledFilterDataWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}
