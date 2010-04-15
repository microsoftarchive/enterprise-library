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
using Console.Wpf.Tests.VSTS.Contexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.DevTests.given_a_validation_service
{
    [TestClass]
    public class when_validating_invalid_type_property : SectionWithValidatablePropertiesContext
    {
        private const string invalidTypeName = "!@AVeryInvalidTypeName";
        protected override string ArrangePropertyName()
        {
            return "ValidatedTypeName";
        }

        protected override void Act()
        {
            Assert.IsFalse(property.ValidationResults.Any());
            property.Value = invalidTypeName;
        }

        [TestMethod]
        public void then_error_returned()
        {
            Assert.IsTrue(property.ValidationResults.Any(e => e.Message.Contains(invalidTypeName)));
        }

        [TestMethod]
        public void then_error_is_warning()
        {
            var error = property.ValidationResults.Single(e => e.Message.Contains(invalidTypeName));
            Assert.IsTrue(error.IsWarning);
        }
    }

    [TestClass]
    public class when_validating_valid_type_property : SectionWithValidatablePropertiesContext
    {
        protected override string ArrangePropertyName()
        {
            return "ValidatedTypeName";
        }

        protected override void Act()
        {
            Assert.IsFalse(property.ValidationResults.Any());
            property.Value = typeof(when_validating_valid_type_property).AssemblyQualifiedName;
        }

        [TestMethod]
        public void then_no_errors_produced()
        {
            Assert.IsFalse(property.ValidationResults.Any());
        }
    }
}
