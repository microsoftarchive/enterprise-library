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

namespace Console.Wpf.Tests.VSTS.BlockSpecific.Overrides
{
    [TestClass]
    public class when_adding_two_overrides_with_same_delta_file : given_two_overrides_sections
    {
        protected override void Act()
        {
            environment1.Property("EnvironmentDeltaFile").Value = "file.dconfig";
            environment2.Property("EnvironmentDeltaFile").Value = "file.dconfig";
        }

        [TestMethod]
        public void then_environment_has_validation_error()
        {
            Assert.AreEqual(1, environment2.Property("EnvironmentDeltaFile").ValidationResults.Where(x => x.IsError).Count());
        }

        [TestMethod]
        public void then_assigning_new_value_fixed_error()
        {
            environment2.Property("EnvironmentDeltaFile").Value = "other.dconfig";
            Assert.AreEqual(0, environment2.Property("EnvironmentDeltaFile").ValidationResults.Where(x => x.IsError).Count());
        }
    }
}
