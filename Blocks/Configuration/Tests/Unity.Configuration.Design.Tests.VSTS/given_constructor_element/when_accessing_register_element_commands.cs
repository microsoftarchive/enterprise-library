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
    public class when_accessing_register_element_commands_after_adding_constuctor_element : given_constructor_element
    {
        IEnumerable<CommandModel> registrationAddCommands;

        protected override void Act()
        {
            registrationAddCommands = RegistrationElement.AddCommands.SelectMany(x => x.ChildCommands);
        }

        [TestMethod]
        public void then_add_constructor_element_cannot_be_executed()
        {
            var addConstructorElement = registrationAddCommands.OfType<AddRegistrationConstructorCommand>().First();
            Assert.IsFalse(addConstructorElement.DefaultAddCommand.CanExecute(null));
        }
    }
}
