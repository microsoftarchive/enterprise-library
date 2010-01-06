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

using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.Tests.VSTS.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Console.Wpf.Tests.VSTS.DevTests.given_view_model_and_config_collection
{
    public abstract class SectionWithMultipleChildrenContext : ContainerContext
    {
        protected override void Arrange()
        {
            base.Arrange();

            var section = new MockSectionWithMultipleChildCollections();
            BuildSection(section);
            ViewModel = SectionViewModel.CreateSection(Container, "name", section);
            
        }

        protected virtual void BuildSection(MockSectionWithMultipleChildCollections section)
        {
            section.Children.Add(new TestHandlerData() { Name = "One" });
            section.Children.Add(new TestHandlerData() { Name = "Two" });
            section.Children.Add(new TestHandlerData() { Name = "Three" });
        }

        protected SectionViewModel ViewModel { get; set; }
    }
}
