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
using System.Collections;

namespace Microsoft.Practices.Unity.Configuration.Design.Tests
{
    public abstract class given_add_constructor_command : given_registration_element
    {
        protected AddRegistrationConstructorCommand AddConstructorCommand;

        protected override void Arrange()
        {
            base.Arrange();

            RegistrationElement.Property("TypeName").Value = typeof(RegistrationType).AssemblyQualifiedName;

            AddConstructorCommand = RegistrationElement.AddCommands
                                                       .SelectMany(x => x.ChildCommands)
                                                       .OfType<AddRegistrationConstructorCommand>()
                                                       .FirstOrDefault();
        }


        protected class RegistrationType
        {
            public RegistrationType()
            {
            }

            protected RegistrationType(Guid arg)
            {
            }

            public RegistrationType(string arg)
            {
            }

            public RegistrationType(IList<string> arg)
            {
            }

            public RegistrationType(int i, string s)
            {
            }
        }
    }
}
