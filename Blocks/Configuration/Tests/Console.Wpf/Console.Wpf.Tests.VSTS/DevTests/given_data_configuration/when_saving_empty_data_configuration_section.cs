using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Configuration;
using Console.Wpf.ViewModel.BlockSpecifics;
using Console.Wpf.ViewModel.Services;
using Console.Wpf.ViewModel;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Console.Wpf.Tests.VSTS.Mocks;

namespace Console.Wpf.Tests.VSTS.DevTests.given_data_configuration
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

            AnnotationService annotationService = Container.Resolve<AnnotationService>();
            ConnectionStringsDecorator.DecorateConnectionStringsSection(annotationService);
            var dataSection = SectionViewModel.CreateSection(Container, "connectionStrings", section);
            dataSection.AfterOpen(source);

            dataSection.Save(source);
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
