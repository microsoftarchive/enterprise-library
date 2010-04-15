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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.Unity;
using Console.Wpf.Tests.VSTS.DevTests;

namespace Console.Wpf.Tests.VSTS.BlockSpecific.Data.given_data_configuration
{
    [TestClass]
    public class when_saving_data_configuration_section : BlockSpecific.Data.given_data_configuration.given_data_configuration
    {
        ProtectedConfigurationSource saveSource = new ProtectedConfigurationSource();

        protected override void Act()
        {
            var configurationSourceModel = Container.Resolve<ConfigurationSourceModel>();
            configurationSourceModel.Load(source);

            databaseSectionViewModel = configurationSourceModel.Sections
                .Where(x => x.SectionName == DataAccessDesignTime.ConnectionStringSettingsSectionName)
                .Single();

            databaseSectionViewModel.Save(saveSource);
        }

        [TestMethod]
        public void then_oracle_section_ends_up_source()
        {
            Assert.IsNotNull(saveSource.GetSection(OracleConnectionSettings.SectionName));
        }

        [TestMethod]
        public void then_connectionstrings_section_ends_up_source()
        {
            Assert.IsNotNull(saveSource.GetSection("connectionStrings"));
        }

        [TestMethod]
        public void then_data_section_ends_up_source()
        {
            Assert.IsNotNull(saveSource.GetSection(DatabaseSettings.SectionName));
        }
    }
}
