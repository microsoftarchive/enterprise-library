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
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// The <see cref="CustomProperty{TProperty}"/> provides custom value storage and is not
    /// built from a <see cref="PropertyDescriptor"/> definition.
    /// </summary>
    /// <remarks>
    /// The <see cref="CustomProperty{TProperty}"/> is used to provide additional properties for a <see cref="ElementViewModel"/> that
    /// are not backed by a property on a <see cref="System.Configuration.ConfigurationElement"/>.
    /// </remarks>
    /// <typeparam name="TProperty">The type for the underlying property value.</typeparam>
    public class CustomProperty<TProperty> : Property
    {
        TypeConverter converter;
        string propertyName;

        /// <summary>
        /// Initializes a <see cref="CustomProperty{TProperty}"/>.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> to use for this property.</param>
        /// <param name="propertyName">The name of this property.</param>
        public CustomProperty(IServiceProvider serviceProvider, string propertyName)
            : this(serviceProvider, TypeDescriptor.GetConverter(typeof(TProperty)), propertyName, new Attribute[0])
        {
        }

        /// <summary>
        /// Initializes a <see cref="CustomProperty{TProperty}"/>.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> to use for this property.</param>
        /// <param name="propertyName">The name of this property.</param>
        /// <param name="attributes">Additional <see cref="Attribute"/> values for this property.</param>
        public CustomProperty(IServiceProvider serviceProvider, string propertyName, params Attribute[] attributes)
            : this(serviceProvider, TypeDescriptor.GetConverter(typeof(TProperty)), propertyName, attributes)
        {
        }


        /// <summary>
        /// Initializes a <see cref="CustomProperty{TProperty}"/>.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> to use for this property.</param>
        /// <param name="converter">The <see cref="TypeConverter"/> to use when converting values for display to or from the user-interface.</param>
        /// <param name="propertyName">The name of this property.</param>
        public CustomProperty(IServiceProvider serviceProvider, TypeConverter converter, string propertyName)
            : this(serviceProvider, converter, propertyName, new Attribute[0])
        {
        }

        /// <summary>
        /// Initializes a <see cref="CustomProperty{TProperty}"/>.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> to use for this property.</param>
        /// <param name="converter">The <see cref="TypeConverter"/> to use when converting values for display to or from the user-interface.</param>
        /// <param name="propertyName">The name of this property.</param>
        /// <param name="attributes">Additional <see cref="Attribute"/> values for this property.</param>
        public CustomProperty(IServiceProvider serviceProvider, TypeConverter converter, string propertyName, params Attribute[] attributes)
            : this(serviceProvider, null, null, converter, propertyName, attributes)
        {
        }

        /// <summary>
        /// Initializes a <see cref="CustomProperty{TProperty}"/>.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> to use for this property.</param>
        /// <param name="propertyDescriptor">The <see cref="PropertyDescriptor"/> for this property.  This is usually <see langword="null"/> for a <see cref="CustomProperty{TProperty}"/>.</param>
        /// <param name="converter">The <see cref="TypeConverter"/> to use when converting values for display to or from the user-interface.</param>
        /// <param name="propertyName">The name of this property.</param>
        /// <param name="attributes">Additional <see cref="Attribute"/> values for this property.</param>
        /// <param name="component">The underlying <see cref="object"/> that provides the underlying .NET property.  This is usually <see langword="null"/> for a <see cref="CustomProperty{TProperty}"/>.</param>
        protected CustomProperty(IServiceProvider serviceProvider, 
            object component, 
            PropertyDescriptor propertyDescriptor, 
            TypeConverter converter,
            string propertyName,
            params Attribute[] attributes) 
            : base(serviceProvider, component, propertyDescriptor, attributes)
        {
            this.converter = converter;
            this.propertyName = propertyName;
        }


        /// <summary>
        /// Gets the <see cref="Type"/> of the property.
        /// </summary>
        public override Type PropertyType
        {
            get
            {
                return typeof(TProperty);
            }
        }

        /// <summary>
        /// Converter that should be used to convert value to and from a string representation.
        /// </summary>
        public override TypeConverter Converter
        {
            get
            {
                return converter;
            }
        }

        /// <summary>
        /// The name of the property.
        /// </summary>
        public override string PropertyName
        {
            get
            {
                return propertyName;
            }
        }

        TProperty value;

        /// <summary>
        /// Sets the underlying, stored value.
        /// </summary>
        /// <remarks>
        /// Once the value is stored, the property is <see cref="Property.Validate()"/>.
        /// 
        /// </remarks>
        /// <param name="value"></param>
        protected override void SetValue(object value)
        {
            this.value = (TProperty)value;
            Validate();
        }

        /// <summary>
        /// Gets the underlying, stored value.
        /// </summary>
        /// <returns></returns>
        protected override object GetValue()
        {
            return value;
        }


        /// <summary>
        /// Gets or sets the underlying, typed value.
        /// </summary>
        public TProperty TypedValue
        {
            get
            {
                return (TProperty)Value;
            }
            set
            {
                Value = value;
            }
        }

    }
}
