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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Console.Wpf.Tests.VSTS.BlockSpecific.Unity;

namespace Microsoft.Practices.Unity.Configuration.Design.Tests.given_namespace_element
{
    [TestClass]
    public class given_namespace_element : given_unity_section
    {
        protected ElementViewModel NamespaceElement;

        protected override void Arrange()
        {
            base.Arrange();

            var namespacesCollection = (ElementCollectionViewModel)UnitySectionViewModel.ChildElement("Namespaces");
            NamespaceElement = namespacesCollection.AddNewCollectionElement(typeof(NamespaceElement));
        }

        [TestMethod]
        public void then_name_property_has_suggested_namespaces()
        {
            var nameProperty = NamespaceElement.Property("Name");
            SuggestedValuesBindableProperty namePropertyBindable = nameProperty.BindableProperty as SuggestedValuesBindableProperty;

            Assert.IsTrue(namePropertyBindable.BindableSuggestedValues.Contains("System.Reflection"));
        }

        [TestMethod]
        public void then_name_property_is_editable()
        {
            var nameProperty = NamespaceElement.Property("Name");
            SuggestedValuesBindableProperty namePropertyBindable = nameProperty.BindableProperty as SuggestedValuesBindableProperty;

            Assert.IsFalse(nameProperty.ReadOnly);
            Assert.IsTrue(namePropertyBindable.SuggestedValuesEditable);
        }
    }
}
