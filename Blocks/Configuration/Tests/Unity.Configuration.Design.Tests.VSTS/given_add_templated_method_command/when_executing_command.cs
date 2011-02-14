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

namespace Microsoft.Practices.Unity.Configuration.Design.Tests.given_add_templated_method_command
{
    [TestClass]
    public class when_executing_command : given_add_templated_method_command
    {
        protected override void Act()
        {
            base.AddTemplatedMethodCommand.Execute(null);
        }

        [TestMethod]
        public void then_method_injection_member_is_added()
        {
            var methodElement = base.InjectionMembersCollection.GetDescendentsOfType<MethodElement>().FirstOrDefault();
            Assert.IsNotNull(methodElement);
        }

        [TestMethod]
        public void then_method_injection_member_has_method_name()
        {
            var methodElement = base.InjectionMembersCollection.GetDescendentsOfType<MethodElement>().FirstOrDefault();
            Assert.AreEqual("MethodName", methodElement.Property("Name").Value);
        }

        [TestMethod]
        public void then_method_has_int_argument_from_template()
        {
            var methodElement = base.InjectionMembersCollection.GetDescendentsOfType<MethodElement>().FirstOrDefault();
            var intParameterElement = methodElement.GetDescendentsOfType<ParameterElement>().Where(x => x.Name == "i").FirstOrDefault();

            Assert.IsNotNull(intParameterElement);
        }

        [TestMethod]
        public void then_method_has_string_argument_from_template()
        {
            var methodEement = base.InjectionMembersCollection.GetDescendentsOfType<MethodElement>().FirstOrDefault();
            var stringParameterElement = methodEement.GetDescendentsOfType<ParameterElement>().Where(x => x.Name == "s").FirstOrDefault();

            Assert.IsNotNull(stringParameterElement);
        }

    }
}
