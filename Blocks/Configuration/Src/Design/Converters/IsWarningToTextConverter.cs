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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Converters
{
    /// <summary>
    /// The <see cref="IsWarningToTextConverter"/> converts a <see cref="bool"/> to a warning or error display string.
    /// </summary>
    /// <remarks>
    /// This converter is used in the display of <see cref="ValidationResult"/> to display an appropriate
    /// string indicating a validation error or warning.
    /// </remarks>
    public class IsWarningToTextConverter : IValueConverter
    {
        /// <summary>
        /// Converts a <see cref="bool"/> to a warning or error display string.
        /// </summary>
        /// <returns>
        /// Returns a Warning display string if <paramref name="value"/> is <see langword="true" />.
        /// Otherwise, returns an Error display string.
        /// </returns>
        /// <param name="value">The value produced by the binding source.
        ///                 </param><param name="targetType">The type of the binding target property.
        ///                 </param><param name="parameter">The converter parameter to use.
        ///                 </param><param name="culture">The culture to use in the converter.
        ///                 </param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(string)) return null;
            if (value == null) return null;
            if (value.GetType() != typeof(bool)) return null;

            bool isWarning = (bool)value;
            return isWarning ? Resources.WarningToTextConverterWarning : Resources.WarningToTextConverterError;
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
