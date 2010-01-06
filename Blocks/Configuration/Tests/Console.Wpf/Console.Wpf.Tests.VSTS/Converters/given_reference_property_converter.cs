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

using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.Converters.ReferenceProperty
{
    [TestClass]
    public class when_converting_null_value : ArrangeActAssert
    {
        private string result;
        private const string noneValue = "<none>";

        protected override void Act()
        {
            var converter = new ReferencePropertyConverter();
            result = (string)converter.ConvertTo(null, CultureInfo.CurrentCulture, null, typeof(string));
        }

        [TestMethod]
        public void then_converted_value_is_correct()
        {
            Assert.AreEqual(noneValue, result);
        }
    }

    [TestClass]
    public class when_converting_empty_string_value : ArrangeActAssert
    {
        private string result;
        private const string noneValue = "<none>";

        protected override void Act()
        {
            var converter = new ReferencePropertyConverter();
            result = (string)converter.ConvertTo(null, CultureInfo.CurrentCulture, string.Empty, typeof(string));
        }

        [TestMethod]
        public void then_converted_value_is_none()
        {
            Assert.AreEqual(noneValue, result);
        }
    }
}
