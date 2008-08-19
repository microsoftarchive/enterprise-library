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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Tests.ValidatorNodes
{
    [TestClass]
    public class NotNullValidatorNodeFixture
    {
        [TestMethod]
        public void CreatedNodeHasAppropriateDefaultValues()
        {
            NotNullValidatorNode node = new NotNullValidatorNode();

            NotNullValidatorData validatorData = node.CreateValidatorData() as NotNullValidatorData;

            Assert.IsNotNull(validatorData);
            Assert.AreEqual(false, validatorData.Negated);

            Assert.AreEqual(false, node.Negated);
        }

        [TestMethod]
        public void CreatedNodeWithValidatorDataHasAppropriateValuesFromData()
        {
            NotNullValidatorData validatorData = new NotNullValidatorData("name");
            validatorData.Negated = true;

            NotNullValidatorNode node = new NotNullValidatorNode(validatorData);

            Assert.AreEqual("name", node.Name);
            Assert.AreEqual(true, node.Negated);
        }

        [TestMethod]
        public void NodeCreatesValidatorDataWithValues()
        {
            NotNullValidatorNode node = new NotNullValidatorNode();
            node.Name = "validator";
            node.Negated = true;

            NotNullValidatorData validatorData = node.CreateValidatorData() as NotNullValidatorData;

            Assert.IsNotNull(validatorData);
            Assert.AreEqual("validator", validatorData.Name);
            Assert.AreEqual(true, validatorData.Negated);
        }
    }
}