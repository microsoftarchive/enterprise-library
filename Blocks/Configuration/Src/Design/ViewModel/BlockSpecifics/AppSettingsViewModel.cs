using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using System.Configuration;

namespace Console.Wpf.ViewModel.BlockSpecifics
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
    }
}
