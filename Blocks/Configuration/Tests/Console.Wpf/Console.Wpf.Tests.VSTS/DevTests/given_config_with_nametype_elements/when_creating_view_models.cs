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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.DevTests.given_config_with_nametype_elements
{
    [TestClass]
    public class when_creating_view_models : ContainerContext
    {
        private SectionViewModel ViewModel;

        protected override void Act()
        {
            var section = new MockSectionWithSingleChild();
            section.Children.Add(new TestHandlerDataWithChildren());

            this.ViewModel = SectionViewModel.CreateSection(Container, "mock section", section);
        }
        
        [TestMethod]
        public void then_nametype_config_elements_offer_custom_property()
        {
            var elements = ViewModel.DescendentElements().Where(e => typeof(NameTypeConfigurationElement).IsAssignableFrom(e.ConfigurationType));

            Assert.IsTrue(elements.Count() > 0);
            foreach(var element in elements)
            {
                Assert.IsTrue(element.Properties.Any(p => typeof (TypeNameProperty).IsAssignableFrom(p.GetType())),
                              element.Name + " does not offer TypeNameProperty");
            }
        }

        [TestMethod]
        public void then_bindable_value_only_shows_time_name()
        {
            var element = ViewModel.DescendentElements().Where(
                e => typeof (TestHandlerDataWithChildren).IsAssignableFrom(e.ConfigurationType)).First();

            var property = element.Properties.OfType<TypeNameProperty>().First();

            var bindableProperty = property.BindableProperty;
            Assert.AreEqual(bindableProperty.BindableValue, 
                ((NameTypeConfigurationElement)element.ConfigurationElement).Type.Name);
        }
    }

         
}
