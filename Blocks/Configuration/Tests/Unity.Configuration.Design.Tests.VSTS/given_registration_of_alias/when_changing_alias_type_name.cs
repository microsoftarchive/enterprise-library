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

namespace Microsoft.Practices.Unity.Configuration.Design.Tests.given_registration_of_alias
{
    [TestClass]
    public class when_changing_alias_type_name : given_registration_of_alias
    {

        protected override void Act()
        {
            base.RegistrationTypeChanged = false;
            base.StringAlias.Property("TypeName").Value = "System.Guid, mscorlib";
        }

        [TestMethod]
        public void then_registration_type_returns_new_type()
        {
        }

        [TestMethod]
        public void then_registration_type_changed_event_was_fired()
        {
            Assert.IsTrue(base.RegistrationTypeChanged);
        }
    }
}
