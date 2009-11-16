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
using System.Configuration;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Windows;
using System.Windows.Input;
using System.Globalization;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    ///<summary>
    /// A property model from a property discovered on a <see cref="ConfigurationElement"/>.
    ///</summary>
    public class ElementProperty : Property
    {
        private readonly ConfigurationPropertyAttribute configurationPropertyAttribute;
        private readonly PropertyInformation configurationProperty;
        private readonly ElementViewModel declaringElement;

        ///<summary>
        /// Initializes an instance of ElementProperty.
        ///</summary>
        ///<param name="serviceProvider">Service provider used to locate certain services for the configuration system.</param>
        ///<param name="parent">The parent <see cref="ElementViewModel"/> owning the property.</param>
        ///<param name="declaringProperty">The description of the property.</param>
        [InjectionConstructor]
        public ElementProperty(IServiceProvider serviceProvider, ElementViewModel parent, PropertyDescriptor declaringProperty)
            : this(serviceProvider, parent, declaringProperty, new Attribute[0])
        {
        }

        ///<summary>
        /// Initializes an instance of ElementProperty.
        ///</summary>
        ///<param name="serviceProvider">Service provider used to locate certain services for the configuration system.</param>
        ///<param name="parent">The parent <see cref="ElementViewModel"/> owning the property.</param>
        ///<param name="declaringProperty">The description of the property.</param>
        ///<param name="additionalAttributes">Additional attributes made available to the ElementProperty.</param>
        ///<exception cref="ArgumentNullException"></exception>
        public ElementProperty(IServiceProvider serviceProvider, ElementViewModel parent, PropertyDescriptor declaringProperty, IEnumerable<Attribute> additionalAttributes)
            : base(serviceProvider, parent == null ? null : parent.ConfigurationElement, declaringProperty, additionalAttributes)
        {
            if (parent == null) throw new ArgumentNullException("parent");
            declaringElement = parent;

            ConfigurationElement parentElement = parent.ConfigurationElement;

            configurationPropertyAttribute = declaringProperty.Attributes.OfType<ConfigurationPropertyAttribute>().FirstOrDefault();
            if (configurationPropertyAttribute != null)
            {
                configurationProperty = parentElement.ElementInformation.Properties[configurationPropertyAttribute.Name];
            }
        }

        /// <summary>
        /// The name of the configuration element or attribute.
        /// </summary>
        public virtual string ConfigurationName
        {
            get { return configurationPropertyAttribute != null ? configurationPropertyAttribute.Name : string.Empty; }
        }

        ///<summary>
        /// Returns true if the property is required.
        ///</summary>
        public virtual bool IsRequired
        {
            get { return configurationProperty != null ? configurationProperty.IsRequired : false; }
        }

        ///<summary>
        /// The element that contains the property.
        ///</summary>
        public ElementViewModel DeclaringElement
        {
            get { return declaringElement; }
        }

    }
}
