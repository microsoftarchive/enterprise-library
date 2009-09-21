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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.ViewModel;
using Console.Wpf.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;

namespace Console.Wpf.Tests.VSTS.DevTests.given_element_reference
{
    [TestClass]
    public class when_creating_element_reference_using_parent_path : ExceptionHandlingSettingsContext
    {
        SectionViewModel ehabModel;
        ElementLookup lookup;

        protected override void Act()
        {
            ehabModel = SectionViewModel.CreateSection(ServiceProvider, base.Section);
            lookup = (ElementLookup)ServiceProvider.GetService(typeof(ElementLookup));

        }

        [TestMethod]
        public void then_has_no_element_if_reference_cannot_be_found()
        {
            ElementReference reference = lookup.CreateReference("/path/to/unknown/collection", "element");

            Assert.IsNull(reference.Element);
        }

        [TestMethod]
        public void then_has_element_if_reference_can_be_found()
        {
            var anyHandler = ehabModel.DescendentElements().Where(x => typeof(ExceptionHandlerData).IsAssignableFrom(x.ConfigurationType)).First();
            ElementReference reference = lookup.CreateReference(anyHandler.ParentElement.Path, anyHandler.Name);

            Assert.IsNotNull(reference.Element);
        }

        [TestMethod]
        public void then_reference_without_element_can_be_fixed_by_adding_target_element_and_changing_its_name()
        {
            var anyHandler = ehabModel.DescendentElements().Where(x => typeof(ExceptionHandlerData).IsAssignableFrom(x.ConfigurationType)).First();
            var parentForHandler = anyHandler.ParentElement as ElementCollectionViewModel;

            ElementReference reference = lookup.CreateReference(anyHandler.ParentElement.Path, anyHandler.Name + "2");

            var newChild = parentForHandler.CreateNewChildElement(anyHandler.ConfigurationType);
            newChild.Property("Name").Value = anyHandler.Name + "2";

            Assert.IsNotNull(reference.Element);
        }


        [TestMethod]
        public void then_element_reference_signals_path_change()
        {
            var anyHandler = ehabModel.DescendentElements().Where(x => typeof(ExceptionHandlerData).IsAssignableFrom(x.ConfigurationType)).First();
            ElementReference reference = lookup.CreateReference(anyHandler.ParentElement.Path, anyHandler.Name);

            string newPath = string.Empty;
            reference.PathChanged += (sender, args) =>
            {
                newPath = ((ElementViewModel)sender).Path;
                Assert.AreEqual("Path", args.PropertyName);
            };

            anyHandler.Property("Name").Value = "new name";

            Assert.IsFalse(string.IsNullOrEmpty(newPath));
            Assert.IsTrue(newPath.EndsWith("new name"));
        }

        [TestMethod]
        public void then_element_reference_signals_name_change()
        {
            var anyHandler = ehabModel.DescendentElements().Where(x => typeof(ExceptionHandlerData).IsAssignableFrom(x.ConfigurationType)).First();
            ElementReference reference = lookup.CreateReference(anyHandler.ParentElement.Path, anyHandler.Name);

            string newName = string.Empty;
            reference.NameChanged += (sender, args) =>
            {
                newName = ((ElementViewModel)sender).Name;
                Assert.AreEqual("Name", args.PropertyName);
            };

            anyHandler.Property("Name").Value = "new name";

            Assert.IsFalse(string.IsNullOrEmpty(newName));
            Assert.IsTrue(newName.Equals("new name"));

        }

        [TestMethod]
        public void then_reference_is_broken_if_referred_element_is_deleted()
        {
            var anyHandler = ehabModel.DescendentElements().Where(x => typeof(ExceptionHandlerData).IsAssignableFrom(x.ConfigurationType)).First();
            ElementReference reference = lookup.CreateReference(anyHandler.ParentElement.Path, anyHandler.Name);
            Assert.IsNotNull(reference.Element);

            ((ElementCollectionViewModel)anyHandler.ParentElement).Delete(anyHandler as CollectionElementViewModel);
            Assert.IsNull(reference.Element);
        }

        [TestMethod]
        public void then_event_is_signaled_when_reference_is_broken()
        {
            bool deletedWasCalled = false;

            var anyHandler = ehabModel.DescendentElements().Where(x => typeof(ExceptionHandlerData).IsAssignableFrom(x.ConfigurationType)).First();
            ElementReference reference = lookup.CreateReference(anyHandler.ParentElement.Path, anyHandler.Name);

            reference.ElementDeleted += (s, a) => deletedWasCalled = true;

            ((ElementCollectionViewModel)anyHandler.ParentElement).Delete(anyHandler as CollectionElementViewModel);
            Assert.IsTrue(deletedWasCalled);

        }

        [TestMethod]
        public void then_reference_uses_updated_path_to_reconnect()
        {
            var anyHandler = ehabModel.DescendentElements().Where(x => typeof(ExceptionHandlerData).IsAssignableFrom(x.ConfigurationType)).First();
            ElementReference reference = lookup.CreateReference(anyHandler.ParentElement.Path, anyHandler.Name);

            anyHandler.Property("Name").Value = "new name"; //path changes;
            ElementCollectionViewModel containingCollection = (ElementCollectionViewModel)anyHandler.ParentElement;
            containingCollection.Delete((CollectionElementViewModel)anyHandler);

            Assert.IsNull(reference.Element);

            var NewHandler = containingCollection.CreateNewChildElement(anyHandler.ConfigurationType);
            NewHandler.Property("Name").Value = "new name";

            Assert.AreEqual(NewHandler, reference.Element);
        }
    }

    [TestClass]
    public class when_creating_broken_element_reference_using_parent_path : ExceptionHandlingSettingsContext
    {

        [TestMethod]
        public void then_reference_can_be_fixed_by_loading_section()
        {
            ElementLookup elementLookup = new ElementLookup();
            var reference = elementLookup.CreateReference("/ExceptionHandlingSettings:Exception Handling Settings", "Policies");

            ServiceProvider.AddService(typeof(ElementLookup), elementLookup);
            SectionViewModel.CreateSection(ServiceProvider, base.Section);

            Assert.IsNotNull(reference.Element);
        }

        [TestMethod]
        public void then_reference_signals_event_if_reference_is_fixed()
        {
            bool elementFoundSignalled = false;

            ElementLookup elementLookup = new ElementLookup();
            var reference = elementLookup.CreateReference("/ExceptionHandlingSettings:Exception Handling Settings");

            reference.ElementFound += (s, a) => elementFoundSignalled = true;

            ServiceProvider.AddService(typeof(ElementLookup), elementLookup);
            SectionViewModel.CreateSection(ServiceProvider, base.Section);

            Assert.IsTrue(elementFoundSignalled);


        }
    }
}
