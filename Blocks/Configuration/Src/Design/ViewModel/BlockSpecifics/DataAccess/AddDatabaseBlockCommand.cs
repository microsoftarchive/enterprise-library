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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{

#pragma warning disable 1591
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class AddDatabaseBlockCommand : AddApplicationBlockCommand
    {
        private readonly string DefaultConnectionStringName = Resources.AddDatabaseBlockCommandDefaultConnectionStringName;

        public AddDatabaseBlockCommand(ConfigurationSourceModel configurationSourceModel, AddApplicationBlockCommandAttribute attribute, IUIServiceWpf uiService)
            : base(configurationSourceModel, attribute, uiService)
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

        protected override void InnerExecute(object parameter)
        {
            base.InnerExecute(parameter);

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

#pragma warning restore 1591
}
