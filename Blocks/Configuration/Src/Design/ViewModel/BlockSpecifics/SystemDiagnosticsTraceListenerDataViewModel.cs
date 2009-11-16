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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    public class SystemDiagnosticsTraceListenerDataViewModel: CollectionElementViewModel
    {
        PropertyDescriptor initDataPropertyDescriptor;

        public SystemDiagnosticsTraceListenerDataViewModel(ElementCollectionViewModel containingCollection, ConfigurationElement thisElement)
            :base(containingCollection, thisElement)
        {
            var systemDiagnosticsProperties = TypeDescriptor.GetProperties(typeof(SystemDiagnosticsTraceListenerData)).OfType<PropertyDescriptor>();
            initDataPropertyDescriptor = systemDiagnosticsProperties.Where(x => x.Name == "InitData").First();
        }

        protected override IEnumerable<Property> GetAllProperties()
        {
            return base.GetAllProperties()
                .Union(new Property[]
                {
                    ContainingSection.CreateElementProperty(this, initDataPropertyDescriptor)
                });
        }
    }
}
