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
using System.Globalization;
using System.Windows.Data;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Converters
{
    /// <summary>
    /// The <see cref="NullToBooleanConverter"/> converts a value that is possibly <see langword="null"/> to a <see cref="bool"/>.
    /// </summary>
    public class NullToBooleanConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value that is possibly <see langword="null"/> to a <see cref="bool"/>.
        /// </summary>
        /// <returns>
        /// Returns <see langword="true"/> if <paramref name="value"/> is <see langword="null"/>.
        /// Otherwise, returns <see langword="false"/>.
        /// </returns>
        /// <param name="value">The value produced by the binding source.
        ///                 </param><param name="targetType">The type of the binding target property.
        ///                 </param><param name="parameter">The converter parameter to use.
        ///                 </param><param name="culture">The culture to use in the converter.
        ///                 </param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        /// <summary>
        /// Converts a value back by always returning <see langword="null"/>.
        /// </summary>
        /// <returns>
        /// Returns <see langword="null"/>.
        /// </returns>
        /// <param name="value">The value that is produced by the binding target.
        ///                 </param><param name="targetType">The type to convert to.
        ///                 </param><param name="parameter">The converter parameter to use.
        ///                 </param><param name="culture">The culture to use in the converter.
        ///                 </param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
