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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration.Fluent
{
    public abstract class Given_BinaryFormatterBuilder : ArrangeActAssert
    {
        protected BinaryFormatterBuilder BinaryFormatterBuilder;
        protected string BinaryFormatterName = "Test Binary Formatter";

        protected override void Arrange()
        {
            BinaryFormatterBuilder = new FormatterBuilder().BinaryFormatterNamed(BinaryFormatterName);
        }

        protected BinaryLogFormatterData GetBinaryFormatterData()
        {
            return ((IFormatterBuilder)BinaryFormatterBuilder).GetFormatterData() as BinaryLogFormatterData;
        }
    }

    [TestClass]
    public class When_CreatingBinaryFormatterBuilder : Given_BinaryFormatterBuilder
    {
        [TestMethod]
        public void Then_BinaryFormatterDataHasSpecifiedName()
        {
            Assert.AreEqual(BinaryFormatterName, GetBinaryFormatterData().Name);
        }

        [TestMethod]
        public void Then_BinaryFormatterDataHasCorrectType()
        {
            Assert.AreEqual(typeof(BinaryLogFormatter), GetBinaryFormatterData().Type);
        }
    }

    [TestClass]
    public class When_CreatingBinaryFormatterPassingNullForName : ArrangeActAssert
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_BinaryFormatterNamed_ThrowsArgumentException()
        {
            new FormatterBuilder().BinaryFormatterNamed(null);
        }
    }
}
