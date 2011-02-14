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
    public class when_setting_null_lifetime_element_property : given_lifetime_element_property
    {
        protected override void Act()
        {
            base.LifetimeElementProperty.Value = "<none>";
        }

        [TestMethod]
        public void then_underlying_lifetime_element_is_null()
        {
            Assert.IsNull(((RegisterElement)RegistrationElement.ConfigurationElement).Lifetime);
        }
    }
}
