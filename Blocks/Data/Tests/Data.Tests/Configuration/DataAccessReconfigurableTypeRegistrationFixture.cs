//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests.Configuration
{
    public abstract class UpdatedConfigurationSourceContext : ArrangeActAssert
    {
        protected UnityContainerConfigurator containerConfigurator;
        protected UnityContainer container;
        protected ConfigurationSourceUpdatable updatableConfigurationSource;
        protected DatabaseSettings databaseSettings;
        protected ConnectionStringsSection connectionStringsSection;

        protected override void Arrange()
        {
            updatableConfigurationSource = new ConfigurationSourceUpdatable();

            connectionStringsSection = new ConnectionStringsSection();
            connectionStringsSection.ConnectionStrings.Add(
                new ConnectionStringSettings
                {
                    Name = "default connection",
                    ConnectionString = "connection string",
                    ProviderName = "System.Data.OracleClient"
                });

            connectionStringsSection.ConnectionStrings.Add(
                new ConnectionStringSettings
                {
                    Name = "other connection",
                    ConnectionString = "connection string",
                    ProviderName = "System.Data.SqlClient"
                });

            databaseSettings = new DatabaseSettings { DefaultDatabase = "default connection" };

            updatableConfigurationSource.Add(null, "connectionStrings", connectionStringsSection);
            updatableConfigurationSource.Add(null, DatabaseSettings.SectionName, databaseSettings);

            container = new UnityContainer();
            containerConfigurator = new UnityContainerConfigurator(container);
            EnterpriseLibraryContainer.ConfigureContainer(containerConfigurator, updatableConfigurationSource);

        }

        protected override void Act()
        {
            updatableConfigurationSource.DoSourceChanged(new string[] { "connectionStrings" });
        }

    }

    [TestClass]
    public class WhenDefaultDatabaseChanges : UpdatedConfigurationSourceContext
    {
        protected override void Act()
        {
            databaseSettings.DefaultDatabase = "other connection";

            base.Act();
        }

        [TestMethod]
        public void ThenNewlyRetrievedInstanceWillReflectConnectionSring()
        {
            Database db = container.Resolve<Database>();
            Assert.IsInstanceOfType(db, typeof(SqlDatabase));

        }
    }
    [TestClass]
    public class WhenProviderNameChanges: UpdatedConfigurationSourceContext
    {
        protected override void Act()
        {
            connectionStringsSection.ConnectionStrings.Cast<ConnectionStringSettings>()
                .Where(x => x.Name == "default connection")
                .First()
                .ProviderName = "System.Data.Odbc";

            base.Act();
        }

        [TestMethod]
        public void ThenNewlyRetrievedInstanceWillReflectProviderType()
        {
            Database db = container.Resolve<Database>();
            Assert.IsInstanceOfType(db, typeof(GenericDatabase));
            
        }
    }

}
