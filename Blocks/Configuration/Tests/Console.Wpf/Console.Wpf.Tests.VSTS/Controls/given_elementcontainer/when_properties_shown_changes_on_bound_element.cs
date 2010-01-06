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
using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.Controls.given_elementcontainer
{
    [TestClass]
    public class when_properties_shown_changes_on_bound_element : PopulatedElementModelContainer
    {

        protected override void Act()
        {
            Element.PropertiesShown = true;
        }

        [TestMethod]
        public void then_element_container_is_expanded()
        {
            Assert.IsTrue(ElementContainer.IsExpanded);
        }
    }

    [TestClass]
    public class when_properties_not_shown_changes_on_bound_element : PopulatedElementModelContainer
    {

        protected override void Act()
        {
            Element.PropertiesShown = false;
        }

        [TestMethod]
        public void then_element_container_is_expanded()
        {
            Assert.IsFalse(ElementContainer.IsExpanded);
        }
    }
}
