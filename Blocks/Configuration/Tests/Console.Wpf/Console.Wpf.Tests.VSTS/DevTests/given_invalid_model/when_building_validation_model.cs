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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Console.Wpf.Tests.VSTS.Mocks;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.Unity;
using Moq;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;

namespace Console.Wpf.Tests.VSTS.DevTests.given_invalid_model
{
    public abstract class ValidatedSourceModel : ContainerContext
    {
        protected ConfigurationSourceModel SourceModel { get; private set; }
        protected SectionViewModel Section { get; private set; }

        protected override void Arrange()
        {
            base.Arrange();

            var locator = new Mock<ConfigurationSectionLocator>();
            locator.Setup(x => x.ConfigurationSectionNames).Returns(new[] { "testSection" });
            Container.RegisterInstance(locator.Object);

            var section = new ElementForValidation();

            var source = new DesignDictionaryConfigurationSource();
            source.Add("testSection", section);

            SourceModel = Container.Resolve<ConfigurationSourceModel>();
            SourceModel.Load(source);

            Section = SourceModel.Sections.Where(s => s.SectionName == "testSection").Single();
        }
    }

    [TestClass]
    public class when_building_validation_model : ValidatedSourceModel
    {
        protected ValidationModel ValidationModel { get; private set; }
        protected override void Act()
        {
            this.ValidationModel = Container.Resolve<ValidationModel>();
        }

        [TestMethod]
        public void then_validation_model_reflects_property_and_element_errors()
        {
            var validationErrors =
                SourceModel.Sections.SelectMany(
                    s => s.Properties.SelectMany(p => p.ValidationResults)
                        .Union(s.DescendentElements().SelectMany(e => e.Properties).SelectMany(p => p.ValidationResults))
                    );

            var elementErrors =
                SourceModel.Sections.SelectMany(
                    s => s.ValidationResults.Union(
                             s.DescendentElements().SelectMany(d => d.ValidationResults)));

            var allErrors = validationErrors.Union(elementErrors);
            CollectionAssert.AreEquivalent(allErrors.ToArray(), ValidationModel.ValidationResults.ToArray());
        }
    }

    [TestClass]
    public class when_validation_corrected_in_source_model : ValidatedSourceModel
    {
        private Property property;
        private bool collectionChanged;
        protected ValidationModel ValidationModel { get; private set; }
        protected override void Arrange()
        {
            base.Arrange();
            this.ValidationModel = Container.Resolve<ValidationModel>();

            ((INotifyCollectionChanged)this.ValidationModel.ValidationResults)
                        .CollectionChanged += (o, e) =>
                                            {
                                                collectionChanged = true;
                                            };
        }

        protected override void Act()
        {
            collectionChanged = false;
            property = Section.Property("PropertyWithConfigurationRequirements");
            property.BindableProperty.BindableValue = "ValidValue";
        }

        [TestMethod]
        public void then_results_updated()
        {
            Assert.IsFalse(ValidationModel.ValidationResults.Any(e => e.PropertyName == property.DisplayName));
        }

        [TestMethod]
        public void then_notify_collection_changed()
        {
            Assert.IsTrue(collectionChanged);
        }
    }

    [TestClass]
    public class when_new_invalidated_element_added : ValidatedSourceModel
    {
        private Property property;
        private bool collectionChanged;
        protected ValidationModel ValidationModel { get; private set; }
        protected override void Arrange()
        {
            base.Arrange();
            this.ValidationModel = Container.Resolve<ValidationModel>();

            ((INotifyCollectionChanged)this.ValidationModel.ValidationResults)
                        .CollectionChanged += (o, e) =>
                        {
                            collectionChanged = true;
                        };
        }

