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

using System.Configuration;
using System.Linq;
using Console.Wpf.Tests.VSTS.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Console.Wpf.Tests.VSTS.DevTests.given_element_for_validation
{
    [TestClass]
    public class when_validating_invalid_element : ContainerContext
    {
        private SectionViewModel sectionModel;

        protected override void Arrange()
        {
            base.Arrange();

            var section = new ElementForValidation();

            sectionModel = SectionViewModel.CreateSection(Container, "mock section", section);
        }

        protected override void Act()
        {
            sectionModel.Validate();
        }

        [TestMethod]
        public void then_validation_errors_appear_in_validationerrors_collection()
        {
            Assert.IsTrue(sectionModel.ValidationResults.Any(x => x.Message == ElementErrorProducingValidator.ErrorMessage));
        }

        [TestMethod]
        public void then_validation_error_appears_once()
        {
            Assert.AreEqual(1, sectionModel.ValidationResults.Count(x => x.Message == ElementErrorProducingValidator.ErrorMessage));
        }
    }

    [TestClass]
    public class when_validating_property_attributed_invalid_child_element : ContainerContext
    {
        private ElementCollectionViewModel collectionElement;

        protected override void Arrange()
        {
            base.Arrange();
            base.Arrange();

            var section = new ElementForValidation();

            var sectionModel = SectionViewModel.CreateSection(Container, "mock section", section);
            collectionElement =
                sectionModel.DescendentConfigurationsOfType<NamedElementCollection<TestNamedElement>>()
                        .Where(x => x.Name == "ValidatedCollection").OfType<ElementCollectionViewModel>().First();

            collectionElement.AddNewCollectionElement(typeof (TestNamedElement));
        }

        protected override void Act()
        {
            collectionElement.Validate();
        }

        [TestMethod]
        public void then_collection_contains_errors()
        {
            Assert.IsTrue(collectionElement.ValidationResults.Any(e => e.Message == CollectionCountOneValidator.Message));
        }

        [TestMethod]
        public void then_error_occurs_once()
        {
            Assert.AreEqual(1, collectionElement.ValidationResults.Count(e => e.Message == CollectionCountOneValidator.Message));
        }

    }

    [TestClass]
    public class when_validating_property_attributed_valid_child_element : ContainerContext
    {

        private ElementCollectionViewModel collectionElement;

        protected override void Arrange()
        {
            base.Arrange();
            base.Arrange();

            var section = new ElementForValidation();

            var sectionModel = SectionViewModel.CreateSection(Container, "mock section", section);
            collectionElement =
                sectionModel.DescendentConfigurationsOfType<NamedElementCollection<TestNamedElement>>()
                        .Where(x => x.Name == "ValidatedCollection").OfType<ElementCollectionViewModel>().First();

            // two elements is valid
            collectionElement.AddNewCollectionElement(typeof(TestNamedElement));
            collectionElement.AddNewCollectionElement(typeof(TestNamedElement));
        }

        protected override void Act()
        {
            collectionElement.Validate();
        }

        [TestMethod]
        public void then_element_contains_no_validation_errors()
        {
            Assert.IsFalse(collectionElement.ValidationResults.Any());
        }
    }

    [TestClass]
    public class when_validating_element_with_no_element_validation : ContainerContext
    {
        private ElementCollectionViewModel collectionElement;

        protected override void Arrange()
        {
            base.Arrange();

            var section = new ElementForValidation();

            var sectionModel = SectionViewModel.CreateSection(Container, "mock section", section);
            collectionElement =
                sectionModel.DescendentConfigurationsOfType<NamedElementCollection<TestNamedElement>>()
                        .Where(x => x.Name == "ReferencedItems").OfType<ElementCollectionViewModel>().First();
        }

        protected override void Act()
        {
            collectionElement.Validate();
        }

        [TestMethod]
        public void then_element_contains_no_validation_errors()
        {
            Assert.IsFalse(collectionElement.ValidationResults.Any());
        }
    }

    [TestClass]
    public class when_element_validator_throws : ContainerContext
    {
        private SectionViewModel sectionModel;

        protected override void Arrange()
        {
            base.Arrange();

            var section = new ElementWithThrowingElementValidator();

            sectionModel = SectionViewModel.CreateSection(Container, "mock section", section);
        }

        protected override void Act()
        {
            sectionModel.Validate();
        }

        [TestMethod]
        public void then_exception_is_captures_as_error()
        {
            Assert.IsTrue(
                sectionModel.ValidationResults.Any(x => x.Message.StartsWith("An error occurred")));
        }

        [ElementValidation(typeof(ExceptionThrowingValidator))]
        class ElementWithThrowingElementValidator : ConfigurationSection
        {

        }
    }

    [TestClass]
    public class when_element_has_multiple_validators : ContainerContext
    {
        private SectionViewModel sectionModel;

        protected override void Arrange()
        {
            base.Arrange();

            var section = new ElementWithMultipleElementValidators();

            sectionModel = SectionViewModel.CreateSection(Container, "mock section", section);
        }

        protected override void Act()
        {
            sectionModel.Validate();
        }

        [TestMethod]
        public void then_both_validation_errors_appear()
        {
            Assert.IsTrue(
                sectionModel.ValidationResults.Any(x => x.Message == ElementErrorProducingValidator.ErrorMessage));

            Assert.IsTrue(
                sectionModel.ValidationResults.Any(x => x.Message.StartsWith("An error occurred")));

        }

        [ElementValidation(typeof(ExceptionThrowingValidator))]
        [ElementValidation(typeof(ElementErrorProducingValidator))]
        class ElementWithMultipleElementValidators : ConfigurationSection
        {

        }
    }

}
