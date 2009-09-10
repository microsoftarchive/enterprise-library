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
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Configuration;
using System.Linq;
using System.Text;
using Console.Wpf.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Console.Wpf.ViewModel
{
    public class ConfigurationSourceViewModel
    {
        private readonly IServiceProvider serviceProvider;

        public ConfigurationSourceViewModel(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            Sections = new ObservableCollection<SectionViewModel>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ObservableCollection<SectionViewModel> Sections { get; private set; }


        public void Open(IConfigurationSource configSource)
        {
            var locator = (ConfigurationSectionLocator)serviceProvider.GetService(typeof (ConfigurationSectionLocator));

            foreach (var sectionName in locator.ConfigurationSectionNames)
            {
                ConfigurationSection section = configSource.GetSection(sectionName);
                if (section != null)
                {
                    var sectionContainer = new ServiceContainer(serviceProvider);
                    var sectionViewModel = SectionViewModel.CreateSection(sectionContainer, section);
                    //todo: consider moving into factory or constructor.
                    sectionViewModel.UpdateLayout();
                    Sections.Add(sectionViewModel);
                }
            }
        }
    }
}
