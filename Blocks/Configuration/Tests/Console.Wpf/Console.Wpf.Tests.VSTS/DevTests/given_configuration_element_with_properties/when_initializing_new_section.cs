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
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.Tests.VSTS.DevTests;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.Unity;

namespace Console.Wpf.Tests.VSTS.DevTests.given_configuration_element_with_properties
{
    [TestClass]
    public class when_initializing_new_section : ContainerContext
    {
        SectionViewModel sectionViewModel;

        protected override void Act()
        {
            SectionWithExtendedViewModel section = new SectionWithExtendedViewModel();

            var configSourceModel = Container.Resolve<ConfigurationSourceModel>();
            configSourceModel.AddSection("section", section);

            sectionViewModel = configSourceModel.Sections.OfType<SectionViewModel>().First();
        }

        [TestMethod]
        public void then_properties_are_initialized()
        {
            var property = sectionViewModel.DescendentElements()
                .SelectMany(x =>x.Properties)
                .OfType<CustomProperty>().First();
            
            Assert.IsTrue(property.WasInitialized);
        }
        
    }
}
