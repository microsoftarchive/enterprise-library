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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    public class ConfigurationSourceSectionViewModel : PositionedSectionViewModel
    {
        public ConfigurationSourceSectionViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section)
            : base(builder, sectionName, section) 
        {
            Positioning.PositionCollection("Configuration Sources",
                    typeof(NameTypeConfigurationElementCollection<ConfigurationSourceElement, ConfigurationSourceElement>),
                    typeof(ConfigurationSourceElement),
                    new PositioningInstructions { FixedColumn = 0, FixedRow = 0 });

            Positioning.PositionCollection("Redirected Sections",
                    typeof(NamedElementCollection<RedirectedSectionElement>),
                    typeof(RedirectedSectionElement),
                    new PositioningInstructions { FixedColumn = 1, FixedRow = 0 });
        }

     
    }
}
