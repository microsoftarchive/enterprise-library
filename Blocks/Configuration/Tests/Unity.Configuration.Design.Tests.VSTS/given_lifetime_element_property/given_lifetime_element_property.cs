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
using Microsoft.Practices.Unity.Configuration.Design.ViewModel;

namespace Microsoft.Practices.Unity.Configuration.Design.Tests.given_lifetime_element_property
{
    public class given_lifetime_element_property : given_registration_element
    {
        protected LifetimeElementProperty LifetimeElementProperty;

        protected override void Arrange()
        {
            base.Arrange();

            LifetimeElementProperty = base.RegistrationElement.Properties.OfType<LifetimeElementProperty>().Single();
        }
    }
}
