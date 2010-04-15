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
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Console.Wpf.Tests.VSTS.BlockSpecific.Overrides;

namespace Console.Wpf.Tests.VSTS.DevTests.given_environment_node_in_view_model
{
    [TestClass]
    public class when_adding_second_with_same_name : given_two_overrides_sections
    {
        protected override void Act()
        {
            environment2.NameProperty.Value = environment1.NameProperty.Value;
        }

        [TestMethod]
        public void then_environment_has_validation_error()
        {
            Assert.AreEqual(1, environment2.NameProperty.ValidationResults.Where(x => x.IsError).Count());
        }

        [TestMethod]
        public void then_assigning_new_name_fixed_error()
        {
            environment2.NameProperty.Value = "new name";
            Assert.AreEqual(0, environment2.NameProperty.ValidationResults.Where(x => x.IsError).Count());
        }
    }
}
