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
using System.Linq;
using Console.Wpf.Tests.VSTS.BlockSpecific.Logging;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Wizard;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.LogExceptionsToDbWizard.given_prior_database_configuration
{
    public abstract class DatabaseContext : NewConfigurationSourceModelContext
    {
        protected override void Arrange()
        {
            base.Arrange();

            var builder = new ConfigurationSourceBuilder();
            builder.ConfigureData()
                .ForDatabaseNamed("SomeOracleDatabase")
                    .ThatIs.AnOracleDatabase()
                    .WithConnectionString("SomeOracleConnectionString")
                .ForDatabaseNamed("SomeOleDbDatabase")
                    .ThatIs.AnOleDbDatabase()
                    .WithConnectionString("Some OleDb ConnectionString")
                    .AsDefault();

            var source = new DictionaryConfigurationSource();
            builder.UpdateConfigurationWithReplace(source);

            var configSourceModel = Container.Resolve<ConfigurationSourceModel>();
            configSourceModel.AddSection(DatabaseSettings.SectionName, source.GetSection(DatabaseSettings.SectionName));
            ConnectionStringSection = configSourceModel.AddSection("connectionStrings", source.GetSection("connectionStrings"));
        }

        protected SectionViewModel ConnectionStringSection { get; private set; }
    }

    public abstract class DatabaseWithSelectedConnectionContext : DatabaseContext
    {
        protected SelectDatabaseStep step;
        protected ElementViewModel connectionStringSettings;

        protected override void Arrange()
        {
            base.Arrange();

            step = Container.Resolve<SelectDatabaseStep>();
            connectionStringSettings = ConnectionStringSection.DescendentConfigurationsOfType<ConnectionStringSettings>().First();
        }
    }

    [TestClass]
    public class when_step_properties_are_complete : DatabaseWithSelectedConnectionContext
    {
        protected override void Act()
        {
            base.Act();

            step.ConnectionString.Value = "a";
            step.DatabaseProvider.Value = "b";
            step.Name.Value = "c";
        }

        [TestMethod]
        public void then_step_is_valid()
        {
            Assert.IsTrue(step.IsValid);
        }
    }

    [TestClass]
    public class when_step_properties_are_incomplete : DatabaseWithSelectedConnectionContext
    {
        protected override void Act()
        {
            base.Act();

            step.ConnectionString.Value = "a";
            step.Name.Value = "c";
            step.DatabaseProvider.Value = null;
        }

        [TestMethod]
        public void then_step_is_invalid()
        {
            Assert.IsFalse(step.IsValid);
        }
    }

    [TestClass]
    public class when_existing_connection_is_selected : DatabaseWithSelectedConnectionContext
    {
        [TestMethod]
        public void then_the_names_suggested_values_are_loaded()
        {
            Assert.IsTrue(step.Name.HasSuggestedValues);
        }

        [TestMethod]
        public void then_the_providers_suggested_values_are_loaded()
        {
            Assert.IsTrue(step.DatabaseProvider.HasSuggestedValues);
        }

        [TestMethod]
        public void then_the_default_database_is_selected()
        {
            Assert.AreEqual("SomeOleDbDatabase", step.Name.Value);
        }
    }

    [TestClass]
    public class when_picking_existing_connection : DatabaseWithSelectedConnectionContext
    {
        protected override void Act()
        {
            step.Name.Value = connectionStringSettings.Name;
        }

        [TestMethod]
        public void then_the_other_properties_are_read_only()
        {
            Assert.IsTrue(step.ConnectionString.ReadOnly);
            Assert.IsTrue(step.DatabaseProvider.ReadOnly);
        }

        [TestMethod]
        public void then_connection_string_value_set_to_associated_value()
        {
            Assert.AreEqual(connectionStringSettings.Property("ConnectionString").Value,
                step.ConnectionString.Value);
        }

        [TestMethod]
        public void then_provider_name_value_is_set_to_associated_value()
        {
            Assert.AreEqual(connectionStringSettings.Property("ProviderName").Value,
                            step.DatabaseProvider.Value);
        }
    }
}
