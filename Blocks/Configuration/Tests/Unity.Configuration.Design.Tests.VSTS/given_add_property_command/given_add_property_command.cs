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

namespace Microsoft.Practices.Unity.Configuration.Design.Tests.given_add_property_command
{
    public abstract class given_add_property_command : given_registration_element
    {
        protected AddRegistrationPropertyCommand AddRegistrationPropertyCommand;

        protected override void Arrange()
        {
            base.Arrange();

            RegistrationElement.Property("TypeName").Value = typeof(RegistrationType).AssemblyQualifiedName;

            AddRegistrationPropertyCommand = RegistrationElement.AddCommands
                                                       .SelectMany(x => x.ChildCommands)
                                                       .OfType<AddRegistrationPropertyCommand>()
                                                       .FirstOrDefault();
        }


        protected class RegistrationType
        {
            public string Readonly
            {
                get
                {
                    return "a";
                }
            }

            public static int Static
            {
                get { return 0; }
                set { }
            }

            public int this[int i]
            {
                get { return 1; }
                set { }
            }

            public string StringProperty { get; set; }

            public int SetOnlyProperty { set { } }

        }
    }
}
