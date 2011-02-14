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
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.Unity.Configuration.Design.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Unity.Configuration.Design.Tests
{
    [TestClass]
    public class when_registration_type_changes : given_registration_element
    {
        IEnumerable<PropertyChangedListener> templatedInjectionMemberContainerListeners;

        protected override void Arrange()
        {
            base.Arrange();

            var templatedInjectionMemberCommandContainers =
                    base.RegistrationElement.Commands
                        .Where(x => x.ChildCommands != null)
                        .SelectMany(x => x.ChildCommands)
                        .OfType<TemplatedInjectionMemberCommandContainerBase>();

            templatedInjectionMemberContainerListeners = templatedInjectionMemberCommandContainers.Select(
                                                            x => new PropertyChangedListener(x)).ToArray();
        }

        protected override void Act()
        {
            base.RegistrationElement.Property("TypeName").Value = typeof(string).AssemblyQualifiedName;
        }

        [TestMethod]
        public void then_all_templated_injection_member_containers_signed_child_property_changed()
        {
            Assert.IsTrue(templatedInjectionMemberContainerListeners.All(x => x.ChangedProperties.Contains("ChildCommands")));
        }
    }
}
