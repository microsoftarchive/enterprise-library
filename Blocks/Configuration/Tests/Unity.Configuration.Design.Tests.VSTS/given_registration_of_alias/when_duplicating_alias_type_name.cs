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
    public class when_duplicating_alias_type_name : given_registration_of_alias
    {
        protected override void Act()
        {
            base.StringAlias.Property("Alias").Value = "i";
        }

        [TestMethod]
        public void then_results_in_duplicate_validation_error()
        {
            Assert.IsTrue(base.StringAlias.Property("Alias").ValidationResults.Where(x => x.Message.Contains("Duplicate")).Any());
        }
    }

    [TestClass]
    public class when_removing_duplicate_name_values : given_registration_of_alias
    {
        protected override void Arrange()
        {
            base.Arrange();
            base.StringAlias.Property("Alias").Value = "i";
        }

        protected override void Act()
        {
            base.StringAlias.Property("Alias").Value = "other";
        }

        [TestMethod]
        public void then_property_has_no_validation_errors()
        {
            Assert.IsFalse(base.StringAlias.Property("Alias").ValidationResults.Where(x => x.Message.Contains("Duplicate")).Any());
        }

        [TestMethod]
        public void then_no_alias_has_duplicate()
        {
            Assert.IsFalse(base.Int32Alias.Property("Alias").ValidationResults.Where(x => x.Message.Contains("Duplicate")).Any());
        }
        
    }
}
