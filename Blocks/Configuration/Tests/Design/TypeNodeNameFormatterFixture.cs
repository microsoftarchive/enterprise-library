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

            string name = nameFormatter.CreateName("   a   , b, c, d");
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
    }
}