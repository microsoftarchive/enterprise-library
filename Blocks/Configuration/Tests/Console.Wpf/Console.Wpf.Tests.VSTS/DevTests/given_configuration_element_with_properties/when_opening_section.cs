using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.Tests.VSTS.Mocks;
using Console.Wpf.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Console.Wpf.Tests.VSTS.DevTests.given_configuration_element_with_properties
{
    [TestClass]
    public class when_opening_section : ContainerContext
    {
        SectionViewModel sectionViewModel;

        protected override void Act()
        {
            SectionWithExtendedViewModel section = new SectionWithExtendedViewModel();
            sectionViewModel = SectionViewModel.CreateSection(Container, "section", section);
            sectionViewModel.AfterOpen(new DesignDictionaryConfigurationSource());

        }

        [TestMethod]
        public void then_properties_are_initialized()
        {
            Assert.IsTrue(CustomProperty.WasInitialized);
        }
        
    }
}
