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

namespace Console.Wpf.ViewModel.BlockSpecifics
{
    public class ConfigurationSourceSectionViewModel : SectionViewModel
    {
        public ConfigurationSourceSectionViewModel(IServiceProvider serviceProvider, ConfigurationSection section)
            :base(serviceProvider, section) 
        {
        }

        public ConfigurationSourceSectionViewModel(IServiceProvider serviceProvider, ConfigurationSection section, IEnumerable<Attribute> additionalAttributes)
            : base(serviceProvider, section, additionalAttributes) 
        {
        }
    }
}
