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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Converters
{
    /// <summary>
    /// <see cref="TypeConverter"/> implementation that converts known configuration section names to application block names.<br/>
    /// </summary>
    /// <remarks>
    /// This <see cref="TypeConverter"/> implementation is used for the <see cref="Microsoft.Practices.EnterpriseLibrary.Common.Configuration.RedirectedSectionElement"/>'s Name property.
    /// </remarks>
    public class RedirectedSectionNameConverter : StringConverter
    {
        private readonly Dictionary<string, string> sectionDisplayNames;

        /// <summary>
        /// Initializes a new instance of <see cref="RedirectedSectionNameConverter"/>.
        /// </summary>
        public RedirectedSectionNameConverter()
        {
            sectionDisplayNames = new Dictionary<string, string>();
            sectionDisplayNames.Add(DataAccessDesignTime.ConnectionStringSettingsSectionName, Resources.ConnectionStringsDisplayName);
            sectionDisplayNames.Add(OracleConnectionSettings.SectionName, Resources.OracleConnectionStringsDisplayName);
            sectionDisplayNames.Add(DatabaseSettings.SectionName, Resources.CustomDatabaseSettingsDisplayName);
            sectionDisplayNames.Add(ExceptionHandlingSettings.SectionName, Resources.ExceptionHandlingSettingsDisplayName);
            sectionDisplayNames.Add(LoggingSettings.SectionName, Resources.LoggingSettingsDisplayName);
            sectionDisplayNames.Add(PolicyInjectionSettings.SectionName, Resources.PolicyInjectionSettingsDisplayName);
            sectionDisplayNames.Add(ValidationSettings.SectionName, Resources.ValidationSettingsDisplayName);
        }

        /// <summary>
        /// Converts an application block name to a configuration section name.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="culture">The <see cref="System.Globalization.CultureInfo"/> to use for conversion.</param>
        /// <param name="value">The value to convert</param>
        /// <returns>If the <paramref name="value"/> is an application block name, the associated configuraion section name; Otherwise the string representation of <paramref name="value"/>.</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                var entry = sectionDisplayNames.Where(x => string.Equals(x.Value, (string)value, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                if (entry.Key != null)
                {
                    return entry.Key;
                }
            }
            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Converts <paramref name="value"/> to an application block name.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="culture">The <see cref="System.Globalization.CultureInfo"/> to use for conversion.</param>
        /// <param name="value">The value to convert</param>
        /// <param name="destinationType">The <see cref="Type"/> to which <paramref name="value"/> should be converted.</param>
        /// <returns>If the <paramref name="value"/> is a known configuration section name, the associates application block name; Otherwise the converted <paramref name="value"/>.</returns>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            string displayValue;
            if (destinationType == typeof(string) && sectionDisplayNames.TryGetValue((string)value, out displayValue))
            {
                return displayValue;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>
        /// Returns a collection of standard values from the default context for the data type this type converter is designed for.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <returns>A collection of application block names.</returns>
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(sectionDisplayNames.Values);
        }

        /// <summary>
        /// Returns whether the collection of standard values returned from <see cref="System.ComponentModel.TypeConverter.GetStandardValues()"/> is an exclusive list of possible values, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <returns>Always returns <see langword="false"/>.</returns>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        /// <summary>
        /// Returns whether this object supports a standard set of values that can be picked from a list, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <returns>Always returns <see langword="false"/>.</returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}
