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
using System.Windows;
using System.Windows.Data;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Converters
{
    /// <summary>
    /// Converts a <see langword="bool"/> to a <see cref="Visibility"/> value, usually
    /// in a binding expression.
    /// </summary>
    public class BooleanToHiddenConverter : IValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        /// Converts a <see langword="bool"/> to a <see cref="Visibility"/> enumeration value.
        /// </summary>
        /// <returns>
        /// A converted value. If the input <paramref name="value"/> is true, then returns <see cref="Visibility.Visible"/>.
        /// Otherwise, <see cref="Visibility.Hidden"/> is returned.
        /// </returns>
        /// <param name="value">The value produced by the binding source.
        ///                 </param><param name="targetType">The type of the binding target property.
        ///                 </param><param name="parameter">The converter parameter to use.
        ///                 </param><param name="culture">The culture to use in the converter.
        ///                 </param>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool flag = false;
            if (value is bool)
            {
                flag = (bool)value;
            }
            else if (value is bool?)
            {
                bool? nullable = (bool?)value;
                flag = nullable.HasValue ? nullable.Value : false;
            }
            return (flag ? Visibility.Visible : Visibility.Hidden);

        }

        /// <summary>
        /// Converts a <see cref="Visibility"/> value to a <see langword="bool" />
        /// </summary>
        /// <returns>
        /// Returns <see langword="true" /> if <paramref name="value"/> is <see cref="Visibility.Visible"/>.
        /// Otherwise, <see langword="false" /> is returned.
        /// </returns>
        /// <param name="value">The value that is produced by the binding target.
        ///                 </param><param name="targetType">The type to convert to.
        ///                 </param><param name="parameter">The converter parameter to use.
        ///                 </param><param name="culture">The culture to use in the converter.
        ///                 </param>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((value is Visibility) && (((Visibility)value) == Visibility.Visible));
        }

        #endregion
    }
}
