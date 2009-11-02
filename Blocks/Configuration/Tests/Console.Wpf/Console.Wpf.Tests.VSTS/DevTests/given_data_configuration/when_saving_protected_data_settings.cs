using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.Mocks;
using Console.Wpf.ViewModel;
using System.Configuration;

namespace Console.Wpf.Tests.VSTS.DevTests.given_data_configuration
{
    [TestClass]
    public class when_saving_protected_data_settings : given_data_configuration
    {
        ProtectedConfigurationSource saveSource = new ProtectedConfigurationSource();

        protected override void Act()
        {
            var configurationSection = source.GetSection("connectionStrings");
            var databaseSectionViewModel = SectionViewModel.CreateSection(Container, "connectionStrings", configurationSection);
            databaseSectionViewModel.AfterOpen(base.source);

            databaseSectionViewModel.Property("Protection Provider").Value = ProtectedConfiguration.DefaultProvider;
            databaseSectionViewModel.Save(saveSource);
        }

        [TestMethod]
        public void then_all_configuration_sections_are_protected()
        {
            Assert.AreEqual(3, saveSource.ProtectedAddCallCount);
        }

    }
}
