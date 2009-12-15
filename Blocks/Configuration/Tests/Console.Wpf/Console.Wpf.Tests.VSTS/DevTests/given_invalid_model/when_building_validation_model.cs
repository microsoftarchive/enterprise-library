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
        public void then_validation_model_reflects_errors()
        {
            var validationErrors =
                SourceModel.Sections.SelectMany(
                    s => s.Properties.SelectMany(p => p.ValidationErrors)
                        .Union(s.DescendentElements().SelectMany(e => e.Properties).SelectMany(p => p.ValidationErrors))
                    );

            CollectionAssert.AreEquivalent(validationErrors.ToArray(), ValidationModel.ValidationErrors.ToArray());
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

            ((INotifyCollectionChanged)this.ValidationModel.ValidationErrors)
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
            Assert.IsFalse(ValidationModel.ValidationErrors.Any(e => e.PropertyName == property.DisplayName));
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

            ((INotifyCollectionChanged)this.ValidationModel.ValidationErrors)
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
            Assert.IsTrue(property.ValidationErrors.Any());
        }

        [TestMethod]
        public void then_new_errors_are_available()
        {
            CollectionAssert.IsSubsetOf(property.ValidationErrors.ToArray(), ValidationModel.ValidationErrors.ToArray());
        }

        [TestMethod]
        public void then_notify_collection_change_fires()
        {
            Assert.IsTrue(collectionChanged);
        }
    }

    [TestClass]
    public class when_invalide_element_removed : ValidatedSourceModel
    {
        private bool collectionChanged;
        private ElementViewModel newElement;
        private string propertyName;
        protected ValidationModel ValidationModel { get; private set; }

        protected override void Arrange()
        {
            base.Arrange();
            this.ValidationModel = Container.Resolve<ValidationModel>();

            ((INotifyCollectionChanged)this.ValidationModel.ValidationErrors)
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
            Assert.IsTrue(property.ValidationErrors.Any(e => e.PropertyName == propertyName));
        }

        protected override void Act()
        {
            collectionChanged = false;
            newElement.Delete();
        }

        [TestMethod]
        public void then_property_errors_removed_from_collection()
        {
            Assert.IsFalse(ValidationModel.ValidationErrors.Any(e => e.PropertyName == propertyName));
        }

        [TestMethod]
        public void then_notify_change_occurs()
        {
            Assert.IsTrue(collectionChanged);
        }
    }

    [TestClass]
    public class when_new_propeties_are_added : ValidatedSourceModel
    {
        private Property property;
        private bool collectionChanged;
        protected ValidationModel ValidationModel { get; private set; }
        protected override void Arrange()
        {
            base.Arrange();
            this.ValidationModel = Container.Resolve<ValidationModel>();

            ((INotifyCollectionChanged)this.ValidationModel.ValidationErrors)
                        .CollectionChanged += (o, e) =>
                        {
                            collectionChanged = true;
                        };
        }

        protected override void Act()
        {
            property = new MockCustomProperty("TestAddedProperty");
            Section.Properties.Add(property);
            Assert.IsTrue(property.ValidationErrors.Any());
        }

        [TestMethod]
        public void then_new_properties_errors_in_collection()
        {
            CollectionAssert.IsSubsetOf(property.ValidationErrors.ToArray(), ValidationModel.ValidationErrors.ToArray());
        }
    }

    [TestClass]
    public class when_properties_removed : ValidatedSourceModel
    {
        private Property property;
        private const string propertyName = "TestAddedProperty";
        private bool collectionChanged;
        protected ValidationModel ValidationModel { get; private set; }
        protected override void Arrange()
        {
            base.Arrange();
            this.ValidationModel = Container.Resolve<ValidationModel>();

            ((INotifyCollectionChanged)this.ValidationModel.ValidationErrors)
                        .CollectionChanged += (o, e) =>
                        {
                            collectionChanged = true;
                        };

            property = new MockCustomProperty(propertyName);
            Section.Properties.Add(property);
            Assert.IsTrue(property.ValidationErrors.Any(e => e.PropertyName == propertyName));

        }

        protected override void Act()
        {
            Section.Properties.Remove(property);
        }

        [TestMethod]
        public void then_property_errors_removed()
        {
            Assert.IsFalse(ValidationModel.ValidationErrors.Any(e => e.PropertyName == propertyName));
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
        public void then_childrens_validation_errors_are_reported_through_parent()
        {
            CollectionAssert.IsSubsetOf(
                property.ChildProperties
                    .Where(p => p.PropertyName == "ChildProperty").Single()
                    .ValidationErrors.ToArray(),
                property.ValidationErrors.ToArray());
        }

        [TestMethod]
        public void then_parent_errors_are_available()
        {
            Assert.IsTrue(
                property.ValidationErrors.Any(e => e.PropertyName == property.PropertyName));
        }
    }
    
    class MockCustomProperty : CustomProperty<string>
    {
        public MockCustomProperty(string propertyName)
            : base(new Mock<IServiceProvider>().Object, propertyName)
        {
            InternalErrors.Add(new ValidationError(this, "MockCustomProperty Error Message"));
        }
    }

    class MockCustomPropertyWithChildren : CustomProperty<string>
    {
        private MockCustomProperty childProperty;

        public MockCustomPropertyWithChildren(string propertyName)
            : base(new Mock<IServiceProvider>().Object, propertyName)
        {
            InternalErrors.Add(new ValidationError(this, "MockCustomPropertyWithChildren Error Message"));

            childProperty = new MockCustomProperty("ChildProperty");
        }

        protected override System.Collections.Generic.IEnumerable<Property> GetChildProperties()
        {
            yield return childProperty;
        }
    }
}