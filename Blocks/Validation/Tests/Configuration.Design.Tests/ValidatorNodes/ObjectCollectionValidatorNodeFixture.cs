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

using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.ValidatorNodes;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Tests.ValidatorNodes
{
    [TestClass]
    public class ObjectCollectionValidatorNodeFixture
    {
        [TestMethod]
        public void CreatedNodeHasAppropriateDefaultValues()
        {
            ObjectCollectionValidatorNode node = new ObjectCollectionValidatorNode();

            Assert.AreEqual(string.Empty, node.TargetType);
            Assert.AreEqual(string.Empty, node.TargetRuleset);
        }

        [TestMethod]
        public void CreatedNodeWithValidatorDataHasAppropriateValuesFromData()
        {
            ObjectCollectionValidatorData validatorData = new ObjectCollectionValidatorData("name");
            validatorData.TargetType = typeof(ObjectCollectionValidatorNodeFixture);
            validatorData.TargetRuleset = "ruleset";

            ObjectCollectionValidatorNode node = new ObjectCollectionValidatorNode(validatorData);

            Assert.AreEqual("name", node.Name);
            Assert.AreEqual(typeof(ObjectCollectionValidatorNodeFixture).AssemblyQualifiedName, node.TargetType);
            Assert.AreEqual("ruleset", node.TargetRuleset);
        }

        [TestMethod]
        public void NodeCreatesValidatorDataWithValues()
        {
            ObjectCollectionValidatorNode node = new ObjectCollectionValidatorNode();
            node.Name = "validator";
            node.TargetType = typeof(ObjectCollectionValidatorNodeFixture).AssemblyQualifiedName;
            node.TargetRuleset = "resultset";

            ObjectCollectionValidatorData validatorData = node.CreateValidatorData() as ObjectCollectionValidatorData;

            Assert.IsNotNull(validatorData);
            Assert.AreEqual("validator", validatorData.Name);
            Assert.AreSame(typeof(ObjectCollectionValidatorNodeFixture), validatorData.TargetType);
            Assert.AreEqual("resultset", validatorData.TargetRuleset);
            Assert.AreSame(typeof(ObjectCollectionValidator), validatorData.Type);
        }
    }
}