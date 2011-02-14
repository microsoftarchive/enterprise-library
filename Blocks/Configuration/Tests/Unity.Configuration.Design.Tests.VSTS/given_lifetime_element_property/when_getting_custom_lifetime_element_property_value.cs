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

namespace Microsoft.Practices.Unity.Configuration.Design.Tests.given_lifetime_element_property
{
    [TestClass]
    public class when_getting_custom_lifetime_element_property_value : given_lifetime_element_property
    {
        protected override void Arrange()
        {
            base.Arrange();

            ((RegisterElement)RegistrationElement.ConfigurationElement).Lifetime = new LifetimeElement()
            {
                TypeName = "CustomLifetime, assembly"
            };
            LifetimeElementProperty.RecreateLifetimeElementViewModel();
        }

        [TestMethod]
        public void then_bindable_value_has_child_properties()
        {
            Assert.IsTrue(LifetimeElementProperty.HasChildProperties);
            Assert.AreNotEqual(0, LifetimeElementProperty.ChildProperties.Count());
        }

        [TestMethod]
        public void then_type_child_property_reflects_lifetime()
        {
            var lifetimeTypeChildProperty =
                LifetimeElementProperty.ChildProperties.First(x => x.DeclaringProperty.Name == "TypeName");
            Assert.AreEqual("CustomLifetime, assembly", lifetimeTypeChildProperty.Value);
        }

    }
}
