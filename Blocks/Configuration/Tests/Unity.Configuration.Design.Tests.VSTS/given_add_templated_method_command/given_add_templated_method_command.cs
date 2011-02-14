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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;

namespace Microsoft.Practices.Unity.Configuration.Design.Tests.given_add_templated_method_command
{
    public abstract class given_add_templated_method_command : given_registration_element
    {
        protected AddTemplatedRegistrationMethodCommand AddTemplatedMethodCommand;
        protected InjectionMemberCollectionViewModel InjectionMembersCollection;

        protected override void Arrange()
        {
            base.Arrange();

            InjectionMembersCollection = (InjectionMemberCollectionViewModel)base.RegistrationElement.ChildElement("InjectionMembers");
            var method = typeof(ClassWithIntAndStringMethod).GetMethods().First();

            AddTemplatedMethodCommand = new AddTemplatedRegistrationMethodCommand(
                                                    InjectionMembersCollection,
                                                    method,
                                                    UIServiceMock.Object);

            AddTemplatedMethodCommand.DefaultCollectionElementAddCommandInitialization(Container.Resolve<IApplicationModel>());
        }


        protected class ClassWithIntAndStringMethod
        {
            public int MethodName(int i, string s)
            {
                return 0;
            }
        }
    }
}
