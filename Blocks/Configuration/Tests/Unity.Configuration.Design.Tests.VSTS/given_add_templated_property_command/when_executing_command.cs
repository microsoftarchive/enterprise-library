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
using Console.Wpf.Tests.VSTS.TestSupport;

namespace Microsoft.Practices.Unity.Configuration.Design.Tests.given_add_templated_property_command
{
    [TestClass]
    public class when_executing_command : given_add_templated_property_command
    {
        protected override void Act()
        {
            base.AddTemplatedPropertyCommand.Execute(null);
        }

        [TestMethod]
        public void then_property_injection_member_is_added()
        {
            var propertyElement = base.InjectionMembersCollection.GetDescendentsOfType<PropertyElement>().FirstOrDefault();
            Assert.IsNotNull(propertyElement);
        }

        [TestMethod]
        public void then_property_injection_member_has_method_name()
        {
            var propertyElement = base.InjectionMembersCollection.GetDescendentsOfType<PropertyElement>().FirstOrDefault();
            Assert.AreEqual("Property", propertyElement.Property("Name").Value);
        }

    }
}
