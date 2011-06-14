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
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.Practices.EnterpriseLibrary.Logging.Service;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Provides a unified way of converting a <see cref="string"/> to a <see cref="ILoggingServiceFactory"/>.
    /// </summary>
    [ComVisible(false)]
    public class LoggingServiceFactoryEndpointConfigurationNameConverter : TypeConverter
    {
        /// <summary>
        /// Returns whether the type converter can convert an object from the specified type to the type of this converter.
        /// </summary>
        /// <returns>
        /// true if this converter can perform the conversion; otherwise, false.
        /// </returns>
        /// <param name="context">An object that provides a format context.</param><param name="sourceType">The type you want to convert from.</param>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Converts from the specified value to the intended conversion type of the converter.
        /// </summary>
        /// <returns>
        /// The converted value.
        /// </returns>
        /// <param name="context">An object that provides a format context. </param>
        /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo"/> to use as the current culture. </param>
        /// <param name="value">The value to convert to the type of this converter.</param>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var endpointConfigurationName = value as string;
            if (!string.IsNullOrEmpty(endpointConfigurationName))
            {
                LoggingServiceFactory factory = null;
                try
                {
                    factory = new LoggingServiceFactory();
                    factory.EndpointConfigurationName = endpointConfigurationName;
                    return factory;
                }
                catch
                {
                    if (factory != null) factory.Dispose();
                    throw;
                }
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
