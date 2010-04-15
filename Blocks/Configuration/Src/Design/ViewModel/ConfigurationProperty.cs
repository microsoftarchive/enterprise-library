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
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// A property model that allows a <see cref="System.Configuration.ConfigurationElement"/>'s to be set directly (bypassing the CLR property).
    /// </summary>
    /// <remarks>
    /// This property model can be used to expose a <see cref="System.Configuration.ConfigurationElement"/>'s property as a settable property, even though the CLR property only implements a get-acessor.
    /// </remarks>
    public class ConfigurationProperty : ElementProperty
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ConfigurationProperty"/>
        /// </summary>
        ///<param name="serviceProvider">Service provider used to locate services for the configuration system.</param>
        ///<param name="parent">The parent <see cref="ElementViewModel"/> owning the property.</param>
        ///<param name="declaringProperty">The description of the property.</param>
        [InjectionConstructor]
        public ConfigurationProperty(IServiceProvider serviceProvider, ElementViewModel parent, PropertyDescriptor declaringProperty)
            : base(serviceProvider, parent, declaringProperty, new Attribute[]{new DesignTimeReadOnlyAttribute(false)})
        {
        }

        /// <summary>
        /// Gets the underlying, stored value directly from the <see cref="System.Configuration.ConfigurationElement"/>.
        /// </summary>
        /// <returns>
        /// The underlying, stored value directly from the <see cref="System.Configuration.ConfigurationElement"/>.
        /// </returns>
        protected override object GetValue()
        {
            var element = this.DeclaringElement.ConfigurationElement;
            var elementProperty = element.ElementInformation.Properties[base.ConfigurationName];
            return elementProperty.Value;
        }

        /// <summary>
        /// Sets the underlying, stored value directly to the <see cref="System.Configuration.ConfigurationElement"/>.
        /// </summary>
        /// <remarks>
        /// Once the value is stored, the property is <see cref="Property.Validate()"/>.
        /// </remarks>
        protected override void SetValue(object value)
        {
            var element = this.DeclaringElement.ConfigurationElement;
            var elementProperty = element.ElementInformation.Properties[base.ConfigurationName];
            elementProperty.Value = value;

            OnPropertyChanged("Value");
        }
    }
}
