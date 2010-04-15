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
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Converters
{
    /// <summary>
    /// This code supports the Caching Block design-time and is
    /// not inteded to be used directly from your code.
    /// </summary>
    public class BackingStoreReferenceConverter : StringConverter
    {
        private readonly string NoReference = Resources.ReferencePropertyNoReference;
        private readonly string nullBackingStoreName;

        /// <summary>
        /// Initialzies an instance of <see cref="BackingStoreReferenceConverter"/>
        /// </summary>
        /// <param name="nullBackingStoreName"></param>
        public BackingStoreReferenceConverter(string nullBackingStoreName)
        {
            this.nullBackingStoreName = nullBackingStoreName;
            this.NoReference = Resources.ReferencePropertyNoReference;
        }

        /// <summary>
        /// Converts the specified value object to a <see cref="T:System.String"/> object.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Object"/> that represents the converted value.
        /// </returns>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context. 
        ///                 </param><param name="culture">The <see cref="T:System.Globalization.CultureInfo"/> to use. 
        ///                 </param><param name="value">The <see cref="T:System.Object"/> to convert. 
        ///                 </param><exception cref="T:System.NotSupportedException">The conversion could not be performed. 
        ///                 </exception>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            var stringValue = value as string;
            if (string.IsNullOrEmpty(stringValue) || 0 == String.Compare(NoReference, stringValue, StringComparison.OrdinalIgnoreCase))
            {
                return nullBackingStoreName;
            }
            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Converts the given value object to the specified type, using the specified context and culture information.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Object"/> that represents the converted value.
        /// </returns>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context. 
        ///                 </param><param name="culture">A <see cref="T:System.Globalization.CultureInfo"/>. If null is passed, the current culture is assumed. 
        ///                 </param><param name="value">The <see cref="T:System.Object"/> to convert. 
        ///                 </param><param name="destinationType">The <see cref="T:System.Type"/> to convert the <paramref name="value"/> parameter to. 
        ///                 </param><exception cref="T:System.ArgumentNullException">The <paramref name="destinationType"/> parameter is null. 
        ///                 </exception><exception cref="T:System.NotSupportedException">The conversion cannot be performed. 
        ///                 </exception>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            var stringValue = value as string;
            if (destinationType == typeof(string) && (string.IsNullOrEmpty(stringValue) || 0 == string.Compare(nullBackingStoreName, stringValue, StringComparison.OrdinalIgnoreCase)))
            {
                return NoReference;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
