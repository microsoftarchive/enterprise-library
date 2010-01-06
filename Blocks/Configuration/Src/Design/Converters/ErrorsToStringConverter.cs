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
using System.Windows.Controls;
using System.Windows.Data;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Converters
{
    /// <summary>
    /// Converts an <see cref="IEnumerable{ValidationError}"/> to a string.
    /// </summary>
    /// <seealso cref="ValidationError"/>
    [ValueConversion(typeof(ValidationError), typeof(string))]
    public class ErrorsToStringConverter : IValueConverter
    {
        /// <summary>
        /// Converts the <see cref="ValidationError"/> enumerable to a string.
        /// </summary>
        /// <param name="value">The value of <see cref="IEnumerable{ValidationError}"/></param>
        /// <param name="targetType">The target type to convert to.</param>
        /// <param name="parameter">Any conversion parameters, not used.</param>
        /// <param name="culture">Culture info.</param>
        /// <returns>A <see cref="string"/> of the errors joined by <see cref="Environment.NewLine"/></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var errors = value as IEnumerable<ValidationError>;
            if (errors == null) return string.Empty;

            return string.Join(Environment.NewLine, errors.Select(e => e.ErrorContent.ToString()).ToArray());
        }


        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
