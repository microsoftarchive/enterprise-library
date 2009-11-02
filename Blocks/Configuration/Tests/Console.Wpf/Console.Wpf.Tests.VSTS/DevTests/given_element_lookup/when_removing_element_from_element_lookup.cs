using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.ViewModel;
using Console.Wpf.ViewModel.Services;
using Microsoft.Practices.Unity;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;

namespace Console.Wpf.Tests.VSTS.DevTests.given_element_lookup
{
    [TestClass]
    public class when_removing_element_from_element_lookup : ExceptionHandlingSettingsContext
    {
        SectionViewModel sectionViewModel;
        ElementViewModel removedElement;

        protected override void Arrange()
        {
            base.Arrange();

            sectionViewModel = SectionViewModel.CreateSection(Container, ExceptionHandlingSettings.SectionName, base.Section);
        }

        protected override void Act()
        {
            removedElement = sectionViewModel.GetDescendentsOfType<ExceptionTypeData>().First();
            removedElement.DeleteCommand.Execute(null);
        }

        [TestMethod]
        public void then_element_is_removed_in_lookup()
        {
            ElementLookup lookup = Container.Resolve<ElementLookup>();
            Assert.IsFalse(lookup.FindInstancesOfConfigurationType(typeof(ExceptionTypeData)).Where(x => x.Path == removedElement.Path).Any());
        }

        [TestMethod]
        public void then_contained_elements_are_removed_in_lookup()
        {
            ElementLookup lookup = Container.Resolve<ElementLookup>();
            foreach (var containedElement in removedElement.DescendentElements())
            {
                Assert.IsFalse(lookup.FindInstancesOfConfigurationType(containedElement.ConfigurationType).Where(x => x.Path == containedElement.Path).Any());
            }
        }
    }
}
