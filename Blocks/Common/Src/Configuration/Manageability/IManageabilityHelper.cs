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


namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
    /// <summary>
    /// Defines a helper for manageability configuration.
    /// </summary>
    public interface IManageabilityHelper
    {
        /// <summary>
        /// Updates configuration management from the given configuration.
        /// </summary>
        /// <param name="configurationAccessor">
        /// The accessor for the configuration.
        /// </param>
        void UpdateConfigurationManageability(IConfigurationAccessor configurationAccessor);


        /// <summary>
        /// Updates configuration management from the given configuration in the given section.
        /// </summary>
        /// <param name="configurationAccessor">
        /// The accessor for the configuration.
        /// </param>
        /// <param name="sectionName">
        /// The section to update.
        /// </param>
        void UpdateConfigurationSectionManageability(IConfigurationAccessor configurationAccessor,
                                                     string sectionName);
    }
}
