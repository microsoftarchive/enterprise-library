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
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.HostAdapter;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Console.Hosting;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Hosting;
using System.Windows.Forms.Design;

namespace Console.Wpf.Tests.VSTS.DevTests.given_host_adapter
{
    [TestClass]
    public class when_creating_host_adapter : ArrangeActAssert
    {
        ISingleHierarchyConfigurationUIHostAdapter hostAdapter;
 
        protected override void Act()
        {
            hostAdapter = new SingleHierarchyConfigurationUIHostAdapter(new HostAdapterConfiguration(AppDomain.CurrentDomain.BaseDirectory), null);
        }

        [TestMethod]
        public void then_configuration_source_model_can_be_resolved()
        {
            Assert.IsNotNull(hostAdapter.GetService(typeof(ConfigurationSourceModel)));
        }


        [TestMethod]
        public void then_winforms_editor_can_be_resolved()
        {
            Assert.IsNotNull(hostAdapter.GetService(typeof(IWindowsFormsEditorService)));
        }

    }
}
