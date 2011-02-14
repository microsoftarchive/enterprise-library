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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.Unity;

namespace Microsoft.Practices.Unity.Configuration.Design.Tests
{
    [TestClass]
    public class when_adding_unity_registration_to_configuration_source_model : given_container_element
    {
        ElementViewModel elementViewModel;

        protected override void Arrange()
        {
            base.Arrange();
        }

        protected override void Act()
        {
            ElementCollectionViewModel registrationsCollection = (ElementCollectionViewModel)ContainerElement.ChildElement("Registrations");
            elementViewModel = registrationsCollection.AddNewCollectionElement(typeof(RegisterElement));
        }

        [TestMethod]
        public void then_registration_has_editor_on_type_name_property()
        {
            Assert.IsNotNull(elementViewModel.Property("TypeName"));
            Assert.IsNotNull(elementViewModel.Property("TypeName").BindableProperty as PopupEditorBindableProperty);
        }

        [TestMethod]
        public void then_registration_has_custom_view_model_type()
        {
            Assert.IsInstanceOfType(elementViewModel,typeof(RegisterElementViewModel));
        }
    }
}
