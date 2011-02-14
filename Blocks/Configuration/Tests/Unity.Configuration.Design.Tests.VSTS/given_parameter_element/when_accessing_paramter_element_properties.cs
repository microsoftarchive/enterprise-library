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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Unity.Configuration.Design.Tests
{
    [TestClass]
    public class when_accessing_paramter_element_properties : given_parameter_element
    {
        IEnumerable<Property> properties;

        protected override void Act()
        {
            properties = base.ParameterElement.Properties;
        }

        [TestMethod]
        public void then_parameter_element_has_value_property()
        {
            Assert.IsTrue(properties.Any(x => x.PropertyName == "Value"));
        }

    }
}
