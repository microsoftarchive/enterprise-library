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
using System.Windows.Data;
using System.Windows;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Converters
{
    /// <summary>
    /// The <see cref="InverseBooleanToCollapsedConverter"/> converts a <see cref="bool"/> to <see cref="Visibility.Visible"/>
    /// if a value is <see langword="false" />
    /// </summary>
    public class InverseBooleanToCollapsedConverter : IValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        /// Converts a <see cref="bool"/> value to a <see cref="Visibility"/> value.
        /// </summary>
        /// <returns>
        /// Returns <see cref="Visibility.Visible"/> if <paramref name="value"/> is <see langword="false" />.
        /// Otherwise, returns <see cref="Visibility.Collapsed"/>
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
            return (!flag ? Visibility.Visible : Visibility.Collapsed);
        }


        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value that is produced by the binding target.
        ///                 </param><param name="targetType">The type to convert to.
        ///                 </param><param name="parameter">The converter parameter to use.
        ///                 </param><param name="culture">The culture to use in the converter.
        ///                 </param>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
