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
        public void then_database_section_view_has_properties_from_data_settings()
        {
            var defaultDatabaseProperty = databaseSectionViewModel.Property("DefaultDatabase");
            Assert.IsNotNull(defaultDatabaseProperty);
        }
    }
}
