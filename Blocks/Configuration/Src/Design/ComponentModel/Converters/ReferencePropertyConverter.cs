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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Converters
{
    /// <summary>
    /// <see cref="TypeConverter"/> implementation that converts an empty string to a user friendly display name.<br/>
    /// </summary>
    /// <remarks>
    /// This <see cref="TypeConverter"/> is used in the <see cref="ElementReferenceProperty"/> class, for non-required references.
    /// </remarks>
    public class ReferencePropertyConverter : StringConverter
    {
        private readonly string NoReference;

        /// <summary>
        /// Initializes a new instance of <see cref="ReferencePropertyConverter"/>.
        /// </summary>
        public ReferencePropertyConverter()
        {
            NoReference = Resources.ReferencePropertyNoReference;
        }

        /// <summary>
        /// Converts the user friendly display name, retrieved from <see cref="ConvertTo"/> to an empty string.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="culture">The <see cref="System.Globalization.CultureInfo"/> to use for conversion.</param>
        /// <param name="value">The value to convert</param>
        /// <returns>If the <paramref name="value"/> is the user friendly display name, retrieved from <see cref="ConvertTo"/>, an empty string; Otherwise the string representation of <paramref name="value"/>.</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string && 0 == String.Compare(NoReference, (string)value, StringComparison.OrdinalIgnoreCase))
            {
                return string.Empty;
            }
            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Converts an empty string to an user friendly display name.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="culture">The <see cref="System.Globalization.CultureInfo"/> to use for conversion.</param>
        /// <param name="value">The value to convert</param>
        /// <param name="destinationType">The <see cref="Type"/> to which <paramref name="value"/> should be converted.</param>
        /// <returns>If <paramref name="value"/> is an empty string, an user friendly display name; otherwise the converted <paramref name="value"/>.</returns>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && string.IsNullOrEmpty((string)value))
            {
                return NoReference;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
