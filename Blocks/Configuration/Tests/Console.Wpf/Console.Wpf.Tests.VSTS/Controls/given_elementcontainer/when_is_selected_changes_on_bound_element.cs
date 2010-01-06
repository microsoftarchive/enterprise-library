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
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls;
using Console.Wpf.Tests.VSTS.Mocks;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Input;

namespace Console.Wpf.Tests.VSTS.Controls.given_elementcontainer
{
    public abstract class PopulatedElementModelContainer : ContainerContext
    {
        
        protected override void Arrange()
        {
            base.Arrange();

            var configurationSection = new MockSectionWithSingleChild
            {
                Children = 
                {
                    {new TestHandlerDataWithChildren{Name = "Element"} }
                }
            };

            SectionViewModel sectionViewmodel = SectionViewModel.CreateSection(Container, "MockSection", configurationSection);
            Element = sectionViewmodel.GetDescendentsOfType<TestHandlerDataWithChildren>().First();
            Element.IsSelected = false;
            ElementContainer = new ElementModelContainer { DataContext = Element };
        }

        protected ElementModelContainer ElementContainer { get; private set; }
        protected ElementViewModel Element { get; private set; }
    }

    [TestClass]
    public class when_is_selected_changes_on_bound_element : PopulatedElementModelContainer
    {

        protected override void Act()
        {
            Element.IsSelected = true;
        }

        [TestMethod]
        public void then_element_container_has_focus()
        {
            Assert.AreSame(FocusManager.GetFocusedElement(ElementContainer), ElementContainer);
        }
    }
}
