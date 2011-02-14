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
using Console.Wpf.Tests.VSTS.TestSupport;


namespace Microsoft.Practices.Unity.Configuration.Design.Tests
{
    public abstract class given_container_element : given_unity_section
    {
        protected ElementViewModel ContainerElement;

        protected override void Arrange()
        {
            base.Arrange();

            var containersCollection = (ElementCollectionViewModel)UnitySectionViewModel.ChildElement("Containers");
            containersCollection.AddCommands.First().Execute(null);

            ContainerElement = UnitySectionViewModel.GetDescendentsOfType<ContainerElement>().First();
        }
    }
}
