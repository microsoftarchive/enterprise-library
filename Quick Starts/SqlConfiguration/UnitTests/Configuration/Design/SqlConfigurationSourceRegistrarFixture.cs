//===============================================================================
// Microsoft patterns & practices Enterprise Library
// SQL Configuration Source QuickStart
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.ComponentModel.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Design.Tests
{
    [TestClass]
    public class SqlConfigurationSourceRegistrarFixture
    {
        MockUIComandService cmdService;
        ServiceContainer services;

        [TestInitialize]
        public void TestInitialize()
        {
            cmdService = new MockUIComandService();
            services = new ServiceContainer();
            services.AddService(typeof(IUICommandService), cmdService);
        }

        [TestMethod]
        public void VerifyCommandRegistration()
        {
            SqlConfigurationSourceCommandRegistrar registrar = new SqlConfigurationSourceCommandRegistrar(services);
            registrar.Register();

            Assert.AreEqual(1, cmdService.List[typeof(ConfigurationSourceSectionNode)].Count);
            Assert.AreEqual(2, cmdService.List[typeof(SqlConfigurationSourceElementNode)].Count);
        }
    }
}
