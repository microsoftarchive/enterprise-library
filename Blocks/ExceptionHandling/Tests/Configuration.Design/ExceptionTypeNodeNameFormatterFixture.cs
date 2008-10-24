//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.Tests
{
    [TestClass]
    public class ExceptionTypeNodeNameFormatterFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullConfigurationDataInCreateNameMethodTrows()
        {
            ExceptionTypeNodeNameFormatter nameFormatter = new ExceptionTypeNodeNameFormatter();
            nameFormatter.CreateName((ExceptionTypeData)null);
        }

        [TestMethod]
        public void PassingConfigurationWithNullTypeReturnsEmptyString()
        {
            ExceptionTypeNodeNameFormatter nameFormatter = new ExceptionTypeNodeNameFormatter();
            ExceptionTypeData exceptionTypeData = new ExceptionTypeData("someName", (Type)null, PostHandlingAction.NotifyRethrow);

            string name = nameFormatter.CreateName(exceptionTypeData);
            Assert.IsNotNull(name);
            Assert.AreEqual(0, name.Length);
        }

        [TestMethod]
        public void PassingTypeStringReturnsFirstSegment()
        {
            ExceptionTypeNodeNameFormatter nameFormatter = new ExceptionTypeNodeNameFormatter();
            ExceptionTypeData exceptionTypeData = new ExceptionTypeData("someName", "a, b, c, d", PostHandlingAction.NotifyRethrow);

            string name = nameFormatter.CreateName(exceptionTypeData);
            Assert.IsNotNull(name);
            Assert.AreEqual("a", name);
        }

        [TestMethod]
        public void PassingTypeReturnsTypeName()
        {
            ExceptionTypeNodeNameFormatter nameFormatter = new ExceptionTypeNodeNameFormatter();
            ExceptionTypeData exceptionTypeData = new ExceptionTypeData("someName", typeof(Exception), PostHandlingAction.NotifyRethrow);

            string name = nameFormatter.CreateName(exceptionTypeData);
            Assert.IsNotNull(name);
            Assert.AreEqual("Exception", name);
        }

        [TestMethod]
        public void PassingTypeStringReturnsFirstSegmentAndTrimsSpaces()
        {
            ExceptionTypeNodeNameFormatter nameFormatter = new ExceptionTypeNodeNameFormatter();
            ExceptionTypeData exceptionTypeData = new ExceptionTypeData("someName", "  a, b, c, d", PostHandlingAction.NotifyRethrow);

            string name = nameFormatter.CreateName(exceptionTypeData);
            Assert.IsNotNull(name);
            Assert.AreEqual("a", name);
        }
    }
}
