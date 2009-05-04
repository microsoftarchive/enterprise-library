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

using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Tests
{
    /// <summary>
    /// Summary description for EnvironmentalOverridesNodeFixutre
    /// </summary>
    [TestClass]
    public class EnvironmentalOverridesNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void EnvironmentalOverridesNodeHasPropertName()
        {
            EnvironmentalOverridesNode overridesNode = new EnvironmentalOverridesNode();

            Assert.AreEqual("Environments", overridesNode.Name);
        }

        [TestMethod]
        public void EnvironmentalOverridesValidatesDuplicateMergeFilePaths()
        {
            EnvironmentalOverridesNode overridesNode = base.ApplicationNode.Hierarchy.FindNodeByType(typeof(EnvironmentalOverridesNode)) as EnvironmentalOverridesNode;
            EnvironmentNode node1 = new EnvironmentNode();
            EnvironmentNode node2 = new EnvironmentNode();

            overridesNode.AddNode(node1);
            overridesNode.AddNode(node2);

            node1.EnvironmentDeltaFile = "mergefile";
            node2.EnvironmentDeltaFile = "mergefile";

            List<ValidationError> errorList = new List<ValidationError>();
            overridesNode.Validate(errorList);

            Assert.AreEqual(1, errorList.Count);
        }
    }
}
