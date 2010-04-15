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
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{

    /// <summary>
    /// The <see cref="TypeNameProperty"/> represents a property to display <see cref="Type"/> information within the configuration design-time.
    /// </summary>
    public class TypeNameProperty : ElementProperty
    {
        /// <summary>
        /// Initializes a new instance of <see cref="TypeNameProperty"/>.
        /// </summary>
        /// <param name="serviceProvider">The service provider to use in locating services.</param>
        /// <param name="parent">The parent element.</param>
        /// <param name="declaringProperty">The declaring <see cref="PropertyDescriptor"/> for this property.</param>
        public TypeNameProperty(IServiceProvider serviceProvider, ElementViewModel parent, PropertyDescriptor declaringProperty) : base(serviceProvider, parent, declaringProperty)
        {
        }

        /// <summary>
        /// Converter that should be used to convert value to and from a string representation.
        /// </summary>
        /// <value>
        /// Returns a converter that will convert a <see cref="Type"/> to its short type name for display purposes.
        /// </value>
        public override TypeConverter Converter
        {
            get
            {
                return new TypeNameConverter();
            }
        }

        private class TypeNameConverter : TypeConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                if (typeof(string) == destinationType)
                {
                    var type = System.Type.GetType((string)value, false, true);
                    if (type != null)
                    {
                        return type.Name;
                    }
                }

                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }
}
