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
using Console.Wpf.Tests.VSTS.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Console.Wpf.Tests.VSTS.TestSupport;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.Unity;
using Console.Wpf.Tests.VSTS.DevTests;

namespace Console.Wpf.Tests.VSTS.DevTests.given_data_configuration
{
    public abstract class given_data_configuration : ContainerContext
    {
        protected DesignDictionaryConfigurationSource source;

        protected override void Arrange()
        {
            base.Arrange();

            source = new DesignDictionaryConfigurationSource();

            ConfigurationSourceBuilder sourceBuilder = new ConfigurationSourceBuilder();
            sourceBuilder.ConfigureData().ForDatabaseNamed("connection-string1").ThatIs.ASqlDatabase().WithConnectionString("connectionStringValueXYZ").AsDefault()
                                         .ForDatabaseNamed("connection-string2").ThatIs.AnOracleDatabase().WithConnectionString("OracleConnectionString").WithPackageNamed("x").AndPrefix("XYZ")
                                         .ForDatabaseNamed("connection-string3").ThatIs.AnOracleDatabase().WithConnectionString("OracleConnectionString").WithPackageNamed("x").AndPrefix("XYZ");

            sourceBuilder.UpdateConfigurationWithReplace(source);

            DatabaseSettings dbSettings = (DatabaseSettings)source.GetSection(DatabaseSettings.SectionName);
            dbSettings.ProviderMappings.Add(new DbProviderMapping("dbpro1", "dbtype"));
            dbSettings.ProviderMappings.Add(new DbProviderMapping("dbpro2", "dbtype"));

            source.Remove(DatabaseSettings.SectionName);
            source.Add(DatabaseSettings.SectionName, dbSettings);


            AnnotationService annotationService = Container.Resolve<AnnotationService>();
            ConnectionStringsDecorator.DecorateConnectionStringsSection(annotationService);

            var configurationSection = source.GetSection(DataAccessDesignTime.ConnectionStringSettingsSectionName);
            var configurationSourceModel = Container.Resolve<ConfigurationSourceModel>();
            configurationSourceModel.Load(source);

            databaseSectionViewModel = configurationSourceModel.Sections
                        .Where(x => x.SectionName == DataAccessDesignTime.ConnectionStringSettingsSectionName)
                        .Single();
        }

        protected SectionViewModel databaseSectionViewModel;

    }

    [TestClass]
    public class when_creating_data_section_view_model : given_data_configuration
    {
        [TestMethod]
        public void then_section_model_is_data_section_viewmodel()
        {
            Assert.IsInstanceOfType(databaseSectionViewModel, typeof(DataSectionViewModel));
        }


        [TestMethod]
        public void then_oracle_connection_settings_are_in_second_column()
        {
            var oracleConnections = databaseSectionViewModel.GetGridVisuals().OfType<ElementViewModel>().Where( x=> x.ConfigurationType == typeof(OracleConnectionData));
            Assert.IsTrue(oracleConnections.Any());
            Assert.IsFalse(oracleConnections.Where(x => x.Column != 1).Any());
        }

        [TestMethod]
        public void then_oracle_connection_settings_start_at_row_1()
        {
            var oracleConnections = databaseSectionViewModel.GetGridVisuals().OfType<ElementViewModel>().Where(x => x.ConfigurationType == typeof(OracleConnectionData));
            Assert.IsTrue(oracleConnections.Any());
            Assert.AreEqual(1, oracleConnections.Min(x => x.Row));
        }

        [TestMethod]
        public void then_connection_strings_are_in_first_column()
        {
            var connectionStrings = databaseSectionViewModel.GetDescendentsOfType<ConnectionStringSettings>();
            Assert.IsTrue(connectionStrings.Any());
            Assert.IsFalse(connectionStrings.Where(x => x.Column != 0).Any());
        }

        [TestMethod]
        public void then_connection_strings_start_at_row_1()
        {
            var connectionStrings = databaseSectionViewModel.GetGridVisuals().OfType<ElementViewModel>().Where(x=>x.ConfigurationType == typeof(ConnectionStringSettings));
            Assert.IsTrue(connectionStrings.Any());
            Assert.AreEqual(1, connectionStrings.Min(x => x.Row));
        }

        [TestMethod]
        public void then_connection_strings_header_is_at_row1_column1()
        {
            var connectionStringsHeader = databaseSectionViewModel.GetGridVisuals().Where(x => x.Column == 0 && x.Row == 0).FirstOrDefault() as HeaderViewModel;
            Assert.IsNotNull(connectionStringsHeader);
            Assert.AreEqual("Database Instances", connectionStringsHeader.Name);
        }

        [TestMethod]
        public void then_provider_mappings_are_in_third_column()
        {
            var providerMappings = databaseSectionViewModel.GetGridVisuals().OfType<ElementViewModel>().Where(x => x.ConfigurationType == typeof(DbProviderMapping));
            Assert.IsTrue(providerMappings.Any());
            Assert.IsFalse(providerMappings.Where(x => x.Column != 2).Any());
        }

        [TestMethod]
        public void then_provider_mappings_start_at_row_1()
        {
            var providerMappings = databaseSectionViewModel.GetGridVisuals().OfType<ElementViewModel>().Where(x => x.ConfigurationType == typeof(DbProviderMapping));
            Assert.IsTrue(providerMappings.Any());
            Assert.AreEqual(1, providerMappings.Min(x => x.Row));
        }

        [TestMethod]
        public void then_oracle_connections_header_is_at_row1_column2()
        {
            var mappingsHeader = databaseSectionViewModel.GetGridVisuals().Where(x => x.Column == 1 && x.Row == 0).FirstOrDefault() as HeaderViewModel;
            Assert.IsNotNull(mappingsHeader);
            Assert.AreEqual("Oracle Connections", mappingsHeader.Name);
        }

        [TestMethod]
        public void then_provider_mappings_header_is_at_row1_column3()
        {
            var mappingsHeader = databaseSectionViewModel.GetGridVisuals().Where(x => x.Column == 2 && x.Row == 0).FirstOrDefault() as HeaderViewModel;
            Assert.IsNotNull(mappingsHeader);
            Assert.AreEqual("Custom Databases", mappingsHeader.Name);
        }
        [TestMethod]
        public void then_database_section_view_has_properties_from_data_settings()
        {
            var defaultDatabaseProperty = databaseSectionViewModel.Property("DefaultDatabase");
            Assert.IsNotNull(defaultDatabaseProperty);
        }
    }
}
