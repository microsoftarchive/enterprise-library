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
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration.Fluent
{
    public abstract class Given_TextFormatterBuilder : ArrangeActAssert
    {
        protected TextFormatterBuilder TextFormatterBuilder;
        protected string TextFormatterName = "Test Text Formatter";

        protected override void Arrange()
        {
            TextFormatterBuilder = new FormatterBuilder().TextFormatterNamed(TextFormatterName);
        }

        protected TextFormatterData GetTextFormatterData()
        {
            return ((IFormatterBuilder)TextFormatterBuilder).GetFormatterData() as TextFormatterData;
        }
    }

    [TestClass]
    public class When_CreatingTextFormatterBuilderPassingNullForName
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_TextFormatterNamed_ThrowsArgumentException()
        {
            new FormatterBuilder().TextFormatterNamed(null);
        }

    }

    [TestClass]
    public class When_CreatingTextFormatterBuilder : Given_TextFormatterBuilder
    {
        [TestMethod]
        public void Then_TextFormatterDataHasSpecifiedName()
        {
            Assert.AreEqual(TextFormatterName, GetTextFormatterData().Name);
        }

        [TestMethod]
        public void Then_TextFormatterDataHasDefaultTemplate()
        {
            Assert.IsFalse(String.IsNullOrEmpty(GetTextFormatterData().Template));
        }

        [TestMethod]
        public void Then_TextFormatterDataHasCorrectType()
        {
            Assert.AreEqual(typeof(TextFormatter), GetTextFormatterData().Type);
        }
    }

    [TestClass]
    public class When_SpecifyingTemplateForTextFormatterBuilder : Given_TextFormatterBuilder
    {
        protected override void Act()
        {
            TextFormatterBuilder.UsingTemplate("template");
        }

        [TestMethod]
        public void Then_TextFormatterDataHasDefaultTemplate()
        {
            Assert.AreEqual("template", GetTextFormatterData().Template);
        }
    }

    [TestClass]
    public class When_SpecifyingNullTemplateForTextFormatterBuilder : Given_TextFormatterBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_UsingTemplate_ThrowsArgumentNullException()
        {
            TextFormatterBuilder.UsingTemplate(null);
        }
    }

}
