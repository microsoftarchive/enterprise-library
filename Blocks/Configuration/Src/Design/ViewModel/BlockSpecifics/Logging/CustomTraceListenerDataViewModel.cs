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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using System.ComponentModel;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
#pragma warning disable 1591
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class CustomTraceListenerDataViewModel : CollectionElementViewModel
    {
        PropertyDescriptor formatterPropertyDescriptor;

        public CustomTraceListenerDataViewModel(ElementCollectionViewModel containingCollection, ConfigurationElement thisElement)
            :base(containingCollection, thisElement)
        {
            
            var customTracelistenerProperties = TypeDescriptor.GetProperties(typeof(CustomTraceListenerData)).OfType<PropertyDescriptor>();
            formatterPropertyDescriptor = customTracelistenerProperties.Where(x => x.Name == "Formatter").First();
        }


        protected override IEnumerable<Property> GetAllProperties()
        {
            return base.GetAllProperties()
                .Union( new Property[]
                {
                    
                    ContainingSection.CreateProperty<CustomTraceListenerFormatterProperty>( 
                        new DependencyOverride<ElementViewModel>(this), 
                        new DependencyOverride<PropertyDescriptor>(formatterPropertyDescriptor))
                });
        }


        private class CustomTraceListenerFormatterProperty : ElementReferenceProperty
        {
            public CustomTraceListenerFormatterProperty(IServiceProvider serviceProvider, ElementLookup lookup, ElementViewModel parent, PropertyDescriptor declaringProperty)
                : base(serviceProvider, lookup, parent, declaringProperty, new Attribute[] { new ConfigurationPropertyAttribute("formatter") })
            {
            }
        }
    }
#pragma warning restore 1591
}
