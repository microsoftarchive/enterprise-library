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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.Unity;

namespace Microsoft.Practices.Unity.Configuration.Design.Tests
{
    public abstract class given_registration_element : given_container_element
    {
        protected bool RegistrationTypeChanged;
        protected RegisterElementViewModel RegistrationElement;

        protected override void Arrange()
        {
            RegistrationTypeChanged = false;

            base.Arrange();

            ElementCollectionViewModel registrationsCollection = (ElementCollectionViewModel)base.ContainerElement.ChildElement("Registrations");
            RegistrationElement = (RegisterElementViewModel)registrationsCollection.AddNewCollectionElement(typeof(RegisterElement));

            RegistrationElement.RegistrationTypeChanged
                += (sender, args) => RegistrationTypeChanged = true;

            RegistrationElement.Initialize(new InitializeContext());
        }
    }
}
