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
using System.ComponentModel;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.Unity.Configuration.Design.ComponentModel;
using Microsoft.Practices.Unity.Configuration.Design.ViewModel;

namespace Microsoft.Practices.Unity.Configuration.Design
{
    public class ParameterElementViewModel : CollectionElementViewModel
    {
        ValueElementProperty parameterValueProperty;

        public ParameterElementViewModel(ElementCollectionViewModel containingCollection, ConfigurationElement thisElement)
            : base(containingCollection, thisElement)
        {
            var valueProperty = TypeDescriptor.GetProperties(typeof(ParameterElement))
                                              .Cast<PropertyDescriptor>()
                                              .Where(x => x.Name == "Value")
                                              .First();

            parameterValueProperty = ContainingSection.CreateProperty<ValueElementProperty>(
                                            new DependencyOverride<ElementViewModel>(this),
                                            new DependencyOverride<PropertyDescriptor>(valueProperty));
        }

       
        protected override object CreateBindable()
        {
            return new HorizontalColumnBindingLayout(this, 3);
        }

        protected override IEnumerable<Property> GetAllProperties()
        {

            return base.GetAllProperties().Union(
                    new Property[] { 
                        parameterValueProperty
                    });
        }
    }
}
