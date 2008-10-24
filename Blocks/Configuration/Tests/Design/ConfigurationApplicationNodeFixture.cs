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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestClass]
    public class ConfigurationApplicationNodeFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructNodeWithNullDataThrows()
        {
            ConfigurationApplicationNode node = new ConfigurationApplicationNode(null);
        }

        [TestMethod]
        public void MakeSureThatCurrentAppDomainConfigurationFileIsSetForApplication()
        {
            ConfigurationApplicationFile applicationData = ConfigurationApplicationFile.FromCurrentAppDomain();
            ConfigurationApplicationNode node = new ConfigurationApplicationNode(applicationData);
            ConfigurationUIHierarchy hierarchy = new ConfigurationUIHierarchy(node, ServiceBuilder.Build());

            Assert.AreEqual(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, node.ConfigurationFile);
        }

        [TestMethod]
        public void EnsureThatSettingConfigurationFileUpdatesApplicationDataFile()
        {
            ConfigurationApplicationFile data = ConfigurationApplicationFile.FromCurrentAppDomain();
            ConfigurationApplicationNode node = new ConfigurationApplicationNode(data);
            node.ConfigurationFile = "Foo.config";

            Assert.AreEqual("Foo.config", data.ConfigurationFilePath);
        }

        [TestMethod]
        public void ApplicationNodeWithoutPathIsNamedApplicationConfiguration()
        {
            ConfigurationApplicationNode node = new ConfigurationApplicationNode(new ConfigurationApplicationFile());
            Assert.AreEqual("Application Configuration", node.Name);
        }

        [TestMethod]
        public void FindTypeNodeInHierarchy()
        {
            ConfigurationApplicationFile applicationData = ConfigurationApplicationFile.FromCurrentAppDomain();
            ConfigurationApplicationNode applicationNode = new ConfigurationApplicationNode(applicationData);
            IConfigurationUIHierarchy hierarchy = new ConfigurationUIHierarchy(applicationNode, ServiceBuilder.Build());
            MyConfigNode configNode = new MyConfigNode("MyBlock");
            applicationNode.AddNode(configNode);
            ConfigurationNode node = (ConfigurationNode)hierarchy.FindNodeByType(typeof(MyConfigNode));

            Assert.IsNotNull(node);
            Assert.AreSame(configNode, node);
        }

        class MyConfigNode : ConfigurationNode
        {
            public MyConfigNode(string name)
                : base(name) {}
        }
    }
}
