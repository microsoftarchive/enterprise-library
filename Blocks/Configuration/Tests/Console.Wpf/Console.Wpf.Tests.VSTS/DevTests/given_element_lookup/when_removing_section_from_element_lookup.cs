using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Console.Wpf.ViewModel.Services;
using Microsoft.Practices.Unity;

namespace Console.Wpf.Tests.VSTS.DevTests.given_element_lookup
{
    [TestClass]
    public class when_removing_section_from_element_lookup : ExceptionHandlingSettingsContext
    {
        SectionViewModel sectionViewModel;
        protected override void Arrange()
        {
            base.Arrange();

            sectionViewModel = SectionViewModel.CreateSection(Container, ExceptionHandlingSettings.SectionName, base.Section);
        }

        protected override void Act()
        {
            ElementLookup lookup = Container.Resolve<ElementLookup>();
            lookup.RemoveSection(sectionViewModel);
        }

        [TestMethod]
        public void then_section_is_removed_in_lookup()
        {
            ElementLookup lookup = Container.Resolve<ElementLookup>();
            Assert.IsFalse(lookup.FindInstancesOfConfigurationType(typeof(ExceptionHandlingSettings)).Any());
        }

        [TestMethod]
        public void then_conained_elements_are_removed_from_lookup()
        {
            ElementLookup lookup = Container.Resolve<ElementLookup>();
            Assert.IsFalse(lookup.FindInstancesOfConfigurationType(typeof(WrapHandlerData)).Any());
        }
    }
}
