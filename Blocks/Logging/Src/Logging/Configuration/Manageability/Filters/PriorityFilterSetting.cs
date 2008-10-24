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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Filters
{
    /// <summary>
    /// Represents an instance of <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.PriorityFilterData"/>
    /// as an instrumentation class.
    /// </summary>
    [ManagementEntity]
    public class PriorityFilterSetting : LogFilterSetting
    {
        int maximumPriority;
        int minimumPriority;

        /// <summary>
        /// Initialize a new instance of the <see cref="PriorityFilterSetting"/> class with the filter configuration,
        /// the name of the filter, the maximum and minimum priority.
        /// </summary>
        /// <param name="sourceElement">The filter configuration.</param>
        /// <param name="name">The name of the filter.</param>
        /// <param name="maximumPriority">The maximum priority.</param>
        /// <param name="minimumPriority">The minimum priority.</param>
        public PriorityFilterSetting(PriorityFilterData sourceElement,
                                     string name,
                                     int maximumPriority,
                                     int minimumPriority)
            : base(sourceElement, name)
        {
            this.maximumPriority = maximumPriority;
            this.minimumPriority = minimumPriority;
        }

        /// <summary>
        /// Gets the maximum priority for the represented configuration element.
        /// </summary>
        [ManagementConfiguration]
        public int MaximumPriority
        {
            get { return maximumPriority; }
            set { maximumPriority = value; }
        }

        /// <summary>
        /// Gets the minimum priority for the represented configuration element.
        /// </summary>
        [ManagementConfiguration]
        public int MinimumPriority
        {
            get { return minimumPriority; }
            set { minimumPriority = value; }
        }

        /// <summary>
        /// Returns the <see cref="PriorityFilterSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <param name="Name">The value for the Name key property.</param>
        /// <returns>The published <see cref="PriorityFilterSetting"/> instance specified by the values for the key properties,
        /// or <see langword="null"/> if no such an instance is currently published.</returns>
        [ManagementBind]
        public static PriorityFilterSetting BindInstance(string ApplicationName,
                                                         string SectionName,
                                                         string Name)
        {
            return BindInstance<PriorityFilterSetting>(ApplicationName, SectionName, Name);
        }

        /// <summary>
        /// Returns an enumeration of the published <see cref="PriorityFilterSetting"/> instances.
        /// </summary>
        [ManagementEnumerator]
        public static IEnumerable<PriorityFilterSetting> GetInstances()
        {
            return GetInstances<PriorityFilterSetting>();
        }

        /// <summary>
        /// Saves the changes on the <see cref="PriorityFilterSetting"/> to its corresponding configuration object.
        /// </summary>
        /// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
        /// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return PriorityFilterDataWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}
