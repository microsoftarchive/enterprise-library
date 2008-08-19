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
using System.ComponentModel.Design;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestClass]
    public class OpenApplicationConfigurationCommandFixture
    {
        DialogResult result;
        IServiceProvider serviceProvider;
        IConfigurationUIHierarchy hierarchy;

        [TestInitialize]
        public void TestInitialize()
        {
            ConfigurationApplicationNode node = new ConfigurationApplicationNode(ConfigurationApplicationFile.FromCurrentAppDomain());
            serviceProvider = ServiceBuilder.Build();
            hierarchy = new ConfigurationUIHierarchy(node, serviceProvider);
            ServiceContainer container = (ServiceContainer)serviceProvider;
            container.RemoveService(typeof(IUIService));
            container.AddService(typeof(IUIService), new TestUIService(serviceProvider, this));
        }

        [TestMethod]
        public void OpenCommandOpensAppConfigFile()
        {
            OpenConfigurationApplicationNodeCommand cmd = new OpenConfigurationApplicationNodeCommand(serviceProvider);
            cmd.Execute(null);

            Assert.AreEqual(DialogResult.OK, result);
        }

        class TestUIService : MockUIService
        {
            OpenApplicationConfigurationCommandFixture fixture;

            public TestUIService(IServiceProvider serviceProvider,
                                 OpenApplicationConfigurationCommandFixture fixture)
                : base()
            {
                this.fixture = fixture;
            }

            public override DialogResult ShowOpenDialog(OpenFileDialog dialog)
            {
                fixture.result = DialogResult.OK;
                dialog.FileName = ConfigurationApplicationFile.FromCurrentAppDomain().ConfigurationFilePath;
                return base.ShowOpenDialog(dialog);
            }
        }
    }
}