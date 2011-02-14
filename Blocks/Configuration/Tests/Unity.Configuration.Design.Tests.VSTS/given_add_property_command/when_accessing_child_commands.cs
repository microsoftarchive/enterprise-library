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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.Unity.Configuration.Design.Commands;

namespace Microsoft.Practices.Unity.Configuration.Design.Tests.given_add_property_command
{
    [TestClass]
    public class when_accessing_add_property_child_commands : given_add_property_command
    {
        IEnumerable<CommandModel> childCommands;

        protected override void Act()
        {
            childCommands = base.AddRegistrationPropertyCommand.ChildCommands;
        }

        [TestMethod]
        public void then_default_add_command_is_contained_in_child_commands()
        {
            Assert.IsTrue(childCommands.Contains(AddRegistrationPropertyCommand.DefaultAddCommand));
        }

        [TestMethod]
        public void then_command_has_child_command_for_string_property()
        {
            var stringProperty = typeof(RegistrationType).GetProperty("StringProperty");
            Assert.IsTrue(childCommands.OfType<AddTemplatedInjectionPropertyCommand>().Any(x => x.Property == stringProperty));
        }

        [TestMethod]
        public void then_command_has_child_command_for_set_only_property()
        {
            var intProperty = typeof(RegistrationType).GetProperty("SetOnlyProperty");
            Assert.IsTrue(childCommands.OfType<AddTemplatedInjectionPropertyCommand>().Any(x => x.Property == intProperty));
        }

        [TestMethod]
        public void then_command_doesnt_not_have_child_command_for_readonly_property()
        {
            var intProperty = typeof(RegistrationType).GetProperty("Readonly");
            Assert.IsFalse(childCommands.OfType<AddTemplatedInjectionPropertyCommand>().Any(x => x.Property == intProperty));
        }

        [TestMethod]
        public void then_command_doesnt_not_have_child_command_for_static_property()
        {
            var intProperty = typeof(RegistrationType).GetProperty("Static");
            Assert.IsFalse(childCommands.OfType<AddTemplatedInjectionPropertyCommand>().Any(x => x.Property == intProperty));
        }

        [TestMethod]
        public void then_command_doesnt_have_child_command_for_property_with_args()
        {
            Assert.IsFalse(childCommands.OfType<AddTemplatedInjectionPropertyCommand>().Any(x => x.Property.GetIndexParameters().Length > 0));
        }
    }
}
