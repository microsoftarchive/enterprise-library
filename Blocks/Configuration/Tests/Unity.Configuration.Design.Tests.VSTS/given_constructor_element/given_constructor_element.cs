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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.Unity.Configuration.Design.Tests
{
    public abstract class given_constructor_element : given_registration_element
    {
        protected ElementViewModel ConstructorElement;

        protected override void Arrange()
        {
            base.Arrange();

            ElementCollectionViewModel injectionMembersCollection = (ElementCollectionViewModel)RegistrationElement.ChildElement("InjectionMembers");
            ConstructorElement = injectionMembersCollection.AddNewCollectionElement(typeof(ConstructorElement));
        }

    }
}
