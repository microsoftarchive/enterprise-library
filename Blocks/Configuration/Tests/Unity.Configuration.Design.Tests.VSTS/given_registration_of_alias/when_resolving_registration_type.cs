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
    public class when_resolving_registration_type : given_registration_of_alias
    {
        Type resolvedType;

        protected override void Act()
        {
            resolvedType = base.RegistrationElement.RegistrationType;
        }

        [TestMethod]
        public void then_resolved_type_is_aliased_type()
        {
            Assert.AreEqual(typeof(string), resolvedType);
        }
    }
}
