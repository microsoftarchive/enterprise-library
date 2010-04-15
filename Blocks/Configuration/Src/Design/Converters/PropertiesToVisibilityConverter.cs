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
using System.Globalization;
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using System.Windows;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Converters
{
    /// <summary>
    /// The <see cref="PropertiesToVisibilityConverter"/> converts an <see cref="IEnumerable{ElementProperty}"/> to a <see cref="Visibility"/> value.
    /// </summary>
    public class PropertiesToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts an <see cref="IEnumerable{ElementProperty}"/> to a <see cref="Visibility"/> value.
        /// </summary>
        /// <returns>
        /// Returns <see cref="Visibility.Visible"/> if <paramref name="value"/> is an <see cref="IEnumerable{ElementProperty}"/>
        /// that has at least one <see cref="ElementProperty"/> with <see cref="Property.Hidden"/> that is <see langword="false"/>.
        /// Otherwise, returns <see cref="Visibility.Collapsed"/>.
        /// </returns>
        /// <param name="value">The value produced by the binding source.
        ///                 </param><param name="targetType">The type of the binding target property.
        ///                 </param><param name="parameter">The converter parameter to use.
        ///                 </param><param name="culture">The culture to use in the converter.
        ///                 </param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var collection = value as IEnumerable;
            if (collection != null)
            {
                var enumerator = collection.GetEnumerator();
                if (enumerator.MoveNext())
                {
                    var propertyModel = enumerator.Current as ElementProperty;
                    return propertyModel != null && AllPropertiesAreHidden(collection) ? Visibility.Collapsed : Visibility.Visible;
                }
            }

            return Visibility.Collapsed;
        }

        private static bool AllPropertiesAreHidden(IEnumerable propertyCollection)
        {
            var properties = propertyCollection as IEnumerable<ElementProperty>;
            if (properties != null)
            {
                foreach (var property in properties)
                {
                    if (!property.Hidden)
                        return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Not implemented, always returns <see langword="null"/>.
        /// </summary>
        /// <returns>
        /// Returns <see langword="null"/>
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
