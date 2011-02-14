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

namespace Microsoft.Practices.Unity.Configuration.Design.Tests
{
    [TestClass]
    public class when_accessing_register_element_commands: given_registration_element
    {
        IEnumerable<CommandModel> registrationAddCommands;
        IEnumerable<CommandModel> commands;

        protected override void Act()
        {
            commands = RegistrationElement.Commands;
            registrationAddCommands = RegistrationElement.AddCommands.SelectMany(x => x.ChildCommands);
        }

        [TestMethod]
        public void then_method_can_be_added()
        {
            AddRegistrationMethodCommand addRegistrationMethodCommand = registrationAddCommands.OfType<AddRegistrationMethodCommand>().FirstOrDefault();
            Assert.IsNotNull(addRegistrationMethodCommand);
            Assert.IsNotNull(addRegistrationMethodCommand.DefaultAddCommand);

            Assert.IsTrue(addRegistrationMethodCommand.DefaultAddCommand.CanExecute(null));
        }

        [TestMethod]
        public void then_property_can_be_added()
        {
            AddRegistrationPropertyCommand addRegistrationPropertyCommand = registrationAddCommands.OfType<AddRegistrationPropertyCommand>().FirstOrDefault();
            Assert.IsNotNull(addRegistrationPropertyCommand);
            Assert.IsNotNull(addRegistrationPropertyCommand.DefaultAddCommand);

            Assert.IsTrue(addRegistrationPropertyCommand.DefaultAddCommand.CanExecute(null));
        }

        [TestMethod]
        public void then_constructor_can_be_added()
        {
            AddRegistrationConstructorCommand addConstrcutorCommand = registrationAddCommands.OfType<AddRegistrationConstructorCommand>().FirstOrDefault();
            Assert.IsNotNull(addConstrcutorCommand);
            Assert.IsNotNull(addConstrcutorCommand.DefaultAddCommand);

            Assert.IsTrue(addConstrcutorCommand.DefaultAddCommand.CanExecute(null));
        }
    }
}
