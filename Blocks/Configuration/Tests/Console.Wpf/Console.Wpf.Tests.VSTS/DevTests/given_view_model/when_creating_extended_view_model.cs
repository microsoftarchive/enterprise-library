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
using System.Reflection;
using System.Text;
using System.Windows;
using Console.Wpf.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Console.Wpf.ViewModel;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.Design;
using System.Configuration;
using Console.Wpf.Tests.VSTS.TestSupport;
using Console.Wpf.Tests.VSTS.Mocks;

namespace Console.Wpf.Tests.VSTS.DevTests.given_view_model
{
    [TestClass]
    public class when_creating_extended_view_model : ArrangeActAssert
    {
        SectionWithExtendedViewModel sectionWithExtendedViewmodel;
        SectionViewModel viewModel;
        ServiceContainer container;

        protected override void Arrange()
        {
            sectionWithExtendedViewmodel = new SectionWithExtendedViewModel();
            container = new ServiceContainer();
        }

        protected override void Act()
        {
            viewModel = SectionViewModel.CreateSection(container, sectionWithExtendedViewmodel);
        }

        [TestMethod]
        public void then_section_view_model_has_specified_type()
        {
            Assert.IsNotNull(viewModel);
            Assert.IsInstanceOfType(viewModel, typeof(SectionViewModelEx));
        }

        [TestMethod]
        public void then_element_collection_view_model_has_specified_type()
        {
            Assert.IsTrue(viewModel.DescendentElements().OfType<ElementCollectionViewModelEx>().Any());
        }

        [TestMethod]
        public void then_collection_element_view_model_has_specified_type()
        {
            Assert.IsTrue(viewModel.DescendentElements().OfType<CollectionElementViewModelEx>().Any());
        }

        [TestMethod]
        public void then_element_view_model_has_specified_type()
        {
            Assert.IsTrue(viewModel.DescendentElements().OfType<ElementViewModelEx>().Any());
        }

        [TestMethod]
        public void then_view_model_attribute_on_declaring_property_overwrites_attribute_on_type()
        {
            Assert.IsTrue(viewModel.DescendentElements().OfType<ElementViewModelEx2>().Any());
        }

        [TestMethod]
        public void then_view_model_custom_UIElement_type_is_specified()
        {
            var element = viewModel.DescendentElements().OfType<ElementViewModelEx>().First();
            Assert.AreEqual(typeof(UIElement), element.CustomVisualType);
        }

        [TestMethod]
        public void then_custom_add_commands_attached()
        {
            var element = viewModel.DescendentElements().OfType<ElementCollectionViewModel>().Where(x => x.ConfigurationType == typeof(ElementCollectionWithExtendedViewModel)).First();
            Assert.IsNotNull(element.ChildAdders.OfType<CustomElementCollectionAddCommand>().Single());
            Assert.IsNotNull(element.ChildAdders.OfType<AnotherCustomElementCollectionAddCommand>().Single());
        }

    }
}
