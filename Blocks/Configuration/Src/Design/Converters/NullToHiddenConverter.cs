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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Converters
{
    /// <summary>
    /// The <see cref="NullToHiddenConverter"/> converts a possibly <see langword="null"/> <see cref="object"/> to a <see cref="Visibility"/> value.
    /// </summary>
    public class NullToHiddenConverter : IValueConverter
    {
        /// <summary>
        /// Converts a <see cref="object"/> to a <see cref="Visibility"/> value.
        /// </summary>
        /// <returns>
        /// Returns <see cref="Visibility.Visible"/> if <paramref name="value"/> is not null.
        /// Otherwise, returns <see cref="Visibility.Hidden"/>.
        /// </returns>
        /// <param name="value">The value produced by the binding source.
        ///                 </param><param name="targetType">The type of the binding target property.
        ///                 </param><param name="parameter">The converter parameter to use.
        ///                 </param><param name="culture">The culture to use in the converter.
        ///                 </param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value == null ? Visibility.Hidden : Visibility.Visible);
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
