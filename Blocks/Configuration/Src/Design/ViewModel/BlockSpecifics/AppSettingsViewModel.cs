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
using Microsoft.Practices.Unity;
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    public class AppSettingsViewModel : PositionedSectionViewModel
    {
        public AppSettingsViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section)
            :base(builder, sectionName, section)
        {
            Positioning.PositionCollection("Application Settings",
                        typeof(KeyValueConfigurationCollection),
                        typeof(KeyValueConfigurationElement),
                        new PositioningInstructions { FixedColumn = 0, FixedRow = 0 });
        }

        public override void BeforeSave(ConfigurationSection sectionToSave)
        {
            AppSettingsSection appSettingsSectionToSave = (AppSettingsSection)sectionToSave;
            appSettingsSectionToSave.Settings.Clear();
            foreach(var keyValueConfigurationElement in base.DescendentElements(x=>x.ConfigurationType == typeof(KeyValueConfigurationElement)))
            {
                string key = (string)keyValueConfigurationElement.Property("Key").Value;
                string value =  (string)keyValueConfigurationElement.Property("Value").Value;
                
                appSettingsSectionToSave.Settings.Add(key, value);
            }
        }
    }
}
