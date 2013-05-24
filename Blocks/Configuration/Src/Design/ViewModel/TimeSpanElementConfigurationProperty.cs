#region license
//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Enterprise Application Block Library
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
#endregion
namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Represents a <see cref="TimeSpan"/> in the Enterprise Library configuration designer.
    /// </summary>
    public class TimeSpanElementConfigurationProperty : DefaultElementConfigurationProperty
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSpanElementConfigurationProperty"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider used to locate certain services for the configuration system.</param>
        /// <param name="parent">The parent <see cref="ElementViewModel"/> that owns the property.</param>
        /// <param name="declaringProperty">The description of the property.</param>
        [InjectionConstructor]
        public TimeSpanElementConfigurationProperty(IServiceProvider serviceProvider, ElementViewModel parent, PropertyDescriptor declaringProperty)
            : base(serviceProvider, parent, declaringProperty, new Attribute[] { new TypeConverterAttribute(typeof(StrictTimeSpanTypeConverter)) })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSpanElementConfigurationProperty"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider used to locate certain services for the configuration system.</param>
        /// <param name="parent">The parent <see cref="ElementViewModel"/> that owns the property.</param>
        /// <param name="declaringProperty">The description of the property.</param>
        /// <param name="additionalAttributes">Additional attributes made available to the property.</param>
        public TimeSpanElementConfigurationProperty(IServiceProvider serviceProvider, ElementViewModel parent, PropertyDescriptor declaringProperty, IEnumerable<Attribute> additionalAttributes)
            : base(serviceProvider, parent, declaringProperty, additionalAttributes.Union(new Attribute[] { new TypeConverterAttribute(typeof(StrictTimeSpanTypeConverter)) }))
        {
        }

        private class StrictTimeSpanTypeConverter : TypeConverter
        {
            private static string[] AllowedFormats =
                new[]
            {
                @"hh\:mm\:ss",
                @"d\.hh\:mm\:ss",
                @"hh\:mm\:ss\.FFFFFFF",
                @"d\.hh\:mm\:ss\.FFFFFFF"
            };

            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return ((sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType));
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value is string)
                {
                    string input = ((string)value).Trim();
                    TimeSpan timeSpan;

                    if (string.Equals(TimeSpanOrInfiniteConverter.Infinite, input, StringComparison.OrdinalIgnoreCase))
                    {
                        return Timeout.InfiniteTimeSpan;
                    }

                    foreach (var format in AllowedFormats)
                    {
                        if (TimeSpan.TryParseExact(input, format, culture, out timeSpan))
                        {
                            return timeSpan;
                        }
                    }

                    throw new FormatException(string.Format(CultureInfo.CurrentCulture, Resources.ConvertInvalidPrimitive, (string)value, "TimeSpan"));
                }

                return base.ConvertFrom(context, culture, value);
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == null)
                {
                    throw new ArgumentNullException("destinationType");
                }

                if (destinationType == typeof(string) && value is TimeSpan)
                {
                    var timeSpan = (TimeSpan)value;

                    if (timeSpan == Timeout.InfiniteTimeSpan)
                    {
                        return TimeSpanOrInfiniteConverter.Infinite;
                    }

                    int formatIndex = (timeSpan.Days == 0 ? 0 : 1) + (timeSpan.Milliseconds == 0 ? 0 : 2);

                    return timeSpan.ToString(AllowedFormats[formatIndex], culture);
                }

                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }
}
