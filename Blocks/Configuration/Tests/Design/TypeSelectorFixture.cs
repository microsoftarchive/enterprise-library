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
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestClass]
    public class TypeSelectorFixture
    {
        TreeView treeView;
        TypeSelector selector;

        [TestInitialize]
        public void SetUp()
        {
            treeView = new TreeView();
            treeView.ImageList = new TypeSelectorUI().TypeImageList;
            selector = new TypeSelector(typeof(TestNode), typeof(BaseTestNode), treeView);
        }

        [TestMethod]
        public void IsAssiableFromTest()
        {
            Assert.IsTrue(typeof(BaseTestNode).IsAssignableFrom(typeof(TestNode)));
        }

        [TestMethod]
        public void DefaultIncludeTest()
        {
            bool valid = selector.IsTypeValid(typeof(TestNode));
            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void IncludeBaseTypeTest()
        {
            TypeSelectorIncludes flags = TypeSelectorIncludes.BaseType;
            TypeSelector selector = new TypeSelector(null, typeof(Exception), flags, treeView);
            bool valid = selector.IsTypeValid(typeof(Exception));
            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void ExcludeBaseTypeTest()
        {
            TypeSelectorIncludes flags = TypeSelectorIncludes.None;
            TypeSelector selector = new TypeSelector(null, typeof(Exception), flags, treeView);
            bool valid = selector.IsTypeValid(typeof(Exception));
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void IncludeAbstractTypesTest()
        {
            TypeSelectorIncludes flags = TypeSelectorIncludes.AbstractTypes;
            TypeSelector selector = new TypeSelector(null, typeof(ITest), flags, treeView);
            bool valid = selector.IsTypeValid(typeof(AbstractTest));
            Assert.IsTrue(valid);
            valid = selector.IsTypeValid(typeof(ITest2));
            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void ExcludeAbstractTypesTest()
        {
            TypeSelectorIncludes flags = TypeSelectorIncludes.None;
            TypeSelector selector = new TypeSelector(null, typeof(MarshalByRefObject), flags, treeView);
            bool valid = selector.IsTypeValid(typeof(Stream));
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void IncludeTypesWithConfigurationElementType()
        {
            TypeSelectorIncludes flags = TypeSelectorIncludes.None;
            TypeSelector selector = new TypeSelector(null, typeof(ITest), flags, typeof(TestConfigurationElement), treeView);
            bool valid = selector.IsTypeValid(typeof(TestWithConfigurationType));
            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void ExcludeTypesWithoutConfigurationElementType()
        {
            TypeSelectorIncludes flags = TypeSelectorIncludes.None;
            TypeSelector selector = new TypeSelector(null, typeof(MarshalByRefObject), flags, typeof(TestConfigurationElement), treeView);
            bool valid = selector.IsTypeValid(typeof(ITest));
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void IncludeAllInterfacesTest()
        {
            TypeSelectorIncludes flags = TypeSelectorIncludes.Interfaces;
            TypeSelector selector = new TypeSelector(null, typeof(MarshalByRefObject), flags, treeView);
            bool valid = selector.IsTypeValid(typeof(IComparable));
            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void ExcludeAllInterfacesTest()
        {
            TypeSelectorIncludes flags = TypeSelectorIncludes.None;
            TypeSelector selector = new TypeSelector(null, typeof(MarshalByRefObject), flags, treeView);
            bool valid = selector.IsTypeValid(typeof(IComparable));
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void IncludeNonPublicTypes()
        {
            TypeSelectorIncludes flags = TypeSelectorIncludes.NonpublicTypes;
            TypeSelector selector = new TypeSelector(null, typeof(MarshalByRefObject), flags, treeView);
            bool valid = selector.IsTypeValid(typeof(NonPublicClass));
            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void ExcludeNonPublicTypes()
        {
            TypeSelectorIncludes flags = TypeSelectorIncludes.None;
            TypeSelector selector = new TypeSelector(null, typeof(MarshalByRefObject), flags, treeView);
            bool valid = selector.IsTypeValid(typeof(NonPublicClass));
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void FlagsTest()
        {
            TypeSelectorIncludes flags = TypeSelectorIncludes.AbstractTypes |
                                         TypeSelectorIncludes.Interfaces |
                                         TypeSelectorIncludes.BaseType;
            TypeSelector selector = new TypeSelector(null, typeof(ITest), flags, treeView);
            bool valid = selector.IsTypeValid(typeof(AbstractTest));
            Assert.IsTrue(valid);
            valid = selector.IsTypeValid(typeof(ITest));
            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void IncludeNestedPublic()
        {
            TypeSelector selector = new TypeSelector(null, typeof(EventArgs), TypeSelectorIncludes.None, treeView);
            bool valid = selector.IsTypeValid(typeof(MockInnerTypeTest.InnerInner));
            Assert.IsTrue(valid);
        }

        public class MockInnerTypeTest
        {
            public class InnerInner : EventArgs {}
        }
    }

    public class BaseTestNode : ConfigurationNode
    {
        public BaseTestNode()
            : this("Base Node") {}

        public BaseTestNode(string name)
            : base(name) {}
    }

    public class TestNode : BaseTestNode
    {
        public TestNode()
            : base("Test Name") {}
    }

    public interface ITest
    {
        string Name { get; }
    }

    public interface ITest2 : ITest
    {
        string Age { get; }
    }

    public abstract class AbstractTest : ITest
    {
        public abstract string Name { get; }
    }

    [ConfigurationElementType(typeof(TestConfigurationElement))]
    public class TestWithConfigurationType : ITest
    {
        public string Name
        {
            get { return "Foo"; }
        }
    }

    public class TestConfigurationElement : ConfigurationElement {}

    class NonPublicClass : MarshalByRefObject
    {
        public string Name
        {
            get { return string.Empty; }
        }
    }
}
