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
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls;
using System.Windows;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    public class ConfigurationSourceSectionViewModel : SectionViewModel
    {
        public ConfigurationSourceSectionViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section)
            : base(builder, sectionName, section) 
        {
        }

       

        protected override object CreateBindable()
        {
            var sources = DescendentElements().Where(x => x.ConfigurationType == typeof(NameTypeConfigurationElementCollection<ConfigurationSourceElement, ConfigurationSourceElement>)).First();
            var redirectedSections = DescendentElements().Where(x => x.ConfigurationType == typeof(NamedElementCollection<RedirectedSectionElement>)).First();

            return new HorizontalListViewModel(
                    new HeaderedListViewModel(sources), 
                    new HeaderedListViewModel(redirectedSections));
        }     
    }
}
