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

using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Tests
{
    [TestClass]
    public class AddEnvironmentNodeCommandFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void HierarchyContainsOverridesContainerNodeOnDefault()
        {
            Assert.IsNotNull(ApplicationNode.Hierarchy.FindNodeByType(typeof(EnvironmentalOverridesNode)));
        }

        [TestMethod]
        public void AddEnvironmentNodeAddsNode()
        {
            AddEnvironmentNodeCommand addEnvironmentCommand = new AddEnvironmentNodeCommand(ServiceProvider);
            addEnvironmentCommand.Execute(ApplicationNode);

            EnvironmentNode environmentNode = ApplicationNode.Hierarchy.FindNodeByType(typeof(EnvironmentNode)) as EnvironmentNode;
            Assert.IsNotNull(environmentNode);
        }

        [TestMethod]
        public void AddEnvironmentNodeAddsNodeWithDefaultVaues()
        {
            AddEnvironmentNodeCommand addEnvironmentCommand = new AddEnvironmentNodeCommand(ServiceProvider);
            addEnvironmentCommand.Execute(ApplicationNode);

            EnvironmentNode environmentNode = ApplicationNode.Hierarchy.FindNodeByType(typeof(EnvironmentNode)) as EnvironmentNode;
            Assert.IsNotNull(environmentNode);
            Assert.AreEqual("Environment", environmentNode.Name);
            Assert.AreEqual("Environment.dconfig", environmentNode.EnvironmentDeltaFile);
            Assert.AreEqual(string.Empty, environmentNode.EnvironmentConfigurationFile);
        }
    }
}
