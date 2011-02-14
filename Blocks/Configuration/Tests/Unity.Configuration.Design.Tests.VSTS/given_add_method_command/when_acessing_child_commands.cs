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
using System.Reflection;
using Microsoft.Practices.Unity.Configuration.Design.Commands;

namespace Microsoft.Practices.Unity.Configuration.Design.Tests.given_add_method_command
{
    [TestClass]
    public class when_acessing_child_commands : given_add_method_command
    {
        IEnumerable<CommandModel> childCommands;
        IEnumerable<AddTemplatedRegistrationMethodCommand> addTemplatedRegistrationMethodCommands;

        protected override void Act()
        {
            childCommands = base.AddRegistrationMethodCommand.ChildCommands;
            addTemplatedRegistrationMethodCommands = AddRegistrationMethodCommand
                            .TemplateCommands
                            .Cast<AddTemplatedRegistrationMethodCommand>();
        }

        [TestMethod]
        public void then_default_add_command_is_contained_in_child_commands()
        {
            Assert.IsTrue(childCommands.Contains(AddRegistrationMethodCommand.DefaultAddCommand));
        }

        [TestMethod]
        public void then_has_templated_add_command_for_non_static_methods()
        {
            Assert.IsTrue(addTemplatedRegistrationMethodCommands.Any(x => x.Method.Name == "ParameterlessMethod"));
            Assert.IsTrue(addTemplatedRegistrationMethodCommands.Any(x => x.Method.Name == "MethodWithSingleArg"));
            Assert.IsTrue(addTemplatedRegistrationMethodCommands.Any(x => x.Method.Name == "MethodWithListArgAndReturnValue"));
        }

        [TestMethod]
        public void then_doesnt_have_templated_add_command_for_static_methods()
        {
            Assert.IsFalse(addTemplatedRegistrationMethodCommands.Any(x => x.Method.Name == "Static"));
        }

        [TestMethod]
        public void then_doesnt_have_templated_no_add_command_for_object_members()
        {
            Assert.IsFalse(addTemplatedRegistrationMethodCommands.Any(x => x.Method.Name == "ToString"));
            Assert.IsFalse(addTemplatedRegistrationMethodCommands.Any(x => x.Method.Name == "GetType"));
            Assert.IsFalse(addTemplatedRegistrationMethodCommands.Any(x => x.Method.Name == "Equals"));
        }

        [TestMethod]
        public void then_doesnt_have_property_accessors()
        {
            Assert.IsFalse(addTemplatedRegistrationMethodCommands.Any(x => x.Method.IsSpecialName));
        }
    }
}
