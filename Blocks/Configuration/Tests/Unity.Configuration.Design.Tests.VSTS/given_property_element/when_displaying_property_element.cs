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
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Unity.Configuration.Design.Tests.given_property_element
{
    [TestClass]
    public class when_displaying_property_element : given_property_element
    {
        protected override void Act()
        {
            PropertyElement.Property("Name").Value = "abc";
        }

        [TestMethod]
        public void then_property_element_name_contains_property()
        {
            Assert.IsTrue(PropertyElement.Name.IndexOf("Property", StringComparison.OrdinalIgnoreCase) > -1);
        }

        [TestMethod]
        public void then_property_element_name_contains_property_name()
        {
            Assert.IsTrue(PropertyElement.Name.IndexOf("abc") > -1);
        }
    }
}
