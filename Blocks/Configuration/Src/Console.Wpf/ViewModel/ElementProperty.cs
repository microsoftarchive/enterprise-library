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

namespace Console.Wpf.ViewModel
{
    public class ElementProperty : Property
    {
        private readonly ConfigurationPropertyAttribute configurationPropertyAttribute;
        private readonly PropertyInformation configurationProperty;

        public ElementProperty(IServiceProvider serviceProvider, ElementViewModel parent, PropertyDescriptor declaringProperty)
            : this(serviceProvider, parent, declaringProperty, new Attribute[0])
        {
        }

        public ElementProperty(IServiceProvider serviceProvider, ElementViewModel parent, PropertyDescriptor declaringProperty, IEnumerable<Attribute> additionalAttributes)
            : base(serviceProvider, parent == null ? null : parent.ConfigurationElement, declaringProperty, additionalAttributes)
        {
            if (parent == null) throw new ArgumentNullException("parent");

            ConfigurationElement parentElement = parent.ConfigurationElement;

            configurationPropertyAttribute = declaringProperty.Attributes.OfType<ConfigurationPropertyAttribute>().FirstOrDefault();
            Debug.Assert(configurationPropertyAttribute != null);

            configurationProperty = parentElement.ElementInformation.Properties[configurationPropertyAttribute.Name];
            Debug.Assert(configurationProperty != null);
            
        }

        /// <summary>
        /// The name of the configuration element or attribute.
        /// </summary>
        public virtual string ConfigurationName
        {
            get { return configurationPropertyAttribute != null ? configurationPropertyAttribute.Name : string.Empty; }
        }

        public virtual bool IsRequired
        {
            get { return configurationProperty.IsRequired; }
        }

      
    }


}
