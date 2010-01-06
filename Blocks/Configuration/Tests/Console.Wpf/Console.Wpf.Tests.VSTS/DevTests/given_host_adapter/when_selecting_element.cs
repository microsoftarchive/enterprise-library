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
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.HostAdapter;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Hosting;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Console.Wpf.Tests.VSTS.TestSupport;
using System.ComponentModel;

namespace Console.Wpf.Tests.VSTS.DevTests.given_host_adapter
{
    [TestClass]
    public class when_selecting_element : given_host_adapter
    {
        SelectionChangedEventArgs selectedChangedEvent;

        protected override void Arrange()
        {
            base.Arrange();

            HostAdapter.SelectionChanged += (sender, args) => selectedChangedEvent = args;
        }

        protected override void Act()
        {
            CacheManager.Select();
        }

        [TestMethod]
        public void then_component_has_custom_type_descriptor()
        {
            Assert.IsInstanceOfType(selectedChangedEvent.SelectedComponent, typeof(ICustomTypeDescriptor));
        }

        [TestMethod]
        public void then_component_has_site()
        {
            Assert.IsNotNull(selectedChangedEvent.SelectedComponent.Site);
        }

    }
}
