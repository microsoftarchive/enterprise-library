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
using System.Windows;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Moq;
using Console.Wpf.Tests.VSTS.Mocks;
using Microsoft.Practices.Unity;

namespace Console.Wpf.Tests.VSTS.DevTests.given_model_for_disposal
{
    public abstract class SectionForRemoval : ContainerContext
    {
        protected ConfigurationSourceModel SourceModel { get; set; }

        protected override void Arrange()
        {
            base.Arrange();

            var locator = new Mock<ConfigurationSectionLocator>();
            locator.Setup(x => x.ConfigurationSectionNames).Returns(new[] { "testSection" });
            Container.RegisterInstance(locator.Object);

            var section = new MockSectionWithSingleChild();
            section.Children.Add(new TestHandlerDataWithChildren() { Name = "ParentOne" });
            TestHandlerDataWithChildren child = new TestHandlerDataWithChildren() { Name = "Parent Two" };
            child.Children.Add(new TestHandlerData() { Name = "One" });
            child.Children.Add(new TestHandlerData() { Name = "Two" });
            section.Children.Add(child);

            var source = new DesignDictionaryConfigurationSource();
            source.Add("testSection", section);

            SourceModel = Container.Resolve<ConfigurationSourceModel>();
            SourceModel.Load(source);
        }

    }

    [TestClass]
    public class when_removing_section : SectionForRemoval
    {
        private IEnumerable<WeakReference> elementWeakReferences;
        private WeakReference[] elementPropertyWeakReferences;

        protected override void Arrange()
        {
            base.Arrange();

            var sectionViewModel = SourceModel.Sections.First();

            elementWeakReferences = sectionViewModel.DescendentElements().Union(new[] { sectionViewModel }).Select(e => new WeakReference(e)).ToArray();
            elementPropertyWeakReferences =
                sectionViewModel.DescendentElements().Union(new[] { sectionViewModel }).SelectMany(e => e.Properties).Select(
                    p => new WeakReference(p)).ToArray();
        }

        protected override void Act()
        {
            SourceModel.RemoveSection("testSection");

            GC.Collect();
        }

        [TestMethod]
        public void then_elements_are_collected()
        {
            Assert.IsTrue(elementWeakReferences.All(r => r.IsAlive == false));
        }

        [TestMethod]
        public void then_leaf_properties_collected()
        {
            Assert.IsTrue(elementPropertyWeakReferences.All(r => r.IsAlive == false));
        }
    }

    [TestClass]
    public class when_clearing_all_sections_via_new : SectionForRemoval
    {
        private IEnumerable<WeakReference> elementWeakReferences;
        private WeakReference[] elementPropertyWeakReferences;

        protected override void Arrange()
        {
            base.Arrange();

            UIServiceMock.Setup(
                x =>
                x.ShowMessageWpf(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<System.Windows.MessageBoxButton>()))
                .Returns(MessageBoxResult.No);

            var sectionViewModel = SourceModel.Sections.First();

            elementWeakReferences = sectionViewModel.DescendentElements().Union(new[] { sectionViewModel }).Select(e => new WeakReference(e)).ToArray();
            elementPropertyWeakReferences =
                sectionViewModel.DescendentElements().Union(new[] { sectionViewModel }).SelectMany(e => e.Properties).Select(
                    p => new WeakReference(p)).ToArray();

        }

        protected override void Act()
        {
            var appModel = Container.Resolve<IApplicationModel>();
            appModel.New();

            GC.Collect();
        }

        [TestMethod]
        public void then_elements_are_collected()
        {
            Assert.IsTrue(elementWeakReferences.All(r => r.IsAlive == false));
        }

        [TestMethod]
        public void then_leaf_properties_collected()
        {
            Assert.IsTrue(elementPropertyWeakReferences.All(r => r.IsAlive == false));
        }
    }

    [TestClass]
    public class when_clearing_all_sections_via_with_overrides : SectionForRemoval
    {
        private IEnumerable<WeakReference> elementWeakReferences;
        private WeakReference[] elementPropertyWeakReferences;
        private ApplicationViewModel appModel;

        protected override void Arrange()
        {
            base.Arrange();

            UIServiceMock.Setup(
                x =>
                x.ShowMessageWpf(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<System.Windows.MessageBoxButton>()))
                .Returns(MessageBoxResult.No);

            var sectionViewModel = SourceModel.Sections.First();

            elementWeakReferences = sectionViewModel.DescendentElements().Union(new[] { sectionViewModel }).Select(e => new WeakReference(e)).ToArray();
            elementPropertyWeakReferences =
                sectionViewModel.DescendentElements().Union(new[] { sectionViewModel }).SelectMany(e => e.Properties).Select(
                    p => new WeakReference(p)).ToArray();

            appModel = Container.Resolve<ApplicationViewModel>();
            appModel.NewEnvironment();
        }

        protected override void Act()
        {
            appModel.New();
            GC.Collect();
        }

        [TestMethod]
        public void then_elements_are_collected()
        {
            Assert.IsTrue(elementWeakReferences.All(r => r.IsAlive == false));
        }

        [TestMethod]
        public void then_leaf_properties_collected()
        {
            Assert.IsTrue(elementPropertyWeakReferences.All(r => r.IsAlive == false));
        }
    }
}
