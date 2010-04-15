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
using System.Configuration;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.TestObjects;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration
{
    public abstract class Given_HierarchicalConfigurationSourceHandler : ArrangeActAssert
    {
        protected TestConfigurationSource LocalSource;
        protected TestConfigurationSource ParentSource;
        protected HierarchicalConfigurationSourceHandler HierarchicalSourceHandler;
        protected string SectionName = "DummySection";

        protected override void Arrange()
        {
            LocalSource = new TestConfigurationSource();
            var sectionForLocalSource = Arrange_GetLocalSourceSection();
            if (sectionForLocalSource != null) LocalSource.Add(SectionName, sectionForLocalSource);

            ParentSource = new TestConfigurationSource();
            var sectionForParentSource = Arrange_GetParentSourceSection();
            if (sectionForParentSource != null) ParentSource.Add(SectionName, sectionForParentSource);

            HierarchicalSourceHandler = new HierarchicalConfigurationSourceHandler(LocalSource, ParentSource);
            LocalSource.SetHierarchyHandler(HierarchicalSourceHandler);
        }

        protected virtual ConfigurationSection Arrange_GetLocalSourceSection()
        {
            return null;
        }

        protected virtual ConfigurationSection Arrange_GetParentSourceSection()
        {
            return null;
        }

        protected DummySectionWithCollections GetMergedSection()
        {
            ConfigurationSection localSection = LocalSource.GetSection(SectionName);
            return HierarchicalSourceHandler.CheckGetSection(SectionName, localSection) as DummySectionWithCollections;
        }

    }

    public abstract class Given_HierarhicalConfigurationSourceHandlerAndConfigurationElementCollection : Given_HierarchicalConfigurationSourceHandler
    {
        protected override ConfigurationSection Arrange_GetLocalSourceSection()
        {
            return new DummySectionWithCollections
            {
                LeafElementCollection = new MergeableElementCollection
                (
                    GetLocalSourceCollection()
                )
            };
        }

        protected override ConfigurationSection Arrange_GetParentSourceSection()
        {

            return new DummySectionWithCollections
            {
                LeafElementCollection = new MergeableElementCollection
                (
                    GetParentSourceCollection()
                )
            };
        }

        protected virtual IEnumerable<TestLeafConfigurationElement> GetLocalSourceCollection()
        {
            return Enumerable.Empty<TestLeafConfigurationElement>();
        }

        protected virtual IEnumerable<TestLeafConfigurationElement> GetParentSourceCollection()
        {
            return Enumerable.Empty<TestLeafConfigurationElement>();
        }
    }

    [TestClass]
    public class When_SectionExistsInLocalButNotInParent : Given_HierarchicalConfigurationSourceHandler
    {
        DummySectionWithCollections section;

        protected override ConfigurationSection Arrange_GetLocalSourceSection()
        {
            return new DummySectionWithCollections { Name = "Name", Value = 12 };
        }

        protected override void Act()
        {
            section = GetMergedSection();
        }

        [TestMethod]
        public void Then_ValuesOnSectionEqualValuesFromLocalSource()
        {
            Assert.IsNotNull(section);
            Assert.AreEqual("Name", section.Name);
            Assert.AreEqual(12, section.Value);
        }
    }

    [TestClass]
    public class When_SectionExistsInParentbutNotInCurrent : Given_HierarchicalConfigurationSourceHandler
    {
        DummySectionWithCollections section;

        protected override ConfigurationSection Arrange_GetParentSourceSection()
        {
            return new DummySectionWithCollections { Name = "Name2", Value = 13 };
        }

        protected override void Act()
        {
            section = GetMergedSection();
        }

        [TestMethod]
        public void Then_ValuesOnSectionEqualValuesFromParentSource()
        {
            Assert.IsNotNull(section);
            Assert.AreEqual("Name2", section.Name);
            Assert.AreEqual(13, section.Value);
        }
    }

    [TestClass]
    public class When_SectionExistsInCurrentAndParentSectionIsEmpty : Given_HierarchicalConfigurationSourceHandler
    {
        DummySectionWithCollections section;

        protected override ConfigurationSection Arrange_GetParentSourceSection()
        {
            return new DummySectionWithCollections { };
        }

        protected override ConfigurationSection Arrange_GetLocalSourceSection()
        {
            return new DummySectionWithCollections { Name = "Name2", Value = 13 };
        }

        protected override void Act()
        {
            section = GetMergedSection();
        }

        [TestMethod]
        public void Then_ValuesOnSectionEqualValuesFromLocalSource()
        {
            Assert.IsNotNull(section);
            Assert.AreEqual("Name2", section.Name);
            Assert.AreEqual(13, section.Value);
        }
    }

    [TestClass]
    public class When_SectionExistsInCurrentAndParentSectionIsNotEmpty : Given_HierarchicalConfigurationSourceHandler
    {
        DummySectionWithCollections section;

        protected override ConfigurationSection Arrange_GetParentSourceSection()
        {
            return new DummySectionWithCollections { Name = "Name1", Value = 23 };
        }

        protected override ConfigurationSection Arrange_GetLocalSourceSection()
        {
            return new DummySectionWithCollections { Name = "Name2", Value = 13 };
        }

        protected override void Act()
        {
            section = GetMergedSection();
        }

        [TestMethod]
        public void Then_ValuesOnSectionEqualValuesFromLocalSource()
        {
            Assert.IsNotNull(section);
            Assert.AreEqual("Name2", section.Name);
            Assert.AreEqual(13, section.Value);
        }
    }

    [TestClass]
    public class When_SectionExistsInParentAndLocalSectionIsEmpty : Given_HierarchicalConfigurationSourceHandler
    {
        DummySectionWithCollections section;

        protected override ConfigurationSection Arrange_GetParentSourceSection()
        {
            return new DummySectionWithCollections { Name = "Name1", Value = 23 };
        }

        protected override ConfigurationSection Arrange_GetLocalSourceSection()
        {
            return new DummySectionWithCollections { };
        }

        protected override void Act()
        {
            section = GetMergedSection();
        }

        [TestMethod]
        public void Then_ValuesOnSectionEqualValuesFromParentSource()
        {
            Assert.IsNotNull(section);
            Assert.AreEqual("Name1", section.Name);
            Assert.AreEqual(23, section.Value);
        }
    }

    [TestClass]
    public class When_SectionExistsInParentAndLocalSectionOmitsNameProperty : Given_HierarchicalConfigurationSourceHandler
    {
        DummySectionWithCollections section;

        protected override ConfigurationSection Arrange_GetParentSourceSection()
        {
            return new DummySectionWithCollections { Name = "Name1", Value = 23 };
        }

        protected override ConfigurationSection Arrange_GetLocalSourceSection()
        {
            return new DummySectionWithCollections { Value = 12 };
        }

        protected override void Act()
        {
            section = GetMergedSection();
        }

        [TestMethod]
        public void Then_NameValueIsInheritedFromParentSection()
        {
            Assert.IsNotNull(section);
            Assert.AreEqual("Name1", section.Name);
        }

        [TestMethod]
        public void Then_NameValueIsRetrievedFromLocalSection()
        {
            Assert.AreEqual(12, section.Value);
        }
    }

    [TestClass]
    public class When_SectionHasLeafThatExstsInLocalButNotInParent : Given_HierarchicalConfigurationSourceHandler
    {
        DummySectionWithCollections section;
        Guid leafId = Guid.NewGuid();

        protected override ConfigurationSection Arrange_GetParentSourceSection()
        {
            return new DummySectionWithCollections { };
        }

        protected override ConfigurationSection Arrange_GetLocalSourceSection()
        {
            return new DummySectionWithCollections { LeafElement = new TestLeafConfigurationElement { ID = leafId } };
        }

        protected override void Act()
        {
            section = GetMergedSection();
        }

        [TestMethod]
        public void Then_LeafSectionIsRetrievedFromLocal()
        {
            Assert.IsNotNull(section);
            Assert.IsNotNull(section.LeafElement);
            Assert.AreEqual(leafId, section.LeafElement.ID);
        }
    }

    [TestClass]
    public class When_SectionHasLeafThatExstsInParentButNotInLocal : Given_HierarchicalConfigurationSourceHandler
    {
        DummySectionWithCollections section;
        Guid leafId = Guid.NewGuid();

        protected override ConfigurationSection Arrange_GetParentSourceSection()
        {
            return new DummySectionWithCollections { LeafElement = new TestLeafConfigurationElement { ID = leafId } };
        }

        protected override ConfigurationSection Arrange_GetLocalSourceSection()
        {
            return new DummySectionWithCollections { };
        }

        protected override void Act()
        {
            section = GetMergedSection();
        }

        [TestMethod]
        public void Then_LeafSectionIsRetrievedFromParent()
        {
            Assert.IsNotNull(section);
            Assert.IsNotNull(section.LeafElement);
            Assert.AreEqual(leafId, section.LeafElement.ID);
        }
    }

    [TestClass]
    public class When_SectionHasLeafThatExstsInBothParentAndLocal : Given_HierarchicalConfigurationSourceHandler
    {
        DummySectionWithCollections section;
        Guid leafId = Guid.NewGuid();

        protected override ConfigurationSection Arrange_GetParentSourceSection()
        {
            return new DummySectionWithCollections { LeafElement = new TestLeafConfigurationElement { ID = leafId } };
        }

        protected override ConfigurationSection Arrange_GetLocalSourceSection()
        {
            return new DummySectionWithCollections { LeafElement = new TestLeafConfigurationElement { } };
        }

        protected override void Act()
        {
            section = GetMergedSection();
        }

        [TestMethod]
        public void Then_OmittedPropertiesOnLocalLeaveAreInheritedFromParent()
        {
            Assert.IsNotNull(section);
            Assert.IsNotNull(section.LeafElement);
            Assert.AreEqual(leafId, section.LeafElement.ID);
        }
    }

    [TestClass]
    public class When_SectionHasCollectionWithNoElementsInLocalAndElementInParent : Given_HierarhicalConfigurationSourceHandlerAndConfigurationElementCollection
    {
        DummySectionWithCollections section;

        protected override IEnumerable<TestLeafConfigurationElement> GetParentSourceCollection()
        {
            yield return new TestLeafConfigurationElement
            {
                ID = Guid.NewGuid(),
                OtherKeyPart = "p1"
            };
        }

        protected override void Act()
        {
            section = GetMergedSection();
        }

        [TestMethod]
        public void Then_OmittedPropertiesOnLocalLeaveAreInheritedFromParent()
        {
            Assert.IsNotNull(section);
            Assert.IsNotNull(section.LeafElementCollection);
            Assert.AreEqual(1, section.LeafElementCollection.Count);

            TestLeafConfigurationElement firstCollectionElement = section.LeafElementCollection.OfType<TestLeafConfigurationElement>().First();

            Assert.AreEqual("p1", firstCollectionElement.OtherKeyPart);
        }
    }

    [TestClass]
    public class When_SectionHasCollectionWithElementsInBothLocalAndParent : Given_HierarhicalConfigurationSourceHandlerAndConfigurationElementCollection
    {
        DummySectionWithCollections section;


        protected override IEnumerable<TestLeafConfigurationElement> GetParentSourceCollection()
        {
            yield return new TestLeafConfigurationElement
            {
                ID = Guid.NewGuid(),
                OtherKeyPart = "p1",
                AnInt = 20
            };
        }

        protected override IEnumerable<TestLeafConfigurationElement> GetLocalSourceCollection()
        {
            yield return new TestLeafConfigurationElement
            {
                ID = Guid.NewGuid(),
                OtherKeyPart = "p2"
            };
        }

        protected override void Act()
        {
            section = GetMergedSection();
        }

        [TestMethod]
        public void Then_BothElementsEndUpInMergedCollection()
        {
            Assert.IsNotNull(section);
            Assert.IsNotNull(section.LeafElementCollection);
            Assert.AreEqual(2, section.LeafElementCollection.Count);

            var elements = section.LeafElementCollection.OfType<TestLeafConfigurationElement>();

            Assert.IsTrue(elements.Any(x => x.OtherKeyPart == "p2"));
            Assert.IsTrue(elements.Any(x => x.OtherKeyPart == "p1"));
        }

        [TestMethod]
        public void Then_OrderIsPreserved()
        {
            var elements = section.LeafElementCollection.OfType<TestLeafConfigurationElement>();
            Assert.AreEqual("p1", elements.First().OtherKeyPart);
            Assert.AreEqual("p2", elements.Skip(1).First().OtherKeyPart);

        }
    }

    [TestClass]
    public class When_SectionHasCollectionWithSameElementsInBothLocalAndParent : Given_HierarhicalConfigurationSourceHandlerAndConfigurationElementCollection
    {
        DummySectionWithCollections section;
        Guid id = Guid.NewGuid();

        protected override IEnumerable<TestLeafConfigurationElement> GetParentSourceCollection()
        {
            yield return new TestLeafConfigurationElement
            {
                ID = id,
                OtherKeyPart = "p1",
                SomeOtherValue = "parent",
                AnInt = 20
            };
        }

        protected override IEnumerable<TestLeafConfigurationElement> GetLocalSourceCollection()
        {
            yield return new TestLeafConfigurationElement
            {
                ID = id,
                OtherKeyPart = "p1",
                SomeOtherValue = "local",
            };
        }

        protected override void Act()
        {
            section = GetMergedSection();
        }

        [TestMethod]
        public void Then_OnlyOneElementEndsUpInMergeResults()
        {
            Assert.IsNotNull(section);
            Assert.IsNotNull(section.LeafElementCollection);
            Assert.AreEqual(1, section.LeafElementCollection.Count);

            var element = section.LeafElementCollection.OfType<TestLeafConfigurationElement>().First();

            Assert.AreEqual("p1", element.OtherKeyPart);
        }

        [TestMethod]
        public void Then_propertiesMissingSpeciedInBothElementsAreOverwrittenByLocal()
        {
            var element = section.LeafElementCollection.OfType<TestLeafConfigurationElement>().First();
            Assert.AreEqual("local", element.SomeOtherValue);
        }

        [TestMethod]
        public void Then_propertiesMissingFromLocalAreInheritedFromSource()
        {
            var element = section.LeafElementCollection.OfType<TestLeafConfigurationElement>().First();
            Assert.AreEqual(20, element.AnInt);
        }
    }

    [TestClass]
    public class When_SectionHasCollectionWithEmitClearInLocal : Given_HierarhicalConfigurationSourceHandlerAndConfigurationElementCollection
    {
        DummySectionWithCollections section;

        protected override IEnumerable<TestLeafConfigurationElement> GetParentSourceCollection()
        {
            yield return new TestLeafConfigurationElement
            {
                ID = Guid.NewGuid(),
                OtherKeyPart = "p1",
            };

            yield return new TestLeafConfigurationElement
            {
                ID = Guid.NewGuid(),
                OtherKeyPart = "p2",
            };
        }

        protected override IEnumerable<TestLeafConfigurationElement> GetLocalSourceCollection()
        {
            yield return new TestLeafConfigurationElement
            {
                ID = Guid.NewGuid(),
                OtherKeyPart = "p3",
            };
        }

        protected override ConfigurationSection Arrange_GetLocalSourceSection()
        {
            DummySectionWithCollections dummySection = (DummySectionWithCollections)base.Arrange_GetLocalSourceSection();
            dummySection.LeafElementCollection.EmitClear = true;

            return dummySection;
        }

        protected override void Act()
        {
            section = GetMergedSection();
        }

        [TestMethod]
        public void Then_OnlyElementsFromSourceEndUpInMerge()
        {
            Assert.IsNotNull(section);
            Assert.IsNotNull(section.LeafElementCollection);
            Assert.AreEqual(1, section.LeafElementCollection.Count);

            var element = section.LeafElementCollection.OfType<TestLeafConfigurationElement>().First();

            Assert.AreEqual("p3", element.OtherKeyPart);
        }
    }

    [TestClass]
    public class When_HierarchicalConfigurationSourceNeedsToMergeConnectionStringSettingsCollection : Given_HierarchicalConfigurationSourceHandler
    {

        protected override ConfigurationSection Arrange_GetParentSourceSection()
        {
            ConnectionStringSettingsCollection parentConnectionStrings = new ConnectionStringSettingsCollection();
            parentConnectionStrings.Add(new ConnectionStringSettings("name1", "connstr1"));
            parentConnectionStrings.Add(new ConnectionStringSettings("name2", "connstr2"));

            return new DummySectionWithCollections
            {
                ConnectionStringSettingsCollection = parentConnectionStrings
            };
        }

        protected override ConfigurationSection Arrange_GetLocalSourceSection()
        {
            ConnectionStringSettingsCollection localConnectionStrings = new ConnectionStringSettingsCollection();
            localConnectionStrings.Add(new ConnectionStringSettings("name1", "overwrite"));
            localConnectionStrings.Add(new ConnectionStringSettings("name3", "connstr3"));

            return new DummySectionWithCollections
            {
                ConnectionStringSettingsCollection = localConnectionStrings
            };
        }

        DummySectionWithCollections section;

        protected override void Act()
        {
            section = base.GetMergedSection();
        }

        [TestMethod]
        public void Then_MergedSectionContainsConnectionStringsFromBothParentAndLocal()
        {
            Assert.IsNotNull(section.ConnectionStringSettingsCollection);
            Assert.AreEqual(3, section.ConnectionStringSettingsCollection.Count);
        }

        [TestMethod]
        public void Then_SettingsAreOverwrittenIfKeyAttributesMatch()
        {
            var name1ConnectionString = section.ConnectionStringSettingsCollection
                .OfType<ConnectionStringSettings>()
                .Where(x => x.Name == "name1")
                .FirstOrDefault();

            Assert.IsNotNull(name1ConnectionString);
            Assert.AreEqual("overwrite", name1ConnectionString.ConnectionString);
        }

        [TestMethod]
        public void Then_OrderIsPreserved()
        {
            var connectionStrings = section.ConnectionStringSettingsCollection
                .OfType<ConnectionStringSettings>();

            Assert.AreEqual("name1", connectionStrings.First().Name);
            Assert.AreEqual("name2", connectionStrings.Skip(1).First().Name);
            Assert.AreEqual("name3", connectionStrings.Skip(2).First().Name);

        }
    }

    [TestClass]
    public class When_HierarchicalConfigurationSourceNeedsToMergeLocallyClearedConnectionStringSettingsCollection : Given_HierarchicalConfigurationSourceHandler
    {

        protected override ConfigurationSection Arrange_GetParentSourceSection()
        {
            ConnectionStringSettingsCollection parentConnectionStrings = new ConnectionStringSettingsCollection();
            parentConnectionStrings.Add(new ConnectionStringSettings("name1", "connstr1"));
            parentConnectionStrings.Add(new ConnectionStringSettings("name2", "connstr2"));

            return new DummySectionWithCollections
            {
                ConnectionStringSettingsCollection = parentConnectionStrings
            };
        }

        protected override ConfigurationSection Arrange_GetLocalSourceSection()
        {
            ConnectionStringSettingsCollection localConnectionStrings = new ConnectionStringSettingsCollection();
            localConnectionStrings.Add(new ConnectionStringSettings("name1", "overwrite"));
            localConnectionStrings.Add(new ConnectionStringSettings("name3", "connstr3"));
            localConnectionStrings.EmitClear = true;

            return new DummySectionWithCollections
            {
                ConnectionStringSettingsCollection = localConnectionStrings
            };
        }

        DummySectionWithCollections section;

        protected override void Act()
        {
            section = base.GetMergedSection();
        }

        [TestMethod]
        public void Then_ParentSettingsAreNotInherited()
        {
            var connectionStrings = section.ConnectionStringSettingsCollection
                .OfType<ConnectionStringSettings>();

            Assert.AreEqual(2, connectionStrings.Count());
            Assert.AreEqual("name1", connectionStrings.First().Name);
            Assert.AreEqual("name3", connectionStrings.Skip(1).First().Name);
        }
    }

    [TestClass]
    public class When_HierarchicalConfigurationSourceNeedsToMergeKeyValueSettingsCollection : Given_HierarchicalConfigurationSourceHandler
    {
        protected override ConfigurationSection Arrange_GetParentSourceSection()
        {
            KeyValueConfigurationCollection parentSettings = new KeyValueConfigurationCollection();
            parentSettings.Add(new KeyValueConfigurationElement("key1", "value1"));
            parentSettings.Add(new KeyValueConfigurationElement("key2", "value2"));

            return new DummySectionWithCollections
            {
                AppSettingsLikeCollection = parentSettings
            };
        }

        protected override ConfigurationSection Arrange_GetLocalSourceSection()
        {
            KeyValueConfigurationCollection localSettings = new KeyValueConfigurationCollection();
            localSettings.Add(new KeyValueConfigurationElement("key1", "overwrite"));
            localSettings.Add(new KeyValueConfigurationElement("key3", "connstr3"));

            return new DummySectionWithCollections
            {
                AppSettingsLikeCollection = localSettings
            };
        }

        DummySectionWithCollections section;

        protected override void Act()
        {
            section = base.GetMergedSection();
        }

        [TestMethod]
        public void Then_MergedSectionContainsSettingsFromBothParentAndLocal()
        {
            Assert.IsNotNull(section.AppSettingsLikeCollection);
            Assert.AreEqual(3, section.AppSettingsLikeCollection.Count);
        }

        [TestMethod]
        public void Then_SettingsAreOverwrittenIfKeyAttributesMatch()
        {
            var key1Setting = section.AppSettingsLikeCollection
                .OfType<KeyValueConfigurationElement>()
                .Where(x => x.Key == "key1")
                .FirstOrDefault();

            Assert.IsNotNull(key1Setting);
            Assert.AreEqual("overwrite", key1Setting.Value);
        }

        [TestMethod]
        public void Then_OrderIsPreserved()
        {
            var applicationSettings = section.AppSettingsLikeCollection
                .OfType<KeyValueConfigurationElement>();

            Assert.AreEqual("key1", applicationSettings.First().Key);
            Assert.AreEqual("key2", applicationSettings.Skip(1).First().Key);
            Assert.AreEqual("key3", applicationSettings.Skip(2).First().Key);

        }
    }

    [TestClass]
    public class When_HierarchicalConfigurationSourceNeedsToMergeLocallyClearedKeyValueSettingsCollection : Given_HierarchicalConfigurationSourceHandler
    {
        protected override ConfigurationSection Arrange_GetParentSourceSection()
        {
            KeyValueConfigurationCollection parentSettings = new KeyValueConfigurationCollection();
            parentSettings.Add(new KeyValueConfigurationElement("key1", "value1"));
            parentSettings.Add(new KeyValueConfigurationElement("key2", "value2"));

            return new DummySectionWithCollections
            {
                AppSettingsLikeCollection = parentSettings
            };
        }

        protected override ConfigurationSection Arrange_GetLocalSourceSection()
        {
            KeyValueConfigurationCollection localSettings = new KeyValueConfigurationCollection();
            localSettings.Add(new KeyValueConfigurationElement("key1", "overwrite"));
            localSettings.Add(new KeyValueConfigurationElement("key3", "connstr3"));
            localSettings.EmitClear = true;

            return new DummySectionWithCollections
            {
                AppSettingsLikeCollection = localSettings
            };
        }

        DummySectionWithCollections section;

        protected override void Act()
        {
            section = base.GetMergedSection();
        }

        [TestMethod]
        public void Then_ParentSettingsAreNotInherited()
        {
            var applicationSettings = section.AppSettingsLikeCollection
                .OfType<KeyValueConfigurationElement>();

            Assert.AreEqual(2, applicationSettings.Count());
            Assert.AreEqual("key1", applicationSettings.First().Key);
            Assert.AreEqual("key3", applicationSettings.Skip(1).First().Key);
        }
    }

    [TestClass]
    public class When_HierarchicalConfigurationSourceNeedsToMergePolymorphicCollectionWithInCompatibleElementTypes : Given_HierarchicalConfigurationSourceHandler
    {
        protected override ConfigurationSection Arrange_GetParentSourceSection()
        {
            PolymorphicElementCollection collection = new PolymorphicElementCollection();
            collection.Add(new OtherDerivedPolymorphicElement() { Name = "el1" });
            return new DummySectionWithCollections
            {
                PolymorphicCollection = collection
            };
        }

        protected override ConfigurationSection Arrange_GetLocalSourceSection()
        {
            PolymorphicElementCollection collection = new PolymorphicElementCollection();
            collection.Add(new CustomPolymorphicElement() { Name = "el1" });
            return new DummySectionWithCollections
            {
                PolymorphicCollection = collection
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationSourceErrorsException))]
        public void Then_GetMergedSection_Throws()
        {
            base.GetMergedSection();
        }
    }

    [TestClass]
    public class When_HierarchicalConfigurationSourceIsCreatedPassingNullSource : ArrangeActAssert
    {

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_ConstructorThrows_ArgumentNullException()
        {
            new HierarchicalConfigurationSourceHandler(null);
        }
    }

    [TestClass]
    public class When_GetMergedSectionIsCalledPassingEmptySectionName : Given_HierarchicalConfigurationSourceHandler
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        [Ignore] // designtime managers occasionally call GetSection passing String.Empty. should be first fixed there.
        public void Then_GetMergedSectionThrows_ArgumentException()
        {
            HierarchicalSourceHandler.CheckGetSection(String.Empty, new DummySection());
        }
    }

    [TestClass]
    public class When_ParentAndChildSectionsDifferInType : Given_HierarchicalConfigurationSourceHandler
    {
        protected override ConfigurationSection Arrange_GetLocalSourceSection()
        {
            return new DummySection();
        }

        protected override ConfigurationSection Arrange_GetParentSourceSection()
        {
            return new ConnectionStringsSection();
        }

        [TestMethod]
        public void Then_GetMergedSectionThrows_HierarchicalConfigurationErrorsException()
        {
            try
            {
                base.GetMergedSection();
                Assert.Fail("sould have cought exception");
            }
            catch (ConfigurationSourceErrorsException)
            {
                //TODO: pick right exception, and assert on data
            }
        }
    }

    [TestClass]
    public class When_MissingConfiguredParent : ArrangeActAssert
    {
        private HierarchicalConfigurationSourceHandler hierarchicalConfigurationSourceHandler;
        private ConfigurationSourceSection sourceSection;

        protected override void Arrange()
        {
            base.Arrange();

            sourceSection = new ConfigurationSourceSection() {ParentSource = "MissingParentSource"};
            var localSource = new TestConfigurationSource();
            localSource.Add(ConfigurationSourceSection.SectionName, sourceSection);
            hierarchicalConfigurationSourceHandler = new HierarchicalConfigurationSourceHandler(localSource);
            localSource.SetHierarchyHandler(hierarchicalConfigurationSourceHandler);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationSourceErrorsException))]
        public void Then_ThrowsWhenSectionRequested()
        {
            hierarchicalConfigurationSourceHandler.CheckGetSection("SomeNonExistentSection", sourceSection);
        }
    }
}
