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
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.Tests.VSTS.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Console.Wpf.Tests.VSTS.DevTests;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;

namespace Console.Wpf.Tests.VSTS.BlockSpecific.Data.given_data_configuration
{
    [TestClass]
    public class when_saving_empty_data_configuration_section : ContainerContext
    {
        DesignDictionaryConfigurationSource source;

        protected override void Act()
        {
            var section = new ConnectionStringsSection();
            source = new DesignDictionaryConfigurationSource();
            source.Add("connectionStrings", section);

            var configurationSection = source.GetSection(DataAccessDesignTime.ConnectionStringSettingsSectionName);
            var configurationSourceModel = Container.Resolve<ConfigurationSourceModel>();
            configurationSourceModel.Load(source);

            var databaseSectionViewModel = configurationSourceModel.Sections
                .Where(x => x.SectionName == DataAccessDesignTime.ConnectionStringSettingsSectionName)
                .Single();

            databaseSectionViewModel.Save(source);
        }


        [TestMethod]
        public void then_oracle_section_is_not_saved()
        {
            Assert.IsNull(source.GetSection(OracleConnectionSettings.SectionName));
        }

        [TestMethod]
        public void then_connectionstrings_section_is_saved()
        {
            Assert.IsNotNull(source.GetSection("connectionStrings"));
        }

        [TestMethod]
        public void then_data_section_is_not_saved()
        {
            Assert.IsNull(source.GetSection(DatabaseSettings.SectionName));
        }
    }
}
