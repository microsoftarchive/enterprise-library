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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.EnvironmentalOverrides;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// A custom property model that allows a <see cref="System.Configuration.ConfigurationSection"/>'s <see cref="System.Configuration.SectionInformation.RequirePermission"/> property to be modified.
    /// </summary>
    /// <remarks>
    /// <para>Becuase this value is not stored as part of the <see cref="System.Configuration.ConfigurationSection"/>, the <see cref="SectionViewModel"/> is responsible for storing the value of this property when it saved the <see cref="System.Configuration.ConfigurationSection"/> instance.<br/></para>
    /// <para>This property can be overridden using a Configuration Environment and implements <see cref="IEnvironmentalOverridesProperty"/>.<br/></para>
    /// </remarks>
    public class RequirePermissionProperty : CustomProperty<bool>, IEnvironmentalOverridesProperty
    {
        private const string RequirePermissionPropertyXPathFormat = "/configuration/configSections/section[@name='{0}']";

        /// <summary>
        /// Initializes a new instance of <see cref="RequirePermissionProperty"/>.
        /// </summary>
        /// <param name="serviceProvider">A <see cref="IServiceProvider"/> instance that is used to obtain service instances.</param>
        public RequirePermissionProperty(IServiceProvider serviceProvider)
            : base(serviceProvider, new BooleanConverter(), "Require Permission")
        {
        }

        /// <summary>
        /// Gets the XPath to the XML element that declares the attribute for this configuration property.
        /// </summary>
        /// <value>
        /// The XPath to the XML element that declares the attribute for this configuration property.
        /// </value>
        public string ContainingElementXPath
        {
            get { return string.Format(CultureInfo.InvariantCulture, RequirePermissionPropertyXPathFormat, ContainingSection.SectionName); }
        }

        /// <summary>
        /// Gets whether this property allows itself to be overwritten.<br/>
        /// </summary>
        /// <value>
        /// Always returns <see langword="true"/>.
        /// </value>
        public bool SupportsOverride
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the name of the attribute which is used to serialize this configuration property in XML.
        /// </summary>
        /// <value>
        /// Always returns 'requirePermission'.
        /// </value>
        public string PropertyAttributeName
        {
            get { return "requirePermission"; }
        }

        /// <summary>
        /// Gets the name of the configuration section that contains the property.
        /// </summary>
        /// <value>
        /// The name of the configuration section that contains the property.
        /// </value>
        public string ConfigurationSectionName
        {
            get { return ContainingSection.SectionName; }
        }

        /// <summary>
        /// Gets the XPath to the XML element that declares the containing configuration section.
        /// </summary>
        /// <value>
        /// The XPath to the XML element that declares the containing configuration section.
        /// </value>
        public string ContainingSectionXPath
        {
            get { return ContainingSection.Path; }
        }

        /// <summary>
        /// Gets the <see cref="TypeConverter"/> that converts the internal overridden value to a string that can be stored in the delta configuration file.<br/>
        /// </summary>
        /// <value>
        /// The <see cref="TypeConverter"/> that converts the internal overridden value to a string that can be stored in the delta configuration file.<br/>
        /// </value>
        public TypeConverter DeltaConfigurationStorageConverter
        {
            get { return new RequirePermissionPropertyDeltaConfigurationStorageConverter(); }
        }

        private class RequirePermissionPropertyDeltaConfigurationStorageConverter : DefaultDeltaConfigurationStorageConverter
        {
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                string converted = (string) base.ConvertFrom(context, culture, value);
                return converted.ToLower(culture);
            
            }
        }
    }
}
