using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Console.Wpf.ViewModel.Services;
using Console.Wpf.ViewModel.BlockSpecifics;
using Console.Wpf.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.Unity;
using Console.Wpf.Tests.VSTS.Mocks;

namespace Console.Wpf.Tests.VSTS.DevTests.given_data_configuration
{
    [TestClass]
    public class when_saving_data_configuration_section : given_data_configuration
    {
        ProtectedConfigurationSource saveSource = new ProtectedConfigurationSource();

        protected override void Act()
        {
            var configurationSection = source.GetSection("connectionStrings");
            var databaseSectionViewModel = SectionViewModel.CreateSection(Container, "connectionStrings", configurationSection);
            databaseSectionViewModel.AfterOpen(base.source);

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
