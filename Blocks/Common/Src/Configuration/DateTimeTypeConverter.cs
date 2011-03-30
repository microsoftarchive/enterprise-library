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
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Represents a configuration converter that converts most strings to <see cref="DateTime"/>.
    /// </summary>
    public class DateTimeTypeConverter : TypeConverter
    {
        private const string EnUsCulture = "en-US";

        private const string DefaultUniversalFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";

        /// <summary>
        /// Returns the string representation of the passed in DateTime.
        /// </summary>
        /// <param name="context">The container representing this System.ComponentModel.TypeDescriptor.</param>
        /// <param name="culture">Culture info for assembly</param>
        /// <param name="value">Value to convert.</param>
        /// <param name="destinationType">Type to convert to.</param>
        /// <returns>The DateTime as string with the following format : yyyy'-'MM'-'dd'T'HH':'mm':'ss.</returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value != null)
            {
                DateTime dateTimeValue = value is DateTime ? (DateTime)value : new DateTime();

                if (dateTimeValue == DateTime.MinValue)
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.ExceptionCanNotConvertDateTime));
                }

                return dateTimeValue.ToString(DefaultUniversalFormat);
            }

            return null;
        }


        /// <summary>
        /// Returns a DateTime based on the string passed in as data.
        /// </summary>
        /// <param name="context">The container representing this System.ComponentModel.TypeDescriptor.</param>
        /// <param name="culture">Culture info for assembly.</param>
        /// <param name="value">Data to convert.</param>
        /// <returns>The DateTime value.</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string dateTimeString = (string) value;

            if (!string.IsNullOrEmpty(dateTimeString))
            {
                DateTime dateTimeValue;

                if (!DateTime.TryParse(dateTimeString, new CultureInfo(EnUsCulture), DateTimeStyles.AllowWhiteSpaces,
                                       out dateTimeValue))
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                                                              Resources.ExceptionUnsupportedDateTimeFormat,
                                                              dateTimeString));
                }

                return dateTimeValue;
            }
            return null;
        }
    }
}
