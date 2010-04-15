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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.Unity;

namespace Console.Wpf.Tests.VSTS.DevTests.given_configuration_container
{
    [TestClass]
    public class when_requesting_save_operation : ArrangeActAssert
    {
        private ConfigurationContainer container;
        private SaveOperation saveOperation;

        protected override void Arrange()
        {
            this.container = new ConfigurationContainer();
        }

        protected override void Act()
        {
            saveOperation = container.Resolve<SaveOperation>();
        }

        [TestMethod]
        public void then_instance_is_returned()
        {
            Assert.IsNotNull(saveOperation);
        }

        [TestMethod]
        public void then_instance_is_singleton()
        {
            Assert.AreEqual(saveOperation, container.Resolve<SaveOperation>());
        }
    }
}
