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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Console.Wpf.Tests.VSTS.DevTests.given_element_reference
{
    public abstract class given_reference_property : Console.Wpf.Tests.VSTS.DevTests.Contexts.ContainerContext
    {

        protected override void Arrange()
        {
            base.Arrange();

            SectionWithReference = new SectionWithReference();
            SectionWithReference.ItemsCollection.Add(new NamedReferencedItem() { Name = "FirstItem" });

            SectionViewModel = SectionViewModel.CreateSection(Container, "testSection", SectionWithReference);

            ReferencingProperty = (ElementReferenceProperty)SectionViewModel.Property("DefaultItem");
            ReferencingProperty.Initialize(null);
        }

        protected SectionViewModel SectionViewModel { get; private set; }
        protected SectionWithReference SectionWithReference { get; private set; }
        protected ElementReferenceProperty ReferencingProperty { get; private set; }
    }

    [TestClass]
    public class when_dropping_down_reference_picker : given_reference_property
    {
        [TestMethod]
        public void then_reference_property_has_none_option()
        {
            Assert.IsTrue(((SuggestedValuesBindableProperty)ReferencingProperty.BindableProperty).BindableSuggestedValues.Contains("<none>"));
        }
    }

    [TestClass]
    public class when_setting_reference_to_none : given_reference_property
    {
        protected override void Act()
        {
            ReferencingProperty.BindableProperty.BindableValue = "<none>";
        }

        [TestMethod]
        public void then_property_value_is_empty_string()
        {
            Assert.IsTrue(string.IsNullOrEmpty((string)ReferencingProperty.Value));
        }
    }

    [TestClass]
    public class when_gettting_empty_reference_property : given_reference_property
    {
        protected override void Act()
        {
            SectionWithReference.DefaultItem = "";
        }

        [TestMethod]
        public void then_property_bindable_value_is_none()
        {
            Assert.AreEqual("<none>", ReferencingProperty.BindableProperty.BindableValue);
        }

        [TestMethod]
        public void then_property_value_is_empty()
        {
            Assert.IsTrue(string.IsNullOrEmpty((string)ReferencingProperty.Value));
        }

        [TestMethod]
        public void then_scope_containing_element_available()
        {
            Assert.AreSame(this.SectionViewModel, ReferencingProperty.ContainingScopeElement);
        }
    }

    public class SectionWithReference : ConfigurationSection
    {
        private const string itemsCollection = "itemsCollection";
        private const string defaultItem = "defaultItem";

        [ConfigurationProperty(defaultItem, IsRequired = false)]
        [Reference(typeof(NamedReferencedItem), ScopeIsDeclaringElement = true)]
        public string DefaultItem
        {
            get { return (string) this[defaultItem]; }
            set { this[defaultItem] = value; }
        }

        [ConfigurationProperty(itemsCollection)]
        [ConfigurationCollection(typeof(NamedReferencedItem))]
        public NamedElementCollection<NamedReferencedItem> ItemsCollection
        {
            get { return (NamedElementCollection<NamedReferencedItem>)this[itemsCollection]; }
            set { this[itemsCollection] = value;}
        }

    }

    public class NamedReferencedItem : NamedConfigurationElement
    {
        
    }
}
