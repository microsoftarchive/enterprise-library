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
using Console.Wpf.Tests.VSTS.BlockSpecific.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Unity.Configuration.Design.Tests.given_assembly_element
{
    [TestClass]
    public class given_assembly_element: given_unity_section
    {
        protected ElementViewModel AssemblyElement;

        protected override void Arrange()
        {
            base.Arrange();

            var assembliesCollection = (ElementCollectionViewModel)UnitySectionViewModel.ChildElement("Assemblies");
            AssemblyElement = assembliesCollection.AddNewCollectionElement(typeof(AssemblyElement));
        }

        [TestMethod]
        public void then_name_property_has_suggested_asssemblies()
        {
            var nameProperty = AssemblyElement.Property("Name");
            SuggestedValuesBindableProperty namePropertyBindable = nameProperty.BindableProperty as SuggestedValuesBindableProperty;

            Assert.IsTrue(namePropertyBindable.BindableSuggestedValues.Contains("mscorlib"));
        }

        [TestMethod]
        public void then_name_property_is_editable()
        {
            var nameProperty = AssemblyElement.Property("Name");
            SuggestedValuesBindableProperty namePropertyBindable = nameProperty.BindableProperty as SuggestedValuesBindableProperty;

            Assert.IsFalse(nameProperty.ReadOnly);
            Assert.IsTrue(namePropertyBindable.SuggestedValuesEditable);
        }
    }
}
