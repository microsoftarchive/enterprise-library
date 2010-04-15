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

using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.Tests.VSTS.Mocks;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.DevTests.given_element_for_validation
{
    [TestClass]
    public class when_providing_element_for_validation_error : ContainerContext
    {
        private SectionViewModel sectionModel;
        private ElementValidationResult validationResult;

        protected override void Arrange()
        {
            base.Arrange();

            var section = new ElementForValidation();

            sectionModel = SectionViewModel.CreateSection(Container, "mock section", section);
        }

        protected override void Act()
        {
            validationResult = new ElementValidationResult(sectionModel, "TestMessage", true);
        }

        [TestMethod]
        public void then_element_name_matches_elements_displayname()
        {
            Assert.AreEqual(sectionModel.Name, validationResult.ElementName);
        }

        [TestMethod]
        public void then_property_name_is_empty()
        {
            Assert.AreEqual(string.Empty, validationResult.PropertyName);
        }

        [TestMethod]
        public void then_path_name_matches_element_path()
        {
            Assert.AreEqual(sectionModel.ElementId, validationResult.ElementId);
        }
    }

    [TestClass]
    public class when_element_name_changes_for_element_validation_error : ContainerContext
    {
        private SectionViewModel sectionModel;
        private ElementValidationResult validationResult;
        private PropertyChangedListener listener;

        protected override void Arrange()
        {
            base.Arrange();

            var section = new ElementForValidation();
            sectionModel = SectionViewModel.CreateSection(Container, "mock section", section);
            validationResult = new ElementValidationResult(sectionModel, "TestMessage", true);
            listener = new PropertyChangedListener(validationResult);
        }

        protected override void Act()
        {
            sectionModel.Property("Name").Value = "NewName";
        }

        [TestMethod]
        public void then_validation_model_fires_elementname_change()
        {
            Assert.IsTrue(listener.ChangedProperties.Contains("ElementName"));            
        }
    }
}
