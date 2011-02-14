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
using Microsoft.Practices.Unity.Configuration.Design.Commands;

namespace Microsoft.Practices.Unity.Configuration.Design.Tests.given_add_method_command
{
    public abstract class given_add_method_command : given_registration_element
    {
        protected AddRegistrationMethodCommand AddRegistrationMethodCommand;

        protected override void Arrange()
        {
            base.Arrange();

            RegistrationElement.Property("TypeName").Value = typeof(RegistrationType).AssemblyQualifiedName;

            AddRegistrationMethodCommand = RegistrationElement.AddCommands
                                                       .SelectMany(x => x.ChildCommands)
                                                       .OfType<AddRegistrationMethodCommand>()
                                                       .FirstOrDefault();
        }


        protected class RegistrationType
        {
            public void ParameterlessMethod()
            {
            }

            public void MethodWithSingleArg(string arg)
            {
            }

            public int MethodWithListArgAndReturnValue(IList<string> arg)
            {
                return 0;
            }

            public static int Static()
            {
                return 0;
            }

            public bool PropertyWithAccessors
            {
                get;
                set;
            }

        }
    }
}
