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
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;

namespace Common.ContainerInfrastructure.Tests.VSTS
{
    [TestClass]
    public class when_adding_extension_with_addnewextension : ArrangeActAssert
    {
        private UnityContainer container;

        protected override void Act()
        {
            container = new UnityContainer();
            container.AddNewExtension<EnterpriseLibraryCoreExtension>();
        }

        [TestMethod]
        public void then_extension_is_added()
        {
            Assert.IsNotNull(container.Configure(typeof(EnterpriseLibraryCoreExtension)));
        }
    }
}
