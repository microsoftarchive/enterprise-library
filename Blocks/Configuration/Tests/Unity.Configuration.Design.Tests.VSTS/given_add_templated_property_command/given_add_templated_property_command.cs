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

namespace Microsoft.Practices.Unity.Configuration.Design.Tests.given_add_templated_property_command
{
    public abstract class given_add_templated_property_command : given_registration_element
    {
        protected AddTemplatedInjectionPropertyCommand AddTemplatedPropertyCommand;
        protected InjectionMemberCollectionViewModel InjectionMembersCollection;

        protected override void Arrange()
        {
            base.Arrange();

            InjectionMembersCollection = (InjectionMemberCollectionViewModel)base.RegistrationElement.ChildElement("InjectionMembers");
            var property = typeof(ClassWithProperty).GetProperties().First();

            AddTemplatedPropertyCommand = new AddTemplatedInjectionPropertyCommand(
                                                    InjectionMembersCollection,
                                                    property,
                                                    UIServiceMock.Object);

            AddTemplatedPropertyCommand.DefaultCollectionElementAddCommandInitialization(Container.Resolve<IApplicationModel>());
        }


        protected class ClassWithProperty
        {
            public string Property { get; set; }
        }
    }
}
