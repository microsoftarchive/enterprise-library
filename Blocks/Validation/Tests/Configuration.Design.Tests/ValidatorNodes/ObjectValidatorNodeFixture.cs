//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.ValidatorNodes;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Tests.ValidatorNodes
{
    [TestClass]
    public class ObjectValidatorNodeFixture
    {
        [TestMethod]
        public void CreatedNodeHasAppropriateDefaultValues()
        {
            ObjectValidatorNode node = new ObjectValidatorNode();

            Assert.AreEqual(string.Empty, node.TargetRuleset);
        }

        [TestMethod]
        public void CreatedNodeWithValidatorDataHasAppropriateValuesFromData()
        {
            ObjectValidatorData validatorData = new ObjectValidatorData("name");
            validatorData.TargetRuleset = "ruleset";

            ObjectValidatorNode node = new ObjectValidatorNode(validatorData);

            Assert.AreEqual("name", node.Name);
            Assert.AreEqual("ruleset", node.TargetRuleset);
        }

        [TestMethod]
        public void NodeCreatesValidatorDataWithValues()
        {
            ObjectValidatorNode node = new ObjectValidatorNode();
            node.Name = "validator";
            node.TargetRuleset = "resultset";

            ObjectValidatorData validatorData = node.CreateValidatorData() as ObjectValidatorData;

            Assert.IsNotNull(validatorData);
            Assert.AreEqual("validator", validatorData.Name);
            Assert.AreEqual("resultset", validatorData.TargetRuleset);
            Assert.AreSame(typeof(ObjectValidator), validatorData.Type);
        }
    }
}