        protected override void Act()
        {
            collectionChanged = false;
            var elementCollection = Section.GetDescendentsOfType<NamedElementCollection<TestNamedElement>>()
                    .Where(e => e.Name == "ReferencedItems").OfType<ElementCollectionViewModel>().Single();
            var newElement = elementCollection.AddNewCollectionElement(typeof(TestNamedElement));
            property = newElement.Property("Name");
            property.BindableProperty.BindableValue = "";
            Assert.IsTrue(property.ValidationResults.Any());
        }

        [TestMethod]
        public void then_new_errors_are_available()
        {
            CollectionAssert.IsSubsetOf(property.ValidationResults.ToArray(), ValidationModel.ValidationResults.ToArray());
        }

        [TestMethod]
        public void then_notify_collection_change_fires()
        {
            Assert.IsTrue(collectionChanged);
        }
    }

    [TestClass]
    public class when_invalid_element_removed : ValidatedSourceModel
    {
        private bool collectionChanged;
        private ElementViewModel newElement;
        private string propertyName;
        protected ValidationModel ValidationModel { get; private set; }

        protected override void Arrange()
        {
            base.Arrange();
            this.ValidationModel = Container.Resolve<ValidationModel>();

            ((INotifyCollectionChanged)this.ValidationModel.ValidationResults)
                        .CollectionChanged += (o, e) =>
                        {
                            collectionChanged = true;
                        };

            var elementCollection = Section.GetDescendentsOfType<NamedElementCollection<TestNamedElement>>()
                    .Where(e => e.Name == "ReferencedItems").OfType<ElementCollectionViewModel>().Single();
            newElement = elementCollection.AddNewCollectionElement(typeof(TestNamedElement));
            var property = newElement.Property("Name");
            propertyName = property.DisplayName;
            property.BindableProperty.BindableValue = "";
            Assert.IsTrue(property.ValidationResults.Any(e => e.PropertyName == propertyName));
        }

        protected override void Act()
        {
            collectionChanged = false;
            newElement.Delete();
        }

        [TestMethod]
        public void then_property_errors_removed_from_collection()
        {
            Assert.IsFalse(ValidationModel.ValidationResults.Any(e => e.PropertyName == propertyName));
        }

        [TestMethod]
        public void then_notify_change_occurs()
        {
            Assert.IsTrue(collectionChanged);
        }
    }

    [TestClass]
    public class when_new_properties_are_added : ValidatedSourceModel
    {
        private Property property;
        private bool collectionChanged;
        protected ValidationModel ValidationModel { get; private set; }
        protected override void Arrange()
        {
            base.Arrange();
            this.ValidationModel = Container.Resolve<ValidationModel>();

            ((INotifyCollectionChanged)this.ValidationModel.ValidationResults)
                        .CollectionChanged += (o, e) =>
                        {
                            collectionChanged = true;
                        };
        }

        protected override void Act()
        {
            property = new MockCustomProperty("TestAddedProperty");
            Section.Properties.Add(property);
            Assert.IsTrue(property.ValidationResults.Any());
        }

        [TestMethod]
        public void then_new_properties_errors_in_collection()
        {
            CollectionAssert.IsSubsetOf(property.ValidationResults.ToArray(), ValidationModel.ValidationResults.ToArray());
        }

        [TestMethod]
        public void then_collection_change_invoked()
        {
            Assert.IsTrue(collectionChanged);
        }
    }

    [TestClass]
    public class when_properties_removed : ValidatedSourceModel
    {
        private Property property;
        private const string propertyName = "TestAddedProperty";
        protected ValidationModel ValidationModel { get; private set; }
        protected override void Arrange()
        {
            base.Arrange();
            this.ValidationModel = Container.Resolve<ValidationModel>();

            property = new MockCustomProperty(propertyName);
            Section.Properties.Add(property);
            Assert.IsTrue(property.ValidationResults.Any(e => e.PropertyName == propertyName));

        }

        protected override void Act()
        {
            Section.Properties.Remove(property);
        }

        [TestMethod]
        public void then_property_errors_removed()
        {
            Assert.IsFalse(ValidationModel.ValidationResults.Any(e => e.PropertyName == propertyName));
        }
    }

