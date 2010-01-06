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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    public class AddDatabaseBlockCommand : AddApplicationBlockCommand
    {
        private readonly string DefaultConnectionStringName = "Connection String";

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
                        Name = DefaultConnectionStringName,
                        ConnectionString = @"Database=Database;Server=(local)\SQLEXPRESS;Integrated Security=SSPI",
                        ProviderName = "System.Data.SqlClient"
                    }
                }}
            };
        }

        public override void Execute(object parameter)
        {
            base.Execute(parameter);

            if (AddedSection != null)
            {
                var defaultDatabaseProperty = AddedSection.Property("DefaultDatabase");
                if (defaultDatabaseProperty != null)
                {
                    defaultDatabaseProperty.BindableProperty.BindableValue = DefaultConnectionStringName;
                }
            }
        }
    }
}
