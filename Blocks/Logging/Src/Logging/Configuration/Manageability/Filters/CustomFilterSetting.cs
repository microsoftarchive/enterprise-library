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
    /// Represents an instance of <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.CustomLogFilterData"/>
    /// as an instrumentation class.
    /// </summary>
    [ManagementEntity]
    public partial class CustomFilterSetting : LogFilterSetting
    {
        string[] attributes;
        string filterType;

        /// <summary>
        /// Initialize a new instance of the <see cref="CustomLogFilterData"/> class with the filter configuration,
        /// the name of the filter, the filter type, and the attributes.
        /// </summary>
        /// <param name="sourceElement">The filter configuration.</param>
        /// <param name="name">The name of the filter.</param>
        /// <param name="filterType">The filter type.</param>
        /// <param name="attributes">The filter attributes.</param>
        public CustomFilterSetting(CustomLogFilterData sourceElement,
                                   string name,
                                   string filterType,
                                   string[] attributes)
            : base(sourceElement, name)
        {
            this.filterType = filterType;
            this.attributes = attributes;
        }

        /// <summary>
        /// Gets the attributes for the represented configuration element.
        /// </summary>
        /// <remarks>
        /// The attributes are encoded as an string array of name/value pairs.
        /// </remarks>
        [ManagementConfiguration]
        public string[] Attributes
        {
            get { return attributes; }
            set { attributes = value; }
        }

        /// <summary>
        /// Gets the assembly qualified name of the filter type for the represented configuration element.
        /// </summary>
        [ManagementConfiguration]
        public string FilterType
        {
            get { return filterType; }
            set { filterType = value; }
        }

        /// <summary>
        /// Returns the <see cref="CustomFilterSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <param name="Name">The value for the Name key property.</param>
        /// <returns>The published <see cref="CustomFilterSetting"/> instance specified by the values for the key properties,
        /// or <see langword="null"/> if no such an instance is currently published.</returns>
        [ManagementBind]
        public static CustomFilterSetting BindInstance(string ApplicationName,
                                                       string SectionName,
                                                       string Name)
        {
            return BindInstance<CustomFilterSetting>(ApplicationName, SectionName, Name);
        }

        /// <summary>
        /// Returns an enumeration of the published <see cref="CustomFilterSetting"/> instances.
        /// </summary>
        [ManagementEnumerator]
        public static IEnumerable<CustomFilterSetting> GetInstances()
        {
            return GetInstances<CustomFilterSetting>();
        }

        /// <summary>
        /// Saves the changes on the <see cref="CustomFilterSetting"/> to its corresponding configuration object.
        /// </summary>
        /// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
        /// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return CustomLogFilterDataWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}