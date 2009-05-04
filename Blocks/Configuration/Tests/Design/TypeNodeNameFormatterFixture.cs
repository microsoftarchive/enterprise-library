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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    /// <summary>
    /// Summary description for TypeNodeNameFormatterFixture
    /// </summary>
    [TestClass]
    public class TypeNodeNameFormatterFixture
    {
        [TestMethod]
        public void PassingTypeStringReturnsFirstSegment()
        {
            TypeNodeNameFormatter nameFormatter = new TypeNodeNameFormatter();

            string name = nameFormatter.CreateName("a, b, c, d");
            Assert.IsNotNull(name);
            Assert.AreEqual("a", name);
        }

        [TestMethod]
        public void PassingTypeReturnsTypeName()
        {
            TypeNodeNameFormatter nameFormatter = new TypeNodeNameFormatter();

            string name = nameFormatter.CreateName(typeof(Exception).AssemblyQualifiedName);
            Assert.IsNotNull(name);
            Assert.AreEqual(typeof(Exception).Name, name);
        }

        [TestMethod]
        public void PassingTypeStringReturnsFirstSegmentAndTrimsSpaces()
        {
            TypeNodeNameFormatter nameFormatter = new TypeNodeNameFormatter();

            string name = nameFormatter.CreateName("   a, b, c, d");
            Assert.IsNotNull(name);
            Assert.AreEqual("a", name);
        }

        [TestMethod]
        public void PassingRegularStringReturnsRegularString()
        {
            TypeNodeNameFormatter nameFormatter = new TypeNodeNameFormatter();

            string name = nameFormatter.CreateName("Some arbitrary string");
            Assert.IsNotNull(name);
            Assert.AreEqual("Some arbitrary string", name);
        }

        [TestMethod]
        public void CanFormatOpenGenericTypeName()
        {
            string typeName = typeof(List<>).AssemblyQualifiedName;

            string name = new TypeNodeNameFormatter().CreateName(typeName);

            Assert.AreEqual("List<>", name);
        }

        [TestMethod]
        public void CanFormatOpenGenericTypeNameWithTwoParameters()
        {
            string typeName = typeof(Dictionary<,>).AssemblyQualifiedName;

            string name = new TypeNodeNameFormatter().CreateName(typeName);

            Assert.AreEqual("Dictionary<,>", name);
        }

        [TestMethod]
        public void CanFormatClosedSingleArgumentGenericTypeNameWithAssemblyQualifiedNameParameterTypeName()
        {
            string typeName = typeof(List<int>).AssemblyQualifiedName;

            string name = new TypeNodeNameFormatter().CreateName(typeName);

            Assert.AreEqual("List<Int32>", name);
        }

        [TestMethod]
        public void CanFormatClosedTwoArgumentsGenericTypeNameWithAssemblyQualifiedNameParameterTypeName()
        {
            string typeName = typeof(Dictionary<int, string>).AssemblyQualifiedName;

            string name = new TypeNodeNameFormatter().CreateName(typeName);

            Assert.AreEqual("Dictionary<Int32, String>", name);
        }

        [TestMethod]
        public void CanFormatClosedGenericTypeNameWithNonAssemblyQualifiedNameParameterTypeName()
        {
            string typeName = typeof(Dictionary<int, string>).AssemblyQualifiedName;
            typeName = typeName.Replace("[" + typeof(int).AssemblyQualifiedName + "]", typeof(int).FullName);

            string name = new TypeNodeNameFormatter().CreateName(typeName);

            Assert.AreEqual("Dictionary<Int32, String>", name);
        }

        [TestMethod]
        public void CanFormatClosedGenericTypeNameWithNonAssemblyQualifiedNameParameterTypeNameInTheLastPosition()
        {
            string typeName = typeof(Dictionary<int, string>).AssemblyQualifiedName;
            typeName = typeName.Replace("[" + typeof(string).AssemblyQualifiedName + "]", typeof(string).FullName);

            string name = new TypeNodeNameFormatter().CreateName(typeName);

            Assert.AreEqual("Dictionary<Int32, String>", name);
        }

        [TestMethod]
        public void CanFormatMultiLevelClosedGenerics()
        {
            string typeName = typeof(Dictionary<int, List<string>>).AssemblyQualifiedName;

            string name = new TypeNodeNameFormatter().CreateName(typeName);

            Assert.AreEqual("Dictionary<Int32, List<String>>", name);
        }

        [TestMethod]
        public void CanFormatNestedTypes()
        {
            string typeName = typeof(Dictionary<int, Empty>).AssemblyQualifiedName;

            string name = new TypeNodeNameFormatter().CreateName(typeName);

            Assert.AreEqual("Dictionary<Int32, TypeNodeNameFormatterFixture.Empty>", name);
        }

        [TestMethod]
        public void CanFormatNonAssemblyQualifiedOpenGeneric()
        {
            string typeName = typeof(Dictionary<,>).FullName;

            string name = new TypeNodeNameFormatter().CreateName(typeName);

            Assert.AreEqual("Dictionary<,>", name);
        }

        [TestMethod]
        public void DetectsNonMatchingBracketsWhenDealingWithNonAssemblyQualifiedGenerics()
        {
            string typeName = "Foo`1[Bar`1[Int32]]";
            string name1 = new TypeNodeNameFormatter().CreateName(typeName + "]");
            string name2 = new TypeNodeNameFormatter().CreateName(typeName);

            Assert.AreEqual(typeName+"]", name1);
            Assert.AreEqual("Foo<Bar<Int32>>", name2);
        }

        public class Empty
        {
        }
    }
}
