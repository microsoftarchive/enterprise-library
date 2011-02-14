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

using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity.Configuration.Design.Commands;

namespace Microsoft.Practices.Unity.Configuration.Design.Tests
{
    public abstract class given_add_templated_ctor_command : given_registration_element
    {
        protected AddTemplatedRegistrationConstructorCommand AddTemplatedContructorCommand;
        protected InjectionMemberCollectionViewModel InjectionMembersCollection;

        protected override void Arrange()
        {
            base.Arrange();

            InjectionMembersCollection = (InjectionMemberCollectionViewModel)base.RegistrationElement.ChildElement("InjectionMembers");
            var constructor = typeof(ClassWithIntAndStringConstructor).GetConstructors().First();

            AddTemplatedContructorCommand = new AddTemplatedRegistrationConstructorCommand(
                                                    InjectionMembersCollection,
                                                    constructor,
                                                    UIServiceMock.Object);

            AddTemplatedContructorCommand.DefaultCollectionElementAddCommandInitialization(Container.Resolve<IApplicationModel>());
        }


        protected class ClassWithIntAndStringConstructor
        {
            public ClassWithIntAndStringConstructor(int i, string s)
            {

            }
        }
    }
}
