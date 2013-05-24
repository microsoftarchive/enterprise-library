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

using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration
{
    [TestClass]
    public class GivenTextFormatterDataSection
    {
        private TextFormatterData formatterData;

        [TestInitialize]
        public void Given()
        {
            this.formatterData = new TextFormatterData("formatterName", "someTemplate");
        }

        [TestMethod]
        public void when_creating_formatter_then_creates_text_formatter()
        {
            var formatter = (TextFormatter)this.formatterData.BuildFormatter();

            Assert.AreEqual("someTemplate", formatter.Template);
        }
    }
}
