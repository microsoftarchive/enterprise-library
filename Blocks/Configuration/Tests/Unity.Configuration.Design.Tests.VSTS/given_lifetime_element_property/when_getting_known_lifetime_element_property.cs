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
    public class when_getting_known_lifetime_element_property : given_lifetime_element_property
    {
        protected override void Arrange()
        {
            base.Arrange();

            ((RegisterElement)base.RegistrationElement.ConfigurationElement).Lifetime = new LifetimeElement()
                                                                                            {
                                                                                                TypeName = "PerThreadLifetimeManager"   
                                                                                            };
            LifetimeElementProperty.RecreateLifetimeElementViewModel();
        }

        [TestMethod]
        public void then_bindable_value_is_specific_lifetime_manager_type_name()
        {
            Assert.AreEqual("PerThreadLifetimeManager", LifetimeElementProperty.BindableProperty.BindableValue);
        }

        [TestMethod]
        public void then_property_has_no_child_properties()
        {
            Assert.IsFalse(LifetimeElementProperty.HasChildProperties);
            Assert.AreEqual(0, LifetimeElementProperty.ChildProperties.Count());
        }

    }
}
