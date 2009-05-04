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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation.Tests
{
    [TestClass]
    public class ValidationAttributeExceptionFixture : ConfigurationDesignHost
    {
        TypeValidationAttribute attribute = new TypeValidationAttribute();
        string name = "Name";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidateWithANullInstanceThrows()
        {
            PropertyInfo info = name.GetType().GetProperty("Length");
            attribute.Validate(null, info, new List<ValidationError>(), ServiceProvider);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidateWithANullPropertyThrows()
        {
            attribute.Validate(new ConfigurationApplicationNode(ConfigurationApplicationFile.FromCurrentAppDomain()), null, new List<ValidationError>(), ServiceProvider);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidateWithANullErrorListThrows()
        {
            PropertyInfo info = name.GetType().GetProperty("Length");
            attribute.Validate(new ConfigurationApplicationNode(ConfigurationApplicationFile.FromCurrentAppDomain()), info, null, ServiceProvider);
        }
    }
}
