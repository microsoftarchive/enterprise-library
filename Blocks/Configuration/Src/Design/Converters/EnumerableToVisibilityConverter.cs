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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Converters
{
    /// <summary>
    /// The <see cref="EnumerableToVisibilityConverter"/> converts an <see cref="IEnumerable"/> to a <see cref="Visibility"/> setting.
    /// </summary>
    public class EnumerableToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a <see cref="IEnumerable"/> to a <see cref="Visibility"/> setting.
        /// </summary>
        /// <returns>
        /// Returns <see cref="Visibility.Visible"/> if there are any items in the <see cref="IEnumerable"/>,
        /// otherwise returns <see cref="Visibility.Collapsed"/>.
        /// </returns>
        /// <param name="value">The value produced by the binding source.
        ///                 </param><param name="targetType">The type of the binding target property.
        ///                 </param><param name="parameter">The converter parameter to use.
        ///                 </param><param name="culture">The culture to use in the converter.
        ///                 </param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumerable = value as IEnumerable;
            if (enumerable == null) return Visibility.Collapsed;

            var enumerator = enumerable.GetEnumerator();
            if (enumerator.MoveNext())
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
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
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
