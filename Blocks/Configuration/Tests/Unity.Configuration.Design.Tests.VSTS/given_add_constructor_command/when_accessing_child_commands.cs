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

namespace Microsoft.Practices.Unity.Configuration.Design.Tests
{
    [TestClass]
    public class when_accessing_child_commands : given_add_constructor_command
    {
        IEnumerable<CommandModel> childCommands;

        protected override void Act()
        {
            childCommands = base.AddConstructorCommand.ChildCommands;
        }

        [TestMethod]
        public void then_default_add_command_is_contained_in_child_commands()
        {
            Assert.IsTrue(childCommands.Contains(AddConstructorCommand.DefaultAddCommand));
        }

        [TestMethod]
        public void then_has_templated_add_command_for_nondefault_constructors()
        {
            foreach (ConstructorInfo ctor in typeof(RegistrationType).GetConstructors())
            {
                if (ctor.GetParameters().Length > 0)
                {
                    Assert.IsTrue(AddConstructorCommand.TemplateCommands
                                        .Cast<AddTemplatedRegistrationConstructorCommand>()
                                        .Any(x => x.Constructor == ctor));
                }
            }
        }

        [TestMethod]
        public void then_doesn_have_templated_add_command_for_protected_constructors()
        {
            foreach (ConstructorInfo ctor in typeof(RegistrationType).GetConstructors())
            {
                if (ctor.IsAbstract)
                {
                    Assert.IsFalse(AddConstructorCommand.TemplateCommands
                                        .Cast<AddTemplatedRegistrationConstructorCommand>()
                                        .Any(x => x.Constructor == ctor));
                }
            }
        }
    }
}
