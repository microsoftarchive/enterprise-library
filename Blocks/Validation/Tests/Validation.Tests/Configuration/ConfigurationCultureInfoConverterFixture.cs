//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Configuration
{
    [TestClass]
    public class ConfigurationCultureInfoConverterFixture
    {
        private ConfigurationCultureInfoConverter converter = new ConfigurationCultureInfoConverter();

        [TestMethod]
        public void CanDesializeEmptyString()
        {
            var culture = (CultureInfo)converter.ConvertFromInvariantString("");
            Assert.IsNull(culture);
        }

        [TestMethod]
        public void CanDesializeNull()
        {
            var culture = (CultureInfo)converter.ConvertFromInvariantString(null);
            Assert.IsNull(culture);
        }

        [TestMethod]
        public void CanSerializeSpecificCulture()
        {
            var value = converter.ConvertToInvariantString(CultureInfo.GetCultureInfo("nl-NL"));
            Assert.AreEqual("nl-NL", value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotSerializeInvariantCulture()
        {
            converter.ConvertToInvariantString(CultureInfo.InvariantCulture);
        }
    }
}
