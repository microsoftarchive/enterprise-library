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

namespace Microsoft.Practices.Unity.Configuration.Design.Tests.given_method_element
{
    [TestClass]
    public class when_displaying_method_element : given_method_element
    {
        protected override void Act()
        {
            MethodElement.Property("Name").Value = "abc";
        }

        [TestMethod]
        public void then_method_element_name_contains_method()
        {
            Assert.IsTrue(MethodElement.Name.IndexOf("Method", StringComparison.OrdinalIgnoreCase) > -1);
        }

        [TestMethod]
        public void then_method_element_name_contains_method_name()
        {
            Assert.IsTrue(MethodElement.Name.IndexOf("abc") > -1);
        }
    }
}
