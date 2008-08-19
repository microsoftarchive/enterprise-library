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

using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Filters
{
    /// <summary>
    /// Provides an implementation for <see cref="CustomLogFilterData"/> that
    /// splits policy overrides processing and WMI objects generation, performing approriate logging of 
    /// policy processing errors.
    /// </summary>
    public class CustomLogFilterDataManageabilityProvider
        : CustomProviderDataManageabilityProvider<CustomLogFilterData>
    {
        /// <summary>
        /// The name of the attributes property.
        /// </summary>
        public new const String AttributesPropertyName = CustomProviderDataManageabilityProvider<CustomLogFilterData>.AttributesPropertyName;
        
        /// <summary>
        /// The name of the type property.
        /// </summary>
        public const String TypePropertyName = ProviderTypePropertyName;

        /// <summary>
        /// Initialize a new instance of the <see cref="CustomLogFilterDataManageabilityProvider"/> class
        /// </summary>
        public CustomLogFilterDataManageabilityProvider()
            : base(Resources.FilterPolicyNameTemplate)
        {
            CustomLogFilterDataWmiMapper.RegisterWmiTypes();
        }

        /// <summary>
        /// Creates the <see cref="ConfigurationSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
        protected override void GenerateWmiObjects(CustomLogFilterData configurationObject,
                                                   ICollection<ConfigurationSetting> wmiSettings)
        {
            CustomLogFilterDataWmiMapper.GenerateWmiObjects(configurationObject, wmiSettings);
        }
    }
}