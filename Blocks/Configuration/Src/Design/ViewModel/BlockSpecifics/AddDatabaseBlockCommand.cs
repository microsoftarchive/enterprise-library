using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Console.Wpf.ViewModel.Commands;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Manageability;

namespace Console.Wpf.ViewModel.BlockSpecifics
{
    public class AddDatabaseBlockCommand : AddApplicationBlockCommand
    {
        public AddDatabaseBlockCommand(ConfigurationSourceModel configurationSourceModel, AddApplicationBlockCommandAttribute attribute)
            : base(configurationSourceModel, attribute)
        {
        }

        protected override System.Configuration.ConfigurationSection CreateConfigurationSection()
        {
            
            return new ConnectionStringsSection()
            {
                ConnectionStrings = 
                {{
                    new ConnectionStringSettings
                    {
                        Name = "Connection String",
                        ConnectionString = @"Database=Database;Server=(local)\SQLEXPRESS;Integrated Security=SSPI",
                        ProviderName = "System.Data.SqlClient"
                    }
                }}
            };
        }
    }
}
