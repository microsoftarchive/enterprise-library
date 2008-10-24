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
    public class PropertyComparisonValidatorNodeFixture
    {
        [TestMethod]
        public void CreatedNodeHasAppropriateDefaultValues()
        {
            PropertyComparisonValidatorNode node = new PropertyComparisonValidatorNode();

            Assert.AreEqual(ComparisonOperator.Equal, node.ComparisonOperator);
            Assert.AreEqual("", node.PropertyToCompare);
            Assert.AreEqual(false, node.Negated);
        }

        [TestMethod]
        public void CreatedNodeWithValidatorDataHasAppropriateValuesFromData()
        {
            PropertyComparisonValidatorData validatorData = new PropertyComparisonValidatorData("name");
            validatorData.Negated = true;
            validatorData.ComparisonOperator = ComparisonOperator.GreaterThanEqual;
            validatorData.PropertyToCompare = "property";

            PropertyComparisonValidatorNode node = new PropertyComparisonValidatorNode(validatorData);

            Assert.AreEqual("name", node.Name);
            Assert.AreEqual(ComparisonOperator.GreaterThanEqual, node.ComparisonOperator);
            Assert.AreEqual("property", node.PropertyToCompare);
            Assert.AreEqual(true, node.Negated);
        }

        [TestMethod]
        public void NodeCreatesValidatorDataWithValues()
        {
            PropertyComparisonValidatorNode node = new PropertyComparisonValidatorNode();
            node.Name = "validator";
            node.Negated = true;
            node.ComparisonOperator = ComparisonOperator.GreaterThanEqual;
            node.PropertyToCompare = "property";

            PropertyComparisonValidatorData validatorData = node.CreateValidatorData() as PropertyComparisonValidatorData;

            Assert.IsNotNull(validatorData);
            Assert.AreEqual("validator", validatorData.Name);
            Assert.AreEqual(true, validatorData.Negated);
            Assert.AreEqual(ComparisonOperator.GreaterThanEqual, validatorData.ComparisonOperator);
            Assert.AreEqual("property", validatorData.PropertyToCompare);
        }
    }
}
