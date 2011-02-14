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

namespace Microsoft.Practices.Unity.Configuration.Design.Tests
{
    [TestClass]
    public class when_setting_map_to_name_property : given_registration_element
    {
        protected override void Act()
        {
            base.RegistrationElement.Property("MapToName").Value = typeof(int).AssemblyQualifiedName;
        }

        [TestMethod]
        public void then_registration_type_changed_event_fired()
        {
            Assert.IsTrue(RegistrationTypeChanged);
        }

        [TestMethod]
        public void then_registration_type_reflects_new_value()
        {
            Assert.AreEqual(typeof(int),
                    base.RegistrationElement.RegistrationType);
        }
    }
}
