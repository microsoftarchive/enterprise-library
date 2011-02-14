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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using System.Configuration;
using Microsoft.Practices.Unity.Configuration.Design.ViewModel;
using System.ComponentModel;

namespace Microsoft.Practices.Unity.Configuration.Design
{
    public class PropertyElementViewModel : InjectionMemberElementViewModel
    {
        ValueElementProperty propertyValueProperty;

        public PropertyElementViewModel(ElementCollectionViewModel containingCollection, ConfigurationElement thisElement)
            : base(containingCollection, thisElement)
        {
            var valueProperty = TypeDescriptor.GetProperties(typeof(PropertyElement))
                                              .Cast<PropertyDescriptor>()
                                              .Where(x => x.Name == "Value")
                                              .First();

            propertyValueProperty = ContainingSection.CreateProperty<ValueElementProperty>(
                                            new DependencyOverride<ElementViewModel>(this),
                                            new DependencyOverride<PropertyDescriptor>(valueProperty));
        }

        protected override IEnumerable<Property> GetAllProperties()
        {

            return base.GetAllProperties().Union(
                    new Property[] { 
                        propertyValueProperty
                    });
        }

        protected override object CreateBindable()
        {
            return new HorizontalColumnBindingLayout(this, 2);
        }
    }
}