    [TestClass]
    public class when_child_properties_have_error : ValidatedSourceModel
    {
        private Property property;
        protected ValidationModel ValidationModel { get; private set; }
        protected override void Arrange()
        {
            base.Arrange();
            this.ValidationModel = Container.Resolve<ValidationModel>();
        }

        protected override void Act()
        {
            property = new MockCustomPropertyWithChildren("ParentProperty");
            Section.Properties.Add(property);
        }

        [TestMethod]
        public void then_childrens_validation_errors_are_not_reported_through_parent()
        {
            CollectionAssert.IsNotSubsetOf(
                property.ChildProperties
                    .Where(p => p.PropertyName == "ChildProperty").Single()
                    .ValidationResults.ToArray(),
                property.ValidationResults.ToArray());
        }

        [TestMethod]
        public void then_parent_errors_are_available()
        {
            Assert.IsTrue(
                property.ValidationResults.Any(e => e.PropertyName == property.PropertyName));
        }
    }

    [TestClass]
    public class when_element_validation_adds_error : ValidatedSourceModel
    {
        private ValidationModel ValidationModel;

        protected override void Arrange()
        {
            base.Arrange();
            this.ValidationModel = Container.Resolve<ValidationModel>();

            Assert.IsFalse(
               this.ValidationModel.ValidationResults.Any(v => v.Message == CollectionCountOneValidator.Message));
        }

        protected override void Act()
        {
            var collection = Section.DescendentConfigurationsOfType<NamedElementCollection<TestNamedElement>>().Where(
                e => e.Name == "ValidatedCollection").OfType<ElementCollectionViewModel>().First();

            collection.AddNewCollectionElement(typeof(TestNamedElement));
        }

        [TestMethod]
        public void then_validation_model_reflects_new_error()
        {
            Assert.IsTrue(
                this.ValidationModel.ValidationResults.Any(v => v.Message == CollectionCountOneValidator.Message));
        }
    }

    [TestClass]
    public class when_element_validation_removes_error : ValidatedSourceModel
    {
        private ValidationModel ValidationModel;
        private ElementViewModel addedElement;

        protected override void Arrange()
        {
            base.Arrange();
            this.ValidationModel = Container.Resolve<ValidationModel>();

            var collection = Section.DescendentConfigurationsOfType<NamedElementCollection<TestNamedElement>>().Where(
                    e => e.Name == "ValidatedCollection").OfType<ElementCollectionViewModel>().First();

            addedElement = collection.AddNewCollectionElement(typeof(TestNamedElement));

            Assert.IsTrue(
               this.ValidationModel.ValidationResults.Any(v => v.Message == CollectionCountOneValidator.Message));
        }

        protected override void Act()
        {
            addedElement.Delete();
        }

        [TestMethod]
        public void then_validation_model_reflects_new_error()
        {
            Assert.IsFalse(
                this.ValidationModel.ValidationResults.Any(v => v.Message == CollectionCountOneValidator.Message));
        }
    }

    class MockCustomProperty : CustomProperty<string>
    {
        public MockCustomProperty(string propertyName)
            : base(new Mock<IServiceProvider>().Object, propertyName)
        {
            ResetValidationResults(new [] {new PropertyValidationResult(this, "MockCustomProperty Error Message")});
        }
    }

    class MockCustomPropertyWithChildren : CustomProperty<string>
    {
        private MockCustomProperty childProperty;

        public MockCustomPropertyWithChildren(string propertyName)
            : base(new Mock<IServiceProvider>().Object, propertyName)
        {
            ResetValidationResults(new[] { new PropertyValidationResult(this, "MockCustomProperty Error Message") });

            childProperty = new MockCustomProperty("ChildProperty");
        }

        protected override System.Collections.Generic.IEnumerable<Property> GetChildProperties()
        {
            yield return childProperty;
        }
    }
}
