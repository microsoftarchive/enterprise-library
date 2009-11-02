using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Console.Wpf.ViewModel.Commands;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Console.Wpf.ViewModel.BlockSpecifics
{
    public class AddConfigurationSourcesBlockCommand : AddApplicationBlockCommand
    {
        public AddConfigurationSourcesBlockCommand(ConfigurationSourceModel configurationSourceModel, AddApplicationBlockCommandAttribute attribute)
            : base(configurationSourceModel, attribute)
        {

        }
        protected override System.Configuration.ConfigurationSection CreateConfigurationSection()
        {
            return new ConfigurationSourceSection
            {
                SelectedSource = "System Configuration Source",
                Sources =
                {{
                     new SystemConfigurationSourceElement
                     {
                         Name = "System Configuration Source"
                     }
                }}
            };
        }
    }
}
