//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration.Fluent
{
    public abstract class Given_CustomFormatterBuilder : ArrangeActAssert
    {
        protected CustomFormatterBuilder CustomFormatterBuilder;
        protected string CustomFormatterName = "Test Custom Formatter";
        protected Type CustomFormatterType = typeof(MockLogFormatter);

        protected override void Arrange()
        {
            CustomFormatterBuilder = new FormatterBuilder().CustomFormatterNamed(CustomFormatterName, CustomFormatterType);
        }

        protected CustomFormatterData GetCustomFormatterData()
        {
            return ((IFormatterBuilder)CustomFormatterBuilder).GetFormatterData() as CustomFormatterData;
        }

        protected class MockLogFormatter : LogFormatter
        {
            public override string  Format(LogEntry log)
            {
 	            throw new NotImplementedException();
            }
        }
    }


    [TestClass]
    public class When_CreatingCustomFormatterBuilderPassingNullForName : Given_CustomFormatterBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_CustomFormatterNamed_ThrowsArgumentException()
        {
            new FormatterBuilder().CustomFormatterNamed(null, typeof(MockLogFormatter));
        }
    }

    [TestClass]
    public class When_CreatingCustomFormatterBuilderPassingNullForType : Given_CustomFormatterBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_CustomFormatterNamed_ThrowsArgumentNullException()
        {
            new FormatterBuilder().CustomFormatterNamed("formatter name", null);
        }
    }

    [TestClass]
    public class When_CreatingCustomFormatterBuilderPassingNonFormatterType : Given_CustomFormatterBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_CustomFormatterNamed_ThrowsArgumentException()
        {
            new FormatterBuilder().CustomFormatterNamed("formatter name", typeof(object));
        }
    }

    [TestClass]
    public class When_CreatingCustomFormatterBuilderPassingNullForAttributes : Given_CustomFormatterBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_CustomFormatterNamed_ThrowsArgumentNullException()
        {
            new FormatterBuilder().CustomFormatterNamed("formatter name", typeof(MockLogFormatter), null);
        }
    }

    [TestClass]
    public class When_CreatingCustomFormatterBuilder : Given_CustomFormatterBuilder
    {
        [TestMethod]
        public void Then_CustomFormatterDataHasSpecifiedName()
        {
            Assert.AreEqual(CustomFormatterName, GetCustomFormatterData().Name);
        }

        [TestMethod]
        public void Then_CustomFormatterDataHasSpecifiedType()
        {
            Assert.AreEqual(CustomFormatterType, GetCustomFormatterData().Type);
        }
    }

    [TestClass]
    public class When_CreatingCustomFormatterBuilderGeneric : Given_CustomFormatterBuilder
    {
        protected override void Arrange()
        {
            CustomFormatterBuilder = new FormatterBuilder().CustomFormatterNamed<MockLogFormatter>(CustomFormatterName);
        }

        [TestMethod]
        public void Then_CustomFormatterDataHasSpecifiedName()
        {
            Assert.AreEqual(CustomFormatterName, GetCustomFormatterData().Name);
        }

        [TestMethod]
        public void Then_CustomFormatterDataHasSpecifiedType()
        {
            Assert.AreEqual(CustomFormatterType, GetCustomFormatterData().Type);
        }
    }


    [TestClass]
    public class When_CreatingCustomFormatterBuilderPassingAttributes : Given_CustomFormatterBuilder
    {
        NameValueCollection attributes = new NameValueCollection();

        protected override void Arrange()
        {
            attributes.Add("key1", "value1");
            attributes.Add("key2", "value2");
            CustomFormatterBuilder = new FormatterBuilder().CustomFormatterNamed(CustomFormatterName, CustomFormatterType, attributes);
        }

        [TestMethod]
        public void Then_CustomFormatterDataHasSpecifiedName()
        {
            Assert.AreEqual(CustomFormatterName, GetCustomFormatterData().Name);
        }

        [TestMethod]
        public void Then_CustomFormatterDataHasSpecifiedType()
        {
            Assert.AreEqual(CustomFormatterType, GetCustomFormatterData().Type);
        }

        [TestMethod]
        public void Then_CustomFormatterDataHasSpecifiedAttributes()
        {
            var formatterData = GetCustomFormatterData();
            Assert.AreEqual(attributes.Count, formatterData.Attributes.Count);
            foreach (string attKey in attributes)
            {
                Assert.AreEqual(attributes[attKey], formatterData.Attributes[attKey]);
            }
        }
    }


    [TestClass]
    public class When_CreatingCustomFormatterBuilderPassingAttributesGeneric : Given_CustomFormatterBuilder
    {
        NameValueCollection attributes = new NameValueCollection();

        protected override void Arrange()
        {
            attributes.Add("key1", "value1");
            attributes.Add("key2", "value2");
            CustomFormatterBuilder = new FormatterBuilder().CustomFormatterNamed <MockLogFormatter>(CustomFormatterName, attributes);
        }

        [TestMethod]
        public void Then_CustomFormatterDataHasSpecifiedName()
        {
            Assert.AreEqual(CustomFormatterName, GetCustomFormatterData().Name);
        }

        [TestMethod]
        public void Then_CustomFormatterDataHasSpecifiedType()
        {
            Assert.AreEqual(CustomFormatterType, GetCustomFormatterData().Type);
        }

        [TestMethod]
        public void Then_CustomFormatterDataHasSpecifiedAttributes()
        {
            var formatterData = GetCustomFormatterData();
            Assert.AreEqual(attributes.Count, formatterData.Attributes.Count);
            foreach (string attKey in attributes)
            {
                Assert.AreEqual(attributes[attKey], formatterData.Attributes[attKey]);
            }
        }
    }
}
