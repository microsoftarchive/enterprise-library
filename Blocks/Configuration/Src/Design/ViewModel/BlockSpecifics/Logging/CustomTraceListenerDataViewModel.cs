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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
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
                    ContainingSection.CreateElementProperty(this, formatterPropertyDescriptor)
                });
        }
    }
}
