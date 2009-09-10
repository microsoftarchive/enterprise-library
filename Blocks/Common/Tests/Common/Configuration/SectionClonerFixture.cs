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
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.TestObjects;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration
{

    public abstract class given_clonable_section_context : ArrangeActAssert
    {
        private ConfigurationSection originalSection;
        private ConfigurationSection clonedSection;

        protected override void Arrange()
        {
            base.Arrange();
            originalSection = ArrangeOriginalSection();
        }

        protected override void Act()
        {
            clonedSection = DoClone(originalSection);
        }

        protected ConfigurationSection OriginalSection { get { return originalSection; } }
        protected T GetOriginalSection<T>() where T : ConfigurationSection
        {
            return (T)originalSection;
        }

        protected ConfigurationSection ClonedSection { get { return clonedSection; } }
        protected T GetClonedSection<T>() where T : ConfigurationSection
        {
            return (T)clonedSection;
        }

        protected virtual ConfigurationSection DoClone(ConfigurationSection section)
        {
            return new ConfigurationSectionCloner().Clone(section);
        }

        protected virtual ConfigurationSection ArrangeOriginalSection()
        {
            var section = new DummySection { Name = "TestName", Value = 42 };
            return section;
        }
    }

    public class given_section_with_collections : given_clonable_section_context
    {
        protected override ConfigurationSection ArrangeOriginalSection()
        {
            return new DummySectionWithCollections()
            {
                LeafElementCollection =
                    new MergeableElementCollection(GetCollectionContents())
            };
        }

        protected virtual IEnumerable<TestLeafConfigurationElement> GetCollectionContents()
        {
            return Enumerable.Empty<TestLeafConfigurationElement>();
        }

        protected MergeableElementCollection ClonedLeafCollection
        {
            get { return GetClonedSection<DummySectionWithCollections>().LeafElementCollection; }
        }
    }

    [TestClass]
    public class when_cloning_a_section_with_basic_properties : given_clonable_section_context
    {
        [TestMethod]
        public void then_cloned_properties_match_original()
        {
            Assert.IsTrue(OriginalSection.Equals(ClonedSection));
        }
    }

    [TestClass]
    public class when_cloning_a_section_with_leaf_element : given_clonable_section_context
    {
        protected override ConfigurationSection ArrangeOriginalSection()
        {
            return new DummySectionWithLeafElement()
                       {
                           Name = "TestSection",
                           Value = 37,
                           LeafElement = new TestLeafConfigurationElement()
                                             {
                                                 ID = Guid.NewGuid(),
                                                 AnInt = 425,
                                                 OtherKeyPart = "OtherKeyPartValue",
                                                 SomeOtherValue = "SomeOtherValueValue",
                                             }
                       };
        }

        [TestMethod]
        public void then_section_elements_match()
        {
            Assert.AreEqual(OriginalSection, ClonedSection);
        }

        [TestMethod]
        public void then_cloned_leaf_created()
        {
            Assert.IsNotNull(GetClonedSection<DummySectionWithLeafElement>());
        }

        [TestMethod]
        public void then_leaf_nodes_match()
        {
            Assert.AreEqual(GetOriginalSection<DummySectionWithLeafElement>().LeafElement,
                            GetClonedSection<DummySectionWithLeafElement>().LeafElement);
        }

        protected class DummySectionWithLeafElement : DummySection
        {
            private const string leafElementProperty = "leaf";

            [ConfigurationProperty(leafElementProperty)]
            public TestLeafConfigurationElement LeafElement
            {
                get { return (TestLeafConfigurationElement)base[leafElementProperty]; }
                set { base[leafElementProperty] = value; }
            }
        }
    }


    [TestClass]
    public class when_cloning_a_section_with_empty_collections : given_section_with_collections
    {
        [TestMethod]
        public void then_clone_collectin_is_empty()
        {
            Assert.AreEqual(0, ClonedLeafCollection.Count);
        }
    }

    [TestClass]
    public class when_cloning_a_section_with_full_mergeable_collections : given_section_with_collections
    {
        private TestLeafConfigurationElement firstItem;
        private TestLeafConfigurationElement secondItem;

        protected override void Arrange()
        {
            firstItem = new TestLeafConfigurationElement() { ID = Guid.NewGuid(), AnInt = 1, SomeOtherValue = "OtherValue1", OtherKeyPart = "OtherKeyPart1" };
            secondItem = new TestLeafConfigurationElement() { ID = Guid.NewGuid(), AnInt = 2, SomeOtherValue = "OtherValue2", OtherKeyPart = "OtherKeyPart2" };

            base.Arrange();
        }
        protected override IEnumerable<TestLeafConfigurationElement> GetCollectionContents()
        {
            yield return firstItem;
            yield return secondItem;
        }

        [TestMethod]
        public void then_cloned_collection_has_same_count_as_parent()
        {
            Assert.AreEqual(2, ClonedLeafCollection.Count);
        }

        [TestMethod]
        public void then_cloned_items_match()
        {
            Assert.AreEqual(firstItem, ClonedLeafCollection.Cast<TestLeafConfigurationElement>().ElementAt(0));
            Assert.AreEqual(secondItem, ClonedLeafCollection.Cast<TestLeafConfigurationElement>().ElementAt(1));
        }
    }

    [TestClass]
    public class when_cloning_polymorphic_collections_from_section : given_section_with_collections
    {
        private PolymorphicElementCollection originalCollection;
        public PolymorphicElementCollection ClonedCollection { get { return GetClonedSection<DummySectionWithCollections>().PolymorphicCollection; } }

        protected override ConfigurationSection ArrangeOriginalSection()
        {
            originalCollection = new PolymorphicElementCollection();
            originalCollection.Add(new OtherDerivedPolymorphicElement() { Name = "el1" });
            originalCollection.Add(new CustomPolymorphicElement() { Name = "el2" });
            return new DummySectionWithCollections
            {
                PolymorphicCollection = originalCollection
            };
        }

        [TestMethod]
        public void then_cloned_collection_contains_right_number_of_polymorhpic_elements()
        {
            Assert.AreEqual(originalCollection.Count, ClonedCollection.Count);
        }

        [TestMethod]
        public void then_cloned_polymorphic_items_match()
        {
            CollectionAssert.AreEqual(originalCollection, ClonedCollection);
        }


    }

    [TestClass]
    public class when_cloning_connection_strings_from_section : given_section_with_collections
    {
        private ConnectionStringSettingsCollection localConnectionStrings;

        protected override ConfigurationSection ArrangeOriginalSection()
        {
            localConnectionStrings = new ConnectionStringSettingsCollection();
            localConnectionStrings.Add(new ConnectionStringSettings("name1", "overwrite"));
            localConnectionStrings.Add(new ConnectionStringSettings("name3", "connstr3"));

            return new DummySectionWithCollections
            {
                ConnectionStringSettingsCollection = localConnectionStrings
            };
        }

        public ConnectionStringSettingsCollection ClonedConnectionStrings { get { return GetClonedSection<DummySectionWithCollections>().ConnectionStringSettingsCollection; } }

        [TestMethod]
        public void then_connection_string_collections_match()
        {
            CollectionAssert.AreEqual(localConnectionStrings, ClonedConnectionStrings);
        }
    }

    [TestClass]
    public class when_cloning_connection_appsettings_from_section : given_section_with_collections
    {
        private KeyValueConfigurationCollection localSettings;

        protected override ConfigurationSection ArrangeOriginalSection()
        {
            localSettings = new KeyValueConfigurationCollection();
            localSettings.Add("key1", "value1");
            localSettings.Add("key2", "value2");
            localSettings.Add("key3", "value3");

            return new DummySectionWithCollections
                       {
                           AppSettingsLikeCollection = localSettings
                       };
        }

        public KeyValueConfigurationCollection ClonedSettings { get { return GetClonedSection<DummySectionWithCollections>().AppSettingsLikeCollection; } }

        [TestMethod]
        public void then_settings_match()
        {
            CollectionAssert.AreEqual(localSettings, ClonedSettings);
        }
    }

   
}
