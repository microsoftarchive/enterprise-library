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
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.EnvironmentalOverrides
{
#pragma warning disable 1591
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// <br/>
    /// Default <see cref="TypeConverter"/> implementation for storing values in a delta configuration file.
    /// </summary>
    /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.IEnvironmentalOverridesProperty"/>
    public class DefaultDeltaConfigurationStorageConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return (string)Convert.ChangeType(value, typeof(string), culture);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            Guard.ArgumentNotNull(destinationType, "destinationType");

            string valueString = value as string;
            if (destinationType.IsEnum)
            {
                return Enum.Parse(destinationType, valueString);
            }
            return Convert.ChangeType(value, destinationType, culture);
        }
    }

#pragma warning restore 1591
}
