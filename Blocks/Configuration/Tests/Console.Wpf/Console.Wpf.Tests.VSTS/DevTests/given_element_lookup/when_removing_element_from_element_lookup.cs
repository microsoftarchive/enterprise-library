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

using System.Linq;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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
            this.Container.RegisterInstance(new Mock<IAssemblyDiscoveryService>().Object);
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
