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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.Unity.Configuration.Design.Tests
{
    [TestClass]
    public class when_adding_assembly_to_configuration_source_model : given_unity_section
    {
        ElementCollectionViewModel assembliesCollection;
        ElementViewModel addedAssembly;

        protected override void Arrange()
        {
            base.Arrange();
            assembliesCollection = (ElementCollectionViewModel)base.UnitySectionViewModel.ChildElement("Assemblies");

        }

        protected override void Act()
        {
            addedAssembly = assembliesCollection.AddNewCollectionElement(typeof(NamedElement));
        }

    }
}
