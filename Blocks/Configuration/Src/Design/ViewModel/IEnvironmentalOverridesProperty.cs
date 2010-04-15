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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// Interface that contains the necessary information needed to environmentally override a property.
    /// </summary>
    public interface IEnvironmentalOverridesProperty
    {
        /// <summary>
        /// <see langword="true"/> if this property meets the requirements to be overridden.
        /// </summary>
        bool SupportsOverride { get; }

        /// <summary>
        /// The name of the attribute which is used to serialize this configuration property in XML.
        /// </summary>
        string PropertyAttributeName { get; }

        /// <summary>
        /// The XPath to the XML element that declares the attribute for this configuration property.
        /// </summary>
        string ContainingElementXPath { get; }

        /// <summary>
        /// The name of the configuration section that contains the property.
        /// </summary>
        string ConfigurationSectionName { get; }

        /// <summary>
        /// The XPath to the XML element that declares the containing configuration section.
        /// </summary>
        string ContainingSectionXPath { get; }

        /// <summary>
        /// The <see cref="TypeConverter"/> that converts the internal overridden value to a string that can be stored in the delta configuration file.<br/>
        /// </summary>
        /// <remarks>
        /// In order to use a default implementation, return an instance of <see cref="Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.EnvironmentalOverrides.DefaultDeltaConfigurationStorageConverter"/>.
        /// </remarks>
        TypeConverter DeltaConfigurationStorageConverter { get; }
    }
}
