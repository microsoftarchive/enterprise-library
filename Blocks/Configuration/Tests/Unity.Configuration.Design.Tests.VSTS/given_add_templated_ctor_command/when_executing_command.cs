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
using Console.Wpf.Tests.VSTS.TestSupport;

namespace Microsoft.Practices.Unity.Configuration.Design.Tests
{
    [TestClass]
    public class when_executing_add_templated_ctor_command : given_add_templated_ctor_command
    {
        protected override void Act()
        {
            base.AddTemplatedContructorCommand.Execute(null);
        }

        [TestMethod]
        public void then_contructor_injection_member_is_added()
        {
            var constructorEement = base.InjectionMembersCollection.GetDescendentsOfType<ConstructorElement>().FirstOrDefault();
            Assert.IsNotNull(constructorEement);
        }

        [TestMethod]
        public void then_constructor_has_int_argument_from_template()
        {
            var constructorEement = base.InjectionMembersCollection.GetDescendentsOfType<ConstructorElement>().FirstOrDefault();
            var intParameterElement = constructorEement.GetDescendentsOfType<ParameterElement>().Where(x => x.Name == "i").FirstOrDefault();
            
            Assert.IsNotNull(intParameterElement);
        }

        [TestMethod]
        public void then_constructor_has_string_argument_from_template()
        {
            var constructorEement = base.InjectionMembersCollection.GetDescendentsOfType<ConstructorElement>().FirstOrDefault();
            var stringParameterElement = constructorEement.GetDescendentsOfType<ParameterElement>().Where(x => x.Name == "s").FirstOrDefault();

            Assert.IsNotNull(stringParameterElement);
        }

        [TestMethod]
        public void then_cannot_execute_command_again()
        {
            Assert.IsFalse(base.AddTemplatedContructorCommand.CanExecute(null));
        }
    }
}
