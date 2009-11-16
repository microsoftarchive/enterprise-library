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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    public class ConfigurationProperty : ElementProperty
    {
        [InjectionConstructor]
        public ConfigurationProperty(IServiceProvider serviceProvider, ElementViewModel parent, PropertyDescriptor declaringProperty)
            : base(serviceProvider, parent, declaringProperty)
        {
        }

        public ConfigurationProperty(IServiceProvider serviceProvider, ElementViewModel parent, PropertyDescriptor declaringProperty, IEnumerable<Attribute> additionalAttributes)
            : base(serviceProvider, parent, declaringProperty, additionalAttributes)
        {
        }


        protected override object GetValue()
        {
            var element = this.DeclaringElement.ConfigurationElement;
            var elementProperty = element.ElementInformation.Properties[base.ConfigurationName];
            return elementProperty.Value;
        }

        protected override void SetValue(object value)
        {
            var element = this.DeclaringElement.ConfigurationElement;
            var elementProperty = element.ElementInformation.Properties[base.ConfigurationName];
            elementProperty.Value = value;

            OnPropertyChanged("Value");
        }


        public override bool ReadOnly
        {
            get
            {
                return false;
            }
        }
    }
}
